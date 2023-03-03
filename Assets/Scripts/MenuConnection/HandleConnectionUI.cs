using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HandleConnectionUI : NetworkBehaviour
{

    [SerializeField] private Button m_ServerButton;
    [SerializeField] private Button m_ClientButton;
    [SerializeField] private Button m_HostButton;
    [SerializeField] private GameObject m_Players;
    [SerializeField] private TextMeshProUGUI m_textPlayers;

    private NetworkVariable<int> players = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    public override void OnNetworkSpawn()
    {
        players.OnValueChanged += (int previousValue, int newValue) =>
        {
            m_textPlayers.text = players.Value + "";
            GameObject.Find("Balls").GetComponent<BillarView>().InitializeBallsImageClientRpc();
        };
    }
    void Awake()
    {

        m_ServerButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
            //NetworkManager.Singleton.SceneManager.LoadScene("GameDavid", LoadSceneMode.Single);
            DisableButtons();
        });

        m_ClientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            //NetworkManager.Singleton.SceneManager.LoadScene("GameDavid", LoadSceneMode.Single);
            DisableButtons();
        });

        m_HostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            //NetworkManager.Singleton.SceneManager.LoadScene("GameDavid", LoadSceneMode.Single);
            DisableButtons();
        });
    }

    private void Update()
    {
        if (!IsOwner) return;
        players.Value = NetworkManager.Singleton.ConnectedClients.Count;
       
    }
    private void DisableButtons()
    {
        m_ServerButton.gameObject.SetActive(false);
        m_ClientButton.gameObject.SetActive(false);
        m_HostButton.gameObject.SetActive(false);
        m_Players.SetActive(true);
    }
}
