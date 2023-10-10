using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    //instance
    public static NetworkManager instance;

    private void Awake()
    {
        // instance can only be called once
        if (instance != null && instance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            //set instance
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();   
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to MasterServer");
    }
    public void CreateRoom (string _roomName)
    {
        PhotonNetwork.CreateRoom(_roomName);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
    }
    public void JoinRoom (string _roomName)
    {
        PhotonNetwork.JoinRoom(_roomName);
    }
    [PunRPC]
    public void LoadLevel(string _sceneName)
    {
        PhotonNetwork.LoadLevel(_sceneName);

    }
}
