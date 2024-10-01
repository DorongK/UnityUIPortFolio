using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image image;//슬롯이 띄울 이미지
    [SerializeField] EquipType _type;
    [SerializeField] private ChatManager EquipSlotLog;

    private Inventory _inventoryAtEquipslot;
    private Equipment _equipment;

    public EquipType type
    {
        get { return _type; }
        set { }
    }

    private Item _item;
    public Item item
    {
        get { return _item; }
        set
        {
            _item = value;
            if (_item != null)
            {
                image.sprite = item.itemImage;
                image.color = new Color(1, 1, 1, 1);
            }
            else
            {
                image.color = new Color(1, 1, 1, 0);
            }
        }
    }
    private void Start()
    {
        _inventoryAtEquipslot = FindObjectOfType<Inventory>();
        _equipment = FindObjectOfType<Equipment>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            _inventoryAtEquipslot.AddItem(_item);
            Debug.Log(item.itemName + " 을 해제했습니다.");
            _item = null;
            _equipment.RefreshDictionary(type);
        }
    }
}
