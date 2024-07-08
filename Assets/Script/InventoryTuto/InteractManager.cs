using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IInteractable
{
    string GetInteractPrompt();
    void OnInteract();
}

public class InteractManager : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    private GameObject curInteractGameobject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera pcamera;

    // Start is called before the first frame update
    void Start()
    {
        pcamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // 마지막으로 체크한 시간이 checkRate를 넘겼다면
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            // 화면의 정 중앙에 상호작용 가능한 물체가 있는지 확인하기

            // Raycast의 순서는 1. 발사할 Ray를 생성해준다. 2. 발사 및 충돌 여부 확인
            Ray ray = pcamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));    // 화면의 정 중앙에서 Ray를 쏘겠다.
            RaycastHit hit;

            // ray에 뭔가 충돌했다면 hit에 충돌한 오브젝트에 대한 정보가 넘어오게 된다.
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                // 부딪힌 오브젝트가 우리가 저장해놓은 상호작용이 가능한 오브젝트들인지 확인하기
                if (hit.collider.gameObject != curInteractGameobject)
                {
                    // 충돌한 물체 가져오기
                    curInteractGameobject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                // 화면의 정 중앙에 상호작용 가능한 물체가 없는 경우
                curInteractGameobject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = string.Format("<b>[E]</b> {0}", curInteractable.GetInteractPrompt());     // <b></b> : 태그, 마크다운 형식 <b>의 경우 볼드체.
    }
}