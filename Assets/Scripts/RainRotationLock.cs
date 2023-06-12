using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainRotationLock : MonoBehaviour
{
    private Quaternion iniRot;

    // Start is called before the first frame update
    void Start()
    {
        iniRot = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = iniRot;
    }
}
