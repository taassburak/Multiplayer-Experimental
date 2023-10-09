using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ItemData : INetworkSerializable
{
    public int Id;
    public int Quantity;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Id);
        serializer.SerializeValue(ref Quantity);
    }
}

public class Item : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<ItemData> ItemData = new NetworkVariable<ItemData>(

        new ItemData { Id = 0, Quantity = 0 },
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    [SerializeField] private Text _quantityText;

    private void Start()
    {
        //NetworkManager.Singleton.OnServerStarted += () => { PopulateView(); };
        ItemData.OnValueChanged += (ItemData previousData, ItemData newData) =>
        {
            _quantityText.text = newData.Quantity.ToString();
        };
    }

    private void PopulateView()
    {
        
    }

    public override void OnNetworkSpawn()
    {


        //GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateItemDataServerRpc()
    {
        Debug.Log("owner id " + OwnerClientId);
        //if (!IsOwner)
        //{
        //    return;
        //}
        var a = ItemData.Value.Quantity;
        var b = ItemData.Value.Id;
        ItemData.Value = new ItemData
        {
            Id = b,
            Quantity = a + 1
        };
    }

    public NetworkVariable<ItemData> GetData()
    {
        return ItemData;
    }
}
