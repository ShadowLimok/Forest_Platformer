using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BoxTrigger : MonoBehaviour
{
    private float AirLiftHeight = 6.5f;
    [SerializeField] private bool isFalling = false;
    [SerializeField] private bool hasLanded = false;
    [SerializeField] private bool playerInside = false;
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

    public bool isInitialize = false;
    public bool isInAirLift = false;
    public string sortingLayerInactive = "BIGLevelObj";
    public string sortingLayerActive = "levelobj";

    private void Start()
    {
        //light = GetComponentInChildren<Light2D>();
        //light.enabled = false;
       Initialize();

    }
    public void Initialize()
    {
        isInitialize = true;
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
        isInAirLift = false;
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
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.otherCollider == physicsCollider &&
            collision.gameObject.CompareTag(ground))

        {
            hasLanded = false;
            isFalling = false;
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
            groundCol.gameObject.layer = LayerMask.NameToLayer("Ground");;
            sr.sortingLayerName = sortingLayerActive;
            isInitialize = false;
        }
    }
    public void AirLiftAction()
    {
        if (transform.parent == null) return;

        
        float distanceY = transform.parent.position.y - transform.position.y;
        if(distanceY <= AirLiftHeight)
        {
            BottomLiftMove();
        }
    }
    private void BottomLiftMove()
    {
        if (isInAirLift)
        {
            rb.velocity = Vector2.zero;
            float speed = 1f;
            Vector2 currentPos = transform.localPosition;
            Vector2 targetPos = new Vector2(0f, AirLiftHeight);
            Vector2 triggerPos = Vector2.Lerp(currentPos, targetPos, speed * Time.fixedDeltaTime);
            transform.localPosition = triggerPos;
            if(Vector2.Distance(triggerPos, targetPos) < 0.05f)
            {
                transform.localPosition = targetPos;
                AirLift airLift = GetComponentInParent<AirLift>();
                if (airLift != null)
                {
                    airLift.boxIsSet = true;
                }
            }      
            Debug.Log("Двигаем коробку вверх: " + transform.localPosition);
            if(!isInitialize)
            {
                Initialize();
            }
            
            return;
        }
    }
    //private IEnumerator LightOn()
    //{
    //    yield return new WaitForSeconds(0.3f);
    //    light.enabled = true;
    //}

}
