using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    public float speed = 5f;
    public float t;

    public Button myButton;
    public Button anotherButton;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AButtonDown()
    {
        StartCoroutine(Mover());
    }

    public IEnumerator Mover()
    {
        t = 0;
        myButton.interactable = false; //Set button's interaction to false when clicking this button.
        anotherButton.interactable = false;

        while (t < 1)
        {
            t += Time.deltaTime;
            Vector2 move = transform.position;
            move.y -= speed * Time.deltaTime;
            transform.position = move;
            yield return null;
        }

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;
            Vector2 move = transform.position;
            move.y += speed * Time.deltaTime;
            transform.position = move;
            yield return null;
        }

        myButton.interactable = false; //After this coroutine finishes, set button's interaction to OK again.
        anotherButton.interactable = true;
    }
}
