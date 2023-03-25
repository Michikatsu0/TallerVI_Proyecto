using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSetting : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}