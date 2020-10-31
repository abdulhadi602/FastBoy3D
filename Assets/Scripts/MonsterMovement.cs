using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MonsterMovement : MonoBehaviour
{
    private float playerSpeed = 35;


    Vector3 Move;



    


    private GameObject GameManager;
    private Manager managerSC;


    private int MoveXspeed = 10;


    private float MoveX = 3;
    private float _moveX;
    private float FinalXvalue;
    private bool isChangingDirection;

  





  

    private ParticleSystem BreakCoin;

    private Text Score;
    private static int scoreCounter;

    
    private AudioSource[] sounds;
    private Transform DeathEffect;
    private ParticleSystem DeathParticles;

    private static bool isDead,isSmashTime;

    private static int SmashesAvailable;


  
   

    private float slowDownSpeedMultiplier = 8;
    private float smashTime = 3.5f;
    private GameObject PowerSmash;
    private static float CurrentValue;

    private Text SmashesAvailableTxt;


    private static float cameraOffsetZ, cameraOffsetY;
    private void Awake()
    {
        PowerSmash = GameObject.FindGameObjectWithTag("SmashBtn");
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        BreakCoin = GameObject.FindGameObjectWithTag("BreakCoin").GetComponent<ParticleSystem>();
        Score = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        DeathEffect = GameObject.FindGameObjectWithTag("DeathParticles").transform;
        SmashesAvailableTxt = PowerSmash.transform.GetChild(1).GetComponent<Text>();
    }
    private void Start()
    {
         cameraOffsetZ = Camera.main.GetComponent<FollowPlayer>().offset.z;
         cameraOffsetY = Camera.main.GetComponent<FollowPlayer>().offset.y;
        SmashesAvailable = 0;
        PowerSmash.SetActive(false);
        
        sounds = GetComponents<AudioSource>();
       
        scoreCounter = 0;
        Move = new Vector3(0, 0, 1);

        Score.text = "0";

    
        managerSC = GameManager.GetComponent<Manager>();
      
    

        DeathParticles = DeathEffect.GetComponent<ParticleSystem>();
        isDead = false;
        isSmashTime = false;
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
            //The below line moves the red line with you on the map
            //LaneRender.position = new Vector3(transform.position.x, -2.845f, transform.position.z - 30f);


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
        if (!isChangingDirection && PowerSmash.transform.GetChild(0).GetComponent<RawImage>().color.a == 1 )
        {
            StopCoroutine(FastDash());
           
      
            
            StartCoroutine(FastDash());
           
            if (SmashesAvailable> 0)
            {
                SmashesAvailableTxt.text = "" + SmashesAvailable;       
            }
            else
            {
                SmashesAvailable = 0;
                PowerSmash.SetActive(false);

            }
        }
    }
    private IEnumerator FastDash()
    {
        SmashesAvailable--;
        if (SmashesAvailable > 0)
        {
            Color a = PowerSmash.transform.GetChild(0).GetComponent<RawImage>().color;
            a.a = 0.2f;
            PowerSmash.transform.GetChild(0).GetComponent<RawImage>().color = a;
        }
  
        Camera.main.GetComponent<FollowPlayer>().offset.z = cameraOffsetZ - 15;
        Camera.main.GetComponent<FollowPlayer>().offset.y = cameraOffsetY + 5;
        sounds[5].Play();

        CurrentValue = smashTime;
       
        isSmashTime = true;
        GetComponent<Animator>().speed = 4;
        transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = false;
       
            transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>().Play();
       
        transform.GetChild(0).transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
        playerSpeed = 50;
       
        while (CurrentValue > 0)
        {
            CurrentValue -= Time.deltaTime;
            if (CurrentValue < smashTime/1.5)
            {
                Camera.main.GetComponent<FollowPlayer>().offset.z += Time.deltaTime * slowDownSpeedMultiplier;
                Camera.main.GetComponent<FollowPlayer>().offset.y -= Time.deltaTime * slowDownSpeedMultiplier/2.5f;            
                
            }
            if (CurrentValue < smashTime / 1.25)
            {
                if (SmashesAvailable > 0)
                {
                    Color a = PowerSmash.transform.GetChild(0).GetComponent<RawImage>().color;
                    a.a = 1f;
                    PowerSmash.transform.GetChild(0).GetComponent<RawImage>().color = a;
                }
            }
            yield return null;
          
        }
        /**while (playerSpeed>35)
        {
           
            yield return null;
        }**/
        Camera.main.GetComponent<FollowPlayer>().offset.z = cameraOffsetZ;
        Camera.main.GetComponent<FollowPlayer>().offset.y = cameraOffsetY;
        isSmashTime = false;
        
        CurrentValue = smashTime;
        playerSpeed = 35;
        transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = true;
        GetComponent<Animator>().speed = 2;
        transform.GetChild(0).transform.GetChild(2).GetComponent<ParticleSystem>().Play();
        transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        
    }

   


    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Ground"))
        {
            if (!isSmashTime)
            {
                GameOver(collision.transform);
            }
            else
            {
                transform.GetChild(0).transform.GetChild(1).GetComponent<ParticleSystem>().Play();
                AudioSource[] blast = collision.gameObject.GetComponents<AudioSource>();
                blast[Random.Range(0,2)].Play();
                if (!collision.collider.CompareTag("Dropped"))
                {
                    collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    Destroy(collision.gameObject, 0.5f);
                }
                else
                {
                    GameOver(collision.transform);
                }
            }
        }
      
    }
    private void GameOver(Transform collision)
    {
        playerSpeed = 0;
        StartCoroutine(WaitForEffectToEnd(collision));
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
        else if (other.CompareTag("End"))
        {
            managerSC.NextLevel();
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
            if (scoreCounter % 100 == 0)
            {
                PowerSmash.SetActive(true);
                PowerSmash.GetComponent<AudioSource>().Play();
                SmashesAvailable++;
                SmashesAvailableTxt.text = ""+SmashesAvailable;
                StartCoroutine(NotifyPowerUp());
            }
            other.transform.localScale -= Vector3.forward * 0.25f;
                other.transform.position += Vector3.forward * 0.25f;
                  
        }
    }
    private IEnumerator NotifyPowerUp()
    {
        Color a = PowerSmash.transform.GetChild(2).GetComponent<RawImage>().color;
        a.a = 0f;
        PowerSmash.transform.GetChild(2).GetComponent<RawImage>().color = a;
        yield return new WaitForSeconds(0.15f);
        a.a = 1f;
        PowerSmash.transform.GetChild(2).GetComponent<RawImage>().color = a;
        yield return new WaitForSeconds(0.15f);
        a.a = 0f;
        PowerSmash.transform.GetChild(2).GetComponent<RawImage>().color = a;
        yield return new WaitForSeconds(0.15f);
        a.a = 1f;
        PowerSmash.transform.GetChild(2).GetComponent<RawImage>().color = a;
        yield return new WaitForSeconds(0.15f);
        a.a = 0f;
        PowerSmash.transform.GetChild(2).GetComponent<RawImage>().color = a;
        yield return new WaitForSeconds(0.15f);
        a.a = 1f;
        PowerSmash.transform.GetChild(2).GetComponent<RawImage>().color = a;
        yield return new WaitForSeconds(0.15f);
        a.a = 0f;
        PowerSmash.transform.GetChild(2).GetComponent<RawImage>().color = a;
        yield return new WaitForSeconds(0.15f);
        a.a = 1f;
        PowerSmash.transform.GetChild(2).GetComponent<RawImage>().color = a;

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
