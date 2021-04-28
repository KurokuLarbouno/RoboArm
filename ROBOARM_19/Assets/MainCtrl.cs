using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCtrl : MonoBehaviour
{
    //Grneral Var
    public enum GameStatus
    {
        INITIAL,    //�̰�¦��l�ơA�]�w�s�u�ΦU����¦�ƭ�
        STANDBY,    //�s�u�Ϊ�l�����A�i�H�\Ū�P����T���٥��s���ʵe
        SYNC,       //�t���s���ʵe
        TPOSE,      //��l�ե�
        ERROR       //�ҥ~���p
    } public GameStatus MainStatus = GameStatus.INITIAL;
    public struct JointPose
    {
        public int jointID;
        public Vector3 rotation;
    } ;
    [SerializeField]  public List<JointPose> AllJoint;
    //Serial
    public List<string> SerialMassage;
    int amount_MaxMas = 4;
    public string[] workPorts;
    public string connectedPort;
    public string newestPort;
    private bool isDevRdy = false;
    public float delay_timer = 0.0f;

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
        Debug.Log("Main Ctrl: T-Pose");
        MainStatus = GameStatus.TPOSE;
    }

    public void EndErr()
    {
        Debug.Log("Main Ctrl: EndErr");
        MainStatus = GameStatus.INITIAL;
    }

    //Device Manerger
    public void DevReady(string Com)
    {
        connectedPort = Com;
        isDevRdy = true;
    }
    public void DevError(string Info)
    {
        isDevRdy = false;
        connectedPort = "";
        MainStatus = GameStatus.ERROR;
        Debug.Log("Main Ctrl: " + Info);
    }


    void Start()
    {
        //test
        for (int i = 0; i < 6; i++)
        {
            UpdateSerail("Hello From MainCtrl!");
        }
        
    }
    /*
     �U�ɬq�{��
        INITIAL
            ����DeviceManeger���ǦC�q�D�[�]�����AUIManerger���L�k���ʪ����A
        STANDBY
            UIManerger�ҰʡAAnimManeger�^��T-Pose�����s��
        SYNC
            AnimManeger�}�l����DeviceManerger�T��
        TPOSE
            UIManerger unSync(disable)�AAnimManeger�^��T-Pose�����s��
            Ū��device�T�w���ƫᥭ���s�Joffset
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
