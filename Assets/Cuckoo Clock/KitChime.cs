using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitChime : MonoBehaviour
{
    public void Chime(int hour)
    {
        Debug.Log("Chiming" + hour + "o'clock !");
    }

    public void ChimeWithoutArgument()
    {
        Debug.Log("Chiming !");
    }
}
