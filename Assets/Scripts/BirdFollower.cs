using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BirdFollower : MonoBehaviour
{
    public Transform player;
    public float followSpeed;
    public float stopDistance;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            return;
        }

        Vector2 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if(distance > stopDistance)
        {
            Vector2 moveDir = direction.normalized;

            transform.position = Vector2.Lerp(transform.position, player.position, followSpeed * Time.deltaTime);

            anim.SetFloat("axisX", moveDir.x);
            anim.SetFloat("axisY", moveDir.y);
        }
    }
}
