using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Learningcurve : MonoBehaviour
{
    /// <summary>
    /// ����ƼC#������ �ʱ�ȭ�� ���ߴµ� 0���� ������.
    /// �޽�尡 �޸� ������ �����Ѵ�? �̰� �ڵ念���� ����Ű�� ���� ����ϴµ�Ŭ?������ �ν��Ͻ�ȭ �Ǿ����� ũ��� ������ ����������� ������ �޴´�.
    /// ��ü ������ �⺻���� ����� ����ũ�μ���Ʈ �� ���� Ȯ�ΰ���.
    /// Ŭ�������� ����� ���� '�ʵ�'�� ����Ǿ��ٰ� �Ѵ�.
    /// </summary>
    public int Currentage;
    int[] arr = new int[5];//C++ int arr[5];
    int[] arr2 = { 1, 2, 3 };//�Ǵ� int[] {1,2,3}�� �ٷ� �ʱ�ȭ.
    List<int> listname =new List<int>() { 0, 1, 2 };//�ʱ�ȭ����� ���� ����
    Dictionary<string, int> map = new Dictionary<string, int>()//�̰� ����...
    {
        {"firstcon",1 },
        {"Secondcon",2 }
    };
    void Start()
    {
        Debug.Log(10);
        Debug.Log(Currentage);
        Currentage = 12;
        Debug.LogFormat("Age :{0}",Currentage);
        foreach (KeyValuePair<string, int> kvp in map)
        {
            Debug.LogFormat("Ditionary-Key:{0},value:{1}", kvp.Key, kvp.Value);
        }
        cameraTransform = this.GetComponent<Transform>();
        Debug.Log(cameraTransform.localPosition);
        DirectionLight = GameObject.Find("Directional Light");
        LightTransform = DirectionLight.GetComponent<Transform>();
        Debug.Log(LightTransform.localPosition);


    }

    // Update is called once per frame
    void Update()
    {
        cameraTransform = this.GetComponent<Transform>();
    }
    public Transform cameraTransform;
    public GameObject DirectionLight;
    public Transform LightTransform;
}
