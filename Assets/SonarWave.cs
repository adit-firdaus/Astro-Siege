using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarWave : MonoBehaviour
{

    public float scaleFactor = 0.1f;
    private float speed = 25f;
    private Rigidbody rb;
    public AudioSource As;
    public AudioClip clips;
    private bool shooted;
    private bool charging = true;
    private Transform Player;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        if (charging)
        {
            Vector3 dir = new Vector3(Player.transform.position.x, 2, Player.transform.position.z);
            transform.LookAt(dir);
            Vector3 currentScale = transform.localScale;
            currentScale += new Vector3(scaleFactor, scaleFactor, scaleFactor) * Time.deltaTime;
            transform.localScale = currentScale;
            transform.position = transform.parent.position;
        }
        else
        {
            rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
            if (shooted == false)
            {
                As.PlayOneShot(clips);
                shooted = true;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Triggered");
            GameObject.FindObjectOfType<Flashbang>().Flashbanged();
            Destroy(gameObject);
        }

    }
    public void release()
    {
        charging = false;
        transform.parent = null;
        Invoke("DestroyMe", 7f);
    }
    public void DestroyMe()
    {
        Destroy(gameObject);

    }
}
