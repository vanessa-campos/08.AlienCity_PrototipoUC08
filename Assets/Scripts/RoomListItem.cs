using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    public Text roomText;
    public RoomInfo info;
    
    public void SetUp(RoomInfo _info){
        info = _info;
        roomText.text = _info.Name;
    }

    public void OnCLick(){
        Launcher.Instance.JoinRoom(info);
    }
}
