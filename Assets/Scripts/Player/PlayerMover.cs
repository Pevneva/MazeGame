using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerMover : MonoBehaviour
{
    private Transform _target;
    private bool _isMoving;
    private bool _isDeadEnd;
    private float _speed;
    private Vector3 _currentDirection;
    private Vector3 _exceptDirection;
    private Player _player;
    private List<Vector3> _preferredDirections = new List<Vector3>();
    private List<Vector3> _moveDirections = new List<Vector3>()
    {
        Vector3.right,Vector3.forward, Vector3.back, Vector3.left
    };

    private void Start()
    {
        _isDeadEnd = true;
        _speed = 2.5f;
        _player = GetComponent<Player>();
        _player.Died += OnPlayerDied;
        _player.FinishReached += OnFinishReached;
    }

    private void Update()
    {
        if (_isMoving == false)
            return;
        
        _currentDirection = GetNewDirection();
        transform.Translate(_speed * Time.deltaTime * _currentDirection);
    }

    public void Init(Transform target)
    {
        _target = target;
    }

    public void Move()
    {
        _isMoving = true;
    }

    private void OnPlayerDied()
    {
        StartCoroutine(StopPlayerMoving(0.35f));
    }

    private IEnumerator StopPlayerMoving(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isMoving = false;
    }
    
    private void OnFinishReached()
    {
        Invoke(nameof(StopMoving), 0.35f);
    }

    private void StopMoving()
    {
        _isMoving = false;
    }

    private bool IsWayEmpty(Vector3 direction)
    {
        RaycastHit hitBoxcast = new RaycastHit();
        Physics.BoxCast(transform.position, transform.localScale / 2, direction, out hitBoxcast, transform.rotation,
            0.2f);

        if (hitBoxcast.collider == null)
            return true;
        else
            return false;
    }
    
    private void SetPrefferedDirections(Transform target)
    {
        _preferredDirections.Clear();
        
        if (transform.position.x < target.position.x)
            _preferredDirections.Add(Vector3.right);
        else
            _preferredDirections.Add(Vector3.right);
        

        if (transform.position.z < target.position.z)
            _preferredDirections.Add(Vector3.forward);
        else
            _preferredDirections.Add(Vector3.back);        
    }

    private Vector3 GetNewDirection()
    {
        SetPrefferedDirections(_target);
        
        foreach (var direction in _preferredDirections)
        {
            if (IsWayEmpty(direction) && direction != _exceptDirection)
            {
                _exceptDirection = Vector3.zero;
                return direction;
            }
        }

        var unprefferedDirection = _moveDirections.Except(_preferredDirections);
        foreach (var direction in unprefferedDirection)
        {
            if (IsWayEmpty(direction))
            {
                if (_exceptDirection == Vector3.zero)
                    _exceptDirection =_currentDirection;
                return direction;
            }
        }        
        
        return Vector3.zero;
    }
}