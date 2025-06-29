using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    [SerializeField][Range(1f, 70f)] float speed;
    [SerializeField][Range(1f, 70f)] float climbingSpeed;
    [SerializeField][Range(1f, 50f)] float jumpForce;
    [SerializeField][Range(1f, 40f)] float wallJumpForce = 15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask boxLayer;

    private float movementX;
    private float movementY;
    private float originalGravityScale;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private WallJump.WallSide wallSide;

    //public bool boxIsNear = false;
    public bool isPushing = false;
    public bool isGrounded = true;
    public bool isOnWall = false;
    public bool isWallJumping = false;

    public Animator animator;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        originalGravityScale = rb.gravityScale;
    }
    public void SetWallState(bool onWall, WallJump.WallSide side)
    {
        
        isOnWall = onWall;
        wallSide = side;

        animator.SetBool("wallJump", onWall);

        if (isOnWall)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, 0);

            sr.flipX = (wallSide == WallJump.WallSide.Right);


        }
        else
        {
            rb.gravityScale = originalGravityScale;
            animator.SetBool("wallJump", false);
        }
    }
    bool BoxIsNear()
    {
        isPushing = false;
        Vector2 origin = rb.position;
        float distance = 0.52f;
        Vector2 directionLeft = Vector2.left;
        Vector2 directionRight = Vector2.right;

        RaycastHit2D hitLeft = Physics2D.Raycast(origin, directionLeft, distance, boxLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(origin, directionRight, distance, boxLayer);
        Debug.DrawRay(origin, directionLeft * distance, Color.yellow);
        Debug.DrawRay(origin, directionRight * distance, Color.green);

        if (hitLeft.collider != null && movementX <0)
        {
            isPushing = true;
            return true;
        }

        if (hitRight.collider != null && movementX >0)
        {
            isPushing = true;
            return true;
        }
        return false;
    }
    bool IsGrounded()
    {

        Vector2 origin = rb.position;
        float distance = 0.1f;
        Vector2 size = new Vector2(0.9f, 1.2f);
        RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f, Vector2.down, distance, groundLayer);
        Debug.DrawRay(origin, Vector2.down * distance, Color.red);
        return hit.collider != null;

        //Vector2 origin = rb.position;
        //Vector2 size = new Vector2(0.9f, 1.2f);
        //float angle = 0f;
        //Vector2 direction = Vector2.down;
        //float distanceCheck = 0.1f;
        //Color debugColor = Color.red;

        //DebugDrawBoxCast(origin, size, angle, direction, distanceCheck, debugColor);

        //RaycastHit2D hit = Physics2D.BoxCast(origin, size, angle, direction, distanceCheck, groundLayer);
        //return hit.collider != null;

    }
    //void DebugDrawBoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, Color color)
    //{
    //    Vector2 halfSize = size / 2f;
    //    Quaternion rot = Quaternion.Euler(0, 0, angle);

    //    // 4 угла прямоугольника на начальной позиции
    //    Vector2[] cornersStart = new Vector2[4];
    //    cornersStart[0] = origin + (Vector2)(rot * new Vector3(-halfSize.x, -halfSize.y, 0f));
    //    cornersStart[1] = origin + (Vector2)(rot * new Vector3(-halfSize.x, halfSize.y, 0f));
    //    cornersStart[2] = origin + (Vector2)(rot * new Vector3(halfSize.x, halfSize.y, 0f));
    //    cornersStart[3] = origin + (Vector2)(rot * new Vector3(halfSize.x, -halfSize.y, 0f));

    //    // 4 угла прямоугольника на позиции после сдвига
    //    Vector2[] cornersEnd = new Vector2[4];
    //    for (int i = 0; i < 4; i++)
    //        cornersEnd[i] = cornersStart[i] + direction.normalized * distance;

    //    // Рисуем боксы (начальный и конечный)
    //    for (int i = 0; i < 4; i++)
    //    {
    //        Debug.DrawLine(cornersStart[i], cornersStart[(i + 1) % 4], color);
    //        Debug.DrawLine(cornersEnd[i], cornersEnd[(i + 1) % 4], color);
    //        Debug.DrawLine(cornersStart[i], cornersEnd[i], color);
    //    }
    //}

    void Update()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        movementY = Input.GetAxisRaw("Vertical");

        if (isWallJumping)
        {
            if (isGrounded || isOnWall)
            {
                ResetWallState();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                animator.SetBool("isGrounded", false);
            }
            else if (isOnWall)
            {
                WallJumpDIr();
            }

        }
        if (!isOnWall)
        {
            if (movementX > 0)
                sr.flipX = false;
            else if (movementX < 0)
                sr.flipX = true;
        }
       

    }
    void ResetWallState()
    {
        isWallJumping = false;
    }
    private void WallJumpDIr()
    {
        bool correctPressed = false;
        if(wallSide == WallJump.WallSide.Left && movementX > 0)
            correctPressed = true;
        else if(wallSide == WallJump.WallSide.Right && movementX < 0)
            correctPressed = true;

        if(correctPressed)
        {
            int jumpDirection = (wallSide == WallJump.WallSide.Left) ? 1 : -1;
            Vector2 wallJumpDir = new Vector2(jumpDirection, 1f).normalized;
            rb.velocity = new Vector2(0f, 0f);
            rb.AddForce(wallJumpDir * wallJumpForce, ForceMode2D.Impulse);
            animator.SetBool("wallJump", false);
            animator.SetBool("climbing", false);
            isOnWall = false;
            rb.gravityScale = originalGravityScale;
            isWallJumping = true;
        }
    }
    void FixedUpdate()
    {
        isGrounded = IsGrounded();
        animator.SetBool("isGrounded", isGrounded);

        if(BoxIsNear() && isGrounded)
        {
            animator.SetBool("boxIsNear", true);
            Debug.Log("Is Pushing");
        }
        else
        {
            animator.SetBool("boxIsNear", false);
            Debug.Log("Not Pushing");
        }

        if (isWallJumping) return;

        if(isOnWall && isGrounded)
        {
            if(Mathf.Abs(movementY)>0.1f)
            {
                SetWallState(isOnWall, wallSide);
                float climbVelocity = movementY * climbingSpeed;
                rb.velocity = new Vector2(0f, climbVelocity);
                animator.SetBool("climbing", Mathf.Abs(movementY) > 0.01f);
                animator.SetFloat("speed", 0f);
            }
            else
            {
                //SetWallState(false, wallSide);
                rb.gravityScale = originalGravityScale;
                rb.velocity = new Vector2(movementX * speed, rb.velocity.y);
                animator.SetFloat("speed", Mathf.Abs(movementX));
                animator.SetBool("climbing", false);
            }
        }
        else if(isOnWall)
        {
            float climbVelocity = movementY * climbingSpeed;
            rb.velocity = new Vector2(0f, climbVelocity);
            animator.SetBool("climbing", Mathf.Abs(movementY) > 0.01f);
            animator.SetFloat("speed", 0f);
        }
        else
        {
            rb.gravityScale = originalGravityScale;
            rb.velocity = new Vector2(movementX * speed, rb.velocity.y);
            animator.SetFloat("speed", Mathf.Abs(movementX));
            animator.SetBool("climbing", false);
        }
        //if (isOnWall)
        //{
        //    float climbVelocity = movementY * climbingSpeed;
        //    rb.velocity = new Vector2(0f, climbVelocity);
        //    animator.SetBool("climbing", Mathf.Abs(movementY) > 0.01f);
        //    animator.SetFloat("speed", 0f);

        //}
        //else
        //{
        //    rb.gravityScale = originalGravityScale;
        //    rb.velocity = new Vector2(movementX * speed, rb.velocity.y);
        //    animator.SetFloat("speed", Mathf.Abs(movementX));
        //    animator.SetBool("climbing", false);
        //}

        if (!isGrounded)
        {
            animator.SetFloat("speed", 0f);
        }
    }
}