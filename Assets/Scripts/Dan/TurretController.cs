using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 10f;
    public float rotationSpeed = 5f;
    public float maxRotationAngle = 60f;
    public float fireRate = 1f;
    public float raycastDistance = 100f;
    public Transform turret;
    public Transform muzzle;

    private bool playerDetected;
    private float lastFireTime;

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRadius)
        {
            playerDetected = true;

            Vector3 direction = (player.position - turret.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            float angle = Quaternion.Angle(turret.rotation, targetRotation);
            if (angle <= maxRotationAngle)
            {
                turret.rotation = Quaternion.Slerp(turret.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (Time.time - lastFireTime >= 1f / fireRate)
            {
                Fire();
            }
        }
        else
        {
            playerDetected = false;
        }
    }

    private void Fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(muzzle.position, player.position - muzzle.position, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Hit player!");

                lastFireTime = Time.time;
            }
        }
    }
}
