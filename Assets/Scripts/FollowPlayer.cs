using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform Player;






    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;
    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(Player.position.x+OffsectX, Player.position.y + OffsectY, -10 ) ;
    }


    private void FixedUpdate()
    {
         desiredPosition = Player.position + offset;
         smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

       // transform.LookAt(Player);
    }
}
