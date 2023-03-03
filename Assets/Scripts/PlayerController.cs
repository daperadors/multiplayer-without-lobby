using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int m_Turn;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    //private void Start()
    //{
    //    m_Turn = NetworkManager.singleton.t;
    //}
}
