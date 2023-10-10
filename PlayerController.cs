using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class PlayerController : MonoBehaviourPunCallbacks
{
    public int PlayerID;


    public float Speed;
    public Rigidbody Rigid;
    public Player PhotonPlayer;

    public Material Mp1;
    public Material Mp2;

    public GameObject[] Plates;
     private void Update()
    {
        GameManager.instance.Timer();

        if (PhotonNetwork.IsMasterClient)
        {
            if (GameManager.instance.CurrentTime <= 0 && !GameManager.instance.GameOver)
            {
                GameManager.instance.GameOver = true;
                GameManager.instance.photonView.RPC("WinGame", RpcTarget.All, PlayerID);
                    
            }
        }
        if (photonView.IsMine)
        {
            Move();
            
            BackToSpawnPoint();
        }
        
    }

    private void Move()
    {
        float xdirection = Input.GetAxis("Horizontal") * Speed;
        float zdirection = Input.GetAxis("Vertical") * Speed;

        Rigid.velocity = new Vector3(xdirection, Rigid.velocity.y, zdirection);
    }

    private void BackToSpawnPoint()
    {
        if (this.GetComponent<Transform>().position.y <= -20 )
        {
            
            this.transform.position = GameManager.instance.SpawnPoints[Random.Range(0, 2)].position;
        }
    }

    [PunRPC]
    public void Initialize(Player _player)
    {
        PhotonPlayer = _player;
        PlayerID = _player.ActorNumber;

        GameManager.instance.Players[PlayerID - 1] = this;

        if (!photonView.IsMine)
        {
            Rigid.isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Plate"))
        {
            if (PlayerID == 1)
            {
                collider.GetComponent<MeshRenderer>().material = Mp1;
                collider.GetComponent<Plate>().IsP1 = true;
                collider.GetComponent<Plate>().IsP2 = false;
            }
            else if (PlayerID == 2)
            {
                collider.GetComponent<MeshRenderer>().material = Mp2;
                collider.GetComponent<Plate>().IsP2 = true;
                collider.GetComponent<Plate>().IsP1 = false;
            }
        }
    }

    public void OnPhotonSerializedView(PhotonStream _stream, PhotonMessageInfo _info)
    {
        if (_stream.IsWriting)
        {
            _stream.SendNext(GameManager.instance.CurrentTime);
        }
        else if(_stream.IsReading)
        {
            GameManager.instance.CurrentTime =  (int)_stream.ReceiveNext();
        }
    }
}
