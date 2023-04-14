using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionalShooting : MonoBehaviour
{
    public Joystick joystickB;
    public GameObject projectilePrefab;
    public float shootDelay = 0.5f;
    private float lastShootTime = 0f;

    private void Update()
    {
        
        if (joystickB.Horizontal != 0f || joystickB.Vertical != 0f)
        {
            float angle = Mathf.Atan2(joystickB.Vertical, joystickB.Horizontal) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
            if (Time.time - lastShootTime > shootDelay)
            {
                Shoot();
                lastShootTime = Time.time;
            }
        }
    }

    private void Shoot()
    {
        Vector3 spawnPosition = transform.position + transform.up * 1.0f;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);
    }
}