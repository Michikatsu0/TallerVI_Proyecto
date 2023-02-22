using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    [SerializeField] int bulletDamage = 1;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] Transform gunPos;
    [SerializeField] GameObject bulletPrefab;


    float keyHorizon;
    bool keyJump;
    bool keyShot;

    public bool isGrounded;
    bool isShooting;
    bool isFacingRight;

    bool keyShotReleace;
    float shorttime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        isFacingRight = true;
    }
    private void FixedUpdate()
    {
        isGrounded = false;
        int layermask = 1 << LayerMask.NameToLayer("Ground");
    }
    private void Update()
    {
        PlayerDirectionInput();
        PlayerShootInput();
        PlayerMovement();

        keyHorizon = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector3(keyHorizon * speed, rb.velocity.y, 0);

        if (keyJump && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
        }
    }

    void PlayerDirectionInput()
    {
        keyHorizon = Input.GetAxisRaw("Horizontal");
        keyJump = Input.GetKeyDown(KeyCode.Space);
        keyShot = Input.GetKey(KeyCode.F);
    }

    void PlayerShootInput()
    {
        float shootTimeLength = 0;
        float keyShootReleaseTimeLength = 0;

        if (keyShot && keyShotReleace)
        {
            isShooting = true;
            keyShotReleace = false;
            shorttime = Time.time;

            Debug.Log("Shoot Bullet");
            ShootBullet();
        }

        if (!keyShot && !keyShotReleace)
        {
            keyShootReleaseTimeLength = Time.time - shorttime;
            keyShotReleace = true;
        }

        if (isShooting)
        {
            shootTimeLength = Time.time - shorttime;
            if (shootTimeLength >= 0.25f || keyShootReleaseTimeLength >= 0.15f)
            {
                isShooting = false;
            }
        }
    }

    void PlayerMovement()
    {
        if (keyHorizon < 0)
        {
            if (isFacingRight)
            {
                Flip();
            }

            if (isGrounded && keyJump)
            {
                rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
        }

        else if (keyHorizon > 0)
        {
            if (!isFacingRight)
            {
                Flip();
            }

            if (isGrounded && keyJump)
            {
                rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
        }

        else
        {
            if (isGrounded && keyJump)
            {
                rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunPos.position, Quaternion.identity);

        bullet.name = bulletPrefab.name;

        bullet.GetComponent<WeaponControl>().SetDamageValue(bulletDamage);
        bullet.GetComponent<WeaponControl>().SetBulletSpeed(bulletSpeed);
        bullet.GetComponent<WeaponControl>().SetBulletDirection((isFacingRight) ? Vector2.right : Vector2.left);
        bullet.GetComponent<WeaponControl>().Shoot();
    }
}