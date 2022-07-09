using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMotion : MonoBehaviour
{
    [HideInInspector]
    public bool isMovingDown = true;
    public bool isMovingUp = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void MoveDown()
    {
        if (isMovingDown)
        {
            Debug.Log("down");
            if (gameObject.GetComponent<Rigidbody>() == null)
                gameObject.AddComponent<Rigidbody>();
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().AddForce(new Vector3(0, -0.5f, 0), ForceMode.Impulse);
            isMovingDown = false;
            isMovingUp = true;
            StartCoroutine("StopMotion", false);
        }
    }
    public void MoveUp()
    {
        if (isMovingUp)
        {
            Debug.Log("up");
            if (gameObject.GetComponent<Rigidbody>() == null)
                gameObject.AddComponent<Rigidbody>();
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 0.5f, 0), ForceMode.Impulse);
            isMovingUp = false;
            isMovingDown = true;
            StartCoroutine("StopMotion", true);
        }
    }

    public IEnumerator StopMotion(bool flag)
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(GetComponent<Rigidbody>());
        isMovingDown = flag;
        isMovingUp = !flag;
    }
}
