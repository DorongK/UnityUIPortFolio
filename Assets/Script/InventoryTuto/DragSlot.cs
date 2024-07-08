using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DragSlot : MonoBehaviour
{
    static public DragSlot instance; //���콺�� �巡������ �� ���� DragSlot ��ü�ڽ�
    public Slot dragSlot;//�巡�׵�slot��ü�� ����

    [SerializeField]Image itemImage;//��� �̹���

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
