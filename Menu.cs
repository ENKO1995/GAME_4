using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class Menu : MonoBehaviourPunCallbacks
{
    [Header("Screens")]
    public GameObject MenuView;
    public GameObject Lobby;

    [Header("Menu View")]
    public Button CreateRoom;
    public Button JoinRoom;

    [Header("Lobby Screen")]
    public TextMeshProUGUI PlayerList;
    public Button StartGame;

    private void Start()
    {
        CreateRoom.interactable = false;
        JoinRoom.interactable = false;
    }

    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
    //Usage of these buttons is only possible after connecting to the master server
    public override void OnConnectedToMaster()
    {
        CreateRoom.interactable = true;
        JoinRoom.interactable = true;
    }

    void SetScreen(GameObject _screen)
    {
        MenuView.SetActive(false);
        Lobby.SetActive(false);

        _screen.SetActive(true);
    }

    public void OnCreateRoom(TMP_InputField _roomNameInput)
    {
        NetworkManager.instance.CreateRoom(_roomNameInput.text);
    }

    public void OnJoinRoom(TMP_InputField _roomNameInput)
    {
        NetworkManager.instance.JoinRoom(_roomNameInput.text);
    }

    public void OnPlayerNameUpdate(TMP_InputField _playerNameInput)
    {
        PhotonNetwork.NickName = _playerNameInput.text;
    }

    
    public override void OnJoinedRoom()
    {
        SetScreen(Lobby);

        //if new player joins the lobby, everyones Lobby UI will be updated
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateLobbyUI();
    }

    [PunRPC]
    public void UpdateLobbyUI()
    {
        PlayerList.text = "";
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            PlayerList.text += player.NickName + "\n";
        }

        if (PhotonNetwork.IsMasterClient)
        {
            StartGame.interactable = true;
        }
        else
        {
            StartGame.interactable = false;
        }
    }

    public void OnLeaveLobby()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(MenuView);
    }

    public void OnStartGame()
    {
        NetworkManager.instance.photonView.RPC("LoadLevel", RpcTarget.All, "Game");
    }
}
