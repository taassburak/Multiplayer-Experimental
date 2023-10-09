using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Networking.Transport;

[Serializable]
public struct PublicEnvanterDatas : INetworkSerializable
{
    public int ItemId;
    public int ItemQuantity;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ItemId);
        serializer.SerializeValue(ref ItemQuantity);
    }

    public void UpdateDatas(int newValue)
    {
        ItemQuantity = newValue;
    }
}


[Serializable]
public class PublicEnvanterDataContainer
{
    public List<PublicEnvanterDatas> PublicEnvanterDatas;

    public PublicEnvanterDataContainer()
    {
        PublicEnvanterDatas = new List<PublicEnvanterDatas>();
    }
}
