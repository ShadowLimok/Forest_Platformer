using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BoxSensor : MonoBehaviour
{
    public BoxTrigger parent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parent.NotifyPlayerEntered();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parent.NotifyPlayerExited();
        }
    }
}