using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallingOfPhoto : MonoBehaviour
{
    [SerializeField]
    private GameObject _photo;
    [SerializeField]
    private List<GameObject> _platforms;
    [SerializeField]
    private GameObject _ui;
    //[SerializeField]
    //private GameObject _virtualCamera;

    private List<PlatformMotion> _motions;
    private bool _isRotating = false;
    private Quaternion _startRotation;
    private float _speed = 2f;
    private float _timeCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        _ui.SetActive(false);
        _motions = new List<PlatformMotion>();
        for (int i = 0; i < _platforms.Count; i++)
        {
            _motions.Add(_platforms[i].GetComponent<PlatformMotion>());
        }
        _motions.Reverse();
        _startRotation = _photo.transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        if (_isRotating)
        {
            Vector3 degrees = new Vector3(0, 0, 90f);
            Quaternion end = _startRotation * Quaternion.Euler(degrees);
            _photo.transform.rotation = Quaternion.Slerp(_startRotation, end, _timeCount * _speed);

            _timeCount += Time.deltaTime;
            if (_timeCount >= 1)
            {
                _isRotating = false;
                _timeCount = 0f;
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine("ShowText");
        Destroy(GetComponent<BoxCollider>());
    }

    private IEnumerator ShowText()
    {
        //_virtualCamera.GetComponent<CameraZoom>().isMoving = true;
        _ui.SetActive(true);
        _isRotating = true;
        //StartCoroutine("StartPlatformMotion");
        yield return new WaitForSeconds(2f);
        _ui.SetActive(false);

    }

    private IEnumerator StartPlatformMotion()
    {
        for (int i = 0; i < _motions.Count; i++)
        {
            //_motions[i].isMoving = true;
            yield return new WaitForSeconds(1.5f);
        }
        Destroy(GetComponent<FallingOfPhoto>());
    }
}
