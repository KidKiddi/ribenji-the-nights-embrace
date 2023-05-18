using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInteraction : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("BRUH");
        this.gameObject.SetActive(false);
    }
}
