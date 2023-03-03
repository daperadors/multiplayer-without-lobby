using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class BallView : NetworkBehaviour
{

    [SerializeField] private BillarView _billarController;

    public delegate void BallWhite();
    public static BallWhite OnBallWhiteEnter;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Agujero")) {
            print("si");
            if (gameObject.tag.Equals("PoolBallRed"))
            {
                _billarController.ActiveBallsClientRpc(1);
                Debug.Log("BallRed");
                DestroyObjectClientRpc();
            }
            else if (gameObject.tag.Equals("PoolBallYellow"))
            {
                _billarController.ActiveBallsClientRpc(2);
                Debug.Log("BallYellow");
                DestroyObjectClientRpc();
            }
            else if (gameObject.tag.Equals("PoolBall"))
            {
                OnBallWhiteEnter?.Invoke();
            }
            else if (gameObject.tag.Equals("PoolBallBlack")) {
                _billarController.ComproveVictoryClientRpc();
                DestroyObjectClientRpc();
            }


        }
    }
    [ClientRpc]
    private void DestroyObjectClientRpc()
    {
        Destroy(gameObject);
    }



}
