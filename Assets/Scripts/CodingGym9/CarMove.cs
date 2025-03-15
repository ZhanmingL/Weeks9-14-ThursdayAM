using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
    public float speed = 5f;

    public bool going = true;

    void Update()
    {
        if (going)
        {
            Vector2 pos = transform.position;
            pos.x += speed * Time.deltaTime;

            Vector2 CameraPos = Camera.main.WorldToScreenPoint(pos);
            if (CameraPos.x < 0 || CameraPos.x > Screen.width)
            {
                speed *= -1;
            }

            transform.position = pos;
        }
    }
}
