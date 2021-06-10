using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Gem : MonoBehaviourPun
{

   private void OnTriggerEnter2D(Collider2D other)
   {
       if(PhotonNetwork.IsMasterClient && other.CompareTag("Player")){
           PlayerController2D player = other.gameObject.GetComponent<PlayerController2D>();
           player.AddPoints();
           PhotonNetwork.Destroy(gameObject);
       }
   }
}
