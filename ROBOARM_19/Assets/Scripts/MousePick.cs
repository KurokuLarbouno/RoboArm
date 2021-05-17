using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePick : MonoBehaviour
{
    //GameObject[] ActiveJoint = {null};
    GameObject ActiveJoint = null;
    string activeJointName = "";
    float incSpeed = 20.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit) ) //&& ActiveJoint == null)
        {
            ActiveJoint = hit.transform.gameObject;
            //Debug.DrawLine(Camera.main.transform.position, hit.transform.position, Color.red, 0.1f, true);
            activeJointName = ActiveJoint.name;
            Debug.Log(activeJointName);
        }
        if(activeJointName != null)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (activeJointName.Contains("Arm"))
                {
                    //forearm
                    if (activeJointName.Contains("Forearm"))
                    {
                        int dir = 1;
                        if (activeJointName.Contains("Right")) dir = -1;//反轉角度
                                                                        //設定角度
                        ActiveJoint.transform.Rotate(0, incSpeed * Time.deltaTime * dir, 0);
                    }
                    //arm
                    else
                    {
                        int dir = 1;
                        if (activeJointName.Contains("Right")) dir = -1;//反轉角度
                                                                        //設定角度
                        ActiveJoint.transform.Rotate(0, 0, incSpeed * Time.deltaTime * dir);
                    }
                }
                //hand
                else
                {
                    int dir = 1;
                    if (activeJointName.Contains("Right")) dir = -1;//反轉角度
                                                                    //設定角度
                    ActiveJoint.transform.Rotate(0, 0, incSpeed * Time.deltaTime * dir);
                }
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (activeJointName.Contains("Arm"))
                {
                    //forearm
                    if (activeJointName.Contains("Forearm"))
                    {
                        int dir = 1;
                        if (activeJointName.Contains("Right")) dir = -1;//反轉角度
                                                                        //設定角度
                        ActiveJoint.transform.Rotate(0, incSpeed * Time.deltaTime * -dir, 0);
                    }
                    //arm
                    else
                    {
                        int dir = 1;
                        if (activeJointName.Contains("Right")) dir = -1;//反轉角度
                                                                        //設定角度
                        ActiveJoint.transform.Rotate(0, 0, incSpeed * Time.deltaTime * -dir);
                    }
                }
                //hand
                else
                {
                    int dir = 1;
                    if (activeJointName.Contains("Right")) dir = -1;//反轉角度
                                                                    //設定角度
                    ActiveJoint.transform.Rotate(0, 0, incSpeed * Time.deltaTime * -dir);
                }
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (activeJointName.Contains("Arm"))
                {
                    //forearm
                    if (activeJointName.Contains("Forearm"))
                    {
                    }
                    //arm
                    else
                    {
                        int dir = -1;
                        if (activeJointName.Contains("Right")) dir = 1;//反轉角度
                                                                       //設定角度
                        ActiveJoint.transform.Rotate(0, incSpeed * Time.deltaTime * dir, 0);
                    }
                }
                //hand
                else
                {
                    int dir = 1;
                    if (activeJointName.Contains("Right")) dir = -1;//反轉角度
                                                                    //設定角度
                    ActiveJoint.transform.Rotate(incSpeed * Time.deltaTime * dir, 0, 0);
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (activeJointName.Contains("Arm"))
                {
                    //forearm
                    if (activeJointName.Contains("Forearm"))
                    {
                    }
                    //arm
                    else
                    {
                        int dir = -1;
                        if (activeJointName.Contains("Right")) dir = 1;//反轉角度
                                                                       //設定角度
                        ActiveJoint.transform.Rotate(0, incSpeed * Time.deltaTime * -dir, 0);
                    }
                }
                //hand
                else
                {
                    int dir = 1;
                    if (activeJointName.Contains("Right")) dir = -1;//反轉角度
                                                                    //設定角度
                    ActiveJoint.transform.Rotate(incSpeed * Time.deltaTime * -dir, 0, 0);
                }
            }
        }
        
    }

}
