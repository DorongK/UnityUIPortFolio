using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum ItemPosition
{
    None,
    Inventory,
    Equipment,
    Box
}

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] Image image;//������ ��� �̹���
    [SerializeField] TMP_Text textCount;
    [SerializeField] private ChatManager InventorySlotLog;//��׵� ���߿� ä�øŴ������� Log�Լ� �ݹ����� �ް� �ϸ� �ɵ�?

    public int slotidx;
    
    private ItemPosition _position;
    public ItemPosition position
    {
        get { return _position; }
        set { _position = value; }
    }

    private Inventory _inventory;
    private Equipment _equipManager;
    private Box _BoxInventory;//�̰Ÿ� ���� ��ȣ�ۿ��ϴ°����� �����Ҵ��ϸ� �ڽ� ������ ���� �ɵ�?

    private GameBehavior _gameManager;

    private int _itemCount;
    public int itemCount
    {
        get { return _itemCount; }
        set 
        { 
            _itemCount = value;
            if (_item != null)
            {
                textCount.color = new Color(1, 1, 1, 1);
                textCount.text = _itemCount.ToString();
            }
            else
            {
                textCount.color = new Color(1, 1, 1, 0);
                textCount.text = "";
            }

        }

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
                textCount.color = new Color(1, 1, 1, 0);
                textCount.text = "";
            }
        }
    }

    private void Start()
    {
        _equipManager = FindObjectOfType<Equipment>();
        if (_equipManager == null) Debug.LogWarning("EquipManager�� �ʱ�ȭ���� �ʾҽ��ϴ�.");

        _inventory = FindObjectOfType<Inventory>();
        if (_inventory == null) Debug.LogWarning("Inventory�� �ʱ�ȭ���� �ʾҽ��ϴ�.");

        _BoxInventory = FindObjectOfType<Box>();
        if (_BoxInventory == null) Debug.LogWarning("BoxInventory�� �ʱ�ȭ���� �ʾҽ��ϴ�.");

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameBehavior>();
        if (_gameManager == null) Debug.LogWarning("Game_Manager�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right&& _item != null)
        {
            if (_position == ItemPosition.Inventory)
            {
                Debug.Log("���� ��ġ Inventory");
                if (_gameManager.isActiveBox == true)
                {
                    _BoxInventory.AddItem(_item,_itemCount);
                    ClearSlot();
                }
                else
                {
                    if (_item.itemType == Item.ItemType.Equipment)//����
                    {
                        Debug.Log(item.itemName + "�� �����߽��ϴ�.");
                        _equipManager.EquipItem(_item);
                        ClearSlot();
                    }
                    else//�Һ�
                    {
                        Debug.Log(item.itemName + "�� ����߽��ϴ�.");
                        _itemCount--;
                        textCount.text = _itemCount.ToString();
                        if (_itemCount <= 0)
                        {
                            _itemCount = 0;
                            ClearSlot();
                        }
                    }
                }
            }
            else if (_position == ItemPosition.Box)
            {
                if (_inventory.FindSpace())
                {
                    _inventory.AddItem(_item,_itemCount);
                    ClearSlot();
                }
            }
            else//Equip
            {

            }
            
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_item != null)
                _inventory.SelectItem(slotidx);
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
            _inventory.SwapItems(idx1,idx2);
        }
    }

    public void ClearSlot()
    {
        if (_position == ItemPosition.Inventory)
            _inventory.RemoveAtIdx(slotidx);

        else if (_position == ItemPosition.Box)
            _BoxInventory.RemoveAtIdx(slotidx);

    }
}


