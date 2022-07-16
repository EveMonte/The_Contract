using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public void Ready()
    {
        _isReady = true;
    }
    public void Break(float breakForce)
    {
        if (_isReady)
        {
            if(gameObject.GetComponent<Rigidbody>() != null)
                Destroy(gameObject.GetComponent<Rigidbody>());
            if(gameObject.GetComponent<BoxCollider>() != null)
                gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine("SetIntervalBetweenPlatforms", breakForce);
            StartCoroutine("DestroyRigidBody");
        }
    }
    public IEnumerator SetIntervalBetweenPlatforms(float breakForce)
    {
        float mass = 1f;
        System.Random random = new System.Random();
        _planks = _planks.OrderBy(x => random.Next()).ToList();
        foreach (GameObject go in _planks)
        {
            if(go.GetComponent<Rigidbody>() == null)
                go.AddComponent<Rigidbody>();
            go.GetComponent<Rigidbody>().mass = mass;
            mass += 1;
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
