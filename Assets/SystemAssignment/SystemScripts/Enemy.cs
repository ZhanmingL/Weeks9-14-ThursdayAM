using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float speed = 5f;

    public UnityEvent OnLeft; //The event that controls enemy touching screen left edge.

    public GameManager gameManager; //Reference of GameManager.

    void Start()
    {
        
    }


    void Update()
    {
        //When enemy is instantiated, it will move from right edge of screen to left edge of screen.
        Vector2 pos = transform.position;
        pos.x -= speed * Time.deltaTime;
        transform.position = pos;

        gameManager.FindBulletTouch(gameObject); //Determine bullet touching enemy.

        CheckArriveLeft();
    }

    public void CheckArriveLeft()
    {
        if(transform.position.x < Camera.main.ScreenToWorldPoint(Vector2.zero).x) //Determine the position of enemy touching screen left edge.
        {
            Destroy(gameObject); //Destroy enemy.
            OnLeft.Invoke(); //Losing HP.
        }
    }
}
