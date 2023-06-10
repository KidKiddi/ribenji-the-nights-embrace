using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class MinimapController : MonoBehaviour
{

    public Transform player;
    public float cameraHeight;
    
    void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y += cameraHeight;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
