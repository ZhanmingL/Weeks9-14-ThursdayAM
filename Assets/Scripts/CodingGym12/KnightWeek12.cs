using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KnightWeek12 : MonoBehaviour
{
    SpriteRenderer sr;

    Animator animator;

    public AnimationCurve curve;
    public AnimationCurve moveCurve; //Player's moving, from his position to the position of last point of line renderer.

    public ParticleSystem effect; //jumping effect.

    public float speed = 2;
    public float t = 0;
    public float time;
    public float biggerTime = 1; //Variable that makes the knight being bigger and bigger.

    public bool canRun = true;
    public bool canJump = false;

    public AudioClip[] audioClip;
    public AudioSource audioSource;

    public Tilemap tilemap;
    public Tile stone;

    public LineRenderer targetPos;
    public List<Vector2> ListOfPoints;

    Vector3 movePos; //Vector that gets position of last line renderer.

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        Vector3Int gridPos = tilemap.WorldToCell(transform.position);

        if (Input.GetMouseButtonDown(0) && tilemap.GetTile(gridPos) == stone && canRun == true)
        {
            canRun = false;

            Vector3 newTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            ListOfPoints.Add(newTarget);
            targetPos.positionCount++;
            targetPos.SetPosition(targetPos.positionCount - 1, newTarget);

            movePos = newTarget;

            StartCoroutine(LerpMove()); //player is getting bigger, and moving!
        }
       

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && !canJump)
        {
            canJump = true;
            effect.Stop();
            effect.Emit(5);
        }
        if (canJump)
        {
            Jump();
        }
        //attack
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("attack");
            canRun = false;
        }
    }

    public void AttackHasFinished()
    {
        canRun = true;
    }

    public void RandomFootSteps()
    {
        int i = Random.Range(0, 9);
        audioSource.PlayOneShot(audioClip[i]);
    }

    public void Jump()
    {
        t += Time.deltaTime;
        Vector2 jump = transform.position;
        jump.y = curve.Evaluate(t);

        if (t > 1)
        {
            canJump = false;
            t = 0;
            effect.Stop();
            effect.Emit(5);
        }

        transform.position = jump;
    }

    IEnumerator LerpMove()
    {
        time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            biggerTime += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, movePos, curve.Evaluate(time));
            transform.localScale = Vector3.one * biggerTime;
            yield return null;
        }
        canRun = true;
        time = 0;
    }
}