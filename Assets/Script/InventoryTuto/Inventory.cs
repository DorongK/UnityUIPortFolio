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

    public void IndexingSlot()//���Ծ�����Ʈ, list�� ũ�⸸ŭ slot�� item������ �־��ش�.//�̰� �����ؾߵ�.
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
                if (slots[i].item != null && _item.itemName == slots[i].item.itemName)//���� ���� Ȯ��
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
            if (leftAmount > 0)
            {
                print("������ ���� �� �ֽ��ϴ�.");
                ThrowItem(_item, leftAmount);

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

