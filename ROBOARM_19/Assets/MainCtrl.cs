using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCtrl : MonoBehaviour
{
    public enum GameStatus
    {
        INITIAL,    //最基礎初始化，設定連線及各項基礎數值
        STANDBY,    //連線及初始完成，可以閱讀感測資訊但還未連接動畫
        SYNC,       //演側連接動畫
        TPOSE       //初始校正
    } GameStatus gameStatus;
    public struct JointPose
    {
        int jointID;
        Quaternion rotation;
    }

    float boardSetRate = 15;    //FPS
    float boardCurRate = 0;     //FPS
    string boardMassage = "";
    Object CharacterRef = null;
    JointPose[] Joints = new JointPose[4];

    void Start()
    {
        switch (gameStatus)
        {
            case GameStatus.INITIAL:
                Debug.Log("INITIAL");
                break;
            case GameStatus.STANDBY:
                Debug.Log("STAND BY");
                break;
            case GameStatus.SYNC:
                Debug.Log("SYNC");
                break;
        }
    }
}
