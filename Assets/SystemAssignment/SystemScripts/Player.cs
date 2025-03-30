using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    void Start()
    {
        
    }


    void Update()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Get mouse position.
        mouse.z = 0; //I don't want z value in my 2D game.
        Vector2 direction = mouse - transform.position; //use target point minus current gameObject's point, I get a direction vector.
        transform.up = direction; //Then I can rotate.
    }
}
