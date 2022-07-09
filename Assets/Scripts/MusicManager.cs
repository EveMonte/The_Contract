using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource _audio;
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.time = 24;
        _audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
