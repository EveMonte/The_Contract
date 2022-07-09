using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _force;
    public float speed = 2f;
    private bool _isReady = false;
    private List<GameObject> _planks;
    void Start()
    {
        _planks = new List<GameObject> ();
        for (int i = 0; i < transform.childCount; i++)
        {
            _planks.Add(transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ready()
    {
        _isReady = true;
    }
    public void Break(float breakForce)
    {
        if (_isReady)
        {
            Destroy(gameObject.GetComponent<Rigidbody>());
            if(gameObject.GetComponent<BoxCollider>() != null)
                Destroy(gameObject.GetComponent<BoxCollider>());
            StartCoroutine("SetIntervalBetweenPlatforms", breakForce);
            StartCoroutine("DestroyRigidBody");
        }
    }
    public IEnumerator SetIntervalBetweenPlatforms(float breakForce)
    {
        float mass = 1f;
        foreach (GameObject go in _planks)
        {
            go.AddComponent<Rigidbody>();
            //go.GetComponent<Rigidbody>().useGravity = false;
            go.GetComponent<Rigidbody>().mass = mass;
            mass += 1;
            go.AddComponent<BoxCollider>();
            go.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(0, -breakForce, -1f), _force.transform.position, ForceMode.Impulse);

            yield return new WaitForSeconds(0.1f);
        }

    }

    IEnumerator DestroyRigidBody()
    {
        yield return new WaitForSeconds(10f);
        foreach (GameObject go in _planks)
        {
            Destroy(go.GetComponent<Rigidbody>());
            Destroy(go.GetComponent<BreakPlatform>());
        }

    }
}
