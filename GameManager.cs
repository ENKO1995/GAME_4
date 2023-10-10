using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviourPunCallbacks
{

    public static GameManager instance;


    public bool GameOver = false;
    public float CurrentTime = 30;
    
    
    //public TextMeshPro Timer;
    public Text Ttimer;
    

    public string PlayerPrefabLocation;
    public Transform[] SpawnPoints;
    public PlayerController[] Players;
    private int playersInGame;


    public GameObject[] Plates;

    public int P1Points;
    public int P2Points;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Players = new PlayerController[PhotonNetwork.PlayerList.Length];
        photonView.RPC("InGameMassage", RpcTarget.All);

    }

   

    [PunRPC]
    void InGameMassage()
    {
        playersInGame ++;

        if (playersInGame== PhotonNetwork.PlayerList.Length)
        {
            SpawnPlayer();
        }
    }

    void SpawnPlayer()
    {
        //instatiate the player
        GameObject playerRes = PhotonNetwork.Instantiate(PlayerPrefabLocation, SpawnPoints[Random.Range(0, SpawnPoints.Length)].position, Quaternion.identity);

        //Get the script
        PlayerController script = playerRes.GetComponent<PlayerController>();

        script.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }

    public PlayerController GetPlayer (GameObject _player)
    {
        return Players.First(p => p.gameObject == _player);
    }

    public PlayerController GetPlayer(int _playerID)
    {
        return Players.First(p => p.PlayerID == _playerID);
    }

    void CountPlateColor()
    {

        for (int i = 0; i < Plates.Length; i++)
        {
            if (Plates[i].GetComponent<Plate>().IsP1 == true)
            {
                P1Points++;
            }
            else if (!Plates[i].GetComponent<Plate>().IsP1)
            {
                P2Points++;
            }
        }
    }

    public void Timer()
    {
        
            CurrentTime -= Time.deltaTime;
            Ttimer.text = CurrentTime.ToString();

            if (CurrentTime <= 0)
            {
                CurrentTime = 0;
            }
        
    }

    [PunRPC]
    public void WinGame(int _playerID)
    {
        GameOver = true;
        CountPlateColor();
        if (CheckWinner(_playerID) == true)
        {
            PlayerController player = GetPlayer(_playerID);
            UI.instance.SetWinnerText(player.PhotonPlayer.NickName);

            Invoke("GoToMenu", 3.0f);
        }
    }

    
    void GoToMenu()
    {
        PhotonNetwork.LeaveRoom();
        NetworkManager.instance.LoadLevel("Menu");
    }

    public bool CheckWinner(int _playerID)
    {
        bool wonGame = false;

        if ((_playerID == 1 && P1Points > P2Points) || (_playerID == 2 && P2Points > P1Points))
        {
            wonGame = true;
        }

        return wonGame;
    }
}
