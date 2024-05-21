using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private Transform equipslotParent;
    [SerializeField] EquipSlot[] equipSlots;
    [SerializeField] Inventory _equipToInventory;

    private Dictionary<EquipType, EquipSlot> CurrentEquip;

#if UNITY_EDITOR
    private void OnValidate()
    {
        equipSlots = equipslotParent.GetComponentsInChildren<EquipSlot>();
    }
#endif

    void Awake()
    {
        CurrentEquip = new Dictionary<EquipType, EquipSlot>();
        InitializeSlot();
    }

    private void Start()
    {
        _equipToInventory = FindObjectOfType<Inventory>();
    }

    public void InitializeSlot()
    {
        CurrentEquip.Clear();

        foreach (EquipSlot slot in equipSlots)//EquipSlots�迭�� ������ Dictionary�� ����ȭ.
        {
            if (slot != null && !CurrentEquip.ContainsKey(slot.type)&&slot.type!= EquipType.None)
            {
                CurrentEquip.Add(slot.type, slot);
            }
        }
    }

    public void RefreshSlot(EquipType changed)//��������� �ִ� ���Կ��� ������ �����ϱ�
    {
        if (CurrentEquip.ContainsKey(changed))
        {
            foreach (EquipSlot slot in equipSlots)
            {
                if (slot.type == changed)
                {
                    slot.item =CurrentEquip[changed].item;
                    break;
                }
            }
        }
    }

    public void RefreshDictionary(EquipType changed)// EquipSlot���� Ŭ�� �̺�Ʈ�� �߻��ϴ� EquipSlot.item �� ��������� Dictionary���� �˷��ֱ� ���� RefreshSlot�� ����.
    {
        if (CurrentEquip.ContainsKey(changed))
        {
            foreach (EquipSlot slot in equipSlots)
            {
                if (slot.type == changed)
                {
                    CurrentEquip[changed].item = null;
                    break;
                }
            }
        }
    }

    public void EquipItem(Item _item)
    {
        if(_item!=null&&CurrentEquip.ContainsKey(_item.equipType))
        {
            if (CurrentEquip[_item.equipType].item != null)//���� �������� �ִٸ� �κ��丮�� ������
            {
                _equipToInventory.AddItem(CurrentEquip[_item.equipType].item);
            }
            CurrentEquip[_item.equipType].item = _item;
            RefreshSlot(_item.equipType);
        }
    }
}
