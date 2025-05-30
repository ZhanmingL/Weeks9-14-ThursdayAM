using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    //Reference of two visual icons (UI images) on the frozen Button.
    public GameObject frozenCoolingImage; //Unavailable Image, red one.
    public GameObject frozenOKImage; //Available Image, green one.

    public GameObject hitImage; //A red Image that suddenly appears and disappears showing player losing HP.

    bool allowShoot = true; //Bool that determine player can either shoot or not.
    bool frozen = true; //Boolean that allow when freezing Coroutine can run.
    bool enemyOnFreezing = true; //Stop Generating enemy during frozen time.
    public bool gameEnds = false; //Use this bool to destroy all enemy prefabs.
    public bool canMove = true; //Stop enemy's position transforming.

    public float enemyT = 0; //Timer that increasing, in order to spawn an enemy.
    public float enemyTSpeed = 1; //Speed that times DeltaTime to count time and spawn enemy.
    public float timeToSpawnEnemy = 2.3f; //When enemyT reaches this value, spawn an enemy.

    public Transform shooterPos; //Reference of the triangle on spinner. I use it to get the position value.

    //My reference of UI Sliders.
    public Slider HP;
    public Slider Timer;
    public Slider Reloading;

    public TextMeshProUGUI timerText; //Record remaining time of timer.

    public List<GameObject> bulletList; //Track every bullet that spawned.

    Enemy enemy;

    Coroutine timerIsDecreasing; //Reference of Coroutine TimeCounter.

    void Start()
    {
        //Set those pages to false at the beginning.
        gameWinUI.SetActive(false);
        gameOverUI.SetActive(false);

        frozenCoolingImage.SetActive(false); //Frozen has not been used.
        frozenOKImage.SetActive(true); //Frozen is available at the beginning of game.

        hitImage.SetActive(false); //Often don't show this object. Only show when enemy touches left edge.

        //Set Slider-HP value.
        HP.maxValue = 100;
        HP.minValue = 0;
        HP.value = HP.maxValue;
        //Set Slider-Timer value.
        Timer.maxValue = 60;
        Timer.minValue = 0;
        Timer.value = HP.maxValue;
        //Set Slider-Reloading value.
        Reloading.maxValue = 0.5f;
        Reloading.minValue = 0;
        Reloading.value = Reloading.maxValue;

        timerIsDecreasing = StartCoroutine(TimerCounter()); //Run TimerCounter Coroutine when the game is started.

        timerText.text = Timer.value.ToString(); //Set text to 60 when game starts.

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

                //Old version of destroying newbullet prefab.
                //Destroy(newBullet, 3); //Destroy that new bullet in 3 seconds!

                StartCoroutine(DestroyBullet(newBullet)); //Change Destroy() to StartCoroutine() so that I can also remove from list!
                StartCoroutine(BulletCool()); //After shoot one bullet, cool for a little bit time.
            }
        }

        if (enemyOnFreezing) //If not freezing, enemy spawns frequently by counting each period of seconds.
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
        enemy.speed = Random.Range(2, 5); //Each new spawned enemy receives a random speed, it's quite more interesting.
        //I have two listeners that assign to my Event - OnLeft.
        enemy.OnLeft.AddListener(LosingHP); //Register LosingHP function to OnLeft event.
        enemy.OnLeft.AddListener(LosingHPEffect); //Effect function to same event.
        enemy.gameManager = this; //So I assign reference to enemy gameObject.
    }

    public void LosingHP()
    {
        HP.value -= 10; //Once enemy reaches left edge, player's base loses 20 HP.

        if(HP.value <= 0)
        {
            gameOverUI.SetActive(true); //GameOver when player loses all HP.
            enemyOnFreezing = true; //Stop spawning enemies when player wins.
            gameEnds = true; //Destroy all enemy prefabs.
            allowShoot = false; //Player cannot shoot when game ends.
            StopCoroutine(timerIsDecreasing); //Stop decreasing UItimer.
        }
    }
    public void LosingHPEffect()
    {
        StartCoroutine(Hitting());
    }

    IEnumerator Hitting() //Let "player is hitting" effect show 0.15 second.
    {
        float t = 0;
        hitImage.SetActive(true); //Appear hit image.
        while (t < 0.15f) //Keep 0.15 second.
        {
            t += Time.deltaTime;
            yield return null;
        }
        hitImage.SetActive(false); //Disapeear hit image.
    }

    public void FindBulletTouch(GameObject newEnemy) //I want to use this function to determine touching.
    {
        for(int i = 0; i < bulletList.Count; i++)
        { //Find all my existing bullets.
            GameObject bulletNew = bulletList[i];
            float distance = Vector2.Distance(bulletNew.transform.position, newEnemy.transform.position); //I saw "Unity�s Mathf and Vector2/Vector3 functions" in the assignment brief, so I use Vector's distance function.
            if (distance < 0.8f) //If one bullet touches one enemy 0.8 distance between bullet's pos and enemy's pos:
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

    IEnumerator DuringFrozen() //3 seconds of stop enemies and timer, then 10 seconds of waiting.
    {
        frozen = false; //Turn off this bool so that player cannot freeze again when this function is cooling.
        enemyOnFreezing = false; //Stop spawning enemies during freezing time.
        canMove = false; //Stop that enemy in screen.
        frozenCoolingImage.SetActive(true); //Frozen button is starting cooling, showing this red image.
        frozenOKImage.SetActive(false); //Frozen is not OK for now to use.
        StopCoroutine(timerIsDecreasing); //Stop decreasing UItimer.
        float t = 0; //Start timer here.
        while (t < 3) //The first 3 seconds is freezing time, stop timer and enemy.
        {
            t += Time.deltaTime;
            yield return null;
        }
        enemyOnFreezing = true; //Freezing time ends, now spawn enemies again!
        canMove = true; //Oh no! Frozen time over, enemy can move again!
        timerIsDecreasing = StartCoroutine(TimerCounter()); //Continue decreasing UItimer.
        t = 0; //Reset timer.
        while(t < 10) //The second while loop is cooling time, means player cannot freeze again during this 10 seconds.
        {
            t += Time.deltaTime;
            yield return null;
        }
        frozen = true; //Now player can freeze.
        frozenCoolingImage.SetActive(false); //Now has finished cooling, no more waiting.
        frozenOKImage.SetActive(true); //It's OK to use now, show green image.
    }

    IEnumerator TimerCounter() //Timer bar decreases.
    {
        while(Timer.value > 0) //When timer value is bigger than 0, keeping loading time.
        {
            Timer.value -= Time.deltaTime;
            timerText.text = Timer.value.ToString(); //Update timer counter remaining time.
            yield return null;
        }
        //When player experiences all the time, he/she wins and activate winning page.
        gameWinUI.SetActive(true);
        enemyOnFreezing = true; //Stop spawning enemies when player wins.
        gameEnds = true; //Destroy all enemy prefabs.
        allowShoot = false; //Cannot shoot when player wins.
    }

    IEnumerator DestroyBullet(GameObject newBullet) //Coroutine that destroys that bullet after 3 seconds, also remove from list.
    { //Why I made a Coroutine rather than Destroy(), because I also need to remove from List. This is a good way to remove both prefab and List number.
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
        Reloading.value = Reloading.minValue; //Set Reloading value to minValue when start reloading.
        float t = 0; //Count this cooling time from zero.
        while (t < 0.5f) //This period of time has 0.5 second.
        {
            t += Time.deltaTime;
            Reloading.value = t; //The length of reloading slider is increasing by t.
            yield return null;
        }
        allowShoot = true;//After 0.5 second, allow player shoot again.
    }
}
