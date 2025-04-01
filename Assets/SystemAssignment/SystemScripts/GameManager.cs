using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject enemyPrefab;

    public float t = 0;

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
        t += Time.deltaTime;
        if(t > 3)
        {
            t = 0;
            enemySpawn();
        }
    }

    void enemySpawn()
    {
        //Spawn issues, I will fix later.
        Vector2 enemyStartPos = new Vector2(Screen.width, Random.Range(0, 3));
        Vector2 cameraPos = Camera.main.WorldToScreenPoint(enemyStartPos);
        GameObject newEnemy = Instantiate(enemyPrefab, cameraPos, Quaternion.identity);
    }
}
