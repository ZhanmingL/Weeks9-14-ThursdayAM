using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8; //Bullet flying speed.
    void Start()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;
        Vector2 direction = mouse - transform.position;
        transform.up = direction; //It's the same codes as Player one. I just want to get the direction in the first moment when new bullet is spawned,
                                  //so I want to get direction Vector in the 1st frame which is in the Start function.
    }


    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime; //Now, based on first frame's transform.up value, I can constantly update bullet's moving by that specific direction.
    }
}
