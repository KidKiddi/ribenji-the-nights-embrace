using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PosterInteraction : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        this.gameObject.SetActive(false);
    }
}
