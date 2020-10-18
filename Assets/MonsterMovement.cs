using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public Text Score;
    private static int scoreCounter;
    private void Start()
    {
        scoreCounter = 0;
        Move = new Vector3(0, 0, 1);

        Score.text = "0";

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
        LaneRender.position = new Vector3(transform.position.x, -2.845f, transform.position.z - 30f);

        if (!isFalling)
        {
            if (isChangingDirection)
            {
                if (_moveX < 0)
                {
                    if (transform.position.x > FinalXvalue)
                    {
                        Move = new Vector3(_moveX * MoveXspeed * Time.fixedDeltaTime, 0, 1);
                        transform.rotation = Quaternion.Euler(0, 0, 20);
                     
                    }
                    else
                    {
                        isChangingDirection = false;
                        Move = new Vector3(0, 0, 1);
                        transform.position = new Vector3(FinalXvalue, transform.position.y, transform.position.z);
                        transform.rotation = Quaternion.identity;
                    }
                }
                else if (_moveX > 0)
                {
                    if (transform.position.x < FinalXvalue)
                    {
                        Move = new Vector3(_moveX * MoveXspeed * Time.fixedDeltaTime, 0, 1);
                        transform.rotation = Quaternion.Euler(0, 0, -20);
                    }
                    else
                    {
                        isChangingDirection = false;
                        Move = new Vector3(0, 0, 1);
                        transform.position = new Vector3(FinalXvalue, transform.position.y, transform.position.z);
                        transform.rotation = Quaternion.identity;
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
            Camera.main.GetComponent<FollowZ>().enabled = false;
            playerSpeed *= 2;
        }
        else if (other.CompareTag("Dropped") || other.CompareTag("End"))
        {
            managerSC.GameOver();
            //Debug.Log("End");
            return;
        }
        else if (other.CompareTag("Coin"))
        {
            BreakCoin.Play();           
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            scoreCounter++;
            Score.text = ""+scoreCounter;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            StartCoroutine(StopEffect());
            Destroy(other.gameObject);
        }
    }
    private IEnumerator StopEffect()
    {
        yield return new WaitForSeconds(0.25f);
        BreakCoin.Stop();
    }
  
   

}
