using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Blob : MonoBehaviour
{

    public Transform EyePosition;
    public Transform[] Points;

    private NavMeshAgent _agent;

    public void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
    }

	public float ArrivedDelta = 0.2f;

    public void Update()
    {
        PlayerVisible = CanSeePlayer();

        if (Vector3.Distance(transform.position, _currentDestination) <= ArrivedDelta) 
        {
            // Arrived
            UpdateDestination();
        }

    }

    private bool _playerVisible;
    protected bool PlayerVisible
    {
        get { return _playerVisible;}
        set
        {
            if (_playerVisible == value) return;
            _playerVisible = value;
            if (_playerVisible)
            {
                _lastVisiblePlayer?.VisibleByBlobStart(this);
            }
            else 
            {
                _lastVisiblePlayer?.VisibleByBlobStop(this);
            }
        }
    }
    

    private int _pointIndex = -1;
    private int _pointIncrement = 1;
    private Vector3 _currentDestination;

    private void UpdateDestination()
    {
        _pointIndex += _pointIncrement;
        if (_pointIndex < 0 || _pointIndex == Points.Length)
        {
            _pointIncrement *= -1;
            _pointIndex += 2 * _pointIncrement;
        }
        _currentDestination = Points[_pointIndex].position;
        _agent.SetDestination(_currentDestination);
    }


    private PlayerTest _playerInCollider;
    private PlayerTest _lastVisiblePlayer;

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("OnTriggerEnter " + other.gameObject.name);
        var player = other.gameObject.GetComponent<PlayerTest>();
        if (player != null) {
            _playerInCollider = player;
            _lastVisiblePlayer = player;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Debug.Log("OnTriggerExit " + other.gameObject.name);
        var player = other.gameObject.GetComponent<PlayerTest>();
        if (player != null) {
            _playerInCollider = null;
        }
    }

    private bool CanSeePlayer() {

        #if GAME_DEBUG
            DebugText = "";
            _debugLineList.Clear();
        #endif

        if (_playerInCollider == null) return false;
        for (int i = 0; i < _playerInCollider.VisiblePoints.Length; i++)
        {
            var point = _playerInCollider.transform.position + _playerInCollider.VisiblePoints[i];
            var src = EyePosition.transform.position;

            #if GAME_DEBUG
                _debugLineList.Add(Tuple.Create(src, point));
            #endif

            if (Physics.Raycast(src, Direction(src, point), out RaycastHit hitInfo)) 
            {
                #if GAME_DEBUG
                    DebugText = hitInfo.collider.name;
                #endif
                if (hitInfo.collider.gameObject == _playerInCollider.gameObject) return true;
            }
        }
        return false;
    }

    private Vector3 Direction(Vector3 src, Vector3 dest) {
        return (dest - src).normalized;
    }

    #if GAME_DEBUG
    public string DebugText; // A bit of a cheat, but this is a game jam

    private List<Tuple<Vector3, Vector3>> _debugLineList = new List<Tuple<Vector3, Vector3>>();
    private BoxCollider _debugBoxCollider;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        foreach (var line in _debugLineList)
        {
            Gizmos.DrawLine(line.Item1, line.Item2);
        }  

        if (_debugBoxCollider == null) {
            _debugBoxCollider = GetComponent<BoxCollider>();
        } else {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(_debugBoxCollider.center, _debugBoxCollider.size);   
        }      
    }  
    #endif 

}
