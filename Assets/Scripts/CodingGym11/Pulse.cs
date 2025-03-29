using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public float speed = 5;
    public float t = 0;

    public AnimationCurve curve;

    public TrailRenderer trail;

    void Update()
    {
        Vector2 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        t += Time.deltaTime;
        pos.y = curve.Evaluate(t);

        Vector2 cameraPos = Camera.main.WorldToScreenPoint(pos);
        if(cameraPos.x > Screen.width)
        {
            pos.x = -10; //Go back to left.
        }

        transform.position = pos;
    }
}