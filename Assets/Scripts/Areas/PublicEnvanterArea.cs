using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicEnvanterArea : MonoBehaviour, IInteract
{
    public GameObject EnvanterPanel;

    public void ExitInteraction()
    {
        EnvanterPanel.gameObject.SetActive(false);
        Debug.Log("Panel closed");
    }

    public void Interact()
    {
        EnvanterPanel.gameObject.SetActive(true);
        Debug.Log("Panel activated");
    }
}
