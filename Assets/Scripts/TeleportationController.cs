using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationController : MonoBehaviour
{
    public Transform cam; // Reference to the player's camera
    public float teleportRange; // Maximum teleport range
    public LayerMask teleportLayer; // Layer mask for teleportable surfaces
    public float groundAngleThreshold; // Angle threshold for considering the ground
    public float wallTopThreshold; // Distance threshold to check for a flat surface above the wall
    public float teleportOffset; // Offset to prevent teleporting inside walls or ceilings
    public float teleportDuration; // Duration of the teleportation transition

    private Vector3 teleportTarget; // Target position for teleportation
    private bool isTeleporting; // Flag indicating if the player is currently teleporting
    private float teleportTimer; // Timer for the teleportation transition

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && !isTeleporting)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, teleportRange, teleportLayer))
            {
                Vector3 teleportPosition = hit.point + new Vector3(0,1,0);

                // Calculate the angle between the surface normal and the up direction
                float angle = Vector3.Angle(hit.normal, Vector3.up);

                if (angle >= groundAngleThreshold)
                {
                    // Offset the hit point to prevent teleporting inside walls or ceilings
                    Vector3 offset = hit.normal * teleportOffset;
                    teleportPosition += offset;

                    // Check if there is a flat surface above the wall
                    if (Physics.Raycast(hit.point + Vector3.up * wallTopThreshold, Vector3.down, out RaycastHit wallTopHit, teleportRange, teleportLayer))
                    {
                        // Calculate the distance between the hit point and the top of the wall
                        float wallTopDistance = Vector3.Distance(hit.point, wallTopHit.point);

                        // Teleport the player on top of the wall if close to the top
                        if (wallTopDistance <= wallTopThreshold)
                        {
                            teleportPosition = wallTopHit.point + new Vector3(0,1,0);
                        }
                    }
                }

                // Start the teleportation transition
                isTeleporting = true;
                teleportTimer = 0f;
                teleportTarget = teleportPosition;
            }
        }

        // Perform the teleportation transition
        if (isTeleporting)
        {
            teleportTimer += Time.deltaTime;
            float t = teleportTimer / teleportDuration;

            // Move the player smoothly towards the teleport target
            transform.position = Vector3.Lerp(transform.position, teleportTarget, t);

            // Check if the teleportation transition is complete
            if (t >= 1.2f)
            {
                isTeleporting = false;
            }
        }
    }
}


