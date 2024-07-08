using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public GameBehavior GameManager;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("Game_Manager").GetComponent<GameBehavior>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name =="Player")
        {
            Destroy(this.transform.gameObject);
            Debug.Log("Item Collected!");
           
        }
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
