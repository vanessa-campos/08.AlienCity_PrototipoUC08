using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] Text playerText;
    Player player;
    
    public void SetUp(Player _player){
        player = _player;
        playerText.text = _player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer){
        if(player == otherPlayer){
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom(){
        Destroy(gameObject);
    }
}
