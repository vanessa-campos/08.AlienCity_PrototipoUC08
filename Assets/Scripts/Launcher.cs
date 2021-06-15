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

    private void Awake(){
        Instance = this;
    }

    private void Start() {        
        if(!PhotonNetwork.IsConnected){
            Debug.Log("Connecting to Master...");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster() {     
            Debug.Log("Connected to server");
            PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby(){        
        MenuManager.Instance.OpenMenu("menu");
        Debug.Log("Joined Lobby");
    }

    public override void OnDisconnected(DisconnectCause cause){
        Debug.Log("Disconnected. Cause: " + cause);
    }   

    public void ConnectGame(){
        PhotonNetwork.JoinRandomRoom();
    } 

    public override void OnJoinRandomFailed(short returnCode, string message){
        Debug.Log("You have failed to join a room. Code: " + returnCode + "; Message: " + message);
        PhotonNetwork.CreateRoom("gameRoom", new RoomOptions() { MaxPlayers = 2 });
    }
    
    public void CreateRoom(){
        if (string.IsNullOrEmpty(roomNameInputField.text))
            return;
        
        PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions() { MaxPlayers = 4 });
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom(){
        Debug.Log("You have successfully joined a room.");
        if (MenuManager.Instance.currentMenu == 2){
            PhotonNetwork.LoadLevel(1);
        }
        if (MenuManager.Instance.currentMenu == 0){            
            MenuManager.Instance.OpenMenu("room");
            roomNameText.text = PhotonNetwork.CurrentRoom.Name;

            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
            Player[] players = PhotonNetwork.PlayerList;
            foreach (Transform item in playerList) {
                Destroy(item.gameObject);
            }
            for (int i = 0; i < players.Length; i++){
                Instantiate(playerListItem, playerList).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
            startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }
    }

    public void JoinRoom(RoomInfo info){
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }
    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }
    public override void OnLeftRoom(){
        MenuManager.Instance.OpenMenu("menu");
    }  
    public override void OnMasterClientSwitched(Player newMasterClient){
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList){
        foreach (Transform item in roomsList){
            Destroy(item.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++) {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItem, roomsList).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Player[] players = PhotonNetwork.PlayerList;
            foreach (Transform item in playerList) {
                Destroy(item.gameObject);
            }
            for (int i = 0; i < players.Length; i++){
                Instantiate(playerListItem, playerList).GetComponent<PlayerListItem>().SetUp(players[i]);
            }
    }

    public void StartGame(){
        PhotonNetwork.LoadLevel(2);
    }

    public void QuitGame(){
        PhotonNetwork.Disconnect();
        Application.Quit();
    }
}