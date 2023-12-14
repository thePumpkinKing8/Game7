using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Interactable
{
    [SerializeField] private int _value = 100;
    [SerializeField] private float _soundSize = 10f;
    public override void Interact()
    {
        base.Interact();
        Debug.Log("Collectible collected!");
        GameManager.Instance.UpdateScore(_value);
        MakeNoise();
        Destroy(gameObject);
    }

    private void MakeNoise()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,_soundSize);
        foreach (Collider2D collider in colliders)
        {
            NPC enemy = collider.GetComponent<NPC>();
            if (enemy != null)
            {
               enemy.Investigate(transform.position);              
            }
        }
    }
}
