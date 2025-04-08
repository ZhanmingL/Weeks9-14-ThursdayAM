using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Reference of bullet and enemy prefabs.
    public GameObject bulletPrefab;
    public GameObject enemyPrefab;
    //Reference of game ending pages.
    public GameObject gameOverUI;
    public GameObject gameWinUI;

    bool allowShoot = true; //Bool that determine player can either shoot or not.
    bool frozen = true; //Boolean that allow when freezing Coroutine can run.
    bool enemyOnFreezing = true; //Stop Generating enemy during frozen time.

    public float enemyT = 0; //Timer that increasing, in order to spawn an enemy.
    public float enemyTSpeed = 1; //Speed that times DeltaTime to count time and spawn enemy.
    public float timeToSpawnEnemy = 3; //When enemyT reaches this value, spawn an enemy.

    public Transform shooterPos; //Reference of the triangle on spinner. I use it to get the position value.

    public Slider HP;
    public Slider Timer;

    public List<GameObject> bulletList; //Track every bullet that spawned.

    Enemy enemy;

    Coroutine timerIsDecreasing;

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
        Timer.maxValue = 60;
        Timer.minValue = 0;
        Timer.value = HP.maxValue;
        timerIsDecreasing = StartCoroutine(TimerCounter()); //Run TimerCounter Coroutine when the game is started.

        bulletList = new List<GameObject>(); //Initialize my bullet list.

        enemySpawn(); //Spawn one enemy when game starts.
    }


    void Update()
    {
        if (allowShoot) //If player can shoot, spawn bullet, add it to my List. Next, destroy that bullet and remove from List after a few seconds. Finally wait bullet cooling time.
        {
            if (Input.GetMouseButtonDown(0))
            { //When player clicks left mouse button, a bullet will spawn on the triangle shooter's position and flying.
                GameObject newBullet = Instantiate(bulletPrefab, shooterPos.transform.position, Quaternion.identity);

                bulletList.Add(newBullet); //Add spawned new bullets to my List in order to determine position between enemies and bullets.

                //Destroy(newBullet, 3); //Destroy that new bullet in 3 seconds!
                StartCoroutine(DestroyBullet(newBullet)); //Change Destroy() to StartCoroutine() so that I can also remove from list!
                StartCoroutine(BulletCool()); //After shoot one bullet, cool for a little bit time.
            }
        }

        if (enemyOnFreezing)
        {
            enemyT += enemyTSpeed * Time.deltaTime; //Timer that spawns enemies every few seconds.
            if (enemyT > timeToSpawnEnemy) //Time between each enemy spawns.
            {
                enemyT = 0; //Reset time.
                enemyTSpeed += 0.05f; //Next time to spawn an enemy faster. (Increasing difficulty)
                enemySpawn(); //Execute my function to spawn enemy.
            }
        }
    }

    void enemySpawn()
    {
        //I convert screen position to world position easily getting spawn position range.
        Vector2 rightEdge = new Vector2(Screen.width, Random.Range(Screen.height * 1/4, Screen.height * 3/4)); //times 1/4 and 3/4 is to avoid enemy spawns in the position of UI things and shooter.
        Vector2 spawnPos = Camera.main.ScreenToWorldPoint(rightEdge); //I use world position to calculate spawn position.
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity); //spawn

        enemy = newEnemy.GetComponent<Enemy>(); //Due to there is new enemy spawned, I have to get component in order to access Event. Besides, I also want to assign reference.
        enemy.OnLeft.AddListener(LosingHP); //Register LosingHP function to OnLeft event.
        enemy.gameManager = this; //So I assign reference to enemy gameObject.
    }

    public void LosingHP()
    {
        HP.value -= 20; //Once enemy reaches left edge, player's base loses 20 HP.

        if(HP.value <= 0)
        {
            gameOverUI.SetActive(true); //GameOver when player loses all HP.
        }
    }
    
    public void FindBulletTouch(GameObject newEnemy) //I want to use this function to determine collide.
    {
        for(int i = 0; i < bulletList.Count; i++)
        { //Find all my existing bullets.
            GameObject bulletNew = bulletList[i];
            float distance = Vector2.Distance(bulletNew.transform.position, newEnemy.transform.position); //I saw "Unity’s Mathf and Vector2/Vector3 functions" in the assignment brief, so I use Vector's distance function.
            if (distance < 0.5f) //If one bullet touches one enemy (0.5 distance between bullet's pos and enemy's pos:
            {
                bulletList.Remove(bulletNew); //Remove bullet prefab from list.
                //Also destroy prefabs of enemy and bullet when touching together.
                Destroy(bulletNew);
                Destroy(newEnemy);
                return; //Stop this function.
            }
        }
    }

    public void Frozen() //Freezing button uses this function.
    {
        if (frozen)
        {
            StartCoroutine(DuringFrozen()); //Freezing activate!
        }
    }

    IEnumerator DuringFrozen()
    {
        frozen = false; //Turn off this bool so that player cannot freeze again when this function is cooling.
        enemyOnFreezing = false; //Stop spawning enemies during freezing time.
        enemy.canMove = false; //Stop that enemy in screen.
        StopCoroutine(timerIsDecreasing); //Stop decreasing UItimer.
        float t = 0; //Start timer here.
        while (t < 3) //The first 3 seconds is freezing time, stop timer and enemy.
        {
            t += Time.deltaTime;
            yield return null;
        }
        enemyOnFreezing = true; //Freezing time ends, now spawn enemies again!
        timerIsDecreasing = StartCoroutine(TimerCounter()); //Continue decreasing UItimer.
        t = 0; //Reset timer.
        while(t < 10) //The second while loop is cooling time, means player cannot freeze again during this 10 seconds.
        {
            t += Time.deltaTime;
            yield return null;
        }
        frozen = true; //Now player can freeze.
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
        float time = 0; //Start counting from time(timer) = 0.
        while (time < 3)
        {
            time += Time.deltaTime;
            yield return null;
        }
        bulletList.Remove(newBullet); //Remove from list.
        Destroy(newBullet); //Destroy that bullet.
    }

    IEnumerator BulletCool()
    {
        allowShoot = false; //During this period of time, I don't allow player shoot again.
        float t = 0; //Count this cooling time from zero.
        while (t < 0.5f) //This period of time has 0.5 second.
        {
            t += Time.deltaTime;
            yield return null;
        }
        //After 0.5 second, allow player shoot again.
        allowShoot = true;
    }
}
