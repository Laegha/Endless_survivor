using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class UIMessageInfo
{
    [SerializeField] string _message;
    [SerializeField] int _textSize;
    [SerializeField] TMP_FontAsset _textFont;
    [SerializeField] Color _textColor;
    [SerializeField] TextAlignmentOptions _textAlignment;
    [SerializeField] float _messageYPosition;
    [SerializeField] RuntimeAnimatorController _messageAnimator;
    [SerializeField] float _messageExitDelay;
    [SerializeField] bool _drawOnTop;
    [SerializeField] AnimatorUpdateMode _anUpdateMode;
    public float MessageYPosition { get { return _messageYPosition; } }
    public RuntimeAnimatorController MessageAnimator { get { return _messageAnimator; } }
    public float MessageExitDelay { get { return _messageExitDelay; } }
    public bool DrawOnTop {  get { return _drawOnTop; } }
    public AnimatorUpdateMode AnUpdateMode {  get { return _anUpdateMode; } }
    public void ApplyToText(TextMeshProUGUI text)
    {
        text.text = _message;
        text.fontSize = _textSize;
        text.font = _textFont;
        text.color = _textColor;
        text.alignment = _textAlignment;
    }
}
