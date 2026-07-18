using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDisplayer : MonoBehaviour
{
    [SerializeField] UIMessageInfo _messageInfo;

    public void DisplayMessage()
    {
        GameUIManager.uiManager.DisplayUIMessage(_messageInfo);

    }
}
