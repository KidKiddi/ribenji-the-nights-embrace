using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosterInteraction : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Removed Poster");
        this.gameObject.SetActive(false);
    }
}
