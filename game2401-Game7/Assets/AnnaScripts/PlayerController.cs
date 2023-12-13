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
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        oldSpeed = moveSpeed;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        
        //animation
        _animator.SetFloat("X", horizontalInput);
        _animator.SetFloat("Y", verticalInput);

        if(horizontalInput != 0 || verticalInput != 0)
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
            interact.Interact();
            
        }
        moveSpeed = oldSpeed;
    }


    //hides the player from the ai and places the player at/under the object
    public void Hide(GameObject _gameObject)
    {
        if(Hidden == true)
        {
            Debug.Log("unHidden");
            Hidden = false;
            transform.position = _position;
            GetComponent<Collider2D>().enabled = true;
            moveSpeed = oldSpeed;
            _spriteRenderer.sortingOrder = 2;
        }
        else
        {
            Hidden = true;
            _spriteRenderer.sortingOrder=0;
            GetComponent<Collider2D>().enabled = false;
            _position = transform.position;
            transform.position = _gameObject.transform.position;
            moveSpeed = 0;
        }
    }
}