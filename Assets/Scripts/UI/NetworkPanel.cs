using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPanel : MonoBehaviour
{

    #region Server Buttons
    public void StartServerBtn()
    {
        NetworkManager.Singleton.StartServer();
    }

    public void StartHostBtn()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClientBtn()
    {
        NetworkManager.Singleton.StartClient();
    }
    #endregion
}
