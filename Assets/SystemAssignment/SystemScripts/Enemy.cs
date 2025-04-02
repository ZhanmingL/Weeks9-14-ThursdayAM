using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;

    public UnityEvent OnLeft;

    void Start()
    {
        
    }


    void Update()
    {
        //When enemy is instantiated, it will move from right edge of screen to left edge of screen.
        Vector2 pos = transform.position;
        pos.x -= speed * Time.deltaTime;
        transform.position = pos;

        CheckArriveLeft();
    }

    public void CheckArriveLeft()
    {
        if(transform.position.x < Camera.main.ScreenToWorldPoint(Vector2.zero).x)
        {
            Destroy(gameObject); //Destroy enemy.
            OnLeft.Invoke(); //Losing HP.
        }
    }
}
