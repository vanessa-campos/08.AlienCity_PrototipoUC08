using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{   
    public static Launcher Instance;
    public InputField roomNameInputField;
    [SerializeField] Text errorText;
    [SerializeField] Text roomNameText;
    [SerializeField] Transform roomsList;
    [SerializeField] GameObject roomListItem;
    [SerializeField] Transform playerList;
    [SerializeField] GameObject playerListItem;
    [SerializeField] GameObject startGameButton;
    bool connecting = false;

    private void Awake(){
        Instance = this;
    }
    private void Start(){        
        MenuManager.Instance.OpenMenu("menu");   
        Debug.Log("Joined Lobby");
    }

    public void ConnectGame(){
        Debug.Log("Connecting to Master...");
        if(!PhotonNetwork.IsConnected){
           connecting = PhotonNetwork.ConnectUsingSettings();
       }else{
           MenuManager.Instance.OpenMenu("loading");  
           PhotonNetwork.JoinRandomRoom();
       }
    }

    public override void OnConnectedToMaster(){
        if(connecting){
            Debug.Log("Connected to server");
            PhotonNetwork.AutomaticallySyncScene = true;
            MenuManager.Instance.OpenMenu("loading");  
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause){
        Debug.Log("Disconnected. Cause: " + cause);
        connecting = false;
    }

    public override void OnJoinedRoom(){
        Debug.Log("You have successfully joined a room.");
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinRandomFailed(short returnCode, string message){
        Debug.Log("You have failed to join a room. Code: " + returnCode + "; Message: " + message);
        PhotonNetwork.CreateRoom("gameRoom", new RoomOptions(){MaxPlayers = 2});
    }

    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");  
    }

    public override void OnLeftRoom(){  
        MenuManager.Instance.OpenMenu("menu");  
    }

    public void QuitGame(){
        PhotonNetwork.Disconnect();
        Application.Quit(); 
    }

    // public void CreateRoom(){
    //     if(string.IsNullOrEmpty(roomNameInputField.text)){
    //         return;
    //     }
    //     PhotonNetwork.CreateRoom(roomNameInputField.text);   
    //     MenuManager.Instance.OpenMenu("loading");  
    // }

    // public override void OnJoinedRoom(){
    //     MenuManager.Instance.OpenMenu("room");
    //     roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    //     PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");        
    //     foreach (Transform item in playerList){
    //         Destroy(item.gameObject);
    //     }
    //     Player[] players = PhotonNetwork.PlayerList;
    //     for (int i = 0; i < players.Length; i++){
    //         Instantiate(playerListItem, playerList).GetComponent<PlayerListItem>().SetUp(players[i]);        
    //     }
    //     startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    // }

    // public override void OnMasterClientSwitched(Player newMasterClient){
    //     startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    // }
    
    // public override void OnCreateRoomFailed(short returnCode, string message){
    //     errorText.text = "Room creation failed: " + message;
    //     MenuManager.Instance.OpenMenu("error");
    // }

    // public void JoinRoom(RoomInfo info){
    //     PhotonNetwork.JoinRoom(info.Name);
    //     MenuManager.Instance.OpenMenu("loading");
    // }

    // public override void OnRoomListUpdate(List<RoomInfo> roomList){
    //     foreach (Transform item in roomsList){
    //         Destroy(item.gameObject);
    //     }
    //     for (int i = 0; i < roomList.Count; i++){
    //         if(roomList[i].RemovedFromList)
    //             continue;
    //         Instantiate(roomListItem, roomsList).GetComponent<RoomListItem>().SetUp(roomList[i]);
    //     }
    // }

    // public override void OnPlayerEnteredRoom(Player newPlayer){
    //     Instantiate(playerListItem, playerList).GetComponent<PlayerListItem>().SetUp(newPlayer);        
    // }

    // public void StartGame(){
    //     PhotonNetwork.LoadLevel(1);
    // }
}