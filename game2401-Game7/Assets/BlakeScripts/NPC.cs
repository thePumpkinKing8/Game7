using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class NPC : MonoBehaviour
{
    private AIPath _path;
    private AIDestinationSetter _destinationSetter;
    [SerializeField] private Transform[] _patrolPath;
    private int _routeIndex = 0; //the index of the transform the NPC should path to while on patrol

    private Coroutine _currentState;

    [SerializeField] private float _speed = 1f;

    [SerializeField] private GameObject _visionCone;
    private Rigidbody2D _rb;
    private void Awake()
    {
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
       // SetState(OnPatrol());
    }

    // Update is called once per frame
    void Update()
    {
        //_visionCone.transform.LookAt(_destinationSetter.target.position);
    }

    IEnumerator OnPatrol()
    {
        yield return new WaitForFixedUpdate();
        _destinationSetter.target = _patrolPath[_routeIndex];
        if(_routeIndex == _patrolPath.Length)
        {
            _routeIndex = 0;
        }
        else
        {
            _routeIndex++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hi");
    }


}
