using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationController : MonoBehaviour
{
    public Camera cam; // Reference to the player's camera
    public float teleportRange; // Maximum teleport range
    public LayerMask teleportLayer; // Layer mask for teleportable surfaces
    public LayerMask cantTeleportLayer; // Layher mask to stop teleport
    public float groundAngleThreshold; // Angle threshold for considering the ground
    public float wallTopThreshold; // Distance threshold to check for a flat surface above the wall
    public float teleportOffset; // Offset to prevent teleporting inside walls or ceilings
    public float teleportDuration; // Duration of the teleportation transition

    public GameObject particles; // Teleport indicator
    public GameObject tpMaxRangeIndicator; // Max range indicator
    public GameObject crosshair; // Public crosshair reference
    private Material crosshairMat; // Material to change crosshair
    public Light mainDirLight;
    private List<Light> pointLights = new List<Light>();

    private Vector3 teleportTarget; // Target position for teleportation
    private bool isTeleporting; // Flag indicating if the player is currently teleporting
    private float teleportTimer; // Timer for the teleportation transition

    private float oldFOV; // Old fov for smooth teleport animation
    private Vector3 oldPos; // Previous position for smooth movement interpolation
    private Vector3 playerOffset = new Vector3(0,1,0); // General offset so player isn't in the ground
    private Vector3 teleportPosition; // The location to teleport to
    private bool canTeleport; // Information if player can teleport

    private void Start()
    {
        oldFOV = cam.fieldOfView;
        particles.SetActive(false);
        tpMaxRangeIndicator.SetActive(false);
        crosshairMat = crosshair.GetComponent<Renderer>().sharedMaterial;

        // Get all point lights in the scene
        foreach (Light light in FindObjectsOfType<Light>())
        {
            if (light.type == LightType.Point)
            {
                pointLights.Add(light);
            }
        }
        Debug.Log("There are " + pointLights.Count + " point lights in the scene.");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1) && !isTeleporting)
        {
            tpMaxRangeIndicator.SetActive(true);
            //RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, teleportRange, teleportLayer) && IsTargetInShadow(hit.point + hit.normal) && !Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit shouldntHit, teleportRange, cantTeleportLayer))
            {
                canTeleport = true;
                teleportPosition = hit.point + playerOffset;
                crosshairMat.SetFloat("_Index", 0.0f);

                // Calculate the angle between the surface normal and the up direction
                float angle = Vector3.Angle(hit.normal, Vector3.up);
                /*if (IsTargetInShadow(hit.point + hit.normal))
                    Debug.Log("Is In Shadow");*/
                //Debug.Log("wallA: " + angle);

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
                        //Debug.Log("topA: " + topAngle + ", topDis: " + wallTopDistance);

                        // Teleport the player on top of the wall if close to the top
                        if (wallTopDistance <= wallTopThreshold && topAngle <= 40)
                        {
                            //Debug.Log("TopTP");
                            teleportPosition = wallTopHit.point + playerOffset;
                            crosshairMat.SetFloat("_Index", 4.0f);
                        }
                    }
                }
                particles.SetActive(true);
                particles.transform.position = teleportPosition - playerOffset * 0.9f;
            }
            else
            {
                canTeleport = false;
                particles.SetActive(false);
                crosshairMat.SetFloat("_Index", 1.0f);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            tpMaxRangeIndicator.SetActive(false);
            crosshairMat.SetFloat("_Index", 0.0f);
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && canTeleport && !isTeleporting)
        {
            crosshairMat.SetFloat("_Index", 0.0f);
            particles.SetActive(false);
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
        Debug.DrawRay(targetPosition, rayDirection, Color.white, 20.0f);
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
}


