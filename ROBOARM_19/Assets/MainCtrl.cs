using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCtrl : MonoBehaviour
{
    public enum GameStatus
    {
        INITIAL,    //�̰�¦��l�ơA�]�w�s�u�ΦU����¦�ƭ�
        STANDBY,    //�s�u�Ϊ�l�����A�i�H�\Ū�P����T���٥��s���ʵe
        SYNC,       //�t���s���ʵe
        TPOSE       //��l�ե�
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
