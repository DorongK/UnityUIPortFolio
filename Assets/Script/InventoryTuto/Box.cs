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

    public void IndexingSlot()//슬롯업데이트, list의 크기만큼 slot에 item정보를 넣어준다.//이거 수정해야됨.
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
        print("슬롯이 가득 차 있습니다.");
        _playerInventory.AddItem(_item, count);
    }

    public void AddStackItemToSameIDSlot(Item _item, int count)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null && _item.itemName == slots[i].item.itemName && slots[i].itemCount < slots[i].item.maxStackAmount)//같은 종류 확인
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
        print("같은 ID의 아이템이 없습니다.");
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
            else//비어있는 칸에 바로넣기. 만약 모든 인벤토리가 차있다면 return문을 만나지 못하고 for문이 끝나버린다.
            {
                slots[i].item = _item;
                slots[i].itemCount = 1;
                slots[i].position = ItemPosition.Box;
                return;
            }
        }
        print("슬롯이 가득 차 있습니다.");
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
