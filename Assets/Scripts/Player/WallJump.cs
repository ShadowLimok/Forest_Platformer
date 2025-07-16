using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{

    public enum WallSide { Left, Right }

    public WallSide side;

    public void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Movement movement = collider.gameObject.GetComponent<Movement>();
            if (movement != null)
            {
                movement.SetWallState(true, side);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Movement movement = collider.gameObject.GetComponent<Movement>();
            if (movement != null)
            {
                movement.SetWallState(false, side);
            }
        }
    } 
}
