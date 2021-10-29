using System;
using UnityEngine;

namespace LittleSubmarine2
{
    //Code from https://fistfullofshrimp.com/unity-drag-things-around/
    public class DragMouseMove : MonoBehaviour
    {
        //This is where we store the Plane for dragging objects
        private Plane draggingPlane;

        //This will store the difference between where the mouse is clicked
        //on the Plane and where the origin of the object is
        private Vector3 offset;

        //This will be used to cache to main camera
        //You could also use a serialized field to accomplish the same thing
        private Camera mainCamera;

        [SerializeField] private RectTransform safeSpace;
        private RectTransform selfRect;
        private Vector2 screenSize;

        private void Start()
        {
            selfRect = GetComponent<RectTransform>();
            // Cache the camera at the start. 
            mainCamera = Camera.main;
            screenSize = new Vector2(safeSpace.rect.width - selfRect.rect.width / 2, safeSpace.rect.height - selfRect.rect.height / 2);
        }

        private void Update()
        {
            if (transform.position.x < -(screenSize.x / 2))
            {
                transform.position += Vector3.right;
            }
            
            if (transform.position.x > screenSize.x / 2)
            {
                transform.position += Vector3.left;
            }
            
            if (transform.position.y < -(screenSize.y / 2))
            {
                transform.position += Vector3.up;
            }
            
            if (transform.position.y > screenSize.y / 2)
            {
                transform.position += Vector3.down;
            }
        }

        private void OnMouseDown()
        {
            draggingPlane = new Plane(mainCamera.transform.forward, 
                transform.position);
            Ray camRay = mainCamera.ScreenPointToRay(Input.mousePosition);

            float planeDistance;
            draggingPlane.Raycast(camRay, out planeDistance);
            offset = transform.position - camRay.GetPoint(planeDistance);
        }
        
        private void OnMouseDrag()
        {
            Ray camRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            float planeDistance;
            draggingPlane.Raycast(camRay, out planeDistance);
            transform.position = camRay.GetPoint(planeDistance) + offset;
        }
        
        
    }
}