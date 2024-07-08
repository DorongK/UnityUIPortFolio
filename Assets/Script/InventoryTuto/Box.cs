using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform boxSlotParent;
    [SerializeField] private Slot[] boxSlots;//���̾��Ű->GridSlot�ȿ� �ִ� slot��

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
