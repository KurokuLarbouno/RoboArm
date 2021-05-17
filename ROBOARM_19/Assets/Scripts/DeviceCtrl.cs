using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class DeviceCtrl : MonoBehaviour
{
    //Main Control
    MainCtrl MainManerger;
    //Serial
    [SerializeField] SerialPort My_SerialPort = new SerialPort("COM6",115200);
    public List<string> receiveMassage;
    private int baudRate = 115200;
    bool isError = false;
    public MainCtrl.GameStatus state = 0;
    bool isInitial = false;
    MainCtrl.JointPose inputPose;
    Quaternion inputQua;
    public float data_time = 0.0f;

    public void Port_Check()
    {
        MainManerger.workPorts = SerialPort.GetPortNames();
        if (MainManerger.workPorts.Length > 2)
        {
            MainManerger.newestPort = MainManerger.workPorts[MainManerger.workPorts.Length - 1];//以最新加入的port為加入者，通常預設會有兩個com
        }
        else
        {
            MainManerger.newestPort = "";
        }
    }

    public void Port_Connect()
    {
        if (!My_SerialPort.IsOpen)
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
                        isInitial = true;
                        this.CancelInvoke();
                        this.InvokeRepeating("ReadSerialBytes", 0.1f, 0.001f);
                    }
                }
                catch
                {
                    //Debug.Log("Console Connect Faild");
                    this.Invoke("Port_Connect", 0.2f);
                    return;
                }
            }
            else
            {
                this.Invoke("Port_Connect", 0.2f);
            }
        }        
    }

    private void ReadSerialBytes()
    {
        if (My_SerialPort.BytesToRead > 0)
        {
                        string str_Coming;
            str_Coming = My_SerialPort.ReadLine();
            if (str_Coming.Length > 0)
            {
                receiveMassage.Add(str_Coming);
                data_time = 0;
            }
        }        
    }

    private void nReadSerialBytes()
    {
        if (!My_SerialPort.IsOpen) return;
        if (My_SerialPort.BytesToRead > 0)
        {
            byte[] receiveBuffer = new byte[My_SerialPort.ReadBufferSize];

            var numBytesRead = My_SerialPort.Read(receiveBuffer, 0, My_SerialPort.ReadBufferSize);
            byte[] bytesReceived = new byte[numBytesRead];
            Array.Copy(receiveBuffer, bytesReceived, numBytesRead);
            Debug.Log(bytesReceived);
        }
    }

    private void DeviceErr()
    {
        isError = true;
        isInitial = false;
        My_SerialPort.Close();
        this.CancelInvoke();
        MainManerger.DevError("DevCtrl: Port Disconnect!");
    }

    private void ParseString()
    {
        if (receiveMassage.Count > 0)
        {
            MainManerger.delay_timer = 0;
            string[] elements; 
            elements = receiveMassage[0].Split(new char[] { ',' });//分解訊號 ID, w, x, y ,z
            Quaternion inputQua = new Quaternion(0, 0, 0, 0);
            if(elements.Length != 5)
            {
                //Debug.Log("+");
                receiveMassage.RemoveAt(0);
            }
            for (int i = 0; i < elements.Length; i++)
            {
                float value = 0;        //數值

                if (i == 0)
                {                                       
                    try
                    {
                        int deviceId = int.Parse(elements[i]);
                        inputPose.jointID = deviceId;
                    }
                    catch (Exception)
                    {
                        receiveMassage.RemoveAt(0);
                        //Debug.Log("X");
                        return;
                    }
                }
                else
                {
                    try
                    {
                        value = float.Parse(elements[i]);
                    }
                    catch (Exception)
                    {
                        receiveMassage.RemoveAt(0);
                        //Debug.Log("X");
                        return;
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
                }
            }
            inputPose.rotation = inputQua.eulerAngles;
            if (inputPose.jointID != -1)
            {
                MainManerger.AllJoint[inputPose.jointID - 1] = inputPose;
                receiveMassage.RemoveAt(0);
            }
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
        My_SerialPort.Close();
        Port_Check();
        if (MainManerger.newestPort != "")
        {
            Debug.Log("DeviceCtrl: Trying to Connect " + MainManerger.newestPort);
            Port_Connect();
        }        
    }

    void Update()
    {
        data_time += Time.deltaTime;//test
        if (My_SerialPort.IsOpen)
        {
            //con = true;
            try
            {
                if (!isError) { var x = My_SerialPort.CtsHolding; }
            }
            catch (Exception)
            {
                DeviceErr();
                return; //throw;
            }
        }
        if (state != MainManerger.MainStatus)
        {
            //Debug.Log("Trigger Test: " + MainManerger.MainStatus);
            switch (MainManerger.MainStatus)
            {
                case MainCtrl.GameStatus.INITIAL:

                    if (state == MainCtrl.GameStatus.ERROR)
                    {
                        //Debug.Log("Port_Reset");
                        Port_Reset();
                    }
                    break;
                case MainCtrl.GameStatus.ERROR:
                    My_SerialPort.Close();
                    break;
            }
        }
        switch (MainManerger.MainStatus)
        {
            case MainCtrl.GameStatus.INITIAL:
                Port_Connect();
                break;
            case MainCtrl.GameStatus.STANDBY:
            case MainCtrl.GameStatus.SYNC:
            case MainCtrl.GameStatus.TPOSE:
                if (isInitial == true)
                {
                    //input data
                    ParseString();
                }
                break;
            case MainCtrl.GameStatus.ERROR:
                break;
        }
        state = MainManerger.MainStatus;

        Port_Check();
    }
}
