using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform playerTF;
    
    private void Update() // Justin
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            transform.position = raycastHit.point + (Vector3.up);
        }


        Vector3 lookDir = new Vector3(playerTF.position.x, transform.position.y, playerTF.position.z);
        transform.LookAt(lookDir);

    }
    
    
}
