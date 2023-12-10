using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class Flashbang : MonoBehaviour
{
    private AudioSource AS;
    public Volume Volum;
    public bool flashbanged;
    public Transform Cam;
    public float shakeAmount = 0f;
    public float shakeReduction = 1 / 100f;
    void Start()
    {
        AS = gameObject.GetComponent<AudioSource>();
        Cam = Camera.main.transform.parent.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
    
        if (Volum.weight > 0)
        {
            Volum.weight -= 0.15f * Time.deltaTime * 1/Volum.weight;
            Cam.transform.localPosition = Random.insideUnitSphere * shakeAmount;
            shakeAmount = Mathf.MoveTowards(shakeAmount, 0, shakeReduction);
        }

        
    }
    public void Flashbanged()
    {
        AS.Play();
        shakeAmount = 0.7f;
        Volum.weight = 1f;
        shakeAmount = 1f;
    }
}
