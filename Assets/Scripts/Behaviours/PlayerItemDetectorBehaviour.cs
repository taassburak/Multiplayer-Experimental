using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Netcode;
using UnityEngine;

public struct PlayerBackPackDatas : INetworkSerializable
{

    public int HoldedItemId;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref HoldedItemId);
    }

}


public class PlayerItemDetectorBehaviour : NetworkBehaviour
{
    private bool _isBusy;

    [SerializeField] private PlayerController _playerController;

    private NetworkVariable<int> _currentDetectedItemAreaId = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Owner);

    private NetworkVariable<PlayerBackPackDatas> _playerBackPackDatas = new NetworkVariable<PlayerBackPackDatas>(

        new PlayerBackPackDatas
        {
            HoldedItemId = 0
        }
        );

    private ItemBehaviour _currentItemOnHand;

    private ItemDetectionArea _currentDetectedItemArea;

    public static event Action<PlayerController> OnHoldItem;
    public static event Action OnReleaseItem;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _playerBackPackDatas.OnValueChanged += (PlayerBackPackDatas previousData, PlayerBackPackDatas newData) => { Debug.Log($"newData is: {newData.HoldedItemId}"); };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner)
        {
            return;
        }
        if (other.gameObject.CompareTag("item"))
        {
            var item = other.gameObject.GetComponent<ItemDetectionArea>();
            _currentDetectedItemAreaId.Value = item.Id;
            //_currentDetectedItemArea = item;
            Debug.Log($"{OwnerClientId} CURRENT DETECTED ITEM AREA ID IS : {_currentDetectedItemAreaId.Value}");
        }
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_currentDetectedItemAreaId.Value != 0)
            {
                SetParentServerRpc(new ServerRpcParams());
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItemServerRpc(new ServerRpcParams());
        }
    }

    [ServerRpc]
    public void DropItemServerRpc(ServerRpcParams serverRpcParams)
    {
        var playerid = serverRpcParams.Receive.SenderClientId;
        var player = NetworkManager.Singleton.ConnectedClientsList.ToList().Find(x => x.ClientId == playerid);

        _playerBackPackDatas.Value = new PlayerBackPackDatas
        {
            HoldedItemId = 0
        };
    }

    [ServerRpc]
    public void SetParentServerRpc(ServerRpcParams serverRpcParams)
    {
        var playerid = serverRpcParams.Receive.SenderClientId;
        var player = NetworkManager.Singleton.ConnectedClientsList.ToList().Find(x => x.ClientId == playerid);

        //Debug.Log($"{OwnerClientId} client");
        //var items = GameObject.FindGameObjectsWithTag("item");
        //Debug.Log($"{items.Length} item count");
        //var item = items[playerid];
        //Debug.Log($"{item.name} item");
        //Debug.Log($"player {_playerController.name}");



        var items = GameObject.FindGameObjectsWithTag("item");

        var item = items.ToList().Find(x => x.GetComponent<ItemDetectionArea>().Id == _currentDetectedItemAreaId.Value);

        _currentItemOnHand = item.GetComponent<ItemDetectionArea>().ItemBehaviour;

        Debug.Log(_currentItemOnHand.name);

        _currentItemOnHand.HeldByPlayer(player.PlayerObject.GetComponent<PlayerController>());

        _playerBackPackDatas.Value = new PlayerBackPackDatas
        {
            HoldedItemId = _currentDetectedItemAreaId.Value
        };

    }




}
