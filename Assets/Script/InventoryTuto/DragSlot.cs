using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DragSlot : MonoBehaviour
{
    static public DragSlot instance; //마우스로 드래그했을 때 나올 DragSlot 객체자신
    public Slot dragSlot;//드래그된slot객체의 정보

    [SerializeField]Image itemImage;//띄울 이미지

    void Start()
    {
       instance = this;
    }

    public void SetDragImage(Image _itemImage)
    {
        if (_itemImage != null)
        {
            itemImage.sprite = _itemImage.sprite;
            SetColor(1);
        }
    }

    public void SetColor(float _alpha)
    {
        itemImage.color= new Color(1, 1, 1, _alpha); 
    }

}
