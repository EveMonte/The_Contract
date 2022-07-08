using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOD : MonoBehaviour
{
    [SerializeField]
    private GameObject _currentOD;

    private void OnTriggerEnter(Collider other)
    {
        if(_currentOD != null)
        {
            _currentOD.GetComponent<MeshRenderer>().probeAnchor.gameObject.SetActive(true);
            Destroy(_currentOD);

        }
    }
}
