using System.Collections;
using UnityEngine;
using TMPro;
interface IInteractable
{
    public void Interact();
}

public class PlayerInteraction : MonoBehaviour
{
    public Transform cam;
    public float activationDistance;

    public GameObject interactionText;
    private int pgPsTotal = 0;
    private int pgPsFound = 0;

    public LevelStateController levelStateController;

    [Header("Crosshair")]
    public GameObject crosshair_main;
    public GameObject crosshair_cant;
    public GameObject crosshair_up;

    private static int counter = 0;
    
    public int collectPoster = 2;
    public TMP_Text counterText;

    private void Start()
    {
        pgPsTotal = GameObject.FindGameObjectsWithTag("PropagandaPoster").Length;
        Debug.Log("There are " + pgPsTotal + " Propaganda Posters.");
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out RaycastHit hit, activationDistance) && hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
        {
            interactionText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                counter++;
                counterText.text = counter + "/20";
                interactObj.Interact();
                pgPsFound++;
            }
        }
        else
            interactionText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Deathbox"))
        {
            disableCrosshair();
            levelStateController.ShowGameOverScreen();
        }
        else if (other.gameObject.CompareTag("Win") && counter >= collectPoster)
        {
            disableCrosshair();
            levelStateController.ShowWonScreen();
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Deathbox"))
        {
            disableCrosshair();
            levelStateController.ShowGameOverScreen();
        }
        else if (other.gameObject.CompareTag("Win") && counter >= collectPoster)
        {
            disableCrosshair();
            levelStateController.ShowWonScreen();
        }

    }

    private void disableCrosshair()
    {
        crosshair_main.SetActive(false);
        crosshair_cant.SetActive(false);
        crosshair_up.SetActive(false);
    }
}
