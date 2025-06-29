using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class SpikeColision : MonoBehaviour
{
    public GameObject player;
    private GameObject spike;
    private Movement movement;
    public Animator animator;

    [SerializeField][Range(1f, 40f)] public float pushForce = 20f;
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        movement = player.GetComponent<Movement>();
        animator = player.GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {       
                Vector2 pushDir = (collision.transform.position - transform.position).normalized;
                rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
                movement.isGrounded = false;
                animator.SetBool("isGrounded", false);
                animator.SetFloat("speed", 0f);
                animator.SetBool("isDead", true);
                pushForce = 0f;
            }
            StartCoroutine(Death(collision.gameObject));
        }
    }

    private IEnumerator Death(GameObject collision)
    {
        movement.enabled = false;
        SpriteRenderer renderer = player.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = Color.red;
        }
        yield return new WaitForSeconds(3);
        Debug.Log("Player is dead");
    }
}
