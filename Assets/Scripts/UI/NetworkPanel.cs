using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkPanel : MonoBehaviour
{
    [SerializeField] private RelayManager _relayManager;
    [SerializeField] private InputField _joinCodeInput;

    #region Server Buttons
    public void StartServerBtn()
    {

        NetworkManager.Singleton.StartServer();
    }

    public async void StartHostBtn()
    {
        if (_relayManager.IsRelayEnabled)
            await _relayManager.SetupRelay();

        NetworkManager.Singleton.StartHost();
    }

    public async void StartClientBtn()
    {
        if (_relayManager.IsRelayEnabled && !string.IsNullOrEmpty(_joinCodeInput.text))
            await _relayManager.JoinRelay(_joinCodeInput.text);

        NetworkManager.Singleton.StartClient();
    }
    #endregion
}
