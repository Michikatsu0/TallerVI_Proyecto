using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponResponse : MonoBehaviour
{
    [SerializeField] private WeaponSettings weaponSettings;
    [SerializeField] private ParticleSystem[] muzzleEffects;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private Transform raycastOrigin;

    private float accumulatedTime;
    private bool isFiring;
    private Ray ray;
    private RaycastHit hit;
    private Transform raycastDestination;
    void Start()
    {
        raycastDestination = GameObject.Find("Aim_CrossHair").transform;
        foreach (var particleSystem in muzzleEffects)
        {
            ParticleSystem.MainModule ps = particleSystem.GetComponent<ParticleSystem>().main;
            ps.startColor = weaponSettings.colorMuzzle;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFiring)
            UpdateFiring(Time.deltaTime);
    }

    public void StartFiring() 
    {
        isFiring = true;
        accumulatedTime = 0;



    }


    public void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / weaponSettings.fireRate;
        while (accumulatedTime >= 0.0f)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    private void FireBullet()
    {
        foreach (var particleSystem in muzzleEffects)
            particleSystem.Emit(1);

        ray.origin = raycastOrigin.position;
        ray.direction = raycastDestination.position - raycastOrigin.position; // Ajust

        Physics.Raycast(ray, out hit);
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 0.1f);

            hitEffect.transform.position = hit.point;
            hitEffect.transform.forward = hit.normal;
            hitEffect.Emit(1);
        }
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
