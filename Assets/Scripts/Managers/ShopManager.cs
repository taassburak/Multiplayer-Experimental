using System;
using System.Buffers;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ShopManager : NetworkBehaviour
{
    public List<Item> Items;

    public PublicEnvanterDataContainer PublicEnvanterDataContainer;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }
    public void RequestBuyingItem(int itemId)
    {
        var item = Items.Find(x => x.GetData().Value.Id == itemId);
        var itemQuantity = item.GetData().Value.Quantity;
        var id = item.GetData().Value.Id;
        itemQuantity++;
        item.UpdateItemDataServerRpc();
    }

    public string GetQuantity(int itemId)
    {
        var item = Items.Find(x => x.GetData().Value.Id == itemId);
        return item.GetData().Value.Quantity.ToString();
    }


    public void Test(int i)
    {
        TestDataContainerServerRpc(i);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TestDataContainerServerRpc(int index)
    {
        PublicEnvanterDataContainer.PublicEnvanterDatas[index].UpdateDatas(3);
    }
}
