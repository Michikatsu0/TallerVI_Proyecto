using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponResponse : MonoBehaviour
{
    public class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    [SerializeField] private WeaponSettings weaponSettings;
    [SerializeField] private ParticleSystem[] muzzleEffects;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private TrailRenderer tracerEffect;

    private float accumulatedTime, fireInterval;
    private Ray ray;
    private RaycastHit hit;
    private Transform raycastDestination;
    private List<Bullet> bullets = new List<Bullet>();

    void Start()
    {
        PlayerActionsResponse.ActionShootWeaponTrigger += StopFiring;
        raycastDestination = GameObject.Find("Aim_CrossHair").transform;
    }

    // Update is called once per frame
    void Update()
    {

        foreach (var particleSystem in muzzleEffects)
        {
            ParticleSystem.MainModule ps = particleSystem.GetComponent<ParticleSystem>().main;
            ps.startColor = weaponSettings.colorMuzzle;
        }

        if (weaponSettings.isFiring)
            UpdateFiring(Time.deltaTime);
        UpdateBullets(Time.deltaTime);
    }

    Vector3 GetPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * weaponSettings.bulletDrop;
        return bullet.initialPosition + bullet.initialVelocity * bullet.time + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.material.SetColor("_EmissionColor", weaponSettings.colorMuzzle);
        bullet.tracer.AddPosition(position);

        return bullet;
    }

    public void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        fireInterval = 1.0f / weaponSettings.fireRate;
        while (accumulatedTime >= 0.0f)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= weaponSettings.maxLifeTime);
    }

    void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = (end - start).magnitude;
        
        ray.origin = start;
        ray.direction = direction;

        if (Physics.Raycast(ray, out hit, distance))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 0.1f);

            hitEffect.transform.position = hit.point;
            hitEffect.transform.forward = hit.normal;
            hitEffect.Emit(1);

            bullet.tracer.transform.position = hit.point;
            bullet.time = weaponSettings.maxLifeTime;
        }
        else
        {
            bullet.tracer.transform.position = end;
        }

    }

    private void FireBullet()
    {
        foreach (var particleSystem in muzzleEffects)
            particleSystem.Emit(1);

        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * weaponSettings.bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);

    }

    public void StopFiring(bool isFiring)
    {
        weaponSettings.isFiring = isFiring;
    }
}
