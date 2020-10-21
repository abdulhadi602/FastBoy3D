using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayertxt : MonoBehaviour
{
    private Transform Player;







    public Vector3 offset;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z) + offset;
    }
}
