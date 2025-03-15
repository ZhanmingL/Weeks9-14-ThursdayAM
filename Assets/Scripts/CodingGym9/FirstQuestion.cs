using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstQuestion : MonoBehaviour
{
    public Image image;

    public GameObject imagePrefab;

    public Canvas canvas;

    public void MouseJustEnter()
    {
        image.color = Color.red;
    }

    public void MouseJustLeave()
    {
        image.color = Color.white;
    }

    public void MouseJustClick()
    {
        Vector3 spawn = Random.insideUnitCircle * 300;
        spawn.z = 0;
        spawn += canvas.transform.position;
        GameObject newImage = Instantiate(imagePrefab, spawn, Quaternion.identity);
        //newImage.transform.parent = canvas.transform;
        newImage.transform.SetParent(canvas.transform);
    }
}
