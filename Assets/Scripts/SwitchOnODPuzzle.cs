using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnODPuzzle : MonoBehaviour
{
    [SerializeField]
    private GameObject od;
    [SerializeField]
    private GameObject _triggerToWall;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _triggerToWall.GetComponent<BoxCollider>().isTrigger = false;
            if(od != null)
            {
                od.SendMessage("SwitchOnAfterPuzzle");
            }
        }

    }
}
