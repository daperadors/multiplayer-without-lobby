using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public NetworkVariable<int> CurrentTurn = new NetworkVariable<int>(-1);
    public List<NetworkClient> _players = new List<NetworkClient>();
    public NetworkClient actualPlayer;
    public static GameManager m_Instance;
    public Button startButton;
    private bool gameStarted = false;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (m_Instance == null)
            m_Instance = this;
        else
            Destroy(gameObject);



    }
    void Start()
    {
        SceneManager.LoadScene("GameDavid");
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer)
        {
            _players = (List<NetworkClient>)NetworkManager.Singleton.ConnectedClientsList;
            CurrentTurn.Value = 0;
            actualPlayer = _players[CurrentTurn.Value];
            print(_players.ToString());
        }

    }
    private void Update()
    {
        if (!IsServer || gameStarted) return;

        _players = (List<NetworkClient>)NetworkManager.Singleton.ConnectedClientsList;
    }
    [ClientRpc]
    public void EndTurnClientRpc()
    {
        if (!IsServer) return;
        CurrentTurn.Value++;
        if (CurrentTurn.Value >= _players.Count) CurrentTurn.Value = 0;
        actualPlayer = _players[CurrentTurn.Value];
        gameStarted = true;
        Debug.Log(actualPlayer.ClientId);
    }

}
