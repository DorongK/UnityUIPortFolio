using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Learningcurve : MonoBehaviour
{
    /// <summary>
    /// 유니티C#에서는 초기화를 안했는데 0값이 들어가있음.
    /// 메써드가 메모리 공간을 차지한다? 이건 코드영역을 가리키는 것을 얘기하는듯클?래스가 인스턴스화 되었을때 크기는 여전히 멤버변수에만 영향을 받는다.
    /// 전체 변수의 기본값의 목록은 마이크로소프트 런 에서 확인가능.
    /// 클래스내에 선언된 변수 '필드'에 선언되었다고 한다.
    /// </summary>
    public int Currentage;
    int[] arr = new int[5];//C++ int arr[5];
    int[] arr2 = { 1, 2, 3 };//또는 int[] {1,2,3}로 바로 초기화.
    List<int> listname =new List<int>() { 0, 1, 2 };//초기화방법은 위와 같음
    Dictionary<string, int> map = new Dictionary<string, int>()//이건 맵임...
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
