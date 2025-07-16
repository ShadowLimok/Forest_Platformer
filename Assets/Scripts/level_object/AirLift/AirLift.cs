using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirLift : MonoBehaviour
{
    private Box box;
    [SerializeField] private LayerMask boxLayer;
    [SerializeField] private float power = 10f;
    private bool boxInLift = false;
    public BoxCollider2D airLift;
    public bool boxIsSet = false;

    private void Start()
    {
        BoxCollider2D airCollider = GetComponent<BoxCollider2D>();
        airCollider.isTrigger = true;
        boxInLift = false;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Box"))
        {
            Box script = collision.GetComponent<Box>();
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
        if(boxIsSet)
        {
            airLift.size = new Vector2(0f, 0f);
        }
    }
}