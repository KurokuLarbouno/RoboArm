using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using UnityEditorInternal;

public class DeviceManerger : MonoBehaviour
{
    //不論流程於哪個階段，
    //Serial
    private SerialPort sp;
    private bool isTimerSet = false;
    private string receiveMassage;
    
    [SerializeField]
    private GameObject[] ActiveJoint = new GameObject[3];
    [SerializeField]
    private Quaternion[] AllJoints = new Quaternion[3];



    // Start is called before the first frame update
    void Start()
    {
        sp = new SerialPort("COM5", 115200);
        //Serial
        sp.Open();
        sp.ReadTimeout = 20;
        //if (SerialPort.GetPortNames().ToString().Contains("COM5"))
        //{
            
        //}
        //find joint
        for (int i = 0; i < ActiveJoint.Length; i++)
        {
            switch (i)
            {
                case 2:
                    ActiveJoint[i] = GameObject.Find("mixamorig:RightArm");
                    break;
                case 1:
                    ActiveJoint[i] = GameObject.Find("mixamorig:RightForeArm");
                    break;
                case 0:
                    ActiveJoint[i] = GameObject.Find("mixamorig:RightHand");
                    break;
                default:
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sp!=null)
        {
            if (sp.IsOpen && !isTimerSet)
            {
                //定時傳送
                this.InvokeRepeating("setMassage", 1.0f, 0.005f);//"methodName" in "time" seconds, then repeatedly every "repeatRate" seconds.
                isTimerSet = true;
            }
        }
        //rotate
        for (int i = 0; i < ActiveJoint.Length; i++)
        {
            ActiveJoint[i].transform.rotation = AllJoints[i];
        }
    }

    private void setMassage()
    {
        if (sp.IsOpen)
        {
            try
            {
                int jointId = 0;
                receiveMassage = sp.ReadLine();
                //Debug.Log(receiveMassage);
                //分解訊號 ID, w, x, y ,z
                string[] elements = receiveMassage.Split(new char[] { ',' });
                //Debug.Log(elements);
                for (int i = 0; i < elements.Length; i++)
                {
                    float value = -20;
                    if (i == 0)
                    {
                        int id = int.Parse(elements[i]);
                        jointId = (int)id - 1;
                    }
                    else
                    {
                        value = float.Parse(elements[i]);
                    }
                   // Debug.Log(value);  //測試轉換結果
                    
                    switch (i)
                    {
                        case 1:
                            AllJoints[jointId].w = value;
                            break;
                        case 2:
                            AllJoints[jointId].x = -value;
                            break;
                        case 4:
                            AllJoints[jointId].y = -value;
                            break;
                        case 3:
                            AllJoints[jointId].z = -value;
                            break;
                        default:
                            break;
                    }
                }
                //Debug.Log(AllJoints);
            }
            catch
            {
                Debug.Log("WriteLine Timeout!");
            }

        }
        else
        {
            Debug.Log(" Serial Error.");
            CancelInvoke();
        }
    }
    void RotateJoint()
    {

    }
}