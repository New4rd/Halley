using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraPosition : MonoBehaviour
{
    [SerializeField] private Transform camera;

    private void Update()
    {
        transform.position = camera.transform.position;
    }
}
