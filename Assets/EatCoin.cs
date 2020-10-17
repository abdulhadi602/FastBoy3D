using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatCoin : MonoBehaviour
{
    private GameObject Player;
    private ParticleSystem BreakCoin;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        BreakCoin = Player.transform.Find("BreakCoin").GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if (transform.localScale.z < 0)
        {
            BreakCoin.Stop();
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       /** if (other.CompareTag("Player"))
        {
            BreakCoin.Play();
            transform.localScale -= Vector3.forward*0.5f;
            transform.position += Vector3.forward;
        }**/
    }
    private void OnTriggerStay(Collider other)
    {
        BreakCoin.Play();
        transform.localScale -= Vector3.forward * 0.25f;
        transform.position += Vector3.forward * 0.25f;
    }
    private void OnTriggerExit(Collider other)
    {
        BreakCoin.Stop();
    }

}
