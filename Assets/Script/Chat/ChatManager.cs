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
    [SerializeField] private ChatSlot chatSlotPrefab; // ���������� ����
    [SerializeField] private ChatSlot[] chatSlots;//���̾��Ű+ä��â�� ǥ�õ� Text��ü

    public Dictionary<ChatType,Queue<ChatSlot>> chatLog;
    public Queue<ChatSlot> dialog;
    public Queue<ChatSlot> notice;
    public TMP_InputField inputField;
    public Button sendBtn;
  
    ScrollRect scroll_rect = null; //��ũ�ѹ� ����
    private int maxChatLogCount = 20; // �ִ� ä�� �α� ����
    private int currentSlotIndex = 0; // ���� ����� ������ �ε���

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
        // ��� ������ ��Ȱ��ȭ
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
        //����� ���� ��������
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

    public void SendButtonOnClicked()//SendButton Ŭ�� �̺�Ʈ
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
        inputField.ActivateInputField(); // �޼��� ���� �� �ٷ� �޼����� �Է��� �� �ְ� ��Ŀ���� Input Field�� �ű�� ���� ���
        inputField.text = "";
    }

    IEnumerator ScrollUpdate()
    {
        yield return null;
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }
}
