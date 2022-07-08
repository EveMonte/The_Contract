using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    //[SerializeField]
    //private GameObject _trigger;
    [SerializeField]
    private List<GameObject> _neighbours;
    [SerializeField]
    private float _speed = 5f;
    private bool _triggerStay = false;
    private bool _isRotating;
    private List<Quaternion> _rotations;
    private int _counter;
    float timeCount = 0f;
    public float speed = 1f;
    private bool firstTime = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && _triggerStay && !_isRotating)
        {
            _rotations = new List<Quaternion>();

            _isRotating = true;
            foreach (GameObject go in _neighbours)
            {
                _rotations.Add(go.transform.rotation);
            }
        }
        if (_isRotating)
        {
            Quaternion rot = Quaternion.AngleAxis(90, Vector3.right);
            foreach (GameObject go in _neighbours)
            {
                go.transform.rotation = Quaternion.Slerp(_rotations[_counter], rot * _rotations[_counter], timeCount);
                _counter++;
            }
            timeCount += Time.deltaTime;
            if(timeCount > 1 && firstTime)
            {
                timeCount = 1;
                firstTime = false;
            }
            else if(timeCount >= 1)
            {
                _isRotating = false;
                timeCount = 0f;
                firstTime=true;

            }
        }
        _counter = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        _triggerStay = true;

    }
    private void OnTriggerExit(Collider other)
    {
        _triggerStay = false;
    }
}