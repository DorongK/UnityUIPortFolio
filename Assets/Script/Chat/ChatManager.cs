using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ChatType
{
    Dialog,
    Notice
}

public class ChatManager : MonoBehaviour
{
    [SerializeField] private Transform slotParent;
    [SerializeField] private ChatSlot chatSlotPrefab; // 프리팹으로 변경
    [SerializeField] private ChatSlot[] chatSlots;//하이어라키+채팅창에 표시될 Text객체

    public Dictionary<ChatType,Queue<ChatSlot>> chatLog;
    public Queue<ChatSlot> dialog;
    public Queue<ChatSlot> notice;
    public TMP_InputField inputField;
    public Button sendBtn;
  
    ScrollRect scroll_rect = null; //스크롤바 고정
    private int maxChatLogCount = 20; // 최대 채팅 로그 개수
    private int currentSlotIndex = 0; // 현재 사용할 슬롯의 인덱스

#if UNITY_EDITOR
    private void OnValidate()
    {
        chatSlots = slotParent.GetComponentsInChildren<ChatSlot>();
    }
#endif

    // Start is called before the first frame update
    void Start()
    {
        scroll_rect = FindObjectOfType<ScrollRect>();
        chatLog = new Dictionary<ChatType, Queue<ChatSlot>>();
        chatLog.Add(ChatType.Dialog, dialog);
        chatLog.Add(ChatType.Notice, notice);
        sendBtn.onClick.AddListener(SendButtonOnClicked);
        // 모든 슬롯을 비활성화
        foreach (var slot in chatSlots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !inputField.isFocused) 
            SendButtonOnClicked();
    }

    public void ReceiveMsg(string msg,ChatType type)
    {
        //사용할 슬롯 가져오기
        ChatSlot slot = chatSlots[currentSlotIndex];
        slot.gameObject.SetActive(true);
        if(type==ChatType.Dialog)
        {
            slot.SetDialogText(msg);
        }
        else
        {
            slot.SetNoticeText(msg);
        }
        slot.transform.SetSiblingIndex(slotParent.childCount - 1);
        currentSlotIndex = (currentSlotIndex + 1) % maxChatLogCount;
         
        StartCoroutine(ScrollUpdate());
    }

    public void SendButtonOnClicked()//SendButton 클릭 이벤트
    {
        if (inputField.text.Equals(""))
        {
            Debug.Log("Empty");
            return;
        }
        string msg = string.Format("{0}", inputField.text);
        ReceiveMsg(msg,ChatType.Dialog);
        InputStart();
    }

    public void InputStart()
    {
        inputField.ActivateInputField(); // 메세지 전송 후 바로 메세지를 입력할 수 있게 포커스를 Input Field로 옮기는 편의 기능
        inputField.text = "";
    }

    IEnumerator ScrollUpdate()
    {
        yield return null;
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }
}
