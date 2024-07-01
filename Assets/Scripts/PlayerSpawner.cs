using System;
using System.Collections;
using System.Collections.Generic;
using TikTokLiveUnity;
using UnityEngine;
using TikTokLiveSharp;
using TMPro;

public class PlayerSpawner : MonoBehaviour
{

    [SerializeField]
    GameObject playerPrefab;

    public Transform[] boundariesSpawn; //LT LB RB RT

    public TikTokConnection connecteur;
    
    public void SpawnPlayer(string nickName, Material avatar, long id)
    {
        System.Random r = new System.Random();
        Vector3 pos = new Vector3((float)r.Next((int)boundariesSpawn[0].position.x, (int)boundariesSpawn[3].position.x),0,r.Next((int)boundariesSpawn[1].position.z, (int)boundariesSpawn[0].position.z));
        GameObject tempP = Instantiate(playerPrefab, pos,Quaternion.identity,gameObject.GetComponent<Transform>());
        //GameObject tempP = Instantiate(playerPrefab,gameObject.GetComponent<Transform>());
        tempP.GetComponent<PlayerScript>().setPlayer(avatar, nickName,id);
        //Debug.Log("Ce joueur est ajouté au plateau : " + nickName);
        connecteur.addPlayerInList(tempP.GetComponent<PlayerScript>());

    }

  
}
