using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ChatType
{
    Normal,
    Notice,
    Party,
}
public enum ChatTab
{
    All,
    Party,
    Notice
}

public class ChatManager : MonoBehaviour
{
    [SerializeField] private Transform allslotParent;
    [SerializeField] private Transform partyslotParent;
    [SerializeField] private Transform noticeslotParent;

    [SerializeField] private ChatSlot[] allChatSlots;//채팅창에 표시될 Text객체 ChatTab.All
    [SerializeField] private ChatSlot[] partyChatSlots;//ChatTab.Party
    [SerializeField] private ChatSlot[] noticeChatSlots;//ChatTab.Notice
       
    public TMP_InputField inputField;
    public Button sendBtn;

    public Button allTabBtn;
    public Button partyTabBtn;
    public Button noticeTabBtn;

    public Button chatModeBtn;
    public TMP_Text chatModeText;

    ChatTab currentTab= ChatTab.All;
    ChatType currentChat= ChatType.Normal;

    ScrollRect scroll_rect = null; //스크롤바 고정울 위함.
    private int maxChatLogCount = 20; // 최대 채팅 로그 개수
    private int allTabSlotIndex = 0;
    private int partyTabSlotIndex = 0;
    private int noticeTabSlotIndex = 0;

#if UNITY_EDITOR
    private void OnValidate()
    {
        allChatSlots = allslotParent.GetComponentsInChildren<ChatSlot>();
        partyChatSlots = partyslotParent.GetComponentsInChildren<ChatSlot>();
        noticeChatSlots = noticeslotParent.GetComponentsInChildren<ChatSlot>();
    }
#endif

    void Start()
    {
        scroll_rect = FindObjectOfType<ScrollRect>();
                
        sendBtn.onClick.AddListener(SendButtonOnClicked);
        chatModeBtn.onClick.AddListener(ChangeCurrentChat);

        allTabBtn.onClick.AddListener(() => SwitchTab(ChatTab.All));
        partyTabBtn.onClick.AddListener(() => SwitchTab(ChatTab.Party));
        noticeTabBtn.onClick.AddListener(() => SwitchTab(ChatTab.Notice));

        foreach (var slot in allChatSlots)
        {
            slot.gameObject.SetActive(false);
        }
        foreach (var slot in partyChatSlots)
        {
            slot.gameObject.SetActive(false);
        }
        foreach (var slot in noticeChatSlots)
        {
            slot.gameObject.SetActive(false);
        }
        SetTabVisibility(ChatTab.Notice, false);
        SetTabVisibility(ChatTab.Party, false);


    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !inputField.isFocused) 
            SendButtonOnClicked();

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeCurrentChat();
        }
    }

    public void ReceiveMsg(string msg,ChatType type)
    {
        SetTabVisibility(ChatTab.All, false);
        SetTabVisibility(ChatTab.Party, false);
        SetTabVisibility(ChatTab.Notice, false);

        SetTabVisibility(currentTab, true);
        //사용할 슬롯 가져오기
        ChatSlot slot = allChatSlots[allTabSlotIndex];
        slot.gameObject.SetActive(true);
        slot.transform.SetSiblingIndex(allslotParent.childCount - 1);
        allTabSlotIndex = (allTabSlotIndex + 1) % maxChatLogCount;
        switch (type)
        {
            case ChatType.Normal:
                slot.SetNormalText(msg);
                break;
            case ChatType.Party:
                slot.SetPartyText(msg);
                ChatSlot partyslot = partyChatSlots[partyTabSlotIndex];
                partyslot.gameObject.SetActive(true);
                partyslot.SetPartyText(msg);
                partyslot.transform.SetSiblingIndex(partyslotParent.childCount - 1);
                partyTabSlotIndex = (partyTabSlotIndex + 1) % maxChatLogCount;
                break;
            case ChatType.Notice:
                slot.SetNoticeText(msg);
                ChatSlot noticeslot = noticeChatSlots[noticeTabSlotIndex];
                noticeslot.gameObject.SetActive(true);
                noticeslot.SetNoticeText(msg);
                noticeslot.transform.SetSiblingIndex(noticeslotParent.childCount - 1);
                noticeTabSlotIndex = (noticeTabSlotIndex + 1) % maxChatLogCount;
                break;
        }
              
        StartCoroutine(ScrollUpdate());
    }

    public void SwitchTab(ChatTab next)
    {
        if (currentTab == next)
            return;

        SetTabVisibility(currentTab, false);
        SetTabVisibility(next, true);

        NotSelectedTabBtn(allTabBtn);
        NotSelectedTabBtn(partyTabBtn);
        NotSelectedTabBtn(noticeTabBtn);

        currentTab = next;

        switch (next)
        {
            case ChatTab.All:
                SelectedTabBtn(allTabBtn);
                break;
            case ChatTab.Party:
                SelectedTabBtn(partyTabBtn);
                break;
            case ChatTab.Notice:
                SelectedTabBtn(noticeTabBtn);
                break;
        }
    }

    private void SetTabVisibility(ChatTab tab, bool isVisible)
    {
        switch (tab)
        {
            case ChatTab.All:
                foreach (var slot in allChatSlots)
                    slot.gameObject.SetActive(isVisible);
                break;
            case ChatTab.Party:
                foreach (var slot in partyChatSlots)
                    slot.gameObject.SetActive(isVisible);
                break;
            case ChatTab.Notice:
                foreach (var slot in noticeChatSlots)
                    slot.gameObject.SetActive(isVisible);
                break;
        }
    }

    public void SelectedTabBtn(Button btn)
    {

        btn.image.color = new Color(0.1f, 0.1f, 0.1f, 0.75f);

    }
    public void NotSelectedTabBtn(Button btn)
    {
        btn.image.color = new Color(0.1f, 0.1f, 0.1f, 0.4f);
    }


    public void ChangeCurrentChat()
    {
        switch(currentChat)
        {
            case ChatType.Normal:
                currentChat += 1;
                chatModeText.text = "Notice";
                chatModeText.color = new Color(1, 1, 0, 1);
                break;
            case ChatType.Notice:
                currentChat += 1;
                chatModeText.text = "Party";
                chatModeText.color = new Color(0, 0.6f, 1, 1);
                break;
            case ChatType.Party:
                currentChat = 0;
                chatModeText.text = "Normal";
                chatModeText.color = new Color(1, 1, 1, 1);
                break;
        }
    }

    public void SendButtonOnClicked()//SendButton 클릭 이벤트
    {
        if (inputField.text.Equals(""))
        {
            Debug.Log("Empty");
            return;
        }
        string msg = string.Format("{0}", inputField.text);
        ReceiveMsg(msg,currentChat);
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
