using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBehavior : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float RotateSpeed = 75f;
    public float JumpVelocity = 5f;
    public float DistanceToGround = 0.1f;
        
    public LayerMask GroundLayer;
    public GameObject Bullet;
    public float BulletSpeed=100f;

    private GameObject curInteractGameobject;
    private IInteractable curInteractable;
    public Animator anim;

    private GameBehavior _gameManager;
    private float _vInput;
    private float _hInput;
    private bool _isJumping;
    private bool _isShooting;
    
    private Rigidbody _rb;
    private CapsuleCollider _col;

    [SerializeField]
    private Inventory playerInventory;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameBehavior>();
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        _vInput = Input.GetAxis("Vertical") * MoveSpeed;
        anim.SetFloat("Speed", _vInput);

        _hInput = Input.GetAxis("Horizontal") * RotateSpeed;

        _isJumping |= Input.GetKeyDown(KeyCode.J);

        if(_isJumping)
        anim.SetTrigger("Jump");

        _isShooting |= Input.GetKeyDown(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.I))
             _gameManager.InputInventory();
       
        if (Input.GetKeyDown(KeyCode.T))
             _gameManager.InputShop();

        if (Input.GetKeyDown(KeyCode.Tab))
            _gameManager.InputChatting();

        Interact();
        
    //this.transform.Translate(Vector3.forward * _vInput * Time.deltaTime);
    //this.transform.Rotate(Vector3.up * _hInput * Time.deltaTime);
}
    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E)&& curInteractGameobject != null)
        {
            Debug.Log(curInteractGameobject.GetComponent<ItemObject>().itemdata.name + " In Your Bag!");  // �κ��丮 �ֱ�
            playerInventory.AddItem(curInteractGameobject.GetComponent<ItemObject>().itemdata);
            //Destroy(curInteractGameobject);
            if(curInteractGameobject!=null)
            curInteractGameobject.GetComponent<ItemObject>().OnInteract();

            _gameManager.UnsetPromptText();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name=="Enemy")
        {
        }
        if (collision.gameObject != curInteractGameobject)
        {
            // �浹�� ��ü ��������
            curInteractGameobject = collision.gameObject;
            curInteractable = collision.gameObject.GetComponent<IInteractable>();
            if(curInteractable!=null)
            _gameManager.SetPromptText(curInteractable);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        curInteractGameobject = null;
        curInteractable = null;
        _gameManager.UnsetPromptText();
    }

    //FixedUpdate�� �����Ӵ� �ѹ� ������ �ƴϹǷ� Update���� �Է��� Ȯ���ϰ� FixedUpdate���� �������� ó���� �ϴ°��� ����.
    //��� ��������, ������ٵ� ���� �ڵ�� ������ �ӵ��� ������ �����ʴ´�(��ǻ�ͼ��ɿ� ���� �ٲ��� �ʴ´�)
    void FixedUpdate()
    {
        Vector3 rotation = Vector3.up * _hInput;
        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);

        _rb.MovePosition(this.transform.position + this.transform.forward * _vInput * Time.deltaTime);
        _rb.MoveRotation(_rb.rotation * angleRot);

        if(IsGrounded()&&_isJumping)
        {
            _rb.AddForce(Vector3.up * JumpVelocity, ForceMode.Impulse);
        }
        _isJumping = false;

        if(_isShooting)
        {
            GameObject newBullet = Instantiate(Bullet, this.transform.position + new Vector3(0, 0, 1), this.transform.rotation);
            Rigidbody BulletRB = newBullet.GetComponent<Rigidbody>();
            BulletRB.velocity = this.transform.forward * BulletSpeed;
        }
        _isShooting = false;
    }
    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom, DistanceToGround, GroundLayer, QueryTriggerInteraction.Ignore);
        
        return grounded;
    }
   
}
