using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent<GameObject> onInteract;

    public virtual void Interact()
    {
        onInteract.Invoke(gameObject);
    }
}

