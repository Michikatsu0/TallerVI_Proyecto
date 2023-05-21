using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class WeaponResponse : MonoBehaviour
{
    public class Bullet
    {
        public int bounce;
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    [SerializeField] private WeaponSettings weaponSettings;
    [SerializeField] private ParticleSystem[] muzzleEffects;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private Transform raycastOrigin;

    public string weaponName;
    private float accumulatedTime, fireInterval;
    private Ray ray;
    private RaycastHit hit;
    [HideInInspector] public Transform raycastDestination;
    private List<Bullet> bullets = new List<Bullet>();
    private AudioSource audioSource;
    private Rigidbody rgbd;
    private Collider[] boxColliders;
    private GameObject laser;
    private bool isDeath = false;

    void Start()
    {
        rgbd = GetComponent<Rigidbody>();
        rgbd.useGravity = false;
        rgbd.isKinematic = true;
        rgbd.detectCollisions = false;
        boxColliders = GetComponents<Collider>();

        foreach (Collider collider in boxColliders)
            collider.enabled = false;
        
        foreach (var particleSystem in muzzleEffects)
        {
            ParticleSystem.MainModule ps = particleSystem.GetComponent<ParticleSystem>().main;
            ps.startColor = weaponSettings.colorMuzzle;
        }
        PlayerActionsResponse.ActionWeaponDeath += OnDeathWeapon;
        PlayerActionsResponse.ActionShootWeaponTrigger += OnFiringWeapon;
        raycastDestination = GameObject.Find("Aim_CrossHair").transform;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponSettings.isFiring)
            UpdateFiring(Time.deltaTime);
        UpdateBullets(Time.deltaTime);

        if (isDeath)
        {
            isDeath = false;
            rgbd.useGravity = true;
            rgbd.isKinematic = false;
            rgbd.detectCollisions = true;
            foreach (BoxCollider boxCollider in boxColliders)
                boxCollider.enabled = true;
        }
    }

    public void OnDeathWeapon(bool isDeath)
    {
        this.isDeath = isDeath;
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
        bullet.tracer = Instantiate(weaponSettings.tracerEffect, position, Quaternion.identity); //Pool
        bullet.tracer.material.SetColor("_EmissionColor", weaponSettings.colorMuzzle);
        bullet.tracer.AddPosition(position);
        bullet.bounce = weaponSettings.maxNumberBounces;
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
    
    void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            if (bullet.tracer != null)
            {
                Vector3 p0 = GetPosition(bullet);
                bullet.time += deltaTime;
                Vector3 p1 = GetPosition(bullet);

                RaycastSegment(p0, p1, bullet);
            }
        });
    }

    void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= weaponSettings.maxLifeTime);
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

            var ramdonHitClip = Random.Range(0, weaponSettings.weaponHitsAudioClips.Count);
            PlayAudioAtPosition.PlayClipAtPoint(weaponSettings.weaponHitsAudioClips[ramdonHitClip], hit.point, Random.Range(0.05f,0.1f));

            bullet.time = weaponSettings.maxLifeTime;
            bullet.tracer.transform.position = hit.point;
            end = hit.point;

            if (bullet.bounce > 0)
            {
                bullet.time = 0;
                bullet.initialPosition = hit.point;
                bullet.initialVelocity = Vector3.Reflect(bullet.initialVelocity, hit.normal);
                bullet.bounce--;
            }

            var rgbd = hit.collider.GetComponent<Rigidbody>();
            if (rgbd && !rgbd.isKinematic)
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    //enemy Reciver funtion script
                }
                else if (hit.collider.gameObject.CompareTag("Probs"))
                {
                    rgbd.AddForceAtPosition(ray.direction * 5, hit.point, ForceMode.Impulse);
                }
                else if (hit.collider.gameObject.CompareTag("Bridges"))
                {
                    rgbd.AddForceAtPosition(ray.direction * 2.5f, hit.point, ForceMode.Impulse);
                }
            }

            var playerHealth = hit.collider.gameObject.GetComponent<HealthResponse>();
            if (playerHealth)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    playerHealth.TakeDamage(weaponSettings.damage);
                }
            }
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

        var ramdonShootClip = Random.Range(0, weaponSettings.weaponShootAudioClips.Count);
        audioSource.PlayOneShot(weaponSettings.weaponShootAudioClips[ramdonShootClip], 0.5f);
    }

    public void OnFiringWeapon(bool isFiring)
    {
        weaponSettings.isFiring = isFiring;
    }

    private void OnDestroy()
    {
        PlayerActionsResponse.ActionShootWeaponTrigger -= OnFiringWeapon;
    }

}
