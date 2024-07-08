using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Vector3 CamOffset = new Vector3(0f, 0f, 0f);
    public Transform camareTransform;
    
    private Transform _target;
    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.Find("Player").transform;
    }
    void LateUpdate()
    {
        this.transform.position = _target.transform.TransformPoint(CamOffset);
        this.transform.LookAt(_target);
    }
}
