using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : InventoryBase, IInteractable
{
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private Transform boxSlotParent;
    [SerializeField] private Slot[] _boxSlots;//���̾��Ű->GridSlot�ȿ� �ִ� slot��

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
                if (slots[i].item != null && _item.itemName == slots[i].item.itemName)//���� ���� Ȯ��
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
                }//�Լ��� ������� ������ ���� ������ �ִ°���.
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
                print("������ ���� �� �ֽ��ϴ�.");

            }
        }
        //Canstack�� �ƴ� ������
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                continue;
            }
            else//����ִ� ĭ�� �ٷγֱ�. ���� ��� �κ��丮�� ���ִٸ� return���� ������ ���ϰ� for���� ����������.
            {
                slots[i].item = _item;
                return;
            }
        }
        print("������ ���� �� �ֽ��ϴ�.");
    }

    public string GetInteractPrompt()
    {
        return string.Format("Open Box");
    }
    public void OnInteract()
    {
    }

}
