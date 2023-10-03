using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemDetectionArea : NetworkBehaviour
{
    public int Id;
    public ItemBehaviour ItemBehaviour { get; private set; }

    public void Initialize(ItemBehaviour itemBehaviour)
    {
        ItemBehaviour = itemBehaviour;
    }
}
