using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject _question;
    [SerializeField] private GameObject _exclaim;
    private Animator _animator;
    private Vector3 _lastDirection;
    private GameObject _player;
    private AIPath _path;
    private AIDestinationSetter _destinationSetter;
    private Seeker _seeker;
    [SerializeField] private Transform[] _patrolPath;
    private int _routeIndex = 1; //the index of the transform the NPC should path to while on patrol

    private Coroutine _currentState;
    private bool _isChasing;

    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _chaseSpeed = 4f;
    [SerializeField] private float _searchTime = 2f;

    [SerializeField] private GameObject _visionCone;
    private float _coneLength = 11.0f;

    private Rigidbody2D _rb;
    private Transform _playerTarget;
    private Vector3 _originalPosition;
    private void Awake()
    {
        _question.SetActive(false);
        _exclaim.SetActive(false);
        _animator = GetComponentInChildren<Animator>();
        _player = GameObject.Find("Player"); // Reference to the player
        _seeker = GetComponent<Seeker>();
        _path = GetComponent<AIPath>();
        _destinationSetter = GetComponent<AIDestinationSetter>();
        _path.maxSpeed = _speed;
        _rb = GetComponent<Rigidbody2D>();
    }

    private bool PlayerIsVisible(PlayerController player) // Casts a ray to see if the AI can see the player
    {
        if(player.Hidden == true)
        {
            return false;
        }
        bool playerVisible = true;
        Vector2 direction = player.transform.position - this.transform.position;
        float distance = direction.magnitude;
        direction.Normalize();
        RaycastHit2D hit = new RaycastHit2D();

        if(Physics2D.Raycast(this.transform.position, direction, distance, LayerMask.GetMask("Obstacle")))
        {
            playerVisible = false;
        }
        return playerVisible;
    }

    private void SetState(IEnumerator newState) //when we change states, we stop our previous coroutine and then initialize a new one. Technically this can be done with "StopAllCoroutines()" because we only have one coroutine running, but in the case that we had more, we will only a stop a specific coroutine.
    {
        if (_currentState != null)
        {
            StopCoroutine(_currentState);
        }
        _currentState = StartCoroutine(newState);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SetState(OnPatrol());
        _originalPosition = transform.position;
        PlayerIsVisible(_player.GetComponent<PlayerController>());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = gameObject.transform.position - _originalPosition; //makes the vision cone follow the ai's direction
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            _visionCone.transform.rotation = Quaternion.Lerp(_visionCone.transform.rotation,Quaternion.AngleAxis(angle,Vector3.forward),3f); //change to lerp
        }
        _originalPosition = transform.position;
        

        Vector3 velocity = _path.desiredVelocity;
        //animation
        if(velocity == Vector3.zero)
        {
            _animator.SetBool("Moving", false);
            _animator.SetFloat("X", _lastDirection.normalized.x);
            _animator.SetFloat("Y", _lastDirection.normalized.y);
        }
        else
        {
            _animator.SetBool("Moving", true);
            _animator.SetFloat("X",velocity.normalized.x);
            _animator.SetFloat("Y", velocity.normalized.y);
            _lastDirection = velocity.normalized;
        }
        if(velocity.normalized.x < 0)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = false;
          
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        ConeScale(velocity.normalized);
    }

    void ConeScale(Vector3 moveDirection)
    {
        RaycastHit2D hit = new RaycastHit2D();
        hit = Physics2D.Raycast(this.transform.position, moveDirection, _coneLength, LayerMask.GetMask("Obstacle"));
        if(hit)
        {
            Vector3 point = hit.point;
            float distanceScale = ( point - transform.position).magnitude/_coneLength;
            _visionCone.transform.localScale = new Vector3 (distanceScale, distanceScale, distanceScale);
        }
    }

    IEnumerator OnPatrol() //sets the target within the ai's patrol path
    {
        _question.SetActive(false);
        _exclaim.SetActive(false);
        Debug.Log("patrolling");
        _path.maxSpeed = _speed;
        yield return new WaitForFixedUpdate();
        _destinationSetter.target = _patrolPath[_routeIndex];
        Debug.Log(_routeIndex);
        SetState(MovingPatrol());
    }

    IEnumerator MovingPatrol() //has the AI move to the target and set a new routeIndex
    {
        _path.maxSpeed = _speed;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (_path.reachedEndOfPath == true)
            {
                if (_routeIndex == _patrolPath.Length - 1)
                {
                    _routeIndex = 0;
                }
                else
                {
                    _routeIndex++;
                }
                SetState(OnPatrol());
            }
        }
    }

    IEnumerator Chasing() //Ai chases the player for as long as they remain visible to it
    {
        _isChasing = true;
        AudioManager.Instance.PlayChaseMusic();
        _question.SetActive(false);
        _exclaim.SetActive(true);
        Debug.Log("chasing");
        PlayerController player = _player.GetComponent<PlayerController>();
        while(true)
        {
            yield return new WaitForFixedUpdate();
            if (PlayerIsVisible(player))
            {
                _path.maxSpeed = _chaseSpeed;
                _destinationSetter.target = _player.transform;
            }
            else
            {
                yield return new WaitForSeconds(2);
                if(!PlayerIsVisible(player))
                {
                    AudioManager.Instance.PlayGameMusic();
                    _isChasing = false;
                    SetState(Search());
                }
            }
        }
    }

    IEnumerator MoveToSearch()  //Ai moves to investigate noises
    {
        AudioManager.Instance.PlayHmm();
        _question.SetActive(true);
        _exclaim.SetActive(false);
        Debug.Log("investigate");
        _path.maxSpeed = _speed;
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (_path.reachedEndOfPath == true)
            {
                SetState(Search());
            }
        }
    }

    IEnumerator Search() 
    {
        _question.SetActive(true);
        _exclaim.SetActive(false);
        Debug.Log("searching");
        yield return new WaitForSeconds(_searchTime);
        SetState(OnPatrol());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = _player.GetComponent<PlayerController>();

        if(collision.tag == "Player")
        {
            if (PlayerIsVisible(player))
            {
                SetState(Chasing());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = _player.GetComponent<PlayerController>();

        if (collision.gameObject.tag == "Player")
        {
            PlayerIsVisible(player);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("got");
            GameManager.Instance.Lose();
        }
    }


    public void Investigate(Vector2 direction) // function triggered by "noises"
    {
        PlayerController player = _player.GetComponent<PlayerController>();
        if (!_isChasing) //prevents player from stopping a chase by making noise
        {
            Debug.Log("hi");
            _destinationSetter.target.position = direction;
            SetState(MoveToSearch());
        }
    }

}
