using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstQuestion : MonoBehaviour
{
    public GameObject image;

    void Start()
    {
        image.SetActive(false);
    }

    public void MouseEnter()
    {
        image.SetActive(true);

    }

    public void MouseLeave()
    {
        image.SetActive(false);
    }
}
