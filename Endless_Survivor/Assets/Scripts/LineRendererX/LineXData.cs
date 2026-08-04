using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LineData", menuName = "ScriptableObjects/LineData", order = 3)]
public class LineXData : ScriptableObject
{
    [SerializeField] Material _lineMaterial;
    [SerializeField] AnimationCurve _lineWidth;
    [SerializeField] int _lineRenderOffset;
    [SerializeField] CustomAnimation _lineStartPointAnimation;
    [SerializeField] CustomAnimation _lineEndPointAnimation;

    public Material LineMaterial { get { return _lineMaterial; } }
    public AnimationCurve LineWidth { get { return _lineWidth; } }
    public int LineRenderOffset {  get { return _lineRenderOffset; } }
    public CustomAnimation LineStartPointAnimation { get { return _lineStartPointAnimation; } }
    public CustomAnimation LineEndPointAnimation { get { return _lineEndPointAnimation; } }
}
