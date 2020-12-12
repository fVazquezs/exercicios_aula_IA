using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemy : MonoBehaviour
{

    public float Speed = 1;
    public List<Transform> Points;

    private int _currentTargetIndex;
    private int _currentTargetDirection;


    private float _distanceToTarget;
    private float _distanceWantsToMoveThisFrame;

    void Start()
    {
        _currentTargetIndex = 0;
        _currentTargetDirection = 1;
    }

    bool HasReachedTarget()
    {
        return _distanceWantsToMoveThisFrame >= _distanceToTarget;
    }

    void MoveCharacter(Vector3 frameMovement)
    {
        transform.position += frameMovement;
    }

    // Update is called once per frame
    void Update()
    {
        Transform currentTarget = Points[_currentTargetIndex];

        Vector3 direction = currentTarget.position - transform.position;
        direction.y = 0;
        _distanceToTarget = direction.magnitude;

        direction.Normalize();

        _distanceWantsToMoveThisFrame = Speed * Time.deltaTime;

        float actuaMovementThisFrame = Mathf.Min(_distanceToTarget, _distanceWantsToMoveThisFrame);

        MoveCharacter(actuaMovementThisFrame * direction);

        if (HasReachedTarget())
        {
            // _currentTargetIndex++;

            _currentTargetIndex += _currentTargetDirection;
            if (_currentTargetIndex == Points.Count - 1 || _currentTargetIndex == 0)
            {
                _currentTargetDirection *= -1;
            }
        }
    }
}
