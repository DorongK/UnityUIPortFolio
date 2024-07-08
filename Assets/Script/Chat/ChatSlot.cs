using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatSlot : MonoBehaviour
{
    public TMP_Text _text;

    private void Awake()
    {
        _text.color = new Color(0, 0, 0, 0);
        _text.fontSize = 20;
    }
   
    public void SetNormalText(string str)
    {
        _text.text = "User:" + str;
        _text.color = new Color(1, 1, 1, 1);
    }

    public void SetNoticeText(string str)
    {
        _text.text = "[¾Ë¸²]" + str;
        _text.color = new Color(1, 1, 0, 1);
    }

   public void SetPartyText(string str)
    {
        _text.text = "User:" + str;
        _text.color = new Color(0, 0.6f, 1, 1);
    }
}
   
    

