using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)   
    {

        // Raycasting to determine what side of the brick the ball hits
        Ray myRay = new Ray(transform.position, collision.gameObject.transform.position);
        RaycastHit myRayHit;

        Physics.Raycast(myRay, out myRayHit);
        Vector2 hit = collision.contacts[0].normal;
       // Debug.Log("Hit  " + hit);
        float angle = Vector3.Angle(Vector2.right,hit);
        Debug.DrawRay(transform.position, collision.gameObject.transform.position,Color.red);
      
       // Debug.Log("Angle  " +angle);
        if (Mathf.Approximately(angle, 0))// Left
        {
            Debug.Log("Left");
        }
        if (Mathf.Approximately(angle, 180))// Right
        {
            Debug.Log("Right");
        }
        if (Mathf.Approximately(angle, 90))
        {
            Vector3 cross = Vector3.Cross(Vector2.right, hit);
            Debug.Log("Cross " + cross);
            if (cross.z > 0) 
            {
                Debug.Log("Bottom");
            }
            else
            {       
                Debug.Log("Top");
            }

        }

    }
}
