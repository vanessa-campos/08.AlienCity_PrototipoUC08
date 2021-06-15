using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CrateSpawner : MonoBehaviourPun
{
    [SerializeField] GameObject cratePrefab;
    [SerializeField] int maxCrates = 5;
    [SerializeField] float spawnTime = 2;

    private void Start(){
        StartCoroutine(SpawnCrateRoutine());
    }

    IEnumerator SpawnCrateRoutine(){
        while(true){
            SpawnCrate();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    private void SpawnCrate(){
        if(!PhotonNetwork.IsMasterClient || cratePrefab == null)
            return;
       
        int cratesCount = GameObject.FindGameObjectsWithTag("ammo").Length;
        if(cratesCount < maxCrates){
            Vector3 pos = new Vector3();
            pos.y = 0.15f;
            pos.x = Random.Range (-11, 11); 
            pos.z = Random.Range(-12, 12);
            PhotonNetwork.Instantiate(cratePrefab.name, pos, Quaternion.identity);
        } 
    }
}
