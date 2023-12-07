using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightSwitch : Interactable
{
    public override void Interact()
    {
        base.Interact();
        Debug.Log("Light switch toggled!");
    }
}
