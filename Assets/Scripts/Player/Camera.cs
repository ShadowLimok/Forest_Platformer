using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Vector3 startingPos;
    private new Camera camera;

    [SerializeField][Range(-50f, 1f)] public float distance = -20f;
    [SerializeField][Range(-50f, 50f)] public float offset = 4f;
    public Transform player;
    [SerializeField][Range(1f, 10f)] public float speed = 5f;
    void Start()
    {
        camera = GetComponent<Camera>();
        startingPos = transform.position;
    }
    void FixedUpdate()
    {

        Vector3 targetPos = new Vector3(player.position.x, player.position.y + offset, distance);
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.fixedDeltaTime);
    }
}
