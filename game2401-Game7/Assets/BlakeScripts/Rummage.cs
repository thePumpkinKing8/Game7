using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rummage : Interactable
{
    [SerializeField] private int _value = 50;
    [SerializeField] private float _soundSize = 10f;
    public override void Interact()
    {
        Debug.Log("rummaging");
        GameManager.Instance.UpdateScore(_value);
        MakeNoise();
        AudioManager.Instance.PlayRummage();
    }

    private void MakeNoise()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _soundSize);
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
