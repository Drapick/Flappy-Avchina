using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clickSound;

    public void PlayClick()
    {
        // Это самый легкий метод
        source.PlayOneShot(clickSound);
    }
}
