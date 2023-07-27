using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 20f; //ÃÑ¾Ë °ø°Ý·Â
    public float speed = 1000f; //ÃÑ¾Ë ¼Óµµ

    Rigidbody rb;
    Transform tr;
    TrailRenderer trail;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        trail = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        rb.AddForce(transform.forward * speed);
    }

    private void OnDisable()
    {
        trail.Clear();
        tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rb.Sleep();
    }
}
