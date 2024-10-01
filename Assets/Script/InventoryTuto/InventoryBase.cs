using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryBase : MonoBehaviour
{
    [SerializeField] protected Transform slotParent;
    [SerializeField] protected Slot[] slots;//���̾��Ű->GridSlot�ȿ� �ִ� slot��

    private class ItemComparer : IComparer<Slot>
    {
        public int Compare(Slot a, Slot b)
        {
            return a.item.ItemID - b.item.ItemID;
        }
    }
    private class ItemSwapper : ISwapper<Slot>
    {
        public void Swap(Slot[] array, int i, int j)
        {
            Item temp = array[i].item;
            array[i].item = array[j].item;
            array[j].item = temp;
        }
    }
    private static readonly ItemComparer _itemComparer = new ItemComparer();//���ı�ɿ� �� Comparer
    private static readonly ItemSwapper _itemSwapper = new ItemSwapper();

    public abstract void AddItem(Item _item,int count);
    public abstract void AddItem(Item _item);
    public void SortAll()
    {
        // 1:Trim �˰��� ->���ڸ��� ���� ä���.
        int i = 0;
        int j = 0;
        while (i < slots.Length)//null�̶�� i�� �ش� ��ġ���� ���� �� j�� i��ġ�� 
        {
            if (slots[i].item != null)
            {
                i++;
                j = i;
            }
            else { break; }
        }
        while (j < slots.Length)
        {
            if (slots[j].item != null)//i�� ���ڸ������� , j�� ���ڸ��� �ƴѰ��� ã�� Ž��.!= null�̶�� ���߰� �Ʒ��������� ����
            {
                slots[i].item = slots[j].item;
                slots[i].itemCount= slots[j].itemCount;
                slots[j].item = null;
                slots[j].itemCount = 0;
                i++;
            }
            j++;
        }
        Debug.Log("Trim End");
        ///////////////////
        Dictionary<Item, int> itemCountMap = new Dictionary<Item, int>(); // ������ ID�� �� ������ �����ϴ� ��ųʸ�

        for (int k = 0; k < i; k++)
        {
            if (slots[k].item != null && slots[k].item.canStack)
            {
                Item item = slots[k].item;
                if (itemCountMap.ContainsKey(item))
                {
                    itemCountMap[item] += slots[k].itemCount;
                }
                else
                {
                    itemCountMap[item] = slots[k].itemCount;
                }
                slots[k].item = null;
                slots[k].itemCount = 0;
            }
        }

        foreach (var entry in itemCountMap)
        {
            int remainingCount = entry.Value;
            for (int k = 0; k < slots.Length && remainingCount > 0; k++)
            {
                if (slots[k].item == null)
                {
                    slots[k].item = entry.Key; //  ������ �������� 
                    if (remainingCount > slots[k].item.maxStackAmount)
                    {
                        slots[k].itemCount = slots[k].item.maxStackAmount;
                        remainingCount -= slots[k].item.maxStackAmount;
                    }
                    else
                    {
                        slots[k].itemCount = remainingCount;
                        remainingCount = 0;
                    }
                }
            }
        }


        // 2. Sort using IntroSort
        SortHelper.Sort(slots, 0, i - 1, _itemComparer, _itemSwapper);
        // 3. Update
        UpdateAllSlot();
    }
    private void UpdateAllSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].item = slots[i].item; // ������ setter ȣ���Ͽ� UI ����
            slots[i].itemCount = slots[i].itemCount;
        }
    }
    public void SwapItems(int index1, int index2)
    {
        if (index1 < 0 || index1 >= slots.Length || index2 < 0 || index2 >= slots.Length)
        {
            Debug.Log("�߸��� �ε����Դϴ�.");
            return;
        }
        Item tempItem = slots[index1].item;
        int tempCount = slots[index1].itemCount;
        slots[index1].item = slots[index2].item;
        slots[index1].itemCount = slots[index2].itemCount;

        slots[index2].item = tempItem;
        slots[index2].itemCount = tempCount;

    }

    public bool FindSpace()
    {
        bool isvalidSpace = false;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                isvalidSpace = true;
        }
        return isvalidSpace;
    }

    public void RemoveAtIdx(int i)
    {
        slots[i].item = null;
        slots[i].itemCount = 0;

    }
}
