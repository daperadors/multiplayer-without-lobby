using Unity.Netcode;
using UnityEngine;

public class DragAndShoot : NetworkBehaviour
{
    [SerializeField] private float m_PowerBall = 10f;
    [SerializeField] private float m_Impulse = 2f;
    [SerializeField] private Vector2 m_MinPower;
    [SerializeField] private Vector2 m_MaxPower;

    //TrajectoryLine m_TrajLine;
    private Camera m_Camera;
    private Rigidbody2D m_Rigidbody;
    private NetworkVariable<Vector2> m_Force = new NetworkVariable<Vector2>(/*Vector2.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner*/);
    private Vector3 m_StartPoint;
    private Vector3 m_EndPoint;

    private GameManager m_manager;
    [SerializeField] private Camera _mainCamera;
    public NetworkVariable<bool> _canMove = new NetworkVariable<bool>();
    [SerializeField] private Transform _spawnPoint;

    void Start()
    {

        BallView.OnBallWhiteEnter += SetSpawnPoint;
        _canMove.Value = false;
        Physics2D.gravity = Vector2.zero;
        //m_TrajLine = GetComponent<TrajectoryLine>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Camera = Camera.main;
        m_manager = GameObject.Find("GameController").GetComponent<GameManager>();
    }

    public override void OnNetworkSpawn()
    {
        m_Force.OnValueChanged += (Vector2 previousValue, Vector2 newValue) =>
        {
            Debug.Log(OwnerClientId + " new force -> " + newValue);
        };
    }

    void Update()
    {
        MoveBallServerRpc();
    }


    private void OnMouseDown()
    {
        print(gameObject.name);
        //if (!IsOwner) return;
        GetMousePositionServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    private void GetMousePositionServerRpc()
    {
        if (m_Rigidbody.velocity == Vector2.zero)
        {
            m_StartPoint = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            m_StartPoint.z = 15;
        }
    }
    private void OnMouseDrag()
    {
        //if (!IsOwner) return;
        if (m_Rigidbody.velocity == Vector2.zero)
        {
            Vector3 currentPoint = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            m_StartPoint.z = 15;
            // m_TrajLine.Value.RenderLineServerRpc(m_StartPoint, currentPoint);
        }
    }
    private void OnMouseUp()
    {
        //if (!IsOwner) return;
        print("Disparo1");
        DisparoBolaServerRpc();
        print("Disparo2");

    }
    [ServerRpc(RequireOwnership = false)]
    private void DisparoBolaServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;
        Debug.Log(clientId);
        if (clientId != GameManager.m_Instance.actualPlayer.ClientId) return;
        if (m_Rigidbody.velocity == Vector2.zero)
        {
            m_EndPoint = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            m_EndPoint.z = 15;

            m_Force.Value = new Vector2(Mathf.Clamp(m_StartPoint.x - m_EndPoint.x, m_MinPower.x, m_MaxPower.x),
                                  Mathf.Clamp(m_StartPoint.y - m_EndPoint.y, m_MinPower.y, m_MaxPower.y));
            m_Rigidbody.AddForce(m_Force.Value * m_PowerBall * m_Impulse, ForceMode2D.Impulse);
            print("Disparo2");
            m_manager.EndTurnClientRpc();
            
            //   m_TrajLine.Value.EndLineServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void MoveBallServerRpc(ServerRpcParams serverRpcParams = default) {

        ulong clientId = serverRpcParams.Receive.SenderClientId;
        if (clientId != GameManager.m_Instance.actualPlayer.ClientId) return;
        if (_canMove.Value) 
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                worldPosition.z = 0f;
                transform.position = worldPosition;
                _canMove.Value = false;
            }
        }


    }

    public void SetSpawnPoint() {
        _canMove.Value = true;
        transform.position = _spawnPoint.position;
        this.GetComponent<Rigidbody2D>().velocity = new Vector3(0f,0f,0f);
    }


}

