using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum ItemPosition
{
    None,
    Inventory,
    Equipment,
    Box
}

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] Image image;//슬롯이 띄울 이미지
    [SerializeField] TMP_Text textCount;
    [SerializeField] private ChatManager InventorySlotLog;//얘네들 나중에 채팅매니저에서 Log함수 콜백으로 받게 하면 될듯?

    public int slotidx;
    
    private ItemPosition _position;
    public ItemPosition position
    {
        get { return _position; }
        set { _position = value; }
    }

    private Inventory _inventory;
    private Equipment _equipManager;
    private Box _BoxInventory;//이거를 현재 상호작용하는것으로 동적할당하면 박스 여러개 만들어도 될듯?

    private GameBehavior _gameManager;

    private int _itemCount;
    public int itemCount
    {
        get { return _itemCount; }
        set 
        { 
            _itemCount = value;
            if (_item != null)
            {
                textCount.color = new Color(1, 1, 1, 1);
                textCount.text = _itemCount.ToString();
            }
            else
            {
                textCount.color = new Color(1, 1, 1, 0);
                textCount.text = "";
            }

        }

    }


    private Item _item;
    public Item item
    {
        get { return _item; }
        set
        {
            _item = value;
            if (_item != null)
            {
                image.sprite = item.itemImage;
                image.color = new Color(1, 1, 1, 1);
            }
            else
            {
                image.color = new Color(1, 1, 1, 0);
                textCount.color = new Color(1, 1, 1, 0);
                textCount.text = "";
            }
        }
    }

    private void Start()
    {
        _equipManager = FindObjectOfType<Equipment>();
        if (_equipManager == null) Debug.LogWarning("EquipManager가 초기화되지 않았습니다.");

        _inventory = FindObjectOfType<Inventory>();
        if (_inventory == null) Debug.LogWarning("Inventory가 초기화되지 않았습니다.");

        _BoxInventory = FindObjectOfType<Box>();
        if (_BoxInventory == null) Debug.LogWarning("BoxInventory가 초기화되지 않았습니다.");

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameBehavior>();
        if (_gameManager == null) Debug.LogWarning("Game_Manager가 초기화되지 않았습니다.");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right&& _item != null)
        {
            if (_position == ItemPosition.Inventory)
            {
                Debug.Log("슬롯 위치 Inventory");
                if (_gameManager.isActiveBox == true)
                {
                    _BoxInventory.AddItem(_item,_itemCount);
                    ClearSlot();
                }
                else
                {
                    if (_item.itemType == Item.ItemType.Equipment)//장착
                    {
                        Debug.Log(item.itemName + "을 장착했습니다.");
                        _equipManager.EquipItem(_item);
                        ClearSlot();
                    }
                    else//소비
                    {
                        Debug.Log(item.itemName + "을 사용했습니다.");
                        _itemCount--;
                        textCount.text = _itemCount.ToString();
                        if (_itemCount <= 0)
                        {
                            _itemCount = 0;
                            ClearSlot();
                        }
                    }
                }
            }
            else if (_position == ItemPosition.Box)
            {
                if (_inventory.FindSpace())
                {
                    _inventory.AddItem(_item,_itemCount);
                    ClearSlot();
                }
            }
            else//Equip
            {

            }
            
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_item != null)
                _inventory.SelectItem(slotidx);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;//드래그한 슬롯의 원본이 이 slot임을 넣어준다.
            DragSlot.instance.SetDragImage(image);//image복사해가!
            DragSlot.instance.transform.position = eventData.position;

        }
    }

    public void OnDrag(PointerEventData eventData)    // 마우스 드래그 중일 때 계속 발생하는 이벤트
    {
        if (item != null)
        DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)    // 마우스 드래그가 끝났을 때 발생하는 이벤트
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)//이건 드래그가 끝난 위치가 slot이라면,끝난위치에 있는 슬롯에서 발생.
    {
        if (DragSlot.instance.dragSlot != null)
        {
            int idx1 = DragSlot.instance.dragSlot.slotidx;//처음 드래그한 슬롯의 원본 인덱스
            int idx2 = slotidx;//드래그가 끝난 위치에 있던 slot의 index를 받아옴
            _inventory.SwapItems(idx1,idx2);
        }
    }

    public void ClearSlot()
    {
        if (_position == ItemPosition.Inventory)
            _inventory.RemoveAtIdx(slotidx);

        else if (_position == ItemPosition.Box)
            _BoxInventory.RemoveAtIdx(slotidx);

    }
}


