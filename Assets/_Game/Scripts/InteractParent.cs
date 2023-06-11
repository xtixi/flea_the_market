using System.Collections;
using System.Collections.Generic;
using _Game.Scripts;
using UnityEngine;

public class InteractParent : MonoBehaviour, IInteractable
{
    public Item parent;

    public void OnMouseDown()
    {
        parent.OnMouseDown();
    }

    public void OnInteraction()
    {
        parent.OnInteraction();
    }

    public void UnInteraction()
    {
        parent.UnInteraction();
    }
}
