using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Interactable
{
    public override void Interact()
    {
        base.Interact();
        Debug.Log("Collectible collected!");
        Destroy(gameObject);
    }
}
