using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPlace : Interactable
{
    private void Start()
    {
        onInteract.AddListener(FindObjectOfType<PlayerController>().Hide);
    }
    public override void Interact()
    {
        Debug.Log("hidden");
        base.Interact();
    }
}
