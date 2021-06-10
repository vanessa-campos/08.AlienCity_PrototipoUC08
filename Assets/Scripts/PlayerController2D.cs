using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController2D : MonoBehaviourPun, IPunObservable
{
    public float Speed = 10;
    public float Torq = 5;
    public float SqrMaxVelocity = 150;
    [HideInInspector] public int Points;
    public AudioSource soundImpulse;
    public AudioSource soundCollect;
    Rigidbody2D rb2d;
    Material mat;    

    private void Awake(){
        rb2d = GetComponent<Rigidbody2D>();
        mat = GetComponent<SpriteRenderer>().material;
    }

    private void Start() {
        if(!photonView.IsMine){
            mat.color = Color.yellow;
        }
    }

    void FixedUpdate(){
        if (!photonView.IsMine)
            return;

        if (Input.GetButton("Fire1") && rb2d.velocity.sqrMagnitude < SqrMaxVelocity){
            rb2d.AddForce (transform.up * Speed, ForceMode2D.Force);
            soundImpulse.Play();
        }
        float v = Input.GetAxis("Horizontal") * Torq * Time.deltaTime;
        rb2d.AddTorque(-v);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("gem")){
            soundCollect.Play();
        }
    }

    [PunRPC]
    void RPCAddPoints(){
        Points += 1;
    }
    public void AddPoints(){
        photonView.RPC("RPCAddPoints", RpcTarget.All);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){
            stream.SendNext(Points);
        }else{
            Points = (int)stream.ReceiveNext();
        }
    }
   
}
