using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public float Speed = 1;

    private List<Vector3> _points;
    private int _currentTargetIndex;

    private float _distanceToTarget;
    private float _distanceWantsToMoveThisFrame;
    public bool _isFollowingPath;

    void Awake()
    {
        _currentTargetIndex = 0;
        _isFollowingPath = false;
        _points = new List<Vector3>();
    }

    bool HasReachedTarget()
    {
        return _distanceWantsToMoveThisFrame >= _distanceToTarget;
    }

    void MoveCharacter(Vector3 frameMovement)
    {
        transform.position += frameMovement;
    }

    public void FollowPath(List<Vector3> points)
    {
        if (points.Count == 0)
        {
            return;
        }
        
        _isFollowingPath = true;
        _currentTargetIndex = 0;

        _points.Clear();
        _points.AddRange(points);
    }

    void Update()
    {
        if (!_isFollowingPath) return;

        Vector3 currentTarget = _points[_currentTargetIndex];

        Vector3 direction = currentTarget - transform.position;
        direction.y = 0;
        _distanceToTarget = direction.magnitude;

        direction.Normalize();

        _distanceWantsToMoveThisFrame = Speed * Time.deltaTime;

        // Faz o movimento terminar exatamente em cima do alvo
        float actualMovementThisFrame = Mathf.Min(_distanceToTarget, _distanceWantsToMoveThisFrame);

        MoveCharacter(actualMovementThisFrame * direction);

        if (HasReachedTarget())
        {
            //_currentTargetIndex = (_currentTargetIndex + 1) % Points.Count;

            _currentTargetIndex++;
            if(_currentTargetIndex == _points.Count)
            {
                _isFollowingPath = false;
            }
        }
    }
}