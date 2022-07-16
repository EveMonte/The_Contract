using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnOD : MonoBehaviour
{
    [SerializeField]
    private GameObject od;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && od != null)
            od.SendMessage("SwitchOnOD");

    }
}
