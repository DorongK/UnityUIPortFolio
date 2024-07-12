using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryBase : MonoBehaviour
{
    [SerializeField] protected Transform slotParent;
    [SerializeField] protected Slot[] slots;//하이어라키->GridSlot안에 있는 slot들
    
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
    private static readonly ItemComparer _itemComparer = new ItemComparer();//정렬기능에 쓸 Comparer
    private static readonly ItemSwapper _itemSwapper = new ItemSwapper();

    public abstract void AddItem(Item _item);
    public void SortAll()
    {
        // 1:Trim 알고리즘 ->빈자리를 먼저 채운다.
        int i = 0;
        int j = 0;
        while (i < slots.Length)//null이라면 i는 해당 위치에서 멈춘 뒤 j도 i위치로 
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
            if (slots[j].item != null)//i는 빈자리에고정 , j는 빈자리가 아닌곳을 찾아 탐색.!= null이라면 멈추고 아래구문으로 진행
            {
                slots[i].item = slots[j].item;
                slots[j].item = null;
                i++;
            }
            j++;
        }
        Debug.Log("Trim End");

        // 2. Sort using IntroSort
        SortHelper.Sort(slots, 0, i - 1, _itemComparer, _itemSwapper);
        // 3. Update
        UpdateAllSlot();
    }

    private void UpdateAllSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].item = slots[i].item; // 강제로 setter 호출하여 UI 갱신
        }
    }
    public void SwapItems(int index1, int index2)
    {
        if (index1 < 0 || index1 >= slots.Length || index2 < 0 || index2 >= slots.Length)
        {
            Debug.Log("잘못된 인덱스입니다.");
            return;
        }
        Item tempItem = slots[index1].item;
        slots[index1].item = slots[index2].item;
        slots[index2].item = tempItem;
    }

    public void RemoveAtIdx(int i)
    {
        slots[i].item = null;
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
