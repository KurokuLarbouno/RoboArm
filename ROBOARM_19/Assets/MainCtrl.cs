using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCtrl : MonoBehaviour
{
    //Grneral Var
    public enum GameStatus
    {
        INITIAL,    //最基礎初始化，設定連線及各項基礎數值
        STANDBY,    //連線及初始完成，可以閱讀感測資訊但還未連接動畫
        SYNC,       //演側連接動畫
        TPOSE       //初始校正
    } public GameStatus MainStatus = GameStatus.INITIAL;
    public struct JointPose
    {
        int jointID;
        Vector3 rotation;
    } public List<JointPose> AllJoint;

    //Serial
    public List<string> SerialMassage;
    int amount_MaxMas = 4;

    public void UpdateSerail(string string_Incom)
    {
        if(SerialMassage.Count < amount_MaxMas)
        {
            SerialMassage.Add(string_Incom);
        }
        else
        {
            for (int i = 0; i < SerialMassage.Count - 1; i++)
            {
                SerialMassage[i] = SerialMassage[i + 1];
            }
            SerialMassage[SerialMassage.Count - 1] = string_Incom;
        }
    }

    //UIManerger
    public void Sync()
    {
        Debug.Log("SYNC");
        MainStatus = GameStatus.SYNC;
    }

    public void StandBy()
    {
        Debug.Log("STAND BY");
        MainStatus = GameStatus.STANDBY;                
    }
    
    public void Tpose()
    {
        Debug.Log("T-Pose");
        MainStatus = GameStatus.TPOSE;
    }
    //Device Manerger
    public bool isDevRdy =false;

    void Start()
    {
        //test
        for (int i = 0; i < 6; i++)
        {
            UpdateSerail("Hello From MainCtrl!");
        }
        
    }
    /*
     各時段程序
        INITIAL
            等待DeviceManeger的序列通道架設完成，UIManerger為無法互動的狀態
        STANDBY
            UIManerger啟動，AnimManeger回到T-Pose不做連接
        SYNC
            AnimManeger開始接收DeviceManerger訊號
        TPOSE
            UIManerger unSync(disable)，AnimManeger回到T-Pose不做連接
            讀取device固定次數後平均存入offset
     */
    void Update()
    {
        switch (MainStatus)
        {
            case GameStatus.INITIAL:
                //Debug.Log("INITIAL");
                if (isDevRdy)
                {
                    StandBy();
                }
                break;
            case GameStatus.STANDBY:
                //Debug.Log("STAND BY");
                break;
            case GameStatus.SYNC:
                //Debug.Log("SYNC");
                break;
        }
    }
}
