using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirLift : MonoBehaviour
{
    private BoxTrigger box;
    [SerializeField] private LayerMask boxLayer;
    [SerializeField] private float power = 10f;

    private void Start()
    {
        BoxCollider2D airCollider = GetComponent<BoxCollider2D>();
        airCollider.isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Box"))
        {
            BoxTrigger script = collision.GetComponent<BoxTrigger>();
            collision.gameObject.transform.SetParent(transform, true);
            box = script;
            box.isInAirLift = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            box.isInAirLift = false;

        }
    }

    private void FixedUpdate()
    {
        if (box != null)
        {
            if (box.isInAirLift)
            {
                box.AirLiftAction();
            }
        }
    }
}