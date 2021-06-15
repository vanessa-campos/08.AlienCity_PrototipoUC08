using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Ammo : MonoBehaviourPun
{
    void OnTriggerEnter(Collider other){ 
        if(PhotonNetwork.IsMasterClient && other.CompareTag("Player")){
            PlayerController3D player = other.GetComponent<PlayerController3D>();                    
            PhotonNetwork.Destroy(gameObject);
            player.AddAmmo();        
        }
    }
}

