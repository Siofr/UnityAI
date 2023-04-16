using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLookAtCamera : MonoBehaviour
{
    private Transform sceneCamera;

    // Start is called before the first frame update
    void Start()
    {
        sceneCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position - (sceneCamera.position - transform.position));
    }
}
