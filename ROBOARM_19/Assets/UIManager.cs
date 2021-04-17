using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //Main Control
    [SerializeField] MainCtrl MainManerger;
    //顯示各關節數值
    [SerializeField] GameObject JointInfo;          //母區塊
    [SerializeField] GameObject Text_JointInfo;     //資訊寫入區
    [SerializeField] GameObject Btn_JointInfo;      //展開/收闔收合鍵
    Vector3 vec_MoveJoint = new Vector3(-224, 0, 0);    //視窗位移量
    List<string> string_JointInfo;                  //資訊
    //Serial Consle
    [SerializeField] GameObject SerialInfo;         //母區塊
    [SerializeField] GameObject Text_SrlInfo;       //資訊寫入區
    [SerializeField] GameObject Btn_SrlInfo;        //展開/收闔收合鍵
    Vector3 vec_MoveSrl = new Vector3(-300, 0, 0);      //視窗位移量
    List<string> string_Serial;                     //資訊
    //各關節狀態顯示
    [SerializeField] GameObject DeviceInfo;         //母區塊
    [SerializeField] List<GameObject> DeviceSign;       //燈號
    //同步
    [SerializeField] GameObject Btn_Sync;           //同步關節按鍵
    bool isSync = false;
    //Tpose
    [SerializeField] GameObject Btn_TPose;          //Tpose設定
    //General
    Color clr_normal, clr_work = new Color(0, 255, 0), clr_error = new Color(255, 0, 0);

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

    public void ToggleSync() 
    {
        if (MainManerger.MainStatus == MainCtrl.GameStatus.STANDBY) {Btn_Sync.GetComponent<Image>().color = clr_work; MainManerger.Sync(); }
        else if (MainManerger.MainStatus == MainCtrl.GameStatus.SYNC) { Btn_Sync.GetComponent<Image>().color = clr_normal; MainManerger.StandBy(); }
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
        //Button
        clr_normal = Btn_Sync.GetComponent<Image>().color;

    }
        

    void Update()
    {
        switch (MainManerger.MainStatus)
        {
            case MainCtrl.GameStatus.INITIAL:
                //Debug.Log("INITIAL");
                //disable UI
            case MainCtrl.GameStatus.STANDBY:
                //Debug.Log("STAND BY");
                //ensable UI
                break;
            case MainCtrl.GameStatus.SYNC:
                //Debug.Log("SYNC");
                break;
            case MainCtrl.GameStatus.TPOSE:
                //Debug.Log("TPOSE");
                //disable UI
                break;
        }
        //text test
        string tmp = "";
        for (int i = 0; i < MainManerger.SerialMassage.Count; i++)
        {
            tmp += MainManerger.SerialMassage[i];
            tmp += "\n";
        }
        Text_SrlInfo.GetComponent<TMP_Text>().text = tmp;
    }
}
