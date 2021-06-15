using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager3D : MonoBehaviourPunCallbacks
{
    public static GameManager3D Instance;
    [SerializeField] GameObject playerPrefab;
    public Transform[] spawnPos;
    PlayerController3D lastAlive = null;
    int defeated;
    GameObject minimap;

    private void Awake() {
        Instance = this;
        minimap = GameObject.FindGameObjectWithTag("minimap");
        minimap.SetActive(false);
    }
    
    private void Start(){
        if(playerPrefab != null){   
            if(PhotonNetwork.CountOfPlayersInRooms != 0){
                int i = PhotonNetwork.CurrentRoom.PlayerCount - 1;
                PhotonNetwork.Instantiate(playerPrefab.name, spawnPos[i].position,spawnPos[i].rotation);               
            }else{             
                PhotonNetwork.Instantiate(playerPrefab.name, spawnPos[0].position,spawnPos[0].rotation);  
            }
        }        
        minimap.SetActive(true);  
    }
    
    private void Update(){
        PlayerController3D[] players = GameObject.FindObjectsOfType<PlayerController3D>();  
        for (int i = 0; i < players.Length; i++){
            if(players[i].dead == true){
                players[i].ShowEnd(false);
                defeated++;
                Invoke(nameof(LeaveRoom), 2);
            }else{
                lastAlive = players[i];
            }
        }   
        if(players.Length > 1 && defeated == players.Length - 1 && lastAlive != null){
            lastAlive.ShowEnd(true);
            Invoke(nameof(LeaveRoom), 3);
        }
    }

    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom(){
        SceneManager.LoadScene("Menu");
    }
}
