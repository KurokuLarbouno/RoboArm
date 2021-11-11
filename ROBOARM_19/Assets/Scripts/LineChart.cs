using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineChart : MonoBehaviour
{
    private RectTransform graphContainer;
    //Main Control
    private MainCtrl MainManerger;
    //graph size
    public float sampleTime = 1;//minute
    private float yMin = 0, yMax = 360;//, anchorValue = 0;
    private float xSize, ySize;
    //line setting
    public float lineWidth = 1f;
    public int lineAmount = 500;

    private struct Line
    {
        public bool active;
        public string itemName;
        public List<Vector2> valueList; //time,value
        public Color lineColor;
        public GameObject line;
        public GameObject[] lineParts;
        public int lineUsedIndex;//紀錄上一禎使用的最大值
    } Line[] allData;
    string[] dataName = { "R_Arm.X" , "R_Arm.Y", "R_Arm.Z"};//, "R_ForeArm.X", "R_ForeArm.Y", "R_ForeArm.Z", "R_Hand.X", "R_Hand.Y", "R_Hand.Z" };
    Color[] dataColor = { Color.red, Color.green, Color.blue, Color.red, Color.green, Color.blue, Color.red, Color.green, Color.blue };
    void Awake()
    {
        //Main Manerger
        GameObject Main = GameObject.Find("MainCtrl");
        MainManerger = Main.GetComponent<MainCtrl>();
        //繪圖Root
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        //依照數值補上陣列
        allData = new Line[dataName.Length];
        //初始化all data
        for (int i = 0; i < dataName.Length; i++)
        {
            allData[i].active = true;
            allData[i].itemName = dataName[i];
            allData[i].valueList = new List<Vector2> {};
            allData[i].lineColor = dataColor[i];
            GameObject gameObject = new GameObject(dataName[i]);
            gameObject.transform.SetParent(graphContainer, false);
            allData[i].line = gameObject;
            allData[i].lineParts = new GameObject[lineAmount];
            //初始化折線圖
            for (int u = 0; u < lineAmount; u++)
            {
                GameObject linePart = new GameObject("rect", typeof(Image));
                linePart.transform.SetParent(allData[i].line.transform, false);
                linePart.GetComponent<Image>().color = allData[i].lineColor;
                linePart.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000f, -2000f);
                allData[i].lineParts[u] = linePart;
            }
        }
    }
    
    void FixedUpdate()
    {
        UpdateValue();
        UpdateSize();
        DrawGraph();
    }
    private void UpdateSize()
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;
        xSize = graphWidth / sampleTime / 60f;
        ySize = graphHeight / (yMax - yMin);
    }
    private void DrawGraph()
    {
        //update start point
        float xOffset = Time.time - (sampleTime * 60f);

        for (int k = 0; k < allData.Length; k++)
        {
            //update all points
            int usedIndex = 0;
            if (allData[k].valueList.Count < 2) return;
            //尋訪所有數值
            for (int i = 0; i < allData[k].valueList.Count - 1; i++)
            {
                if (usedIndex >= lineAmount) break;//粒子不夠時跳脫
                float deltaTime = allData[k].valueList[i].x - xOffset;
                //超出界線
                if (deltaTime < 0)
                {                    
                    float nextTime = allData[k].valueList[i + 1].x; float nextValue = allData[k].valueList[i + 1].y;
                    float oldTime = allData[k].valueList[i].x; float oldValue = allData[k].valueList[i].y;
                    if (oldTime < nextTime)//計算超出點跟下一點交會於畫框邊緣之值
                    {
                        float slope = (nextValue - oldValue) / (nextTime - oldTime);
                        allData[k].valueList[i] = new Vector2(xOffset, oldValue - deltaTime * slope);
                    }
                    else allData[k].valueList.RemoveAt(i);
                }
                //更新圖型
                float newValue = allData[k].valueList[i].y;
                //if (i == 0) { yMin = anchorValue; yMax = anchorValue; }
                //if (newValue < yMin) yMin = newValue; if (newValue > yMax) yMax = newValue;
                float nextDelta = allData[k].valueList[i + 1].x - xOffset;
                Vector2 nextPos = new Vector2(nextDelta * xSize, (allData[k].valueList[i + 1].y - yMin) * ySize);
                Vector2 nowPos = new Vector2(deltaTime * xSize, (allData[k].valueList[i].y - yMin) * ySize);
                Vector2 dir = nextPos - nowPos;
                float distance = dir.magnitude;
                dir = dir.normalized;
                float eulr;
                if (Vector2.Dot(Vector2.up, dir) > 0) eulr = Vector2.Angle(Vector2.right, dir);
                else eulr = Vector2.Angle(Vector2.left, dir);
                //輸入線段圖型數值
                allData[k].lineParts[usedIndex].GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                allData[k].lineParts[usedIndex].GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
                allData[k].lineParts[usedIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(distance, lineWidth);
                allData[k].lineParts[usedIndex].GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                allData[k].lineParts[usedIndex].GetComponent<RectTransform>().anchoredPosition = nowPos + dir * distance * 0.5f;
                allData[k].lineParts[usedIndex].GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, eulr);
                usedIndex++;
            }
            //歸位上禎已經繪製的圖型
            for (int j = usedIndex; j < allData[k].lineUsedIndex; j++)
            {
                allData[k].lineParts[j].GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                allData[k].lineParts[j].GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
                allData[k].lineParts[j].GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
                allData[k].lineParts[j].GetComponent<RectTransform>().anchorMin = new Vector2(-200, -200);
                allData[k].lineParts[j].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                allData[k].lineParts[j].GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
            }
            allData[k].lineUsedIndex = usedIndex;
        }
        
    }
    private void UpdateValue()
    {
        for (int k = 0; k < allData.Length; k++)
        {
            //分析allData對應位置
            int axisIndex = k % 3;
            int jointIndex = k / 3;
            float newValue = 0;
            if (axisIndex == 0) newValue = MainManerger.AllJoint[jointIndex].rotation.x;
            else if (axisIndex == 1) newValue = MainManerger.AllJoint[jointIndex].rotation.y;
            else if (axisIndex == 2) newValue = MainManerger.AllJoint[jointIndex].rotation.z;
            //檢查數值有不同則更新於valueList
            int oldIndex = allData[k].valueList.Count;
            if (oldIndex < 1)
            {
                if (newValue != 0)//為第一個放入的數值就不用比較
                {
                    allData[k].valueList.Add(new Vector2(Time.time, newValue));
                    if (newValue < yMin) yMin = newValue;
                    if (newValue > yMax) yMax = newValue;
                }                
                return;
            }
            float oldValue = allData[k].valueList[oldIndex - 1].y;
            if (oldValue != newValue)
            {
                allData[k].valueList.Add(new Vector2(Time.time, newValue));
                if (newValue < yMin) yMin = newValue;
                if (newValue > yMax) yMax = newValue;
            }
        }          
    }
}
