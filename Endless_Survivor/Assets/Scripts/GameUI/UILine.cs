using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UILine : Graphic
{
    [SerializeField] Gradient _lineColor;
    [SerializeField] float _lineWidth;
    [SerializeField] float _startingSquareSizeMultiplier = 1.5f;

    List<Vector2> _lineVertices = new List<Vector2>();
    public List<Vector2> LineVertices { get { return _lineVertices; } }
    protected override void OnEnable()
    {
        _lineVertices.Clear();
    }
    public void AddVertex(Vector2 vertexPosition)
    {
        _lineVertices.Add(vertexPosition);
        SetAllDirty();
    }
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if(_lineVertices.Count == 0)
            return;
        //create square on first vertex
        //calculating directions for the square
        Vector2 squareDir = _lineVertices.Count > 1 ? (_lineVertices[1] - _lineVertices[0]).normalized : Vector2.up;
        bool isXSquareZero = squareDir.x == 0;
        float squareY = isXSquareZero ? -(squareDir.x * 1) / squareDir.y : 1;
        float squareX = isXSquareZero ? 1 : -(squareDir.y * 1) / squareDir.x;

        Vector2 squareWidthVector = new Vector2(squareX, squareY).normalized;

        float squareSizeFactor = _lineWidth * _startingSquareSizeMultiplier;
        //using directions calculate measures
        Vector2 squareSize1 = squareDir * squareSizeFactor;
        Vector2 squareSize2 = squareWidthVector * squareSizeFactor;

        //Constructing square
        UIVertex squareVertex = UIVertex.simpleVert;
        squareVertex.color = _lineColor.Evaluate(0);

        squareVertex.position = _lineVertices[0] + squareSize1 + squareSize2;
        vh.AddVert(squareVertex);
        
        squareVertex.position = _lineVertices[0] - squareSize1 - squareSize2;
        vh.AddVert(squareVertex);
        
        squareVertex.position = _lineVertices[0] - squareSize1 + squareSize2;
        vh.AddVert(squareVertex);

        squareVertex.position = _lineVertices[0] + squareSize1 - squareSize2;
        vh.AddVert(squareVertex);

        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(0, 1, 3);

        //Join vertices with lines
        int addedVerts = 4;
        for(int i = 0; i < _lineVertices.Count-1; i++)
        {
            Vector2 currVertexPos = _lineVertices[i];
            Vector2 nextVertexPos = _lineVertices[i + 1];

            //Based on the line vertex positions, get the actual pairs of vertex positions that will form a line with width
            Vector2 segmentDir = (nextVertexPos - currVertexPos).normalized;
            bool isXZero = segmentDir.x == 0;
            //since one is linearly dependent of the other, we solve V * W = 0 (perpendicular vector equation), ensuring we dont divide by 0
            float y = isXZero ? -(segmentDir.x * 1) / segmentDir.y : 1;
            float x = isXZero ?  1 : -(segmentDir.y * 1) / segmentDir.x;

            Vector2 segmentWidthVector = new Vector2(x, y).normalized;
            Vector2 currVtSide1Pos = currVertexPos + segmentWidthVector * _lineWidth;
            Vector2 currVtSide2Pos = currVertexPos - segmentWidthVector * _lineWidth;

            Vector2 nextVtSide1Pos = nextVertexPos + segmentWidthVector * _lineWidth;
            Vector2 nextVtSide2Pos = nextVertexPos - segmentWidthVector * _lineWidth;

            //Creating the vertices that will make the segment
            float colorValue = (i * 100) / _lineVertices.Count;
            colorValue /= 100;
            Color currVertexColor = _lineColor.Evaluate(colorValue);
            Color nextVertexColor = _lineColor.Evaluate(colorValue);

            UIVertex currSideVertex1 = UIVertex.simpleVert;
            currSideVertex1.position = currVtSide1Pos;
            currSideVertex1.color = currVertexColor;
            UIVertex currSideVertex2 = UIVertex.simpleVert;
            currSideVertex2.position = currVtSide2Pos;
            currSideVertex2.color = currVertexColor;
            UIVertex nextSideVertex1 = UIVertex.simpleVert;
            nextSideVertex1.position = nextVtSide1Pos;
            nextSideVertex1.color = nextVertexColor;
            UIVertex nextSideVertex2 = UIVertex.simpleVert;
            nextSideVertex2.position = nextVtSide2Pos;
            nextSideVertex2.color = nextVertexColor;

            //Getting ids (previous verts are needed to fill spaces between segments)
            int prevVert1Id = addedVerts - 1;
            int prevVert2Id = addedVerts - 2;
            int currSideVert1Id = addedVerts;
            int currSideVert2Id = addedVerts + 1;
            int nextSideVert1Id = addedVerts + 2;
            int nextSideVert2Id = addedVerts + 3;

            //Adding created verts to the mesh and the count
            vh.AddVert(currSideVertex1);
            vh.AddVert(currSideVertex2);
            vh.AddVert(nextSideVertex1);
            vh.AddVert(nextSideVertex2);


            //Adding the triangles for the segment
            //vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(currSideVert1Id, currSideVert2Id, nextSideVert1Id);
            vh.AddTriangle(currSideVert2Id, nextSideVert1Id, nextSideVert2Id);

            if (addedVerts > 4)
            {
                //Adding the triangles for the gap with the previous segment
                vh.AddTriangle(currSideVert1Id, currSideVert2Id, prevVert1Id);
                vh.AddTriangle(currSideVert1Id, currSideVert2Id, prevVert2Id);
                vh.AddTriangle(prevVert1Id, prevVert2Id, currSideVert1Id);
                vh.AddTriangle(prevVert1Id, prevVert2Id, currSideVert2Id);

            }

            addedVerts += 4;
        }
    }
}