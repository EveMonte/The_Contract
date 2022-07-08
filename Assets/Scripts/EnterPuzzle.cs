using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPuzzle : MonoBehaviour
{
    [SerializeField]
    private GameObject _virtualCamera;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        _virtualCamera.GetComponent<CameraZoom>().isMoving = true;
    }
}
