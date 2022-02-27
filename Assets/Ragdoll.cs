using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public bool hit;
    void Start()
    {
        SetKinematic(true);
    }

    void Update()
    {
        if (hit)
        {
            SetKinematic(false);
            GetComponent<Animator>().enabled = false;
        }
        Debug.Log(gameObject.tag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            hit = true;
            gameObject.tag = "Untagged";
        }
    }

    void SetKinematic(bool newValue)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies)
        {
            rb.isKinematic = newValue;
        }
    }
}
