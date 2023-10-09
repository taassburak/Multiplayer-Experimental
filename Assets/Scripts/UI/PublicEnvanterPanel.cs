using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PublicEnvanterPanel : NetworkBehaviour
{
    [SerializeField] private ShopManager _shopMamager;
    [SerializeField] private Text _text;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += () => { };
    }

    public override void OnNetworkSpawn()
    {
        gameObject.SetActive(false);
    }

    public void TryToBuyItemButton(int id)
    {
        _shopMamager.RequestBuyingItem(id);
    }

    private void Update()
    {
        //Debug.Log(IsOwner);
    }

    public void PopulateView()
    {
        //_text.text = _shopMamager.GetQuantity(0);
    }
}
