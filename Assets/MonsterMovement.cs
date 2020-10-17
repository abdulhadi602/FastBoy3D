using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float playerSpeed = 2.0f;


    Vector3 Move;



    public bool isGrounded;


    public GameObject GameManager;
    private Manager managerSC;


    public int MoveXspeed;


    public float MoveX;
    private float _moveX;
    private float FinalXvalue;
    public bool isChangingDirection;

  

    public float hopHeight = 1.25f;
    public float timeToCompleteJump = 0.24f;
    public float jumpDistance = 4f;
    //private bool hopping = false;

    public bool isJumping;


    private static Vector3 startPos;
    private float timer = 0.0f;
    Vector3 dest;

    private static bool isFalling;

    public Transform LaneRender;

    private ParticleSystem BreakCoin;
    private void Start()
    {

        Move = new Vector3(0, 0, 1);



        isJumping = false;
        managerSC = GameManager.GetComponent<Manager>();
        isFalling = false;
        BreakCoin = transform.Find("BreakCoin").GetComponent<ParticleSystem>();
    }
    private void FixedUpdate()
    {
        if (!isFalling)
        {
            if (timer <= 1.0 && isJumping)
            {

                var height = Mathf.Sin(Mathf.PI * timer) * hopHeight;
                transform.position = Vector3.Lerp(startPos, dest, timer) + Vector3.up * height;

                timer += Time.deltaTime / timeToCompleteJump;

            }
            else if (timer > 1.0)
            {

                ResetJump();
            }
            if (!isJumping)
            {
                transform.position += Move * Time.deltaTime * playerSpeed;
            }
        }
        else
        {
            transform.position += Vector3.down * Time.deltaTime * playerSpeed;
        }

    }
    private void ResetJump()
    {
        isJumping = false;
        timer = 0;
    }
    void Update()
    {
       

        if (!isFalling)
        {
            if (isChangingDirection)
            {
                if (_moveX < 0)
                {
                    if (transform.position.x > FinalXvalue)
                    {
                        Move = new Vector3(_moveX * MoveXspeed * Time.fixedDeltaTime, 0, 1);
                       
                        //transform.rotation = Quaternion.Euler(0,-20, 0);
                    }
                    else
                    {
                        isChangingDirection = false;
                        Move = new Vector3(0, 0, 1);
                        transform.position = new Vector3(FinalXvalue, transform.position.y, transform.position.z);
                     
                    }
                }
                else if (_moveX > 0)
                {
                    if (transform.position.x < FinalXvalue)
                    {
                        Move = new Vector3(_moveX * MoveXspeed * Time.fixedDeltaTime, 0, 1);
                      
                    }
                    else
                    {
                        isChangingDirection = false;
                        Move = new Vector3(0, 0, 1);
                        transform.position = new Vector3(FinalXvalue, transform.position.y, transform.position.z);
                    
                    }
                }
            }

        }

     

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
        if (!isJumping)
        {
            isJumping = true;
            startPos = transform.position;
            dest = new Vector3(transform.position.x, transform.position.y, transform.position.z + jumpDistance);
        }
   
    }

  
    private void OnCollisionStay(Collision collision)
    {
        LaneRender.gameObject.SetActive(true);
        LaneRender.localScale = new Vector3(transform.lossyScale.x * 1.5f, collision.transform.localScale.y, collision.transform.localScale.z);
        LaneRender.position = new Vector3(transform.position.x, collision.transform.position.y + 0.01f, collision.transform.position.z + 0.1f);

    }
    private void OnCollisionExit(Collision collision)
    {
        LaneRender.gameObject.SetActive(false);
    }
  
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Ground"))
        {
            playerSpeed = 0;
            managerSC.GameOver();
        }
        else if (collision.collider.CompareTag("Ground") && isJumping)
        {
            ResetJump();
            transform.position = new Vector3(collision.transform.position.x, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("NearEnd"))
        {
            Camera.main.GetComponent<FollowPlayer>().enabled = false;
            playerSpeed *= 2;
        }
        else if (other.CompareTag("Dropped") || other.CompareTag("End"))
        {
            managerSC.GameOver();
            //Debug.Log("End");
            return;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            if (other.transform.localScale.z < 0.5f)
            {
                BreakCoin.Stop();
                Destroy(other.gameObject);
                return;
            }
            BreakCoin.Play();
            other.transform.localScale -= Vector3.forward * 0.35f;
            other.transform.position += Vector3.forward * 0.35f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            BreakCoin.Stop();
        }
    }
}
