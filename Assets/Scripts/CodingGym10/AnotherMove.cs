using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnotherMove : MonoBehaviour
{
    public float speed = 5f;
    public float t;

    public Button anotherButton;
    public Button myButton;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BButtonDown()
    {
        StartCoroutine(Mover());
    }

    public IEnumerator Mover()
    {
        t = 0;
        anotherButton.interactable = false; //Set button's interaction to false when clicking this button.
        myButton.interactable = false;

        while (t < 1)
        {
            t += Time.deltaTime;
            Vector2 move = transform.position;
            move.y += speed * Time.deltaTime;
            transform.position = move;
            yield return null;
        }

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;
            Vector2 move = transform.position;
            move.y -= speed * Time.deltaTime;
            transform.position = move;
            yield return null;
        }

        anotherButton.interactable = false; //After this coroutine finishes, set button's interaction to OK again.
        myButton.interactable = true;
    }
}
