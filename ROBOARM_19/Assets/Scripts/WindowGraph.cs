using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    Mesh m;
    private List<Vector2> PoseList = new List<Vector2>() { };

    private void Awake()
    {
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();

        List<int> valueList = new List<int>() { 5, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33 };
        ShowDot(valueList);
        DrawLine(PoseList);
        //CreatDotConnection(PoseList);
    }
    private void CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
    }
    private void ShowDot(List<int> valeList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMax = 100f;
        float xSize = 50f;
        for (int i = 0; i < valeList.Count; i++)
        {
            float xPos = i * xSize;
            float yPos = (valeList[i] / yMax) * graphHeight;
            CreateCircle(new Vector2(xPos, yPos));
            PoseList.Add(new Vector2(xPos, yPos));
        }
    }

    private void CreatDotConnection(List<Vector2> valeList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMax = 100f;
        float xSize = 50f;

        if (valeList.Count < 2) return;
        for (int i = 0; i < valeList.Count - 1; i++)
        {
            GameObject gameObject = new GameObject("rect", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().color = Color.green;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector2 dir = valeList[i + 1] - valeList[i];
            float distance = dir.magnitude;
            dir = dir.normalized;
            float eulr;
            if (Vector2.Dot(Vector2.up, dir) > 0) 
            {
                eulr = Vector2.Angle(Vector2.right, dir);
            }else
            {
                eulr = Vector2.Angle(Vector2.left, dir);
            }

            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(distance, 3f);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchoredPosition = valeList[i] + dir * distance * 0.5f;
            rectTransform.localEulerAngles = new Vector3(0, 0, eulr);
        }
    }

    private void ShowLine(List<int> valeList)
    {
        GameObject gameObject = new GameObject("circle", typeof(LineRenderer));
        gameObject.transform.SetParent(graphContainer, false);
        LineRenderer lineRend = gameObject.GetComponent<LineRenderer>();
        lineRend.positionCount = valeList.Count; 
        float graphHeight = graphContainer.sizeDelta.y;
        float yMax = 100f;
        float xSize = 50f;
        for (int i = 0; i < valeList.Count; i++)
        {
            float xPos = i * xSize;
            float yPos = (valeList[i] / yMax) * graphHeight;
            lineRend.SetPosition(i, new Vector3(xPos, yPos));
        }
    }
    private void DrawLine(List<Vector2> valeList)
    {
        int thickness = 10;
        if (valeList.Count < 2) return;
        for (int i = 0; i < valeList.Count - 1; i++)
        {
            //list抓前後值作為線段向量
            Vector2 guidLine = valeList[i + 1] - valeList[i];
            guidLine = guidLine.normalized;
            //依照厚度找到化三角形的位置
            //index0單位向量*1/2厚度逆時針轉90度, index1單位向量*1/2厚度順時針轉90度, index2單位向量*1/2厚度逆時針轉90度 index3單位向量*1/2厚度順時針轉90度
            Vector2 virticalVec;
            virticalVec.x = -guidLine.y * thickness * 0.5f; virticalVec.y = guidLine.x * thickness * 0.5f;//順時90
            Vector2 recPt0 = valeList[i] + virticalVec;
            Vector2 recPt2 = valeList[i + 1] + virticalVec;
            virticalVec.x = guidLine.x * thickness * 0.5f; virticalVec.y = -guidLine.y * thickness * 0.5f;//逆時90
            Vector2 recPt1 = valeList[i] + virticalVec;
            Vector2 recPt3 = valeList[i + 1] + virticalVec;
        }

    }

}
