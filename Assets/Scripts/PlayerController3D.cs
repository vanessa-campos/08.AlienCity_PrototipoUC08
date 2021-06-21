using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController3D : MonoBehaviourPunCallbacks, IPunObservable
{
    public int HP = 10;
    public int Ammo = 10;
    public float Speed = 7;
    public float SpinSpeed = 3;
    public float jumpForce = 5;
    public bool isGrounded = false;
    public Transform groundCheck;
    public Transform shootpoint;
    public GameObject target;
    public GameObject bulletPrefab;
    public SkinnedMeshRenderer colorSkin;
    public GameObject UI;
    public GameObject UIhead;
    public GameObject panelVictory;
    public Image imageHealthBar1;
    public Image imageHealthBar2;
    public Text textAmmo;    
    public AudioSource soundShoot;
    public AudioSource soundCollect;
    public AudioSource soundDead;

    Rigidbody _rigidbody;
    Animator _animator;
    float horizontal;
    float vertical;
    bool jump;
    bool run;

    private void Awake(){
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        panelVictory.SetActive(false);
        target.SetActive(false);
    }

    private void Start(){
        if(!photonView.IsMine){
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(_rigidbody);
            Destroy(UI);
            colorSkin.material.color = Color.yellow;
            Ammo = 10;
        }
    }

    private void Update(){
        if(!photonView.IsMine)
            return;
            
        // Set Input Axis
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        // Get Jump Button
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            jump = true;    
        }
        // Get Fire Button
        if(Ammo > 0){
            if(Input.GetKeyDown(KeyCode.LeftControl)){
                soundShoot.Play();
                Ammo -= 1;
                PhotonNetwork.Instantiate(bulletPrefab.name, shootpoint.position, transform.rotation);
            }
        }
        // Set Animator Parameters
        _animator.SetBool("run", run);
        if(vertical != 0 || horizontal != 0){
            run = true;
        }else{
            run = false;
        }
        if(HP <= 0){
            _animator.SetTrigger("dead");
            ShowEnd(false);
        }
        // Ammo Text
        if(Ammo < 0){
            Ammo = 0;
        }
        textAmmo.text = Ammo.ToString();   
        // HealthBar   
        imageHealthBar1.fillAmount = imageHealthBar2.fillAmount;
        // Show Target Object
        // if(Physics.Raycast(transform.position, transform.forward, 7)){
        //     target.SetActive(false);
        // }else{
        //     target.SetActive(true);
        // }
    }

    private void FixedUpdate(){
        if(!photonView.IsMine)
            return;
            
        // Walk
        Vector3 newVelocity = transform.forward * vertical * Speed;
        // Keep the velocity on y to not influence on gravity
        newVelocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = newVelocity;
        // Spin
        _rigidbody.angularVelocity = new Vector3(0, horizontal * SpinSpeed, 0);
        // IsGrounded
        isGrounded = Physics.Linecast(transform.position,groundCheck.position, 1 << LayerMask.NameToLayer("ground"));
        if(jump){
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpForce, 0);
            jump = false;
        }        
    }

    [PunRPC]
    void RPCTakeHP(){
        HP -= 1;
        imageHealthBar2.fillAmount = HP * 0.1f;
        StartCoroutine(Hit());
        if(HP<=0){
            UIhead.SetActive(false);
            soundDead.Play();
        }
    }
    public void TakeHP(){
        photonView.RPC("RPCTakeHP", RpcTarget.All);
    }
    IEnumerator Hit(){
        colorSkin.material.color = Color.red;
        yield return new WaitForSeconds(.5f);
        if(!photonView.IsMine){
            colorSkin.material.color = Color.yellow;
        }else{
            colorSkin.material.color = Color.white;
        }
    }

    [PunRPC]
    void RPCAddAmmo(){
        soundCollect.Play();
        Ammo += 5;        
    }
    public void AddAmmo(){
        photonView.RPC("RPCAddAmmo", RpcTarget.All);
    }
    
    [PunRPC]
    void RpcShowEnd(bool victory){
        if(!photonView.IsMine)
            return;
        panelVictory.SetActive(true); 
        Text victoryText = GameObject.Find("TextVictory").GetComponent<Text>();
        if(victory){
            victoryText.text = "WINNER";
            Invoke(nameof(LeaveRoom), 2);
        }else{
            victoryText.text = "GAME OVER";  
            Invoke(nameof(LeaveRoom), 2);
        }
    }    
    public void ShowEnd(bool victory){
        photonView.RPC("RpcShowEnd", RpcTarget.All, victory);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){
            stream.SendNext(HP);
        }else{
            HP = (int)stream.ReceiveNext();
        }
    }

    public void LeaveRoom(){
        GameManager3D.Instance.LeaveRoom();
    }

}
