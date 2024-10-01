using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : InventoryBase
{
    [SerializeField] private Inventory _playerInventory;

    public GameObject MergeButton;


#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
#endif

    void Awake()
    {
        IndexingSlot();
    }

    private void Start()
    {
        _playerInventory = FindObjectOfType<Inventory>();
    }

    public void UpdateSlot(int idx)
    {
        slots[idx].item = slots[idx].item;
    }

    public void IndexingSlot()//���Ծ�����Ʈ, list�� ũ�⸸ŭ slot�� item������ �־��ش�.//�̰� �����ؾߵ�.
    {
        for (int i = 0; i < slots.Length; i++)
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
                slots[i].position = ItemPosition.Box;
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
        _playerInventory.AddItem(_item, count);
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
        AddStackItemToEmptySlot(_item, count);
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
                slots[i].position = ItemPosition.Box;
                return;
            }
        }
        print("������ ���� �� �ֽ��ϴ�.");
        _playerInventory.AddItem(_item, 1);
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
            if (_item.maxStackAmount > _item.itemQuantity)
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
    public void MergeItem()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                continue;
            else 
            {
                int idxFound=_playerInventory.FindSameIDIdx(slots[i].item.ItemID);
                if(idxFound != -1)
                {
                    AddItem(_playerInventory.ItemAtIdx(idxFound), _playerInventory.ItemCountAtIdx(idxFound));
                    _playerInventory.RemoveAtIdx(idxFound);
                }
            }
        }
        SortAll();
    }
}
