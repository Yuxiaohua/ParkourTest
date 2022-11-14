using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AduioManager : MonoBehaviour
{
    public static AduioManager instance;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }


    public void play(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>(name);
        audioSource.PlayOneShot(clip);
    }

}
