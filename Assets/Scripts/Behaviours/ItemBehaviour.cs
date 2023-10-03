using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemBehaviour : NetworkBehaviour
{
    [SerializeField]private ItemDetectionArea _itemDetectionArea;
    private NetworkVariable<bool> _isBusy = new NetworkVariable<bool>();

    private void Start()
    {
        _itemDetectionArea.Initialize(this);

        
    }

    public void SetBusyness(bool isBusy)
    {
        _isBusy.Value = isBusy;
    }

    
    public void HeldByPlayer(PlayerController player)
    {
        if (_isBusy.Value) return;

        transform.GetComponent<NetworkObject>().TrySetParent(player.transform);
        transform.localPosition = Vector3.zero;
    }

    public void ReleasedByPlayer()
    {
        _isBusy.Value = false;

        transform.SetParent(null);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    //[ClientRpc]
    //private void SpellWarmUpClientRpc(ulong targetNetPlayerObjId)
    //{
    //    Transform senderFirepoint = GetNetworkObject(targetNetPlayerObjId).GetComponent<PlayerController>().transform;
    //    var warmUpObj = Instantiate(warmUp, senderFirepoint.position, Quaternion.identity);
    //    warmUpObj.transform.parent = senderFirepoint;
    //}
}
