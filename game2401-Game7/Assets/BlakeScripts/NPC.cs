using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class NPC : MonoBehaviour
{
    private AIPath _path;
    private AIDestinationSetter _destinationSetter;
    private Seeker _seeker;
    [SerializeField] private Transform[] _patrolPath;
    private int _routeIndex = 0; //the index of the transform the NPC should path to while on patrol

    private Coroutine _currentState;

    [SerializeField] private float _speed = 1f;

    [SerializeField] private GameObject _visionCone;
    private Rigidbody2D _rb;
    private Transform _playerTarget;

    private Vector3 _originalPosition;
    private void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _path = GetComponent<AIPath>();
        _destinationSetter = GetComponent<AIDestinationSetter>();
        _path.maxSpeed = _speed;
        _rb = GetComponent<Rigidbody2D>();
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
        Vector3 moveDirection = gameObject.transform.position - _originalPosition;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            _visionCone.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        _originalPosition = transform.position;
    }

    IEnumerator OnPatrol()
    {
        yield return new WaitForFixedUpdate();
        _destinationSetter.target = _patrolPath[_routeIndex];
        Debug.Log(_routeIndex);
        SetState(MovingPatrol());
    }

    IEnumerator MovingPatrol()
    {
        
        while(true)
        {
            yield return new WaitForFixedUpdate();
            if (transform.position == _patrolPath[_routeIndex].position)
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

    IEnumerator Chasing()
    {
        while(true)
        {
            yield return new WaitForFixedUpdate();
            _destinationSetter.target = _playerTarget;
        }
    }

    IEnumerator MoveToSearch()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (transform.position == _destinationSetter.target.position)
            {
                SetState(OnPatrol());
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hi");
        if(collision.gameObject.tag == "Player")
        {
            _playerTarget = collision.gameObject.GetComponent<Transform>();
            SetState(Chasing());
        }
    }


    public void Investigate(Transform transform)
    {
        _destinationSetter.target = transform;
        SetState(MoveToSearch());
    }

}
