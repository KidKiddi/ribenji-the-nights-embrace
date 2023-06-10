using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class PlayerInteraction : MonoBehaviour
{
    public Transform cam;
    public float activationDistance;
    bool active;

    public LevelStateController levelStateController;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        active = Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, activationDistance);

        if (Input.GetKeyDown(KeyCode.F) && active)
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactObj.Interact();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            levelStateController.ShowGameOverScreen();
        }
        else if (other.gameObject.CompareTag("Win"))
        {
            levelStateController.ShowWonScreen();
        }

    }

}
