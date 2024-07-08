using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopList : MonoBehaviour
{
    [SerializeField] private Transform slotParent;
    [SerializeField] private ShopSlot[] shopslots;//하이어라키에 있는 slot배열 

#if UNITY_EDITOR
    private void OnValidate()
    {
        shopslots = slotParent.GetComponentsInChildren<ShopSlot>();
    }
#endif
    
}
