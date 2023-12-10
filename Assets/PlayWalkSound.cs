using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWalkSound : MonoBehaviour
{
    public AudioSource WalkSound;
    public void Playwalk()
    {
        WalkSound.Play();
    }
}
