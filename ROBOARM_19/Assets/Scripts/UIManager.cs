using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //Main Control
     MainCtrl MainManerger;
    //顯示各關節數值
     GameObject JointInfo;          //母區塊
     GameObject Text_JointInfo;     //資訊寫入區
     GameObject Btn_JointInfo;      //展開/收闔收合鍵
    Vector3 vec_MoveJoint = new Vector3(-224, 0, 0);    //視窗位移量
    //Serial Consle
     GameObject SerialInfo;         //母區塊
     GameObject Text_SrlInfo;       //資訊寫入區
     GameObject Btn_SrlInfo;        //展開/收闔收合鍵
    Vector3 vec_MoveSrl = new Vector3(-300, 0, 0);      //視窗位移量
    List<string> string_Serial;                     //資訊
                                                    //Error
    [SerializeField] GameObject ErrInfo;         //母區塊
    [SerializeField] GameObject Text_ErrInfo;       //資訊寫入區
    [SerializeField] GameObject Btn_Err;        //展開/收闔收合鍵
    string string_Err = "";
    //各關節狀態顯示
     GameObject DeviceInfo;         //母區塊
    [SerializeField] List<GameObject> DeviceSign;       //燈號
    MainCtrl.JointPose[] passedJoints = new MainCtrl.JointPose[3];
    //同步
    [SerializeField] GameObject Btn_Sync;           //同步關節按鍵
    bool isSync = false;
    //Tpose
    [SerializeField] GameObject Btn_TPose;          //Tpose設定
    //General
    Color clr_normal = Color.white, clr_work = new Color(0, 255, 0), clr_error = new Color(255, 0, 0);
    MainCtrl.GameStatus state = MainCtrl.GameStatus.INITIAL;

    public void ToggleJoint() //展開/收合console介面
    {
        //Debug.Log("ToggleJoint");
        JointInfo.transform.Translate(vec_MoveJoint);
        vec_MoveJoint = new Vector3(vec_MoveJoint.x * -1, vec_MoveJoint.y, vec_MoveJoint.z);
        Btn_JointInfo.transform.Rotate(new Vector3(0, 0, 180));
    }
    public void ToggleSerial() //展開/收合console介面
    {
        //Debug.Log("ToggleSerial");
        SerialInfo.transform.Translate(vec_MoveSrl);
        vec_MoveSrl = new Vector3(vec_MoveSrl.x * -1, vec_MoveSrl.y, vec_MoveSrl.z);
        Btn_SrlInfo.transform.Rotate(new Vector3(0, 0, 180));
    }
    public void TPose() 
    {
        if (MainManerger.MainStatus != MainCtrl.GameStatus.INITIAL)
        {
            MainManerger.Tpose();
            Btn_TPose.GetComponent<Image>().color = clr_work;
            Btn_TPose.GetComponent<Button>().interactable = false;
        }
    }

    public void TposeFin()
    {
        //Debug.Log("TPose Fin");
        Btn_TPose.GetComponent<Image>().color = clr_normal;
        Btn_TPose.GetComponent<Button>().interactable = true;
    }

    private void ShowError()
    {
        ErrInfo.SetActive(true);
    }

    private void UpdateError()
    {
        string outputWords = string_Err + MainManerger.newestPort;
        Text_ErrInfo.GetComponent<TMP_Text>().text = outputWords;
        if (MainManerger.newestPort.Contains("COM"))
        {
            if (!Btn_Err.activeSelf) Btn_Err.SetActive(true);
        }
        else
        {
            Btn_Err.SetActive(false);
        }
    }

    private void UpdateJoint()
    {
        //更新讀取資料
        string outputWords = "";
        for (int i = 0; i < MainManerger.AllJoint.Length; i++)
        {
            switch (i)
            {
                case 0:
                    outputWords += "R_Hand\t";
                    break;
                case 1:
                    outputWords += "R_ForeArm\t";
                    break;
                case 2:
                    outputWords += "R_Arm\t";
                    break;
            }
            outputWords += "X:"; outputWords += MainManerger.AllJoint[i].rotation.x.ToString("000.0");
            outputWords += "Y:"; outputWords += MainManerger.AllJoint[i].rotation.y.ToString("000.0");
            outputWords += "Z:"; outputWords += MainManerger.AllJoint[i].rotation.z.ToString("000.0"); outputWords += "\n";
        }
        Text_JointInfo.GetComponent<TMP_Text>().text = outputWords;
    }
    private void UpdateDeviceInfo()
    {
        //更新燈號
        for (int i = 0; i < passedJoints.Length; i++)
        {
            if (passedJoints[i].rotation != MainManerger.AllJoint[i].rotation)
            {
                DeviceSign[i].GetComponent<Image>().color = Color.green;
            }
            else
            {
                DeviceSign[i].GetComponent<Image>().color = Color.red;
            }
            passedJoints[i] = MainManerger.AllJoint[i];
        }        
    }
    public void EndError()
    {
        MainManerger.EndErr();
        Btn_Err.SetActive(false);
        ErrInfo.SetActive(false);
    }

    public void ToggleSync() 
    {
        if (MainManerger.MainStatus == MainCtrl.GameStatus.STANDBY) {Btn_Sync.GetComponent<Image>().color = clr_work; MainManerger.Sync(); }
        else if (MainManerger.MainStatus == MainCtrl.GameStatus.SYNC) { Btn_Sync.GetComponent<Image>().color = clr_normal; MainManerger.StandBy(); }
    }

    private void DisableUI()
    {
        Btn_Sync.GetComponent<Button>().interactable = false;
        Btn_TPose.GetComponent<Button>().interactable = false;
    }
    private void EnableUI()
    {
        Btn_Sync.GetComponent<Button>().interactable = true;
        Btn_TPose.GetComponent<Button>().interactable = true;
    }
    void Start()
    {
        //Main Manerger
        GameObject Main = GameObject.Find("MainCtrl");
        MainManerger = Main.GetComponent<MainCtrl>();
        //Get Objects
        JointInfo = GameObject.Find("JointInfo");
        Text_JointInfo = GameObject.Find("Info");
        Btn_JointInfo = GameObject.Find("Btn_Info");

        SerialInfo = GameObject.Find("SerialInfo");
        Text_SrlInfo = GameObject.Find("Text_SrlInfo");
        Btn_SrlInfo = GameObject.Find("Btn_Srl");

        ErrInfo = GameObject.Find("ErrInfo");
        Text_ErrInfo = GameObject.Find("Text_ErrInfo");
        Btn_Err = GameObject.Find("Btn_Err");
        Btn_Err.SetActive(false);
        ErrInfo.SetActive(false);
        string_Err = Text_ErrInfo.GetComponent<TMP_Text>().text;

        DeviceInfo = GameObject.Find("DeviceInfo");
        if (DeviceInfo != null)
        {
            for (int i = 1; i < DeviceInfo.transform.childCount; i++)
            {
                DeviceSign.Add(DeviceInfo.transform.GetChild(i).gameObject);
            }
        }
        Btn_Sync = GameObject.Find("Btn_Sync");
        Btn_TPose = GameObject.Find("Btn_Tpose");
        //Joint Info
        vec_MoveJoint.x = JointInfo.GetComponent<RectTransform>().rect.width - 10;//保留部分UI暗示可以展開
        vec_MoveJoint.x *= -1;
        ToggleJoint();
        //Serial Info
        vec_MoveSrl.x = SerialInfo.GetComponent<RectTransform>().rect.width - 10;//保留部分UI暗示可以展開
        ToggleSerial();
        //get Button color
        clr_normal = Btn_Sync.GetComponent<Image>().color;

        DisableUI();
    }

    void Update()
    {
        if (state != MainManerger.MainStatus)
        {
            switch (MainManerger.MainStatus)
            {
                case MainCtrl.GameStatus.INITIAL:
                    DisableUI();
                    break;
                case MainCtrl.GameStatus.STANDBY:
                    EnableUI(); TposeFin();
                    break;
                case MainCtrl.GameStatus.SYNC:
                    break;
                case MainCtrl.GameStatus.TPOSE:
                    DisableUI();
                    break;
                case MainCtrl.GameStatus.ERROR:
                    DisableUI();
                    ShowError();
                    break;
            }
        }
        switch (state)
        {
            case MainCtrl.GameStatus.INITIAL:
                //DisableUI();
                break;
            case MainCtrl.GameStatus.STANDBY:
                //EnableUI();
                break;
            case MainCtrl.GameStatus.ERROR:
                UpdateError();
                break;
        }
        state = MainManerger.MainStatus;
        UpdateJoint();
        UpdateDeviceInfo();

    }
}
