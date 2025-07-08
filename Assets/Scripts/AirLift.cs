using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirLift : MonoBehaviour
{
    private GameObject boxTrigger;
    [SerializeField] private LayerMask boxLayer;
    [SerializeField] private float power = 10f;

    private void Start()
    {
        BoxCollider2D airCollider = GetComponent<BoxCollider2D>();
        airCollider.isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & boxLayer) != 0)
        {
            Rigidbody2D rb = collision.attachedRigidbody;
            if (rb != null)
            {
                rb.AddForce(Vector2.up * power);
            }
        }
    }

    private void Update()
    {

    }
}