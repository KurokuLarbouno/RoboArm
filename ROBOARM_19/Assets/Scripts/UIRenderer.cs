using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRenderer : Graphic
{
    public List<Vector2> points;

    public float thickness = 10f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points.Count < 2)
        {
            return;
        }

        for (int i = 0; i < points.Count - 1; i++)
        {
            DrawVerticesForPoint(i, vh);
        }

        for (int i = 0; i < points.Count - 1; i++)
        {
            int index = i * 4;

            vh.AddTriangle(index + 0, index + 2, index + 1);
            vh.AddTriangle(index + 2, index + 3, index + 1);
        }        
    }
    void DrawVerticesForPoint(int index, VertexHelper vh)
    {
        //負值為尾端狀況
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        //list抓前後值作為線段向量
        Vector2 guidLine = points[index + 1] - points[index];
        guidLine = guidLine.normalized;
        //依照厚度找到化三角形的位置
        //index0單位向量*1/2厚度逆時針轉90度, index1單位向量*1/2厚度順時針轉90度, index2單位向量*1/2厚度逆時針轉90度 index3單位向量*1/2厚度順時針轉90度
        int dir = 1;
        if (guidLine.y / guidLine.x > 0) dir = -1;
        Vector2 virticalVec;
        virticalVec.x = dir * guidLine.y * thickness * 0.5f; virticalVec.y = -dir * guidLine.x * thickness * 0.5f;//順時90
        Vector2 recPt0 = points[index] + virticalVec;
        Vector2 recPt2 = points[index + 1] + virticalVec; 
        virticalVec.x = -dir * guidLine.y * thickness * 0.5f; virticalVec.y = dir * guidLine.x * thickness * 0.5f;//順時90
        Vector2 recPt1 = points[index] + virticalVec;
        Vector2 recPt3 = points[index + 1] + virticalVec;   
        vertex.position = recPt0;   vh.AddVert(vertex);
        vertex.position = recPt1;   vh.AddVert(vertex); 
        vertex.position = recPt2;   vh.AddVert(vertex);
        vertex.position = recPt3;   vh.AddVert(vertex);
    }
}
