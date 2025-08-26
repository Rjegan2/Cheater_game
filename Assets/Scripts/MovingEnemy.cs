using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
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
        MoveEnemy();
    }

    public void MoveEnemy()
    {
        gameObject.transform.position = Vector2.MoveTowards(transform.position, currentEndPoint.transform.position, xSpeed * Time.deltaTime);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "EnemyMoveCollider1")
        {
            currentEndPoint = endPoint2;
            GetComponent<SpriteRenderer>().flipX = false;

        }
        else if (collision.transform.name == "EnemyMoveCollider2")
        {
            currentEndPoint = endPoint1;
            GetComponent<SpriteRenderer>().flipX = true;
        }

       
    }
}
