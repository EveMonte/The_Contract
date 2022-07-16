using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleFinish : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _levers;
    [SerializeField]
    private List<GameObject> _platforms;
    private List<MoveUpPlatformsAfterPuzzle> _motions;
    private bool _isActive = true;
    [SerializeField]
    private GameObject _character;
    [SerializeField]
    private Transform _newRespawn;
    // Start is called before the first frame update
    void Start()
    {
        _motions = new List<MoveUpPlatformsAfterPuzzle>();
        for (int i = 0; i < _platforms.Count; i++)
        {
            _motions.Add(_platforms[i].GetComponent<MoveUpPlatformsAfterPuzzle>());
        }
        _motions.Reverse();
    }

    // Update is called once per frame
    void Update()
    {
        if(_isActive && Quaternion.Angle(_levers[0].transform.rotation, _levers[2].transform.rotation) == 0 && Quaternion.Angle(_levers[0].transform.rotation, _levers[1].transform.rotation) == 0)
        {
            StartCoroutine("StartPlatformMotion");
            _character.SendMessage("ActivateDoubleJump");
            _character.SendMessage("ChangeRespawnPoint", _newRespawn.position);
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
