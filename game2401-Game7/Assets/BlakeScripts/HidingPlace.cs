using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPlace : Interactable
{
    private PlayerController _player;
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<PlayerController>();
        onInteract.AddListener(_player.Hide);
    }
    public override void Interact()
    {
        
        if(_player.Hidden)
        {
            _animator.SetBool("Hiding", false);
        }
        else
        {
            Vector3 _distance = _player.gameObject.transform.position - transform.position;
            Debug.Log("hidden");
            _animator.SetFloat("X", _distance.x);
            _animator.SetFloat("Y", _distance.y);
            _animator.SetBool("Hiding", true);
            if (_distance.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            base.Interact();
        }
    }

    public void UnHide()
    {
        _player.UnHide();
    }
}
