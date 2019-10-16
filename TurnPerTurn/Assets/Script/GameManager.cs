﻿using System;
using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class PokemonObject
{
    //public Action[] listActions = new Action[3];
    public int id;
    public float currentpv;
    public float maxpv;
    public float speed;
    public string name;
    public string type;
    public bool freezed;
    public bool poisonned;

}

public class GameManager : MonoBehaviourPunCallbacks, IPunTurnManagerCallbacks
{
    [SerializeField] private GameObject connectUIView;
    [SerializeField] private GameObject gameUIView;

    [SerializeField] private GameObject localPlayerPanel;
    [SerializeField] private TextMeshProUGUI localPlayerName;
    [SerializeField] private TextMeshProUGUI localPlayerAction;
    [SerializeField] private UIPanel localPlayerUI;

    [SerializeField] private GameObject remotePlayerPanel;
    [SerializeField] private TextMeshProUGUI remotePlayerName;
    [SerializeField] private TextMeshProUGUI remotePlayerAction;
    [SerializeField] private UIPanel remotePlayerUI;

    [SerializeField] private GameObject interfacePanel;

    private Pokemon localPokemonScriptable;
    [SerializeField] private Pokemon[] pokemonScriptable;

    private PokemonObject localPokemon;
    private PokemonObject remotePokemon;

    [SerializeField] private GameObject disconnectedPanel;

    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private TextMeshProUGUI winLoosePanel;

    private Action localSelection;
    private Action remoteSelection;

    private bool isShowingResults;

    private PunTurnManager turnManager;

    private int damage;

    public void Start()
    {
        int random = Random.Range(0, pokemonScriptable.Length);
        localPokemonScriptable = pokemonScriptable[random]; 
        localPokemon = new PokemonObject();
        PhotonPeer.RegisterType(typeof(Action), 23, Action.Serialize, Action.Deserialize);
        localPokemon.maxpv = localPokemonScriptable.maxpv;
        localPokemon.currentpv = localPokemonScriptable.maxpv;
        localPokemon.speed = localPokemonScriptable.speed;
        localPokemon.name = localPokemonScriptable.name;
        localPokemon.type = localPokemonScriptable.type;
        localPokemon.id = random;
        remotePokemon = new PokemonObject();

        turnManager = gameObject.AddComponent<PunTurnManager>();
        turnManager.TurnManagerListener = this;
        turnManager.TurnDuration = 50f;
        
        localPlayerPanel.gameObject.SetActive(false);
        remotePlayerPanel.gameObject.SetActive(false);
        interfacePanel.gameObject.SetActive(false);

    }

    public void Update()
    {
        /*
        if (Input.GetKeyUp(KeyCode.P))
        {
            PhotonNetwork.LeaveRoom();
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        */

        if (!PhotonNetwork.InRoom)
        {
            return;
        }

        if (PhotonNetwork.IsConnected && disconnectedPanel.gameObject.activeSelf)
        {
            disconnectedPanel.gameObject.SetActive(false);
        }

        if (!PhotonNetwork.IsConnected && !disconnectedPanel.gameObject.activeSelf)
        {
            disconnectedPanel.gameObject.SetActive(true);
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            if (turnManager.IsOver)
            {
                return;
            }

            if (turnText != null)
            {
                turnText.text = this.turnManager.Turn.ToString();
            }

            if (turnManager.Turn > 0 && timeText != null && !isShowingResults)
            {

                timeText.text = this.turnManager.RemainingSecondsInTurn.ToString("F1") + " SECONDS";
            }
        }
        UpdatePlayerTexts();

        Action selected = GetAction(localSelection);
        if (selected != null)
        {
            localPlayerAction.gameObject.SetActive(true);
            localPlayerAction.text = selected.ToString();
        }

        if (turnManager.IsCompletedByAll)
        {
            selected = GetAction(remoteSelection);
            if (selected != null)
            {
                remotePlayerAction.text = selected.ToString();
            }
        }
        else
        {
            interfacePanel.SetActive(PhotonNetwork.CurrentRoom.PlayerCount > 1);
            if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
            {
                remotePlayerPanel.SetActive(false);
            }
            else if (turnManager.Turn > 0 && !turnManager.IsCompletedByAll)
            {
                Player remote = PhotonNetwork.LocalPlayer.GetNext();
                if (turnManager.GetPlayerFinishedTurn(remote))
                {
                    remotePlayerPanel.SetActive(true);
                }

                if (remote != null && remote.IsInactive)
                {
                    remotePlayerPanel.SetActive(false);
                }
            }
        }
    }

    private Action GetAction(Action selection)
    {
        return selection;
    }


    private void UpdatePlayerTexts()
    {
        Player remote = PhotonNetwork.LocalPlayer.GetNext();
        Player local = PhotonNetwork.LocalPlayer;

        localPlayerName.text = local.NickName;
        if (remote != null)
        {
            remotePlayerName.text = remote.NickName;
        }
        else
        {
            timeText.text = "";
            remotePlayerName.text = "waiting for another player";
        }
    }

    public void OnTurnBegins(int turn)
    {
        Debug.Log("OnTurnBegins() turn: " + turn);
        localSelection = new Action();
        remoteSelection = new Action();

        localPlayerAction.gameObject.SetActive(false);
        remotePlayerAction.gameObject.SetActive(true);

        remotePlayerPanel.SetActive(true);
        localPlayerPanel.SetActive(true);

        isShowingResults = false;
        interfacePanel.SetActive(true);
    }

    public void OnTurnCompleted(int turn)
    {
        Debug.Log("OnTurnCompleted: " + turn);

        CalculateWinAndLoss();
        UpdateScores();
        OnEndTurn();
    }

    public void OnPlayerMove(Player player, int turn, object move)
    {
        Debug.Log("OnPlayerMove: " + player + " turn: " + turn + " action: " + move);
        
    }

    public void OnPlayerFinished(Player player, int turn, object move)
    {
        Debug.Log("OnTurnFinished: " + player + " " + PhotonNetwork.LocalPlayer + " " + PhotonNetwork.LocalPlayer.GetNext() + " turn: " + turn + " action: " + move);
        if (player == PhotonNetwork.LocalPlayer)
        {
            localSelection = (Action) move;
            Debug.Log("local type " + localSelection.currentPV);
        }
        else
        {
            remoteSelection = (Action) move;
            Debug.Log("remote type " + remoteSelection.currentPV);
        }
    }


    public void OnTurnTimeEnds(int turn)
    {
        if (!isShowingResults)
        {
            Debug.Log("OnTurnTimeEnds: Calling OnTurnCompleted");
            OnTurnCompleted(-1);
        }
    }

    private void CalculateWinAndLoss()
    {
        
        /*if (localSelection.type == Action.Type.SWITCH)
        {
            localPokemon = localSelection.switchedPokemon;
        }
        if (remoteSelection.type == Action.Type.SWITCH)
        {
            remotePokemon = remoteSelection.switchedPokemon;
        }*/
        remotePokemon.currentpv = remoteSelection.currentPV;
        localPokemon.currentpv = localSelection.currentPV;
        if (remotePokemon.speed > localPokemon.speed)
        {
            UpdateAction(remoteSelection, remotePokemon, localPokemon);
            UpdateAction(localSelection, localPokemon, remotePokemon);
        }
        else
        {
            UpdateAction(localSelection, localPokemon, remotePokemon);
            UpdateAction(remoteSelection, remotePokemon, localPokemon);
        }
        if (localPokemon.currentpv < 0)
        {
            winLoosePanel.text = "Loose";
            PhotonNetwork.LeaveRoom();
        } else
        if (remotePokemon.currentpv < 0)
        {
            winLoosePanel.text = "Win";
            PhotonNetwork.LeaveRoom();
        }
        
    }

    private void UpdateAction(Action selection, PokemonObject myPokemon, PokemonObject otherPokemon)
    {
        myPokemon.maxpv = pokemonScriptable[selection.currentPokemonID].maxpv;
        myPokemon.speed = pokemonScriptable[selection.currentPokemonID].speed;
        myPokemon.name = pokemonScriptable[selection.currentPokemonID].name;
        myPokemon.type = pokemonScriptable[selection.currentPokemonID].type;
        switch (selection.type)
        {
            case Action.Type.ATTAQUE:
                otherPokemon.currentpv -= selection.value;
                break;
            case Action.Type.SOIN:
                myPokemon.currentpv += selection.value;
                break;
            case Action.Type.DEF:
                break;
            case Action.Type.FREEZE:
                myPokemon.freezed = true;
                break;
            case Action.Type.POISON:
                myPokemon.poisonned = true;
                break;
        }
    }

    private void UpdateScores()
    {
        localPlayerUI.SetValue(localPokemon.name, localPokemon.maxpv, localPokemon.currentpv, localPokemon.type, localPokemon.freezed, localPokemon.poisonned);
        remotePlayerUI.SetValue(remotePokemon.name, remotePokemon.maxpv, remotePokemon.currentpv, remotePokemon.type, remotePokemon.freezed, remotePokemon.poisonned);
    }

    public void OnEndTurn()
    {
        this.StartCoroutine("ShowResultsBeginNextTurnCoroutine");
    }

    public IEnumerator ShowResultsBeginNextTurnCoroutine()
    {
        interfacePanel.SetActive(false);
        isShowingResults = true;
        //TODO afficher action

        yield return new WaitForSeconds(2.0f);

        StartTurn();
    }


    public void StartTurn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            turnManager.BeginTurn();
        }
    }

    public void MakeTurn(Action selection)
    {
        Debug.Log( PhotonNetwork.LocalPlayer + " " + selection.currentPV);
        this.turnManager.SendMove(selection, true);
    }


    public void OnClickButton(int buttonID)
    {
        localSelection = localPokemonScriptable.listActions[buttonID];
        localSelection.currentPV = localPokemon.currentpv;
        MakeTurn(localSelection);
    }

    public void OnClickConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public void OnClickReConnectAndRejoin()
    {
        PhotonNetwork.ReconnectAndRejoin();
    }

    void RefreshUIViews()
    {

        connectUIView.gameObject.SetActive(!PhotonNetwork.InRoom);
        gameUIView.gameObject.SetActive(PhotonNetwork.InRoom);

        interfacePanel.SetActive(PhotonNetwork.CurrentRoom != null ? PhotonNetwork.CurrentRoom.PlayerCount > 1 : false);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom()");



        RefreshUIViews();
    }

    public override void OnJoinedRoom()
    {
        RefreshUIViews();

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            if (turnManager.Turn == 0)
            {
                StartTurn();
            }
        }
        else
        {
            Debug.Log("Waiting for another player");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Other player arrived");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            if (turnManager.Turn == 0)
            {
                StartTurn();
            }
        }
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Other player disconnected! " + otherPlayer.ToStringFull());
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        disconnectedPanel.gameObject.SetActive(true);
    }

}
