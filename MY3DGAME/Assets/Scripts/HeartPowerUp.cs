using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPowerUp : MonoBehaviour
{
    public float multiplier = 1.4f;
   // public float duration = 4f;

    public GameObject pickupEffect;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other){

        if(other.CompareTag("Player")){
            //StartCoroutin(Pickup(other));
            Pickup(other);
        }
    }

    void Pickup(Collider player){
        //spawn a cool effect
        //Debug.Log("Power Up picked up!");
        Instantiate(pickupEffect, transform.position, transform.rotation);

        //make player's health increase
        PlayerStats stats = player.GetComponent<PlayerStats>();
        stats.health *= multiplier;
        //make power up invisible
        //GetComponent<MeshRenderer>().enabled = false;
        //GetComponent<Collider>().enabled = false;
        //wait x amount of seconds to back
        //yield return new WaitForSeconds(duration);
        //reverse the effect on our player
        //stats.health /= multiplier;
        //remove power up object
        Destroy(gameObject);
    }
}