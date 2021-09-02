using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTouch : MonoBehaviour
{
    private TouchInputManager touchInputManager;
    private Camera cameraMain;
    
    private void Awake()
    {
        touchInputManager = TouchInputManager.Instance;
    }

    private void Start()
    {
        cameraMain = Camera.main;
    }

    private void OnEnable()
    {
        touchInputManager.OnStartTouch += Move;
    }

    private void OnDisable()
    {
        touchInputManager.OnEndTouch -= Move;
    }

    public void Move(Vector2 screenPosition, float time)
    {
        Vector3 screenCoordinates = new Vector3(screenPosition.x, screenPosition.y, cameraMain.nearClipPlane);
        Vector3 worldCoordinates = cameraMain.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0;
        transform.position = worldCoordinates;
    }
}
