using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectShard : MonoBehaviour
{
    private int shardCount = 0;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        //SendMessage to the UI//
        shardCount++;
    }
}
