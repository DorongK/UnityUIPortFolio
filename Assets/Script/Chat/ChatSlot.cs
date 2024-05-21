using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatSlot : MonoBehaviour
{
    public TMP_Text _text;

    private void Awake()
    {
        SetDialogColor();
        _text.fontSize = 55;
    }
   
    public void SetDialogText(string str)
    {
        _text.text = "User:" + str;
        SetDialogColor();
    }
    
    public void SetNoticeText(string str)
    {
        _text.text = "[¾Ë¸²]" + str;
        SetNoticeColor();
    }

    public void SetNoticeColor()
    {
        _text.color = new Color(1, 0.5f, 0, 1);
    }

    public void SetDialogColor()
    {
        _text.color = new Color(1, 1, 0, 1);
    }
}
   
    

