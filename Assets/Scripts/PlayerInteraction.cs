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
        UnityEngine.Debug.Log("Collision");
        if (other.gameObject.CompareTag("Enemy"))
        {
            UnityEngine.Debug.Log("Collision");
        }

    }

}
