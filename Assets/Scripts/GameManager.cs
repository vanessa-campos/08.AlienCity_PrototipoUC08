using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerPrefab;
    public Transform[] spawnPos;
    public Text textPoints;  

    private void Start(){
        if(playerPrefab != null){   
            int i = PhotonNetwork.CountOfPlayersInRooms;
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPos[i].position,spawnPos[i].rotation);                    
        }     
    }

    private void Update(){
        PlayerController2D[] players = GameObject.FindObjectsOfType<PlayerController2D>();        
        string textPoint = "";
        for (int i = 0; i < players.Length; i++){            
            textPoint += "P" + (i+1) + ": " + players[i].Points + "; "; 
        }
        textPoints.text = textPoint;
    }

    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom(){
        SceneManager.LoadScene("Menu");
    }
}

