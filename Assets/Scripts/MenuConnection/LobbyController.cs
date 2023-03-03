
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : NetworkBehaviour
{
    public Button startButton;
    [SerializeField] private GameObject m_Game;
    [SerializeField] private GameObject m_Lobby;
    [SerializeField] private TextMeshProUGUI m_textPlayers;

    private NetworkVariable<int> players = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    private void Awake()
    {
        m_Game.SetActive(false);
        m_Lobby.SetActive(true);
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        players.OnValueChanged += (int previousValue, int newValue) =>
        {
            m_textPlayers.text = players.Value + "";
        };
        if (IsOwner)
        {
            startButton.onClick.AddListener(() =>
            {
                if (!IsOwner) return;
                if (GameManager.m_Instance._players.Count > 0) {
                    m_Lobby.SetActive(false);
                    m_Game.SetActive(true);
                }
                
            });
        }
        else startButton.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (!IsOwner) return;
        players.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }
}
