using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField] private GameObject endPoint1;
    [SerializeField] private GameObject endPoint2;
    private GameObject currentEndPoint;
    [SerializeField] private float xSpeed;

    // Start is called before the first frame update
    void Start()
    {
        currentEndPoint = endPoint1;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
        if(Vector2.Distance(gameObject.transform.position, currentEndPoint.transform.position ) < 4.3f)
        {
            if( currentEndPoint == endPoint1) 
            {
                currentEndPoint = endPoint2;
            }
            else if(currentEndPoint == endPoint2)
            {
                currentEndPoint = endPoint1;
            }
        }
       
    }

    public void MovePlatform()
    {
        gameObject.transform.position = Vector2.MoveTowards(transform.position, currentEndPoint.transform.position, xSpeed * Time.deltaTime) ;
    }

    
}
