using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayCollider : MonoBehaviour
{
    public enum PassSide { Left, Right };
    public PassSide allowedSide;

    public Collider2D playerCollider;

    [SerializeField] private float hysteresis;

    private Collider2D col;
    private bool passed = false;

    void Start()
    {
        col = GetComponent<Collider2D>();
        if (playerCollider == null)
            Debug.LogWarning("Player collider not assigned!");
    }

    void Update()
    {
        if (col == null || playerCollider == null)
            return;

        Bounds wallBounds = col.bounds;
        Bounds playerBounds = playerCollider.bounds;

        float playerCenterX = playerBounds.center.x;
        float wallCenterX = wallBounds.center.x;

        float offset = hysteresis * ((allowedSide == PassSide.Left) ? 1 : -1); 
        float targetX = wallCenterX + offset;

        if (!passed)
        {
            if (allowedSide == PassSide.Left && playerCenterX < targetX)
            {
                col.isTrigger = true;
            }
            else if (allowedSide == PassSide.Right && playerCenterX > targetX)
            {
                col.isTrigger = true;
            }
            else
            {
                passed = true;
                col.isTrigger = false;
            }
        }
        else
        {
            col.isTrigger = false;

            if (allowedSide == PassSide.Left && playerBounds.max.x < wallBounds.min.x)
                passed = false;

            else if (allowedSide == PassSide.Right && playerBounds.min.x > wallBounds.max.x)
                passed = false;
        }
    }









    //public enum PassSide { Left, Right };
    //public PassSide allowedSide;
    //private bool canToggle = false;
    //public Transform player;

    //private Collider2D col;
    //void Start()
    //{
    //    col = GetComponent<Collider2D>();
    //}

    //void Update()
    //{
    //    if (player == null || col == null) return;

    //    Vector3 localPlayerPos = transform.InverseTransformPoint(player.position);
    //    bool shouldBeTrigger = false;

    //        if (allowedSide == PassSide.Left && localPlayerPos.x < 0)
    //            shouldBeTrigger = true;

    //        else if (allowedSide == PassSide.Right && localPlayerPos.x > 0)
    //            shouldBeTrigger = true;

    //    col.isTrigger = shouldBeTrigger;
    //}

}