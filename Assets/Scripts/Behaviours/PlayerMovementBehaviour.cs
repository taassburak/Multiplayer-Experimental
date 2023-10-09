using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public struct MyCustomData : INetworkSerializable
{
    public int Int;
    public bool Bool;
    public FixedString128Bytes String;


    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Int);
        serializer.SerializeValue(ref Bool);
        serializer.SerializeValue(ref String);
    }
}

public class PlayerMovementBehaviour : NetworkBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private NetworkObject _object;
    public Renderer _renderer;
    private List<NetworkClient> _clients;

    private NetworkVariable<int> _speed = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<MyCustomData> _data = new NetworkVariable<MyCustomData>(

        new MyCustomData
        {
            Int = 0,
            Bool = true,
            String = ""
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner

        );

    private PlayerController _playerController;

    internal void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _data.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log($"{OwnerClientId} --- int :{newValue.Int} --- bool :{newValue.Bool} --- message:{newValue.String}");
        };
    }

    void Update()
    {
        if (!IsOwner)
            return;

        MainCameraFollow();

        if (Input.GetKeyDown(KeyCode.T))
        {
            _data.Value = new MyCustomData
            {
                Int = Random.Range(0, 100),
                Bool = true,
                String = "THIS IS THE MESSAGE"
            };
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            _speed.Value = 10;
            Debug.Log($"{OwnerClientId} speed : {_speed.Value}");
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            TestServerRpc();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            //if (IsServer)
            //{
            //    InstantiateObject();
            //}
            //else
            //{
            //    InstantiateRequestedObjectOnServerRpc();
            //}

            InstantiateRequestedObjectOnServerRpc();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SetCharactersColorServerRpc();
            //SetCharactersColorClientRpc();
        }

        Vector3 moveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _characterController.Move(move * Time.deltaTime * _speed.Value);

        if (move != Vector3.zero)
        {
            //_playerController.PlayerAnimationBehaviour.SetAnimation(true);
            gameObject.transform.forward = Vector3.Lerp(transform.eulerAngles, move, 1);
        }
        //else
            //_playerController.PlayerAnimationBehaviour.SetAnimation(false);

        //transform.position += moveDir * _speed.Value * Time.deltaTime;
    }

    [ServerRpc]
    private void TestServerRpc()
    {
        Debug.Log($"Test Server Rpc {OwnerClientId}");
    }

    private void InstantiateObject()
    {
        var spawnedObject = Instantiate(_object);
        spawnedObject.Spawn(true);
    }

    [ServerRpc]
    private void InstantiateRequestedObjectOnServerRpc()
    {
        InstantiateObject();
    }

    private void MainCameraFollow()
    {
        var camera = Camera.main;
        camera.transform.position = new Vector3(transform.position.x, transform.position.y + 6.3f, -8.8f);
    }

    [ClientRpc]
    private void SetCharactersColorClientRpc()
    {
        var players = NetworkManager.Singleton.ConnectedClientsList;

        foreach (var player in players)
        {
            if (player.ClientId == OwnerClientId)
            {
                //player.PlayerObject.transform.GetComponentInChildren<Material>().color = Color.blue;
                var gameobject = player.PlayerObject.transform.GetComponent<PlayerMovementBehaviour>();
                var mat = gameobject._renderer.material;
                mat.color = Color.blue;
                Debug.Log(mat);


            }
            else
            {
                var gameobject = player.PlayerObject.transform.GetComponent<PlayerMovementBehaviour>();
                var mat = gameobject._renderer.material;
                mat.color = Color.red;

            }
        }


    }

    [ServerRpc]
    private void SetCharactersColorServerRpc()
    {
        Debug.Log($"owner : {OwnerClientId}");
        //}
    }

    
}
