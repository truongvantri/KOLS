using System;
using System.Collections;
using System.Collections.Generic;
using ECM2.Components;
using KOLS;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = System.Random;

public class PlayerClientManager : MonoBehaviourPunCallbacks
{
    public static PlayerClientManager Instance;
    
    public CustomeSimpleCameraController myCameraFollow;
    public Transform transPlayerHolder;
    public GameObject playerPrefab;
    public Transform transInitPlayer;
    private CharacterControllerHandle m_myPlayer = null;
    public  List<CharacterControllerHandle> listPlayer = new List<CharacterControllerHandle>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        listPlayer.Clear();
    }

    private void Start()
    {
        playerPrefab.SetActive(false);
        if (PhotonNetwork.IsConnected)
        {
            InitLocalPlayer();
        }
        else
        {
            Debug.LogError("Photon not connected!!");
            PhotonNetwork.LoadLevel("KOLSNetworkRoom");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4});
        }
    }

    public void AddPlayer(CharacterControllerHandle player)
    {
        listPlayer.Add(player);
        //player.transform.SetParent(transPlayerHolder);
        GameObject objPlayer = player.gameObject;
        PhotonView photonView = objPlayer.GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            Debug.Log("Add Other Player");
            player.enabled = false;
            objPlayer.GetComponent<CharacterMovement>().enabled = false;
        }
        else
        {
            Debug.Log("AddPlayer");
        }
    }

    void InitLocalPlayer()
    {
        if (playerPrefab == null)
        {
            return;
        }
        
        if (PhotonNetwork.LocalPlayer.IsLocal)
        {
            Vector3 posInit = Vector3.zero;
            GameObject player = PhotonNetwork.Instantiate(this.playerPrefab.name,posInit, Quaternion.identity, 0);
            if(player == null)
                return;
            
            CharacterControllerHandle pController = player.GetComponent<CharacterControllerHandle>();
            myCameraFollow.SetPlayer(player.transform);
            myCameraFollow.SetTargetFollow(pController.pointCameraFollow.transform);

            if (pController != null)
                pController.SetCamera(myCameraFollow.GetCamera());
            player.SetActive(true);
        }
    }

    float GetRandomValue(float a, float b)
    {
        return UnityEngine.Random.Range(a, b);
    }

    #region PhotonFunc

    public override void OnPlayerEnteredRoom( Player other  )
    {
        Debug.Log( "OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting
        if ( PhotonNetwork.IsMasterClient )
        {
            Debug.LogFormat( "OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient ); // called before OnPlayerLeftRoom
        }
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("OnJoinedLobby");
    }

    #endregion
}
