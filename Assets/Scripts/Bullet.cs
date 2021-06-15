using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    public float speed = 5;
    Rigidbody _rigidbody;

    private void Start(){
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider other){
        if(!photonView.IsMine)
            return;
        PhotonNetwork.Destroy(gameObject);
        if(other.CompareTag("Player")){
            PlayerController3D player = other.GetComponent<PlayerController3D>();
            player.TakeHP();
        }        
    }
}
