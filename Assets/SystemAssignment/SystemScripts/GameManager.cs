using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform shooterPos; //Reference of the triangle on spinner. I use it to get the position value.

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { //When player clicks left mouse button, a bullet will spawn on the triangle shooter's position and flying.
            GameObject newBullet = Instantiate(bulletPrefab, shooterPos.transform.position, Quaternion.identity);
            Destroy(newBullet, 3); //Destroy that new bullet in 3 seconds!
        }
    }
}
