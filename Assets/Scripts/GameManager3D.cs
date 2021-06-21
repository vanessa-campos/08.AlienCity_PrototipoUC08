using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager3D : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerPrefab;
    public static GameManager3D Instance;
    public Transform[] spawnPos;
    GameObject minimap;
    bool morePlayers = false;
    int i;

    private void Awake() {
        Instance = this;
        minimap = GameObject.FindGameObjectWithTag("minimap");
        minimap.SetActive(false);
        if(PhotonNetwork.IsMasterClient && playerPrefab != null){   
            i = PhotonNetwork.CurrentRoom.PlayerCount;
        } 
    }
    
    private void Start(){
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPos[i].position,spawnPos[i].rotation);                      
        minimap.SetActive(true);  
    }
    
    private void Update(){
        PlayerController3D[] players = GameObject.FindObjectsOfType<PlayerController3D>(); 
        if(players.Length > 1){
            morePlayers = true;
        }
        foreach (var player in players){
            if(players.Length == 1 && player.HP > 0 && morePlayers){
                player.ShowEnd(true);
            }  
        }     
    }

    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom(){
        SceneManager.LoadScene("Menu");
    }
}
