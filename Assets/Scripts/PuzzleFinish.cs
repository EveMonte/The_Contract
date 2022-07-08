using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleFinish : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _levers;
    [SerializeField]
    private List<GameObject> _platforms;
    private List<PlatformMotion> _motions;
    private bool _isActive = true;
    [SerializeField]
    private GameObject _virtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        _motions = new List<PlatformMotion>();
        for (int i = 0; i < _platforms.Count; i++)
        {
            _motions.Add(_platforms[i].GetComponent<PlatformMotion>());
        }
        _motions.Reverse();
    }

    // Update is called once per frame
    void Update()
    {
        if(_isActive && _levers[0].transform.rotation.y == _levers[1].transform.rotation.y && _levers[0].transform.rotation.y == _levers[2].transform.rotation.y)
        {
            StartCoroutine("StartPlatformMotion");
        }

    }

    private IEnumerator StartPlatformMotion()
    {
        _isActive = false;
        for (int i = 0; i < _motions.Count; i++)
        {
            _motions[i].isMoving = true;
            yield return new WaitForSeconds(1.5f);
        }
        Destroy(GetComponent<PuzzleFinish>());
    }


}
