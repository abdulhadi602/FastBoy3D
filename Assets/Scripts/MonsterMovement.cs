using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MonsterMovement : MonoBehaviour
{
    public float playerSpeed = 2.0f;


    Vector3 Move;



    


    public GameObject GameManager;
    private Manager managerSC;


    public int MoveXspeed;


    public float MoveX;
    private float _moveX;
    private float FinalXvalue;
    public bool isChangingDirection;

  





    public Transform LaneRender;

    private ParticleSystem BreakCoin;

    public Text Score;
    private static int scoreCounter;

    
    private AudioSource[] sounds;
    public Transform DeathEffect;
    private ParticleSystem DeathParticles;

    private static bool isDead,isSmashTime;


    private GameObject Smashobj;
    private Slider smashSlider;
    private void Start()
    {
        Smashobj = gameObject.transform.Find("Smash").gameObject;
        smashSlider = Smashobj.transform.GetChild(0).GetComponent<Slider>();
        sounds = GetComponents<AudioSource>();
       
        scoreCounter = 0;
        Move = new Vector3(0, 0, 1);

        Score.text = "0";

    
        managerSC = GameManager.GetComponent<Manager>();
      
        BreakCoin = transform.Find("BreakCoin").GetComponent<ParticleSystem>();

        DeathParticles = DeathEffect.GetComponent<ParticleSystem>();
        isDead = false;
    }
    private void FixedUpdate()
    {
        if (!isDead)
        {
            transform.position += Move * Time.deltaTime * playerSpeed;
        }

    }

    void Update()
    {
        if (!isDead)
        {
            LaneRender.position = new Vector3(transform.position.x, -2.845f, transform.position.z - 30f);


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
            PlayMovementSound();
            _moveX = -MoveX;
            FinalXvalue = transform.position.x + _moveX;
            isChangingDirection = true;
        }
    }
    public void MoveRight()
    {
        if (!isChangingDirection)
        {
            PlayMovementSound();
            _moveX = MoveX;
            FinalXvalue = transform.position.x + _moveX;
            isChangingDirection = true;
        }
    }
    private void PlayMovementSound()
    {
        if (!isSmashTime)
        {
            sounds[Random.Range(2, 5)].Play();
        }
    }
    public void Smash()
    {
        if (!isChangingDirection)
        {

            StartCoroutine(FastDash());
        }
    }
    private IEnumerator FastDash()
    {
       
        Camera.main.GetComponent<FollowPlayer>().offset.z -= 10;
        Camera.main.GetComponent<FollowPlayer>().offset.y += 10;
        sounds[5].Play();
        smashSlider.gameObject.SetActive(true);
        smashSlider.value = 5f;
        isSmashTime = true;
        GetComponent<Animator>().speed *= 2;
        transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        playerSpeed = 50;
       
        while (smashSlider.value > 0)
        {
            smashSlider.value -= Time.deltaTime;
            yield return null;
        }
        Camera.main.GetComponent<FollowPlayer>().offset.z += 10;
        Camera.main.GetComponent<FollowPlayer>().offset.y -= 10;
        smashSlider.gameObject.SetActive(false);
        smashSlider.value = 5f;
        playerSpeed = 35;
        GetComponent<Animator>().speed /= 2;
        transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        isSmashTime = false;
    }
  
   
  
  
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Ground"))
        {
            if (!isSmashTime)
            {
                playerSpeed = 0;

                StartCoroutine(WaitForEffectToEnd(collision.transform));
            }
            else
            {
                transform.GetChild(0).transform.GetChild(1).GetComponent<ParticleSystem>().Play();
                AudioSource[] blast = collision.gameObject.GetComponents<AudioSource>();
                blast[Random.Range(0,2)].Play();
                collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
                Destroy(collision.gameObject, 0.5f);
            }
        }
      
    }

    private IEnumerator WaitForEffectToEnd(Transform collision)
    {
        isDead = true;
        transform.GetChild(0).gameObject.SetActive(false);
        DeathEffect.position = collision.position;
        sounds[1].Play();
        DeathParticles.Play();
        yield return new WaitForSeconds(1.5f);
        managerSC.GameOver();
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("NearEnd"))
        {
            LevelCompletionCalculator.LevelCompleted = true;
            Camera.main.GetComponent<FollowPlayer>().enabled = false;
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
            //bubblepop.Play();
            sounds[0].Play();
        }
    }
    private void OnTriggerStay(Collider other)
    {
       
        if (other.CompareTag("Coin") )
        {


            // other.gameObject.GetComponent<MeshRenderer>().enabled = false;

         

                scoreCounter++;
                Score.text = "" + scoreCounter;
                other.transform.localScale -= Vector3.forward * 0.25f;
                other.transform.position += Vector3.forward * 0.25f;
                  
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            StartCoroutine(StopEffect());
            if (other.transform.localScale.z  <= 0.5)
            {
                Destroy(other.gameObject);
            }
            else
            {             
                Destroy(other.gameObject);
            }
        }
    }
    private IEnumerator StopEffect()
    {
        yield return new WaitForSeconds(0.25f);
        BreakCoin.Stop();
    }
  
   

}
