using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float speed;

    public UnityEvent OnLeft; //The event that controls enemy touching screen left edge. Add listener from GameManager class to lose HP.

    public GameManager gameManager; //Reference of GameManager.

    void Start()
    {
        
    }


    void Update()
    {
        //When enemy is instantiated, it will move from right edge of screen to left edge of screen.
        EnemyMovement();

        gameManager.FindBulletTouch(gameObject); //Determine bullet touching enemy.

        if (gameManager.gameEnds) //When game over, destroy all enemy prefabs.
        {
            Destroy(gameObject);
        }

        //Always check each enemy reaches left edge or not.
        CheckArriveLeft();
    }

    public void EnemyMovement()
    {
        if (gameManager.canMove) //Is freezing now?
        { //If not, player can move.
            Vector2 pos = transform.position;
            pos.x -= speed * Time.deltaTime; //From right to left.
            transform.position = pos;
        }
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
