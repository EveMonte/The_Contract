using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    private Transform _start;
    [SerializeField]
    private Transform _target;
    private float _timeCount;
    public float _speed;
    [HideInInspector]
    public bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<CinemachineTransposer>().m_FollowOffset = _start.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            GetComponentInChildren<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(_start.position, _target.position, _timeCount * _speed);
            //go.transform.rotation = rot * go.transform.rotation;
            _timeCount += Time.deltaTime;
            if (_timeCount >= 1)
            {
                isMoving = false;
                _timeCount = 0f;
                SwitchStates();
            }
        }
    }

    public void SwitchStates()
    {
        var temp = _target;
        _target = _start;
        _start = temp;
    }
}
