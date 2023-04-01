using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public float knockbackForce = 500.0f;
    public float invincibilityTime = 2.0f;

    private bool hitConfirmed = false;
    private float timeSinceLastHit = 0.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && timeSinceLastHit >= invincibilityTime)
        {
            Vector3 knockbackDirection = (transform.position - collision.transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(new Vector3(knockbackDirection.x, knockbackDirection.y, 0.0f) * knockbackForce);

            hitConfirmed = true;
            timeSinceLastHit = 0.0f;
        }
    }

    void Update()
    {
        if (timeSinceLastHit >= invincibilityTime)
        {
            hitConfirmed = false;
        }

        timeSinceLastHit += Time.deltaTime;
    }

    public bool HitConfirmed()
    {
        return hitConfirmed;
    }
}
