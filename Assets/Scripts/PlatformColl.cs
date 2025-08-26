using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformColl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
        }
    }
}
