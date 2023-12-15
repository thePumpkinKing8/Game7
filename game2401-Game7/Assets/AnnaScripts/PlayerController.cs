using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float oldSpeed;

    [HideInInspector] public bool Hidden;
    private Vector2 _position;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _idleDirection;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        oldSpeed = moveSpeed;
    }
    private void Start()
    {
        AudioManager.Instance.PlayGameMusic();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        //animation
        _animator.SetFloat("X", horizontalInput);
        _animator.SetFloat("Y", verticalInput);
        if (horizontalInput != 0 || verticalInput != 0)
        {
            _idleDirection = new Vector2(horizontalInput, verticalInput);
        }

        if (horizontalInput != 0 || verticalInput != 0)
        {
            _animator.SetBool("Moving", true);
            if(horizontalInput > 0)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
        }
        else
        {
            _animator.SetBool("Moving", false);
            _animator.SetFloat("X", _idleDirection.x);
            _animator.SetFloat("Y", _idleDirection.y);
        }



        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f);
        movement.Normalize();

        transform.position += movement * moveSpeed * Time.deltaTime;



        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartInteract();
        }
    }
    private void FixedUpdate()
    {
        
    }

    //allows the player to interact with objects that have a "Interactable" monobehavior
    private void StartInteract()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, Vector2.one, 0f);
        foreach (Collider2D collider in colliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            Rummage rummage = collider.GetComponent<Rummage>();
            if (interactable != null)
            {
                _animator.SetTrigger("Rummage");
                interactable.Interact();
                if (Input.GetKey(KeyCode.Space) && rummage != null) //starts the rummage coroutine if the interactable can be rummaged through
                {
                    StartCoroutine(Rummage(interactable));
                }
            }
        }
    }

    private IEnumerator Rummage(Interactable interact) //repeates the interact function for as long as the interact button is held down
    {
        moveSpeed = 0;
        while (Input.GetKey(KeyCode.Space))
        {
            yield return new WaitForSeconds(1);
            _animator.SetTrigger("Rummage");
            interact.Interact();
            
        }
        moveSpeed = oldSpeed;
        AudioManager.Instance.StopRummage();
    }


    //hides the player from the ai and places the player at/under the object
    public void Hide()
    {
        
        oldSpeed = moveSpeed;
        Hidden = true;
        Debug.Log(Hidden);
        GetComponent<Collider2D>().enabled = false;
        _spriteRenderer.enabled = false;
        moveSpeed = 0;
        
    }
    public void UnHide()
    {
        Hidden = false;
        Debug.Log(Hidden);
        GetComponent<Collider2D>().enabled = true;
        _spriteRenderer.enabled = true;
        moveSpeed = oldSpeed;
    }

}