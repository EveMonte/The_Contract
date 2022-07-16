using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    private Vector3 spawnPoint;
    private int _lifeCount = 3;
    private CharacterController characterController;
    private bool _isFirst = true;
    private void Start()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        spawnPoint = transform.position;
        characterController = GetComponent<CharacterController>();

        _lifeCount = PlayerPrefs.GetInt("Lifes");
        if (_lifeCount == 0 || _lifeCount == 3)
        {
            _lifeCount = 3;
        }
        else
        {
            string position = PlayerPrefs.GetString("StartPosition");
            if (position.StartsWith("(") && position.EndsWith(")"))
            {
                position = position.Substring(1, position.Length - 2);
            }
            position = position.Replace("(", "").Replace(")", "").Replace(", ", ",");//Replace "(" and ")" in the string with ""
            characterController.enabled = false;
            string[] sArray = position.Split(',');
            Debug.Log(sArray[0] + sArray[1] + sArray[2]);
            // store as a Vector3
            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));
            transform.position =  result;
            characterController.enabled = true;

        }
        Debug.Log(_lifeCount);

    }
    private void ChangeRespawnPoint(Vector3 newSpawnpoint)
    {
        spawnPoint = newSpawnpoint;
        _isFirst = false;
    }

    private void MoveToSpawnPoint()
    {
        if (_lifeCount > 0)
        {
            _lifeCount--;
            Debug.Log(_lifeCount);

            PlayerPrefs.SetInt("Lifes", _lifeCount);
            PlayerPrefs.SetString("StartPosition", spawnPoint.ToString());
            SceneManager.LoadScene("MainScene");
        }
        if(_lifeCount == 0)
        {
            Debug.Log(_lifeCount);

            PlayerPrefs.SetInt("Lifes", 3);
            SceneManager.LoadScene("MainScene");

        }

    }
}
