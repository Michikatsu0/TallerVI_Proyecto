using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSetting : MonoBehaviour
{
    public LayerMask isGround;
    public float speed = 10f;
    public float lifetime = 5f;
    public float damage = 2;
    private float time = 0;
    private Collider col;



    private void Start()
    {
        col = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        if (time >= 0.03f)
        {
            col.isTrigger = false;
        }
        if (time >= lifetime)
        {
            Destroy(gameObject);
        }
        time += Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (time >= 0.03f)
        {
            Destroy(gameObject);
        }

        GameObject target = collision.gameObject;
        if (target.CompareTag("enemy")) {

        }
    }
}