using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    
    private bool _moveForward = true;

    private void Update()
    {
        transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
        transform.Rotate(transform.forward, rotationSpeed * Time.deltaTime);
    }
}
