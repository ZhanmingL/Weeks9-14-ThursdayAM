using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ThirdQuestion : MonoBehaviour
{
    public Image red;
    public Image yellow;
    public Image green;

    public GameObject car;
    CarMove myCar;

    public UnityEvent GreenLight;
    public UnityEvent RedLight;

    void Start()
    {
        yellow.color = Color.yellow;

        myCar = car.GetComponent<CarMove>();
    }

    public void GreenLightInvoke()
    {
        GreenLight.Invoke();
    }
    public void RedLightInvoke()
    {
        RedLight.Invoke();
    }

    public void Red()
    {
        yellow.color = Color.black;
        red.color = Color.red;
        green.color = Color.black;

        myCar.going = false;
    }

    public void Green()
    {
        yellow.color = Color.black;
        red.color = Color.black;
        green.color = Color.green;

        myCar.going = true;
    }
}
