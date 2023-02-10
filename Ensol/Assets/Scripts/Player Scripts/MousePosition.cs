using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;

    
    private void Update() // Justin
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            transform.position = raycastHit.point;
        }
    }
    
    /*
    public Vector3 worldPosition;
    Plane plane = new Plane(Vector3.up, 0);

    void Update()
    {
        float distance;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }
    }
    */
}
