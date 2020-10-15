using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

   
    public float playerSpeed = 2.0f;
 
    
    Vector3 move;
    public static Rigidbody rb;
    public static Vector3 jump;
    public static float jumpForce = 25;

    public static bool isGrounded;


    public GameObject GameManager;
    private Manager managerSC;

   
    public int MoveXspeed;
   

    public  float MoveX;
    private float _moveX;
    private float FinalXvalue;
    public  bool isChangingDirection;

    public Transform CubeMesh;

    public float hopHeight = 1.25f;
    public float timeToCompleteJump = 0.24f;
    public float jumpDistance = 4f;
    private bool hopping = false;
    private void Start()
    {
        
        move = new Vector3(0, 0, 1);
     
        rb = gameObject.GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 1.0f, 0.0f);


        managerSC = GameManager.GetComponent<Manager>();
    }
    private void FixedUpdate()
    {

        transform.position += move * Time.deltaTime * playerSpeed;

    }
    void Update()
    {
       /** if (!isChangingDirection)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {

                    _moveX = -MoveX;
                    FinalXvalue = transform.position.x + _moveX;
                    isChangingDirection = true;              
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                    _moveX = MoveX;
                    FinalXvalue = transform.position.x + _moveX;
                    isChangingDirection = true;
              
            }
        }else**/
        if(isChangingDirection){
            if (_moveX < 0)
            {
                if (transform.position.x > FinalXvalue)
                {
                    move = new Vector3(_moveX * MoveXspeed * Time.fixedDeltaTime, 0, 1);
                    CubeMesh.rotation = Quaternion.Euler(0,0, 20);
                    //transform.rotation = Quaternion.Euler(0,-20, 0);
                }
                else
                {
                    isChangingDirection = false;       
                     move = new Vector3(0, 0, 1);
                    CubeMesh.rotation = Quaternion.identity;
                }
            }
            else if (_moveX > 0)
            {
                if (transform.position.x < FinalXvalue)
                {
                    move = new Vector3(_moveX * MoveXspeed * Time.fixedDeltaTime, 0, 1);
                    CubeMesh.rotation = Quaternion.Euler(0,0,-20);
                }
                else
                {
                    isChangingDirection = false;
                    move = new Vector3(0, 0, 1);
                    CubeMesh.rotation = Quaternion.identity;
                }
            }
        }


        // Changes the height position of the player..
        /** if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
         {

             rb.AddForce(jump * jumpForce, ForceMode.Impulse);
             isGrounded = false;
         }





         if (Input.GetKeyDown(KeyCode.E))
         {
             transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z * 2);

         }
         else if (Input.GetKeyUp(KeyCode.E))
         {
             transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z / 2);

         }**/
     
    }
    public void MoveLeft()
    {
        if (!isChangingDirection)
        {
            _moveX = -MoveX;
            FinalXvalue = transform.position.x + _moveX;
            isChangingDirection = true;
        }
    }
    public void MoveRight()
    {
        if (!isChangingDirection)
        {
            _moveX = MoveX;
            FinalXvalue = transform.position.x + _moveX;
            isChangingDirection = true;
        }
    }
 
    public void SqueezeDown()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z * 2);
    }
    public void SqueezeUp()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z / 2);
    }
    public void SineJump()
    {
        if (transform.position.y > -3f )
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + jumpDistance);

            StartCoroutine(Hop(pos, timeToCompleteJump));
        }
    }

    IEnumerator Hop(Vector3 dest, float time)
    {
        if (hopping) yield break;

        hopping = true;
        var startPos = transform.position;
        var timer = 0.0f;

        while (timer <= 1.0f)
        {
            var height = Mathf.Sin(Mathf.PI * timer) * hopHeight;
            transform.position = Vector3.Lerp(startPos, dest, timer) + Vector3.up * height;

            timer += Time.deltaTime / time;
            yield return null;
        }
        hopping = false;
    }



    private void OnCollisionEnter(Collision collision)
        {
       



          
          
            Vector2 hit = collision.contacts[0].normal;

            float angle = Vector3.Angle(Vector3.forward, hit);
            //Debug.Log("angle : " + angle);
            //Debug.DrawRay(transform.position, collision.gameObject.transform.position, Color.red);


        if (Mathf.Approximately(angle, 0))// front
        {
            //Debug.Log("front");
            playerSpeed = 0;
            managerSC.GameOver();
            //Debug.Log("Front");
            return;
        }
       /** if (Mathf.Approximately(angle, 180))// Left
        {
           

        }**/
            if (Mathf.Approximately(angle, 90))
            {
                Vector3 cross = Vector3.Cross(Vector3.forward, hit);
               // Debug.Log("Cross " + cross);
                if (cross.x > 0)
                {

                // ResetJump();


                //Top
                playerSpeed = 0;
                managerSC.GameOver();
               // Debug.Log("Hit Top");
                return;
                }
                /**else if (cross.x < 0)
                {
                //kEEP the square in center of track
                transform.position = new Vector3(collision.transform.position.x, transform.position.y, transform.position.z);
                //Debug.Log("Touched Platform");
                }
                 else if(cross.y>0 || cross.y<0)
                {
             
                
                playerSpeed = 0;
                managerSC.GameOver();
                Debug.Log("Left or right side hit");
                return;
                }**/

            }
        
        }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NearEnd"))
        {
            Camera.main.GetComponent<FollowPlayer>().enabled=false;
            move *= 2;
        }
        else if (other.CompareTag("Dropped") || other.CompareTag("End"))
        {
            managerSC.GameOver();
            //Debug.Log("End");
            return;
        }
    }
}
