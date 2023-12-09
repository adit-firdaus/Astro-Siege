using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[RequireComponent(typeof(HealthModule), typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public static Player Instance;

    public HealthModule healthModule;

    [NonSerialized] public Rigidbody rb;

    public bool toggeled;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        healthModule = GetComponent<HealthModule>();
        rb = GetComponent<Rigidbody>();
    }

    [ButtonMethod]
    public void Invoke()
    {
        DungeonDoorLink.Instance.SetTileDoors(DungeonDoorLink.Instance.tiles[0], toggeled);
    }
}
