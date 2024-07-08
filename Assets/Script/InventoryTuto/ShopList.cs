using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopList : MonoBehaviour
{
    [SerializeField] private Transform slotParent;
    [SerializeField] private ShopSlot[] shopslots;//���̾��Ű�� �ִ� slot�迭 

#if UNITY_EDITOR
    private void OnValidate()
    {
        shopslots = slotParent.GetComponentsInChildren<ShopSlot>();
    }
#endif
    
}
