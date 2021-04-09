using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCtrl : MonoBehaviour
{
    GameObject[] ActiveJoint = new GameObject[4];
    void Start()
    {
        for (int i = 0; i < ActiveJoint.Length; i++)
        {
            switch (i)
            {
                case 0:
                    ActiveJoint[i] = GameObject.Find("mixamorig:LeftArm");
                    break;
                case 1:
                    ActiveJoint[i] = GameObject.Find("mixamorig:LeftForeArm");
                    break;
                case 2:
                    ActiveJoint[i] = GameObject.Find("mixamorig:LeftHand");
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
            //ActiveJoint[i].transform.rotation = device.AllJoints[i];
        }
    }
}
