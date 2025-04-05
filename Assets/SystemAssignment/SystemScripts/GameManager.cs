using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject enemyPrefab;

    public GameObject gameOverUI;
    public GameObject gameWinUI;

    public float t = 0;

    public Transform shooterPos; //Reference of the triangle on spinner. I use it to get the position value.

    public Slider HP;
    public Slider Timer;

    public List<GameObject> bulletList;

    void Start()
    {
        //Set those pages to false at the beginning.
        gameWinUI.SetActive(false);
        gameOverUI.SetActive(false);
        //Set Slider-HP value.
        HP.maxValue = 100;
        HP.minValue = 0;
        HP.value = HP.maxValue;
        //Set Slider-Timer value.
        Timer.maxValue = 30;
        Timer.minValue = 0;
        Timer.value = HP.maxValue;
        StartCoroutine(TimerCounter()); //Run TimerCounter Coroutine when the game is started.

        bulletList = new List<GameObject>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { //When player clicks left mouse button, a bullet will spawn on the triangle shooter's position and flying.
            GameObject newBullet = Instantiate(bulletPrefab, shooterPos.transform.position, Quaternion.identity);

            bulletList.Add(newBullet); //Add spawned new bullets to my List in order to determine position between enemies and bullets.

            //Destroy(newBullet, 3); //Destroy that new bullet in 3 seconds!
            StartCoroutine(DestroyBullet(newBullet)); //Change Destroy() to StartCoroutine() so that I can also remove from list!
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
        //This position judgement is so hard for me to understand ( screen or world ).
        Vector2 rightEdge = new Vector2(Screen.width, Random.Range(0, Screen.height * 3/4));
        Vector2 spawnPos = Camera.main.ScreenToWorldPoint(rightEdge);
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        Enemy enemy = newEnemy.GetComponent<Enemy>(); //Due to there is new enemy spawned, I have to get component in order to access Event.
        enemy.OnLeft.AddListener(LosingHP); //Register LosingHP function to OnLeft event.
    }

    public void LosingHP()
    {
        HP.value -= 20; //Once enemy reaches left edge, player's base loses 20 HP.

        if(HP.value <= 0)
        {
            gameOverUI.SetActive(true);
        }
    }

    IEnumerator TimerCounter()
    {
        while(Timer.value > 0) //When timer value is bigger than 0, keeping loading time.
        {
            Timer.value -= Time.deltaTime;
            yield return null;
        }
        //When player experiences all the time, he/she wins and activate winning page.
        gameWinUI.SetActive(true);
    }

    IEnumerator DestroyBullet(GameObject newBullet) //Coroutine that destroys that bullet after 3 seconds, also remove from list.
    {
        float time = 0;
        while (time < 3)
        {
            time += Time.deltaTime;
            yield return null;
        }
        bulletList.Remove(newBullet); //Remove from list.
        Destroy(newBullet); //Destroy that bullet.
    }
}
