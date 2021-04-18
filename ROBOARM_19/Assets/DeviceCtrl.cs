using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class DeviceCtrl : MonoBehaviour
{
    //Main Control
    [SerializeField] MainCtrl MainManerger;
    //Serial
    private SerialPort My_SerialPort;
    private bool isTimerSet = false;
    public List<string> receiveMassage;
    private int baudRate = 115200;
    private bool isError = false;
    //public string[] workPorts;
    MainCtrl.GameStatus state = 0;

    public void Port_Check()
    {
        MainManerger.workPorts = SerialPort.GetPortNames();
        if (MainManerger.workPorts.Length > 2)
        {
            MainManerger.newestPort = MainManerger.workPorts[MainManerger.workPorts.Length - 1];//以最新加入的port為加入者，通常預設會有兩個com
        } 
    }

    public void Port_Connect()
    {
        if (MainManerger.newestPort.Length > 0)
        {
            try
            {
                My_SerialPort = new SerialPort();

                //設定 Serial Port 參數
                My_SerialPort.PortName = MainManerger.newestPort;
                My_SerialPort.BaudRate = baudRate;
                My_SerialPort.ReadTimeout = 20;

                My_SerialPort.Open();

                if (My_SerialPort.IsOpen)
                {
                    MainManerger.DevReady(MainManerger.newestPort);
                    this.CancelInvoke();
                }
            }
            catch
            {
                Debug.Log("Console Connect Faild");
                this.Invoke("Port_Connect", 0.2f);
            }
        }
        else
        {
            this.Invoke("Port_Connect", 0.2f);
        }
    }

    private void ReadSerialBytes()
    {
        if (!My_SerialPort.IsOpen)
        {
            MainManerger.DevError("DevCtrl: Port Disconnect!");
            return;
        }
        string str_Coming;
        do
        {
            str_Coming = My_SerialPort.ReadLine();
            if (str_Coming != null)
            {
                receiveMassage.Add(str_Coming);
            }
            else
            {
                Debug.Log(".");
                break;
            }
        } while (str_Coming.Length > 0);
    }

    private void ParseString()
    {
        while (receiveMassage.Count > 0)
        {
            MainManerger.delay_timer = 0;
            string[] elements = receiveMassage[0].Split(new char[] { ',' });//分解訊號 ID, w, x, y ,z
            for (int i = 0; i < elements.Length; i++)
            {
                float value = 0;        //數值
                MainCtrl.JointPose inputPose;
                inputPose.jointID = -1;
                inputPose.rotation = new Vector3(0, 0, 0);
                Quaternion inputQua = new Quaternion(0, 0, 0, 0);
                if (i == 0)
                {
                    int deviceId = int.Parse(elements[i]);
                    inputPose.jointID = deviceId;
                }
                else
                {
                    value = float.Parse(elements[i]);
                }
                switch (i)
                {
                    case 1:
                        inputQua.w = value;
                        break;
                    case 2:
                        inputQua.x = -value;
                        break;
                    case 4:
                        inputQua.y = -value;
                        break;
                    case 3:
                        inputQua.z = -value;
                        break;
                    default:
                        break;
                }
                inputPose.rotation = inputQua.eulerAngles;
                if (MainManerger.AllJoint.Exists(x => x.jointID == inputPose.jointID))
                {
                    int index = MainManerger.AllJoint.FindIndex(x => x.jointID == inputPose.jointID);
                    MainManerger.AllJoint[index] = inputPose;
                }
                else if (inputPose.jointID != -1)
                {
                    MainManerger.AllJoint.Add(inputPose);
                }
            }
            receiveMassage.RemoveAt(0);
        }
        MainManerger.delay_timer += Time.deltaTime;
    }

    private void Port_Reset()
    {
        Debug.Log("DeviceCtrl: Trying to Reconnect " + MainManerger.newestPort);
        Port_Connect();
        isError = false;
    }

    void Start()
    {
        //Main Manerger
        GameObject Main = GameObject.Find("MainCtrl");
        MainManerger = Main.GetComponent<MainCtrl>();
        //Serail
        Port_Check();
        if (MainManerger.newestPort != "")
        {
            Debug.Log("DeviceCtrl: Trying to Connect " + MainManerger.newestPort);
            Port_Connect();
        }        
    }

    void Update()
    {
        if (state != MainManerger.MainStatus)
        {
            switch (state)
            {
                case MainCtrl.GameStatus.INITIAL:
                    if (My_SerialPort == null)
                    {
                        Port_Check();
                        if (MainManerger.newestPort != "")
                        {
                            Debug.Log("DeviceCtrl: Trying to Connect " + MainManerger.newestPort);
                            Port_Connect();
                        }
                    }
                    if (state == MainCtrl.GameStatus.ERROR)
                    {
                        Port_Reset();
                    }
                    break;
            }
        }
        switch (state)
        {
            case MainCtrl.GameStatus.INITIAL:
                Port_Check();
                Port_Connect();
                break;
            case MainCtrl.GameStatus.STANDBY:
            case MainCtrl.GameStatus.SYNC:
            case MainCtrl.GameStatus.TPOSE:
                ReadSerialBytes();
                //input data
                ParseString();
                break;
            case MainCtrl.GameStatus.ERROR:
                Port_Check();
                break;
        }
        state = MainManerger.MainStatus;
    }
}
