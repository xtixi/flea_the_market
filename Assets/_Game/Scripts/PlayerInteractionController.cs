using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private RectTransform cursor;
    [SerializeField] private float maxDistance;
    [SerializeField] private IInteractable currentInteractable;
    //[SerializeField] private List<KeyToButtonConverter> ;
    
    //public void ControlKey()
    //{
    //    
    //}

    private void Update()
    {
        if (currentInteractable != null && Input.GetMouseButtonDown(0))
        {
            currentInteractable.OnMouseDown();
        }
        if (Time.frameCount % 5 != 0)
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(cursor.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                interactable.OnInteraction();
                if (currentInteractable != null && currentInteractable != interactable )
                {
                    currentInteractable.UnInteraction();
                }
                currentInteractable = interactable;
            }
            else
            {
                if (currentInteractable != null)
                {
                    currentInteractable.UnInteraction();
                    currentInteractable = null;
                }
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                currentInteractable.UnInteraction();
                currentInteractable = null;
            }
        }
    }
}
