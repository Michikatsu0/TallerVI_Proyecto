using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    Rigidbody rb;

    float destroyTime;

    public int damage = 1;

    [SerializeField] float bulletSpeed;
    [SerializeField] Vector2 bulletDirection;
    [SerializeField] float destroyDelay;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime < 0)
        {
            Destroy(gameObject);
        }
    }
    public void SetBulletSpeed(float speed)
    {
        this.bulletSpeed = speed;
    }
    public void SetBulletDirection(Vector2 direction)
    {
        this.bulletDirection = direction;
    }

    public void SetDamageValue(int damage)
    {
        this.damage = damage;
    }
    public void SetBulletDelay(float delay)
    {
        this.destroyDelay = delay;
    }

    public void Shoot()
    {
        rb.velocity = bulletDirection * bulletSpeed;
        destroyTime = destroyDelay;
    }

    void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
