using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BillarView : NetworkBehaviour
{

    public List<GameObject> _redBallsImage = new List<GameObject>();
    public List<GameObject> _yellowBallsImage = new List<GameObject>();
    public int _quantityRedBall = 0;
    public int _quantityYellowBall = 0;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        InitializeBallsImageClientRpc();
    }

    [ClientRpc]
    public void InitializeBallsImageClientRpc()
    {
        int iterador = _redBallsImage.Count;

        for (int x = 0; x < iterador; x++)
        {
            _redBallsImage[x].SetActive(false);
            _yellowBallsImage[x].SetActive(false);
        }

    }

    [ClientRpc]
    public void ActiveBallsClientRpc(int id)
    {
        print("Entra");
        if (id == 1)
        {
            _quantityRedBall++;

            if (_quantityRedBall < _redBallsImage.Count)
            {
                for (int x = 0; x < _quantityRedBall; x++)
                {
                    _redBallsImage[x].SetActive(true);
                }
            }
        }


        else if (id == 2)
        {
            _quantityYellowBall++;

            if (_quantityYellowBall < _yellowBallsImage.Count)
            {
                for (int x = 0; x < _quantityYellowBall; x++)
                {
                    _yellowBallsImage[x].SetActive(true);
                }
            }
        }
        


    }
    [ClientRpc]
    public void ComproveVictoryClientRpc()
    {
        print("Entra");
        int ballsMax = _redBallsImage.Count;
        if (_quantityRedBall == ballsMax || _quantityYellowBall == ballsMax)
        {
            Debug.Log("MI TURNO WIN");
            NetworkManager.Singleton.SceneManager.LoadScene("StartMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else
        {
            NetworkManager.Singleton.SceneManager.LoadScene("StartMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
            Debug.Log("OTRO TURNO WIN");
        }
    }


}
