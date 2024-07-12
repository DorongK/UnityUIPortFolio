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

    private const int IntrosortDepthLimitFactor = 2;//2logN의 재귀를 넘어가면 힙소트로 전환
    private Equipment _equipManager;
   
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
                    if (slots[i].itemCount < slots[i].item.maxStackAmount)
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
            if (leftAmount > 0)
            {
                print("슬롯이 가득 차 있습니다.");
                ThrowItem(_item, leftAmount);

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
        GameObject droppedItem = Instantiate(item.dropPrefab, dropPosition, Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360f));// 아이템 프리팹을 인스턴스화
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

    public void RemoveSelectedItem()//해당 슬롯 아이템정보 삭제 및 Info창 텍스트 숨기기.
    {
        slots[selectedIndex].item = null;
        selectedItemName.color = new Color(0, 0, 0, 0);
        selectedItemDescription.color = new Color(0, 0, 0, 0);
        dropButton.SetActive(false);
        UseButton.SetActive(false);
    }
}

