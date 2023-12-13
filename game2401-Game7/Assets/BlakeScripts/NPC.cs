using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class NPC : MonoBehaviour
{
    private GameObject _player;
    private AIPath _path;
    private AIDestinationSetter _destinationSetter;
    private Seeker _seeker;
    [SerializeField] private Transform[] _patrolPath;
    private int _routeIndex = 1; //the index of the transform the NPC should path to while on patrol

    private Coroutine _currentState;

    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _chaseSpeed = 4f;

    [SerializeField] private GameObject _visionCone;
    private Rigidbody2D _rb;
    private Transform _playerTarget;
    private Vector3 _originalPosition;
    private void Awake()
    {
        _player = GameObject.Find("Player"); // Reference to the player
        _seeker = GetComponent<Seeker>();
        _path = GetComponent<AIPath>();
        _destinationSetter = GetComponent<AIDestinationSetter>();
        _path.maxSpeed = _speed;
        _rb = GetComponent<Rigidbody2D>();
    }

    private bool PlayerIsVisible(PlayerController player) // Casts a ray to see if the AI can see the player
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = gameObject.transform.position - _originalPosition; //makes the vision cone follow the ai's direction
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            _visionCone.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        _originalPosition = transform.position;

    }

    IEnumerator OnPatrol() //sets the target within the ai's patrol path
    {
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
        PlayerController player = _player.GetComponent<PlayerController>();

        if (PlayerIsVisible(player))
        {
            _path.maxSpeed = _chaseSpeed;
            _destinationSetter.target.position = _playerTarget.position;
        }
        else
        {
            yield return new WaitForSeconds(2);

            SetState(MoveToSearch());
            if (_path.reachedEndOfPath == true)
            {
                SetState(OnPatrol());
            }
        }

           
        //while (_playerVisible == true)
        //{
        //    yield return new WaitForFixedUpdate();
        //    if (_playerTarget.GetComponent<PlayerController>().Hidden == true)
        //    {
        //        _playerVisible = false;
        //    }
        //    else
           

                       
        
    }

    IEnumerator MoveToSearch()  //Ai moves to investigate noises
    {
        _path.maxSpeed = _speed;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (_path.reachedEndOfPath == true)
            {
                SetState(OnPatrol());
            }
        }
    }


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //Debug.Log("hi");
    //    if(collision.gameObject.tag == "Player")
    //    {
    //        _playerTarget = collision.gameObject.GetComponent<Transform>();
    //        _playerVisible = true;
    //        SetState(Chasing());
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = _player.GetComponent<PlayerController>();

        if(PlayerIsVisible(player))
        {
            _playerTarget = collision.gameObject.GetComponent<Transform>();
            SetState(Chasing());
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


    public void Investigate(Vector2 direction) // function triggered by "noises"
    {
        PlayerController player = _player.GetComponent<PlayerController>();

        if (!PlayerIsVisible(player)) //prevents player from stopping a chase by making noise
        {
            _destinationSetter.target.position = direction;
            SetState(MoveToSearch());
        }
    }

}
