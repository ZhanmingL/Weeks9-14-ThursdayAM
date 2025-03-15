using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SecondQuestion : MonoBehaviour
{
    public UnityEvent PlayChime;

    public AudioSource chime;

    public GameObject clockHand;

    public float t;
    public float eachRotTime = 3;
    public float eachHourDegree = 30f;

    void Update()
    {
        t += Time.deltaTime;
        if (t > eachRotTime)
        {
            PlayChime.Invoke();
            t = 0;
        }

        
    }

    public void PlayChimeSound()
    {
        chime.Play();
    }

    public void RotateClockHand()
    {
        clockHand.transform.rotation *= Quaternion.Euler(0, 0, -eachHourDegree);
    }
}
