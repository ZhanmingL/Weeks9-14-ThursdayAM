using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 5f;
    public float t;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonDown()
    {
        StartCoroutine(Mover());
    }

    public IEnumerator Mover()
    {
        t = 0;
        while (t < 1.5f)
        {
            t += Time.deltaTime;
            Vector2 move = transform.position;
            move.y -= speed * Time.deltaTime;
            transform.position = move;
            yield return null;
        }
    }
}
