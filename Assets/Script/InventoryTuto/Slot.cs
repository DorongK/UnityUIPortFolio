using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] Image image;//슬롯이 띄울 이미지
    [SerializeField] TMP_Text textCount;
    [SerializeField] private ChatManager InventorySlotLog;//얘네들 나중에 채팅매니저에서 Log함수 콜백으로 받게 하면 될듯?

    public int slotidx;
    public int itemCount;

    private Inventory _inventoryAtslot;
    private Equipment _equipManager;
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
                textCount.color = new Color(1, 1, 1, 1);
                itemCount = item.itemQuantity;
                textCount.text = itemCount.ToString();
            }
            else
            {
                image.color = new Color(1, 1, 1, 0);
                textCount.color = new Color(1, 1, 1, 0);
                textCount.text = "";
            }
        }
    }

    private void Start()
    {
        _equipManager = FindObjectOfType<Equipment>();
        _inventoryAtslot = FindObjectOfType<Inventory>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_item != null)
            {
                if (_item.itemType == Item.ItemType.Equipment)
                {
                    //장착
                    Debug.Log(item.itemName + "을 장착했습니다.");
                    InventorySlotLog.ReceiveMsg(item.itemName + "을 장착했습니다.", ChatType.Notice);

                    _equipManager.EquipItem(_item);
                    ClearSlot();

                }
                else
                {
                    //소비
                    Debug.Log(item.itemName + "을 사용했습니다.");
                    InventorySlotLog.ReceiveMsg(item.itemName + "을 사용했습니다.", ChatType.Notice);

                    itemCount--;
                    textCount.text = itemCount.ToString();

                    if (itemCount <= 0)
                    {
                        itemCount = 0;
                        ClearSlot();
                    }
                }
            }
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_item != null)
                _inventoryAtslot.SelectItem(slotidx);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;//드래그한 슬롯의 원본이 이 slot임을 넣어준다.
            DragSlot.instance.SetDragImage(image);//image복사해가!
            DragSlot.instance.transform.position = eventData.position;

        }
    }

    // 마우스 드래그 중일 때 계속 발생하는 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    // 마우스 드래그가 끝났을 때 발생하는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    // 해당 슬롯에 무언가가 마우스 드롭 됐을 때 발생하는 이벤트
    public void OnDrop(PointerEventData eventData)//이건 드래그가 끝난 위치가 slot이라면,끝난위치에 있는 슬롯에서 발생.
    {
        if (DragSlot.instance.dragSlot != null)
        {
            int idx1 = DragSlot.instance.dragSlot.slotidx;//처음 드래그한 슬롯의 원본 인덱스
            int idx2 = slotidx;//드래그가 끝난 위치에 있던 slot의 index를 받아옴
            _inventoryAtslot.SwapItems(idx1,idx2);
        }

        //ChangeSlot();
    }

    public void ClearSlot()
    {
        _inventoryAtslot.RemoveAtIdx(slotidx);
    }
}


//private void ChangeSlot()
//{
//    Item _tempItem = item;//get한정자 실행예상
//    item = DragSlot.instance.dragSlot.item;//set 한정자 실행예상//해당슬롯의 item을 드래그 해온 아이템으로 교체

//    if (_tempItem != null)//원래 슬롯이 비어있지 않았다면 
//    {
//        DragSlot.instance.dragSlot.item = _tempItem;
//        DragSlot.instance.SetColor(1);
//    }
//    else
//    {
//        DragSlot.instance.dragSlot.ClearSlot();
//    }
//}