using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlatform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _force;
    public float speed = 2f;
    private List<GameObject> _planks;
    void Start()
    {
        _planks = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            _planks.Add(transform.GetChild(i).gameObject);
        }
    }

    public void Break(float breakForce)
    {
        Destroy(gameObject.GetComponent<Rigidbody>());
        if (gameObject.GetComponent<BoxCollider>() != null)
            gameObject.GetComponent<BoxCollider>().enabled = false;
        float mass = 1f;
        foreach (GameObject go in _planks)
        {
            if(go.GetComponent<Rigidbody>() == null)
                go.AddComponent<Rigidbody>();
            go.GetComponent<Rigidbody>().mass = mass;
            mass += 1;
            go.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(0, -breakForce*3, 1f), _force.transform.position, ForceMode.Impulse);
        }
        StartCoroutine("DestroyRigidBody");

    }

    IEnumerator DestroyRigidBody()
    {
        yield return new WaitForSeconds(10f);
        foreach (GameObject go in _planks)
        {
            Destroy(go.GetComponent<Rigidbody>());
            Destroy(go.GetComponent<FakePlatform>());
        }

    }

}
