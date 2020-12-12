using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersueAndFleeEnemy : MonoBehaviour
{

    public float Speed = 1;
    public Transform Target;

    public float TargetDistance = 5;


    void Update()
    {
        Vector3 direction = Target.position - transform.position;
        direction.y = 0;
        float _distanceToTarget = direction.magnitude;
        direction.Normalize();


        if (_distanceToTarget < TargetDistance)
        {
            direction = -direction;
        }

        float distanceWantsToMoveThisFrame = Speed * Time.deltaTime;

        float actuaMovementThisFrame = Mathf.Min(Mathf.Abs(_distanceToTarget - TargetDistance), distanceWantsToMoveThisFrame);

        MoveCharacter(direction * actuaMovementThisFrame);

    }

    void MoveCharacter(Vector3 frameMovement)
    {

        transform.position += frameMovement;
    }

}
