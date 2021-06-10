using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GemmsSpawner : MonoBehaviourPun
{
    [SerializeField] GameObject gemPrefab;
    [SerializeField] int maxGemms = 7;
    [SerializeField] float spawnTime = 2;

    private void Start(){
        StartCoroutine(SpawnGemRoutine());
    }

    IEnumerator SpawnGemRoutine(){
        while(true){
            SpawnGem();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    private void SpawnGem(){
        if(!PhotonNetwork.IsMasterClient || gemPrefab == null)
            return;
       
        int gemmsCount = GameObject.FindObjectsOfType<Gem>().Length;
        if(gemmsCount < maxGemms){
            Vector3 pos = new Vector3();
            pos.x = Random.Range (-7, 7); 
            pos.y = Random.Range(-3, 5);
            
            PhotonNetwork.Instantiate(gemPrefab.name, pos, Quaternion.identity);

        } 
    }
}
