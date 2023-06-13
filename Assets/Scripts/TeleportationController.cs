using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationController : MonoBehaviour
{
    [Header("Teleport Settings")]
    public float teleportRange; // Maximum teleport range
    public float groundAngleThreshold; // Angle threshold for considering the ground
    public float wallTopThreshold; // Distance threshold to check for a flat surface above the wall
    public float teleportOffset; // Offset to prevent teleporting inside walls or ceilings
    public float teleportDuration; // Duration of the teleportation transition
    public float playerSize;

    [Header("References")]
    public Camera cam; // Reference to the player's camera
    public GameObject tpParticleIndicator; // Teleport indicator
    public GameObject tpMaxRangeIndicator; // Max range indicator

    [Header("Light")]
    public Light mainDirLight;
    private List<Light> pointLights = new List<Light>();

    [Header("Layers")]
    public LayerMask teleportLayer; // Layer mask for teleportable surfaces
    public LayerMask cantTeleportLayer; // Layher mask to stop teleport

    [Header("Crosshair")]
    public GameObject crosshair_main;
    public GameObject crosshair_cant;
    public GameObject crosshair_up;

    private Vector3 teleportTarget; // Target position for teleportation
    private bool isTeleporting; // Flag indicating if the player is currently teleporting
    private float teleportTimer; // Timer for the teleportation transition

    private float oldFOV; // Old fov for smooth teleport animation
    private Vector3 oldPos; // Previous position for smooth movement interpolation
    private Vector3 playerOffset; // General offset so player isn't in the ground
    private Vector3 teleportPosition; // The location to teleport to
    private bool canTeleport; // Information if player can teleport

    private void Start()
    {
        oldFOV = cam.fieldOfView;
        tpParticleIndicator.SetActive(false);
        tpMaxRangeIndicator.SetActive(false);
        playerOffset = new Vector3(0, playerSize, 0);

        // Get all point lights in the scene
        foreach (Light light in FindObjectsOfType<Light>())
        {
            if (light.type == LightType.Point)
            {
                pointLights.Add(light);
            }
        }

        swapCrosshair(0);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1) && !isTeleporting)
        {
            if (GetComponent<PlayerMovement>().state.Equals(PlayerMovement.MovementState.crouching))
                playerOffset = new Vector3(0, playerSize/2, 0);
            else
                playerOffset = new Vector3(0, playerSize, 0);

            tpMaxRangeIndicator.SetActive(true);
            
            RaycastHit hit;
            RaycastHit shouldntHit;

            bool hBool = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, teleportRange, teleportLayer);
            bool shBool = Physics.Raycast(cam.transform.position, cam.transform.forward, out shouldntHit, teleportRange, cantTeleportLayer);
            if (hBool && shBool)
                shBool = shouldntHit.distance <= hit.distance;

            if (hBool && !shBool && IsTargetInShadow(hit.point + hit.normal))
            {
                canTeleport = true;
                teleportPosition = hit.point + playerOffset;
                swapCrosshair(0);

                // Calculate the angle between the surface normal and the up direction
                float angle = Vector3.Angle(hit.normal, Vector3.up);

                if (angle >= groundAngleThreshold)
                {
                    // Offset the hit point to prevent teleporting inside walls or ceilings
                    Vector3 offset = hit.normal * teleportOffset;
                    teleportPosition += offset;

                    // Check if there is a flat surface above the wall
                    if (Physics.SphereCast(hit.point + Vector3.up * wallTopThreshold - new Vector3(hit.normal.x, 0, hit.normal.z), 1f, Vector3.down, out RaycastHit wallTopHit, teleportRange, teleportLayer))
                    {
                        // Calculate the distance between the hit point and the top of the wall
                        float wallTopDistance = Vector3.Distance(hit.point, wallTopHit.point);

                        // Calculate the angle between the surface normal and the up direction
                        float topAngle = Vector3.Angle(wallTopHit.normal, Vector3.up);

                        // Teleport the player on top of the wall if close to the top
                        if (wallTopDistance <= wallTopThreshold && topAngle <= 40)
                        {
                            teleportPosition = wallTopHit.point + playerOffset;
                            swapCrosshair(2);
                        }
                    }
                }
                tpParticleIndicator.SetActive(true);
                tpParticleIndicator.transform.position = teleportPosition - playerOffset * 0.9f;
            }
            else
            {
                canTeleport = false;
                tpParticleIndicator.SetActive(false);
                swapCrosshair(1);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            tpMaxRangeIndicator.SetActive(false);
            swapCrosshair(0);
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && canTeleport && !isTeleporting)
        {
            swapCrosshair(0);
            tpParticleIndicator.SetActive(false);
            // Start the teleportation transition
            oldPos = transform.position;
            isTeleporting = true;
            teleportTimer = 0f;
            teleportTarget = teleportPosition;
        }

        // Perform the teleportation transition
        if (isTeleporting)
        {
            teleportTimer += Time.deltaTime;
            float t = teleportTimer / teleportDuration;

            float smooth = Mathf.SmoothStep(0, 1, t);

            if (t < 0.5f)
                cam.fieldOfView = Mathf.Lerp(oldFOV, oldFOV + 20, smooth);
            else
                cam.fieldOfView = Mathf.Lerp(oldFOV + 20, oldFOV, smooth);

            // Move the player smoothly towards the teleport target
            transform.position = Vector3.Lerp(oldPos, teleportTarget, smooth);

            // Check if the teleportation transition is complete
            if (t >= 1.1f)
            {
                isTeleporting = false;
            }
        }
    }

    private bool IsTargetInShadow(Vector3 targetPosition)
    {
        // Cast a ray towards the directional light
        Vector3 rayDirection = -mainDirLight.transform.forward;
        //Debug.DrawRay(targetPosition, rayDirection, Color.white, 20.0f);
        if (Physics.SphereCast(targetPosition, 0.8f, rayDirection, out RaycastHit hit, 200, teleportLayer))
        {
            // If an object is hit between the target position and the light, consider it in shadow
            if (hit.transform != null && hit.transform != mainDirLight.transform)
            {
                return true;
            }
        }
        return false;
    }

    private void swapCrosshair(int index)
    {
        switch (index) {
            case 0:
                crosshair_main.SetActive(true);
                crosshair_cant.SetActive(false);
                crosshair_up.SetActive(false);
                break;
            case 1:
                crosshair_main.SetActive(false);
                crosshair_cant.SetActive(true);
                crosshair_up.SetActive(false);
                break;
            case 2:
                crosshair_main.SetActive(false);
                crosshair_cant.SetActive(false);
                crosshair_up.SetActive(true);
                break;
            default:
                break;
        }
    }
}


