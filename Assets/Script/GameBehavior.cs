using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameBehavior : MonoBehaviour, IManager
{
    public int MaxItem = 1;
       public TextMeshProUGUI promptText;


    public Button WinButton;
    public Button LoseButton;
    public GameObject Inventory;
    public GameObject Shop;
    public GameObject _BoxUI;
    public GameObject _Equipment;
    public GameObject Chatting;

    private bool _isActiveInventory;
    private bool _isActiveEquipment;
    private bool _isActiveShop;
    private bool _isActiveBox;

    public bool isActiveBox
    {
        get { return _isActiveBox; }
        set { }
    }

    private string _state;

    public string State
    {
        get { return _state; }
        set { _state = value; }
    }

    public void Initialize()
    {
        _state = "Game Manager initialized";
        Debug.Log(_state);
        _isActiveInventory = true;
        _isActiveEquipment = true;
        _isActiveBox = true;
    }
       
    public void UpdateScene(string updatedText)
    {
        Time.timeScale = 0;
    }
    
    public void SetPromptText(IInteractable i)
    {
        promptText.gameObject.SetActive(true);
        promptText.text = string.Format("<b>[E]</b> {0}", i.GetInteractPrompt());     // <b></b> : 태그, 마크다운 형식 <b>의 경우 볼드체.
    }
    public void UnsetPromptText()
    {
       promptText.gameObject.SetActive(false);
    }

    public void InputInventory()
    {
        if(!_isActiveInventory)
        {
            Inventory.gameObject.SetActive(true);
            _isActiveInventory = true;
        }
        else
        {
            Inventory.gameObject.SetActive(false);
            _isActiveInventory = false;
        }
    }

    public void InputShop()
    {
        if (!_isActiveShop)
        {
            Shop.gameObject.SetActive(true);
            Chatting.gameObject.SetActive(false);
            _isActiveShop = true;
        }
        else
        {
            Shop.gameObject.SetActive(false);
            Chatting.gameObject.SetActive(true);
            _isActiveShop = false;
        }
    }

    public void OpenBox()
    {
        if (!_isActiveBox)
        {
            _BoxUI.gameObject.SetActive(true);
            InputInventory();
            _isActiveBox = true;
        }
        else
        {
            _BoxUI.gameObject.SetActive(false);
            _isActiveBox = false;
        }
    }

    public void OpenEquip()
    {
        if (!_isActiveEquipment)
        {
            _Equipment.gameObject.SetActive(true);
            _isActiveEquipment = true;
        }
        else
        {
            _Equipment.gameObject.SetActive(false);
            _isActiveEquipment = false;
        }
    }

    public void RestartScene()
    {
        Utilities.RestartLevel(0);
    }

    // Start is called before the first frame update
    void Start()
    {
         Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
