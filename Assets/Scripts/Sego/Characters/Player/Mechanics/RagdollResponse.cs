using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollResponse : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
    private Collider[] colliders;
    private Animator animator;

    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        animator = GetComponentInParent<Animator>();

        DeactiveRagdolls();
    }

    public void DeactiveRagdolls()
    {
        foreach (var collider in colliders)
        {
            collider.isTrigger = true;
        }
        foreach (var rigidBody in rigidbodies)
        {
            rigidBody.isKinematic = true;
        }
        animator.enabled = true;
    }

    public void ActivateRagdolls()
    {
        foreach (var collider in colliders)
        {
            collider.isTrigger = false;
        }
        foreach (var rigidBody in rigidbodies)
        {
            rigidBody.isKinematic = false;
        }
        animator.enabled = false;
    }
}
