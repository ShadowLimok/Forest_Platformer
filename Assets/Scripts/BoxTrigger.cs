using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BoxTrigger : MonoBehaviour
{
    private Animator animator;
    private bool isFalling = false;
    private bool hasLanded = false;
    private bool playerInside = false;
    private SpriteRenderer sr;
    private BoxCollider2D groundCol;
    //private Light2D light;
    [SerializeField] private string inactive = "BoxTrigger";
    [SerializeField] private string active = "BoxCollider";
    [SerializeField] private string ground = "ground";
    [SerializeField] private BoxCollider2D trigger;
    [SerializeField] private BoxCollider2D physicsCollider;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject[] sides;
   


    public string sortingLayerInactive = "BIGLevelObj";
    public string sortingLayerActive = "levelobj";

    private void Start()
    {
        //light = GetComponentInChildren<Light2D>();
        //light.enabled = false;
        groundCol = GetComponent<BoxCollider2D>();
        groundCol.gameObject.layer = LayerMask.NameToLayer(inactive);
        sr = GetComponent<SpriteRenderer>();
        trigger.isTrigger = true;
        trigger.gameObject.layer = LayerMask.NameToLayer(active);
        physicsCollider.isTrigger = false;
        physicsCollider.gameObject.layer = LayerMask.NameToLayer(inactive);
        foreach (GameObject side in sides)
        {
            side.layer = LayerMask.NameToLayer(inactive);
        }
        rb.simulated = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.mass = 50;
        rb.gravityScale = 2;

    }

    public void NotifyPlayerEntered()
    {
        playerInside = true;

        if(!isFalling)
        {
            StartCoroutine(Fall());
        }
    }
    public void NotifyPlayerExited()
    {
        playerInside = false;

        if(hasLanded)
        {
            BecomeSolid();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.otherCollider == physicsCollider &&
            collision.gameObject.CompareTag(ground))

        {
            hasLanded = true;
            isFalling = false;
            BecomeSolid();
        }
    }

    private IEnumerator Fall()
    {
        isFalling = true;
        yield return new WaitForSeconds(1f);

        rb.bodyType = RigidbodyType2D.Dynamic;
        sr.sortingLayerName = sortingLayerInactive;

    }
    private void BecomeSolid()
    {
        //if (hasLanded)
        //{
        //    StartCoroutine(LightOn());
        //}
        if (hasLanded && !isFalling && !playerInside)
        {
            physicsCollider.gameObject.layer = LayerMask.NameToLayer(active);
            foreach (GameObject side in sides)
            {
                side.layer = LayerMask.NameToLayer(active);
            }
            groundCol.gameObject.layer = LayerMask.NameToLayer("Ground");
            //physicsCollider.gameObject.tag = "BoxCollider";
            sr.sortingLayerName = sortingLayerActive;
        }
    }
    //private IEnumerator LightOn()
    //{
    //    yield return new WaitForSeconds(0.3f);
    //    light.enabled = true;
    //}

}
