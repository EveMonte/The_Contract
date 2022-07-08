using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMotion : MonoBehaviour
{
    [HideInInspector]
    public bool isMoving = false;
    [SerializeField]
    private float _speed = 2;
    private float timeCount = 0;
    private Vector3 _startPosition;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;

   }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(_startPosition, gameObject.GetComponent<MeshRenderer>().probeAnchor.position, timeCount * _speed);
            //go.transform.rotation = rot * go.transform.rotation;
            timeCount += Time.deltaTime;
            if (timeCount >= 1)
            {
                isMoving = false;
                timeCount = 0f;
            }
        }
    }
}
