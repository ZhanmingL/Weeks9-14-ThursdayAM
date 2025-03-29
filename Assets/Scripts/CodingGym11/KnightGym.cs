using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnightGym : MonoBehaviour
{
    SpriteRenderer sr;

    Animator animator;

    public AnimationCurve curve;

    public ParticleSystem effect;

    public float speed = 2;
    public float t = 0;

    public bool canRun = true;
    public bool canJump = false;

    public AudioClip[] audioClip;
    public AudioSource audioSource;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        float direction = Input.GetAxis("Horizontal");

        sr.flipX = (direction < 0);
        animator.SetFloat("movement", Mathf.Abs(direction));

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("attack");
            canRun = false;
        }

        if (canRun == true)
        {
            transform.position += transform.right * direction * speed * Time.deltaTime;
        }

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
    }

    public void AttackHasFinished()
    {
        Debug.Log("The animation just finished!");
        canRun = true;
    }

    public void RandomFootSteps()
    {
        int i = Random.Range(0, 9);
        audioSource.PlayOneShot(audioClip[i]);
    }

    public void Jump(){
        t += Time.deltaTime;
        Vector2 jump = transform.position;
        jump.y = curve.Evaluate(t);

        if(t > 1)
        {
            canJump = false;
            t = 0;
            effect.Stop();
            effect.Emit(5);
        }

        transform.position = jump;
    }
}