using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class Inventory : InventoryBase
{
    [SerializeField] private ChatManager InventoryLog;

    private const int IntrosortDepthLimitFactor = 2;//2logN�� ��͸� �Ѿ�� ����Ʈ�� ��ȯ
    private Equipment _equipManager;
    private Box _boxInventory;
    public Transform dropPosition;
    public Vector3 dropOffset = new Vector3(0f, 2f, 4f);

    [Header("Selected ItemInfo")]
    public int selectedIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public GameObject UseButton;
    public GameObject dropButton;
    public GameObject SortButton;
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
#endif
    
    void Awake()
    {
        IndexingSlot();
        dropPosition = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Start()
    {
        _equipManager = FindObjectOfType<Equipment>();
        _boxInventory= FindObjectOfType<Box>();
    }

    public void UpdateSlot(int idx)
    {
        slots[idx].item = slots[idx].item;
    }

    public void IndexingSlot()//���Ծ�����Ʈ, list�� ũ�⸸ŭ slot�� item������ �־��ش�.//�̰� �����ؾߵ�.
    {
        for (int i=0; i < slots.Length; i++)
        {
            slots[i].slotidx = i;
        }
    }
    public void AddStackItemToEmptySlot(Item _item,int count)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                if (count <= 0)
                    return;

                slots[i].item = _item;
                slots[i].position = ItemPosition.Inventory;
                if (count > _item.maxStackAmount)
                {
                    slots[i].itemCount = _item.maxStackAmount;
                    count -= _item.maxStackAmount;
                    continue;
                }
                else
                {
                   
                    slots[i].itemCount = count;
                    count = 0;
                    return;
                }
            }
        }
        print("������ ���� �� �ֽ��ϴ�.");
        ThrowItem(_item, count);
    }

    public void AddStackItemToSameIDSlot(Item _item, int count)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null && _item.itemName == slots[i].item.itemName && slots[i].itemCount < slots[i].item.maxStackAmount)//���� ���� Ȯ��
            {
                int slotAmount = slots[i].itemCount + count;
                if (slotAmount > _item.maxStackAmount)
                {
                    slots[i].itemCount = _item.maxStackAmount;
                    count = slotAmount - _item.maxStackAmount;
                }
                else
                {
                    slots[i].itemCount = slotAmount;
                    count = 0;
                    return;
                }

            }
        }
        print("���� ID�� �������� �����ϴ�.");
        AddStackItemToEmptySlot(_item,count);
    }

    public void AddItemToEmptySlot(Item _item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                continue;
            }
            else//����ִ� ĭ�� �ٷγֱ�. ���� ��� �κ��丮�� ���ִٸ� return���� ������ ���ϰ� for���� ����������.
            {
                slots[i].item = _item;
                slots[i].itemCount = 1;
                slots[i].position = ItemPosition.Inventory;
                return;
            }
        }
        print("������ ���� �� �ֽ��ϴ�.");
        ThrowItem(_item, 1);
    }
    public override void AddItem(Item _item, int count)
    {
        if (_item.canStack)
        {
            if (_item.maxStackAmount < count)
            {
                AddStackItemToEmptySlot(_item, count);
            }
            else
            {
                AddStackItemToSameIDSlot(_item, count);
            }
            return;
        }
        AddItemToEmptySlot(_item);

    }

    public override void AddItem(Item _item)
    {
        if (_item.canStack)
        {
            if (_item.maxStackAmount < _item.itemQuantity)
            {
                AddStackItemToEmptySlot(_item, _item.itemQuantity);
            }
            else
            {
                AddStackItemToSameIDSlot(_item, _item.itemQuantity);
            }
            return;
        }
        AddItemToEmptySlot(_item);

    }

    public void SelectItem(int selected)
    {
        selectedIndex = selected;
        selectedItemName.text = slots[selectedIndex].item.itemName;
        selectedItemName.color = new Color(0, 0, 0, 1);
        selectedItemDescription.text = slots[selectedIndex].item.Description;
        selectedItemDescription.color = new Color(0, 0, 0, 1);
        UseButton.SetActive(true);
        dropButton.SetActive(true);
    }

    public void ThrowItem(Item item, int quantity)
    {
        Vector3 dropPosition = this.dropPosition.TransformPoint(dropOffset);        // ������ ��� ��ġ ����
        GameObject droppedItem = Instantiate(item.dropPrefab, dropPosition, Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360f));// ������ �������� �ν��Ͻ�ȭ
        ItemObject itemObject = droppedItem.GetComponent<ItemObject>();        // �ν��Ͻ�ȭ�� ��ü�� ItemObject ������Ʈ�� �����ͼ� ������ ����
        if (itemObject != null)
        {
            itemObject.itemdata = item;
            itemObject.itemdata.itemQuantity = quantity;
        }
    }

    public int FindSameIDIdx(int boxItemID)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item!=null&& slots[i].item.ItemID==boxItemID)
            {
                return i;
            }
        }
        return -1;
    }

    public Item ItemAtIdx(int i)
    {
        if (slots[i].item != null)
            return slots[i].item;
        else
            return null;
    }
    public int ItemCountAtIdx(int i)
    {
        if (slots[i].item != null)
            return slots[i].itemCount;
        else
            return -1;
    }

    public void OnDropButton()
    {
        ThrowItem(slots[selectedIndex].item, slots[selectedIndex].item.itemQuantity);//���õ� ������ index �� �ִ� �������ν��Ͻ�ȭ
        RemoveSelectedItem();
    }

    public void RemoveSelectedItem()//�ش� ���� ���������� ���� �� Infoâ �ؽ�Ʈ �����.
    {
        slots[selectedIndex].item = null;
        selectedItemName.color = new Color(0, 0, 0, 0);
        selectedItemDescription.color = new Color(0, 0, 0, 0);
        dropButton.SetActive(false);
        UseButton.SetActive(false);
    }
}

