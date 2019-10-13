using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks, IPunTurnManagerCallbacks
{
    [SerializeField] private GameObject connectUIView;
    [SerializeField] private GameObject gameUIView;

    [SerializeField] private GameObject localPlayerPanel;
    [SerializeField] private TextMeshProUGUI localPlayerName;
    [SerializeField] private TextMeshProUGUI localPlayerAction;
    [SerializeField] private GameObject remotePlayerPanel;
    [SerializeField] private TextMeshProUGUI remotePlayerName;
    [SerializeField] private TextMeshProUGUI remotePlayerAction;

    [SerializeField] private GameObject interfacePanel;



    [SerializeField] private GameObject disconnectedPanel;

    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI timeText;

    private string localSelection;
    private string remoteSelection;

    private bool isShowingResults;

    private PunTurnManager turnManager;

    private int damage;

    public void Start()
    {
        turnManager = gameObject.AddComponent<PunTurnManager>();
        turnManager.TurnManagerListener = this;
        turnManager.TurnDuration = 50f;


        localPlayerPanel.gameObject.SetActive(false);
        remotePlayerPanel.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            PhotonNetwork.LeaveRoom();
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            PhotonNetwork.ConnectUsingSettings();
        }

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

        string selected = GetAction(localSelection);
        if (selected != null)
        {
            localPlayerAction.gameObject.SetActive(true);
            localPlayerAction.text = selected;
        }

        if (turnManager.IsCompletedByAll)
        {
            selected = GetAction(remoteSelection);
            if (selected != null)
            {
                remotePlayerAction.text = selected;
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

                remotePlayerName.text = " ";
            }


        }
    }

    private string GetAction(string selection)
    {
        return selection;
    }


    private void UpdatePlayerTexts()
    {
        Player remote = PhotonNetwork.LocalPlayer.GetNext();
        Player local = PhotonNetwork.LocalPlayer;

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
        localSelection = " ";
        remoteSelection = " ";

        localPlayerAction.gameObject.SetActive(false);
        remotePlayerAction.gameObject.SetActive(true);

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
        Debug.Log("OnTurnFinished: " + player + " turn: " + turn + " action: " + move);

        if (player.IsLocal)
        {
            localSelection = (string)move;
        }
        else
        {
            remoteSelection = (string)move;
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

    }

    private void UpdateScores()
    {
        
    }

    public void OnEndTurn()
    {
        this.StartCoroutine("ShowResultsBeginNextTurnCoroutine");
    }

    public IEnumerator ShowResultsBeginNextTurnCoroutine()
    {
        interfacePanel.SetActive(false);
        isShowingResults = true;
        //TODO damage

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

    public void MakeTurn(string selection)
    {
        this.turnManager.SendMove((string)selection, true);
    }


    public void OnClickRock()
    {
        this.MakeTurn("Rock");
    }

    public void OnClickPaper()
    {
        this.MakeTurn("Paper");
    }

    public void OnClickScissors()
    {
        this.MakeTurn("Scissors");
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
