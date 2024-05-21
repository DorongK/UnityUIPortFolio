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

        foreach (EquipSlot slot in equipSlots)//EquipSlots배열의 정보를 Dictionary에 동기화.
        {
            if (slot != null && !CurrentEquip.ContainsKey(slot.type)&&slot.type!= EquipType.None)
            {
                CurrentEquip.Add(slot.type, slot);
            }
        }
    }

    public void RefreshSlot(EquipType changed)//변경사항이 있는 슬롯에만 정보를 갱신하기
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

    public void RefreshDictionary(EquipType changed)// EquipSlot에서 클릭 이벤트로 발생하는 EquipSlot.item 의 변경사항을 Dictionary에도 알려주기 위해 RefreshSlot과 구분.
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
            if (CurrentEquip[_item.equipType].item != null)//장비된 아이템이 있다면 인벤토리에 돌리기
            {
                _equipToInventory.AddItem(CurrentEquip[_item.equipType].item);
            }
            CurrentEquip[_item.equipType].item = _item;
            RefreshSlot(_item.equipType);
        }
    }
}
