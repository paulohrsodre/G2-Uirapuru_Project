using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BirdFollower : MonoBehaviour
{
    public Transform player;
    public float followSpeed;
    public float stopDistance;

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if(distance > stopDistance)
        {
            transform.position = Vector2.Lerp(transform.position, player.position, followSpeed * Time.deltaTime);
        }
    }
}
