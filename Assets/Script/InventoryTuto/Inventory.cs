using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// 아이템 저장될 컨테이너. 게임에서의 인벤토리가 확장이 가능한지, 정렬기능이 있는지 여러가지 요인을 생각해야 할테지만
    /// 당장은 확장X 기본기능 정도만 만든다고 가정.
    /// </summary>
    [SerializeField] private Transform slotParent;
    [SerializeField] private Slot[] slots;//하이어라키->GridSlot안에 있는 slot들
    [SerializeField] private ChatManager InventoryLog;

    private const int IntrosortDepthLimitFactor = 2;//2logN의 재귀를 넘어가면 힙소트로 전환
   
    private Equipment _equipManager;

    private class ItemComparer : IComparer<Slot>
    {
        public int Compare(Slot a, Slot b)
        {
          return a.item.ItemID-b.item.ItemID;
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
    }

    public void UpdateSlot(int idx)
    {
        slots[idx].item = slots[idx].item;
    }
    public void IndexingSlot()//슬롯업데이트, list의 크기만큼 slot에 item정보를 넣어준다.//이거 수정해야됨.
    {
        for (int i=0; i < slots.Length; i++)
        {
            slots[i].slotidx = i;
        }
    }

    public void AddItem(Item _item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (_item.canStack)
            {
                if (slots[i].item != null)//아이템이 있는지
                {
                    if (_item.itemName == slots[i].item.itemName)//이름이 같은 아이템이 있다면?
                    {
                        if (slots[i].itemCount + _item.itemQuantity <= slots[i].item.maxStackAmount)
                        {
                            slots[i].itemCount += _item.itemQuantity;
                            return;
                        }
                        else
                        {
                            slots[i].itemCount = slots[i].item.maxStackAmount;
                            int tempCount = slots[i].itemCount + _item.itemQuantity - slots[i].item.maxStackAmount;
                            for (int j = 0; j < slots.Length; j++)
                            {
                                if (slots[j].item == null)
                                {
                                    slots[j].item = _item;
                                    slots[j].itemCount = tempCount;
                                    return;
                                }
                            }
                            // 남은 수량을 넣을 빈 슬롯이 없는 경우
                            Debug.Log("슬롯이 가득 차 있습니다.");
                            InventoryLog.ReceiveMsg("슬롯이 가득 차 있습니다.", ChatType.Notice);
                            ThrowItem(_item, tempCount);
                            return;
                        }
                    }
                    else//이름이 같지 않다면 
                    {
                        continue;//바로 다음 인덱스 확인
                    }
                    
                }
                else
                {
                    continue;//다음 인덱스 확인 
                }
            }
        }

        //여기까지 함수가 실행됐다면 이름이 같은 아이템을 확인 하지 못한것.아이템이 CanStack이건 아니건 가장 먼저오는 빈칸에 아이템을 넣으면 된다.
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)//빈칸이 아니라면 바로 스킵
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

    /// <summary> 빈 슬롯 없이 채우면서 itemid로 정렬하기 </summary>
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
        SortHelper.Sort(slots, 0, i-1, _itemComparer, _itemSwapper);
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
        Vector3 dropPosition = this.dropPosition.TransformPoint(dropOffset);        // 아이템 드랍 위치 설정
        GameObject droppedItem = Instantiate(item.dropPrefab, dropPosition, Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360f));        // 아이템 프리팹을 인스턴스화
        ItemObject itemObject = droppedItem.GetComponent<ItemObject>();        // 인스턴스화된 객체의 ItemObject 컴포넌트를 가져와서 수량을 설정

        if (itemObject != null)
        {
            itemObject.itemdata = item;
            itemObject.itemdata.itemQuantity = quantity;
        }
    }

    public void OnDropButton()
    {
        ThrowItem(slots[selectedIndex].item, slots[selectedIndex].item.itemQuantity);//선택된 슬롯의 index 에 있는 아이템인스턴스화
        RemoveSelectedItem();
    }

    public void RemoveAtIdx(int i)
    {
        slots[i].item = null;
    }

    public void RemoveSelectedItem()//해당 슬롯 아이템정보 삭제 및 Info창 텍스트 숨기기.
    {
        slots[selectedIndex].item = null;
        selectedItemName.color = new Color(0, 0, 0, 0);
        selectedItemDescription.color = new Color(0, 0, 0, 0);
        dropButton.SetActive(false);
        UseButton.SetActive(false);
    }

}
 
