using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;
    public bool open;
    


    public void Open(){
        if(menuName == "create room"){
            Launcher.Instance.roomNameInputField.text = "";
        }
        open = true;
        gameObject.SetActive(true);
    }

    public void Close(){
        open = false;
        gameObject.SetActive(false);
    }

}
