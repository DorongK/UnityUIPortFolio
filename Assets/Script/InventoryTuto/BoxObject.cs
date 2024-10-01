using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxObject : MonoBehaviour, IInteractable
{
    private GameBehavior _gameManager;
    private void Start()
    {
         _gameManager = GameObject.Find("Game_Manager").GetComponent<GameBehavior>();
    }
    public string GetInteractPrompt()
    {
        return string.Format("Open Box");
    }
    public void OnInteract()
    {
        _gameManager.OpenBox();
    }
}
