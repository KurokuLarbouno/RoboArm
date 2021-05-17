using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCtrl : MonoBehaviour
{
    //Main Control
    MainCtrl MainManerger;
    //test rotation
    public Vector3[] RotArray = new Vector3[3];

    GameObject[] ActiveJoint = new GameObject[3];
    void Start()
    {
        //Main Manerger
        GameObject Main = GameObject.Find("MainCtrl");
        MainManerger = Main.GetComponent<MainCtrl>();

        for (int i = 0; i < ActiveJoint.Length; i++)
        {
            switch (i)
            {
                case 0:
                    ActiveJoint[i] = GameObject.Find("mixamorig:RightArm");
                    break;
                case 1:
                    ActiveJoint[i] = GameObject.Find("mixamorig:RightForeArm");
                    break;
                case 2:
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
        for (int i = 0; i < ActiveJoint.Length; i++)
        {
            //ActiveJoint[i].transform.eulerAngles = MainManerger.AllJoint[i].rotation;
            ActiveJoint[i].transform.eulerAngles = MainManerger.AllJoint[i].rotation;
        }
    }
}
