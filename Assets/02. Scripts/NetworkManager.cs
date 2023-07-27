using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks 
{
    public InputField _NN;


    private void Start()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
    }


}
