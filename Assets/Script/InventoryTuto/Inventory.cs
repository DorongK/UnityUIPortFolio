using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// ������ ����� �����̳�. ���ӿ����� �κ��丮�� Ȯ���� ��������, ���ı���� �ִ��� �������� ������ �����ؾ� ��������
    /// ������ Ȯ��X �⺻��� ������ ����ٰ� ����.
    /// ������ ����ٸ� ������ ����? ī�װ�->�̸�? 
    /// </summary>
    [SerializeField] private Transform slotParent;
    [SerializeField] private Slot[] slots;//���̾��Ű->GridSlot�ȿ� �ִ� slot��
    [SerializeField] private ChatManager InventoryLog;

    private Equipment _equipManager;

    public Transform dropPosition;
    public Vector3 dropOffset = new Vector3(0f, 2f, 4f);

    [Header("Selected ItemInfo")]
    public int selectedIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public GameObject dropButton;

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

    public void IndexingSlot()//���Ծ�����Ʈ, list�� ũ�⸸ŭ slot�� item������ �־��ش�.//�̰� �����ؾߵ�.
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
                if (slots[i].item != null)//�������� �ִ���
                {
                    if (_item.itemName == slots[i].item.itemName)//�̸��� ���� �������� �ִٸ�?
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
                            // ���� ������ ���� �� ������ ���� ���
                            Debug.Log("������ ���� �� �ֽ��ϴ�.");
                            InventoryLog.ReceiveMsg("������ ���� �� �ֽ��ϴ�.", ChatType.Notice);
                            ThrowItem(_item, tempCount);
                            return;
                        }
                    }
                    else//�̸��� ���� �ʴٸ� 
                    {
                        continue;//�ٷ� ���� �ε��� Ȯ��
                    }
                    
                }
                else
                {
                    continue;//���� �ε��� Ȯ�� 
                }
            }
        }

        //������� �Լ��� ����ƴٸ� �̸��� ���� �������� Ȯ�� ���� ���Ѱ�.�������� CanStack�̰� �ƴϰ� ���� �������� ��ĭ�� �������� ������ �ȴ�.
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)//��ĭ�� �ƴ϶�� �ٷ� ��ŵ
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

    public void SortItems()
    {
       //
    }

    public void SwapItems(int index1, int index2)
    {
        if (index1 < 0 || index1 >= slots.Length || index2 < 0 || index2 >= slots.Length)
        {
            Debug.Log("�߸��� �ε����Դϴ�.");
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

        dropButton.SetActive(true);
    }

    public void ThrowItem(Item item, int quantity)
    {
        // ������ ��� ��ġ ����
        Vector3 dropPosition = this.dropPosition.TransformPoint(dropOffset);

        // ������ �������� �ν��Ͻ�ȭ
        GameObject droppedItem = Instantiate(item.dropPrefab, dropPosition, Quaternion.Euler(Vector3.one * Random.value * 360f));

        // �ν��Ͻ�ȭ�� ��ü�� ItemObject ������Ʈ�� �����ͼ� ������ ����
        ItemObject itemObject = droppedItem.GetComponent<ItemObject>();
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

    public void RemoveAtIdx(int i)
    {
        slots[i].item = null;
    }

    public void RemoveSelectedItem()//�ش� ���� ���������� ���� �� Infoâ �ؽ�Ʈ �����.
    {
        slots[selectedIndex].item = null;
        selectedItemName.color = new Color(0, 0, 0, 0);
        selectedItemDescription.color = new Color(0, 0, 0, 0);
    }

}
