using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : InventoryBase, IInteractable
{
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private Transform boxSlotParent;
    [SerializeField] private Slot[] _boxSlots;//하이어라키->GridSlot안에 있는 slot들

    public Slot[] boxSlots
    {
        get { return _boxSlots; }
        set
        {
        }
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        boxSlots = boxSlotParent.GetComponentsInChildren<Slot>();
    }
#endif

    public override void AddItem(Item _item)
    {
        if (_item.canStack)
        {
            int leftAmount = _item.itemQuantity;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null && _item.itemName == slots[i].item.itemName)//같은 종류 확인
                {
                    int slotAmount = slots[i].itemCount + leftAmount;
                    if (slots[i].itemCount< slots[i].item.maxStackAmount)
                    {
                        if (slotAmount > _item.maxStackAmount)
                        {
                            slots[i].itemCount = _item.maxStackAmount;
                            leftAmount = slotAmount - _item.maxStackAmount;
                        }
                        else
                        {
                            slots[i].itemCount = slotAmount;
                            return;
                        }
                    }
                }//함수가 종료되지 않으면 남은 수량이 있는거임.
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    slots[i].item = _item;
                    if (leftAmount <= _item.maxStackAmount)
                    {
                        slots[i].itemCount = leftAmount;
                        return;
                    }
                    else
                    {
                        slots[i].itemCount = _item.maxStackAmount;
                        leftAmount -= _item.maxStackAmount;

                    }
                }
            }
            if(leftAmount>0)
            {
                print("슬롯이 가득 차 있습니다.");

            }
        }
        //Canstack이 아닌 아이템
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                continue;
            }
            else//비어있는 칸에 바로넣기. 만약 모든 인벤토리가 차있다면 return문을 만나지 못하고 for문이 끝나버린다.
            {
                slots[i].item = _item;
                return;
            }
        }
        print("슬롯이 가득 차 있습니다.");
    }

    public string GetInteractPrompt()
    {
        return string.Format("Open Box");
    }
    public void OnInteract()
    {
    }

}
