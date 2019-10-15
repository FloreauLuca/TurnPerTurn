using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    public TMP_InputField InputField;
    public string UserId;

    //string previousRoomPlayerPrefKey = "PreviousRoom";
    //public string previousRoom;

    private const string MainSceneName = "LucaScene";

    const string NickNamePlayerPrefsKey = "NickName";

    private string gameVersion = "1.0";

    void Start()
    {
        InputField.text = PlayerPrefs.HasKey(NickNamePlayerPrefsKey) ? PlayerPrefs.GetString(NickNamePlayerPrefsKey) : "";
    }

    public void ApplyUserIdAndConnect()
    {
        string nickName = "DemoNick";
        if (this.InputField != null && !string.IsNullOrEmpty(this.InputField.text))
        {
            nickName = this.InputField.text;
            PlayerPrefs.SetString(NickNamePlayerPrefsKey, nickName);
        }

        if (PhotonNetwork.AuthValues == null)
        {
            PhotonNetwork.AuthValues = new AuthenticationValues();
        }

        PhotonNetwork.AuthValues.UserId = nickName;

        Debug.Log("Nickname: " + nickName + " userID: " + this.UserId + " , " + PhotonNetwork.LocalPlayer.ActorNumber, this);

        PhotonNetwork.LocalPlayer.NickName = nickName;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

    }


    public override void OnConnectedToMaster()
    {
        this.UserId = PhotonNetwork.LocalPlayer.UserId;
        Debug.Log("UserID " + this.UserId);

        PhotonNetwork.JoinRandomRoom();
        /*
        if (PlayerPrefs.HasKey(previousRoomPlayerPrefKey))
        {
            Debug.Log("getting previous room from prefs: ");
            this.previousRoom = PlayerPrefs.GetString(previousRoomPlayerPrefKey);
            PlayerPrefs.DeleteKey(previousRoomPlayerPrefKey); // we don't keep this, it was only for initial recovery
        }

        if (!string.IsNullOrEmpty(this.previousRoom))
        {
            Debug.Log("ReJoining previous room: " + this.previousRoom);
            PhotonNetwork.JoinRoom(this.previousRoom);
            this.previousRoom = null; // we only will try to re-join once. if this fails, we will get into a random/new room
        }
        else
        {
            // else: join a random room
            PhotonNetwork.JoinRandomRoom();
        }
        */
    }
    public override void OnJoinedLobby()
    {
        OnConnectedToMaster(); // this way, it does not matter if we join a lobby or not
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnPhotonRandomJoinFailed" + message);
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2, PlayerTtl = 20000 }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        //this.previousRoom = PhotonNetwork.CurrentRoom.Name;
        //PlayerPrefs.SetString(previousRoomPlayerPrefKey, this.previousRoom);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnPhotonJoinRoomFailed");
        //this.previousRoom = null;
        //PlayerPrefs.DeleteKey(previousRoomPlayerPrefKey);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        //Debug.Log("Disconnected due to: " + cause + ". this.previousRoom: " + this.previousRoom);
    }

    public override void OnPlayerEnteredRoom(Player otherPlayer)
    {
        Debug.Log("OnPhotonPlayerActivityChanged() for " + otherPlayer.NickName + " IsInactive: " + otherPlayer.IsInactive);
    }
}
