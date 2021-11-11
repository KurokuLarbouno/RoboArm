using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimCtrl : MonoBehaviour
{
    const int JointAmount = 3;
    //Main Control
    MainCtrl MainManerger;
    MainCtrl.GameStatus state;
    //offset rotation
    MainCtrl.JointPose[] offsetPose = new MainCtrl.JointPose[JointAmount];
    MainCtrl.JointPose[] originPose = new MainCtrl.JointPose[JointAmount];
    MainCtrl.JointPose[] passedJoints = new MainCtrl.JointPose[JointAmount];
    public Vector3[] originVec = new Vector3[JointAmount];

    GameObject[] ActiveJoint = new GameObject[JointAmount];
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
            originPose[i].jointID = i;
            originPose[i].rotation = ActiveJoint[i].transform.rotation.eulerAngles;
            offsetPose[i].rotation = originPose[i].rotation;
        }
    }
    void ResetPose()
    {
        for (int i = 0; i < ActiveJoint.Length; i++)
        {
            ActiveJoint[i].transform.eulerAngles = offsetPose[i].rotation;
        }
    }
    void FinishTpose()
    {
        //更新T-pose
        for (int i = 0; i < passedJoints.Length; i++)
        {
            offsetPose[i].rotation = offsetPose[i].rotation / offsetPose[i].jointID; //這邊ID做為counter使用
            offsetPose[i].rotation += originPose[i].rotation;
            originVec[i] = offsetPose[i].rotation;////////////////////////
            offsetPose[i].jointID = 0;
        }
        MainManerger.EndTpose();
    }
    void CollectingPose()
    {        
        //更新T-pose
        for (int i = 0; i < MainManerger.AllJoint.Length; i++)
        {
            if (passedJoints[i].rotation != MainManerger.AllJoint[i].rotation)
            {
                offsetPose[i].jointID += 1;//這邊ID做為counter使用
                offsetPose[i].rotation += MainManerger.AllJoint[i].rotation;
            }
            passedJoints[i] = MainManerger.AllJoint[i];
        }
    }

    void Update()
    {
        if (state != MainManerger.MainStatus)
        {
            switch (MainManerger.MainStatus)
            {
                case MainCtrl.GameStatus.STANDBY:
                    ResetPose();
                    break;
                case MainCtrl.GameStatus.TPOSE:
                    ResetPose();
                    Invoke("FinishTpose", 3.0f);
                    break;
            }
        }
        state = MainManerger.MainStatus;
        switch (MainManerger.MainStatus)
        {
            case MainCtrl.GameStatus.SYNC:
                for (int i = 0; i < ActiveJoint.Length; i++)
                {
                    Vector3 off = new Vector3(180, 0, 0);
                    ActiveJoint[i].transform.eulerAngles = MainManerger.AllJoint[i].rotation + off;// - offsetPose[i].rotation;
                }
                break;
            case MainCtrl.GameStatus.TPOSE:
                for (int i = 0; i < MainManerger.AllJoint.Length; i++)
                {
                    offsetPose[i].rotation = new Vector3(0, 0, 0);
                }
                CollectingPose();
                break;
        }
    }
}
