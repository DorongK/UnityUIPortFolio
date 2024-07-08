using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform boxSlotParent;
    [SerializeField] private Slot[] boxSlots;//하이어라키->GridSlot안에 있는 slot들

#if UNITY_EDITOR
    private void OnValidate()
    {
        boxSlots = boxSlotParent.GetComponentsInChildren<Slot>();
    }
#endif
    public string GetInteractPrompt()
    {
        return string.Format("Open Box");
    }

    public void OnInteract()
    {

    }
}
