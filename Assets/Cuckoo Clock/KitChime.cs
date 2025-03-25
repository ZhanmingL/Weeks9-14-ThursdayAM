using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitChime : MonoBehaviour
{
    public GameObject bird; // I used the same script of doing coding gym, so that there is an error in Unity, because in class job has no bird.

    float t = 0;

    private void Start()
    {
        bird.SetActive(false);
    }
    public void Chime(int hour)
    {
        Debug.Log("Chiming" + hour + "o'clock !");
    }

    public void ChimeWithoutArgument()
    {
        Debug.Log("Chiming !");
    }

    public void BirdPopsOut()
    {
        StartCoroutine(BirdRunning());
    }
    
    private IEnumerator BirdRunning()
    {
        t = 0;
        while (t < 2)
        {
            bird.SetActive(true);
            t += Time.deltaTime;
            yield return null;
        }
        bird.SetActive(false);
    }
}
