using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollResponse : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
    private Collider[] colliders;
    private Animator animator;
    public bool inEnemy;

    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        animator = GetComponentInParent<Animator>();

        DeactiveRagdolls();
    }

    public void DeactiveRagdolls()
    {
        foreach (var rigidBody in rigidbodies)
        {
            rigidBody.isKinematic = true;
        }
        animator.enabled = true;

        if (inEnemy) return;

        foreach (var collider in colliders)
        {
            collider.isTrigger = true;
        }
    }

    public void ActivateRagdolls()
    {
        foreach (var rigidBody in rigidbodies)
        {
            rigidBody.isKinematic = false;
        }
        animator.enabled = false;

        if (inEnemy) return;

        foreach (var collider in colliders)
        {
            collider.isTrigger = false;
        }
    }
}
