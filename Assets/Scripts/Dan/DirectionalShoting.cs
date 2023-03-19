using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalShoting : MonoBehaviour
{
    [Header("Joystick Settings")]
    [SerializeField] public Joystick joystick;
    [SerializeField][Range(0f, 1f)] public float deathZoneX;
    [SerializeField][Range(0f, 1f)] public float deathZoneJumpY;
    [SerializeField][Range(0f, 1f)] public float deathZoneCrouchY;

    [Header("Bullet Settings")]
    [SerializeField] int bulletDamage = 1;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] Transform gunPos;
    [SerializeField] GameObject bulletPrefab;

    bool keyShot;

    bool isShooting;

    bool keyShotReleace;
    float shorttime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BulletDirectionInput();
        PlayerShootInput();

        //keyHorizon = Input.GetAxisRaw("Horizontal");
    }

    void BulletDirectionInput()
    {
        //keyHorizon = Input.GetAxisRaw("Horizontal");
        //keyJump = Input.GetKeyDown(KeyCode.Space);
        //keyShot = Input.GetKey(KeyCode.F);
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

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunPos.position, Quaternion.identity);

        bullet.name = bulletPrefab.name;

        bullet.GetComponent<BulletSetting>().SetDamageValue(bulletDamage);
        bullet.GetComponent<BulletSetting>().SetBulletSpeed(bulletSpeed);
        //bullet.GetComponent<BulletSetting>().SetBulletDirection();
        bullet.GetComponent<BulletSetting>().Shoot();
    }
}
