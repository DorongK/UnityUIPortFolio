using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

//������ ���鶧�� ������ �� �������� ���� ���� Item������ ����ִ� �κ��丮 Ŭ�������� UI���� ItemName/Image/Count/ItemType ������ �Ѱܼ� ���� �и��ϴ� �͵� ������������?
public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] Image image;//������ ��� �̹���
    [SerializeField] TMP_Text textCount;
    [SerializeField] private ChatManager InventorySlotLog;//��׵� ���߿� ä�øŴ������� Log�Լ� �ݹ����� �ް� �ϸ� �ɵ�?

    public int slotidx;
    public int itemCount;

    private Inventory _inventoryAtslot;
    private Equipment _equipManager;
    private GameBehavior _gameManager;

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
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameBehavior>();

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_gameManager.isActiveBox != true)
            {
                if (_item != null)
                {
                    if (_item.itemType == Item.ItemType.Equipment)
                    {
                        //����
                        Debug.Log(item.itemName + "�� �����߽��ϴ�.");
                        //InventorySlotLog.ReceiveMsg(item.itemName + "�� �����߽��ϴ�.", ChatType.Notice);
                        _equipManager.EquipItem(_item);
                        ClearSlot();

                    }
                    else
                    {
                        //�Һ�
                        Debug.Log(item.itemName + "�� ����߽��ϴ�.");
                        //InventorySlotLog.ReceiveMsg(item.itemName + "�� ����߽��ϴ�.", ChatType.Notice);
                        itemCount--;
                        textCount.text = itemCount.ToString();
                        if (itemCount <= 0)
                        {
                            itemCount = 0;
                            ClearSlot();
                        }
                    }
                }
                else 
                { }
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
            DragSlot.instance.dragSlot = this;//�巡���� ������ ������ �� slot���� �־��ش�.
            DragSlot.instance.SetDragImage(image);//image�����ذ�!
            DragSlot.instance.transform.position = eventData.position;

        }
    }

    public void OnDrag(PointerEventData eventData)    // ���콺 �巡�� ���� �� ��� �߻��ϴ� �̺�Ʈ

    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)    // ���콺 �巡�װ� ������ �� �߻��ϴ� �̺�Ʈ

    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)//�̰� �巡�װ� ���� ��ġ�� slot�̶��,������ġ�� �ִ� ���Կ��� �߻�.
    {
        if (DragSlot.instance.dragSlot != null)
        {
            int idx1 = DragSlot.instance.dragSlot.slotidx;//ó�� �巡���� ������ ���� �ε���
            int idx2 = slotidx;//�巡�װ� ���� ��ġ�� �ִ� slot�� index�� �޾ƿ�
            _inventoryAtslot.SwapItems(idx1,idx2);
        }

    }

    public void OnUse()
    {

    }

    public void ClearSlot()
    {
        _inventoryAtslot.RemoveAtIdx(slotidx);
    }
}


