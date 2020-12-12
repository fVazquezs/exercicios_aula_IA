using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyActor : MonoBehaviour
{
    public List<Transform> TestPoints;
    public Transform PathFinderDestination;

    private PathFollower _pathFollower;
    private PathFinder _pathFinder;
    
    private List<Vector3> _pathFindResult = new List<Vector3>();


    private void Awake()
    {
        _pathFollower = GetComponent<PathFollower>();
        _pathFinder = GetComponent<PathFinder>();
    }

    private void Start()
    {
        if (TestPoints.Count > 0)
        {
            _pathFollower.FollowPath(TestPoints.Select(x => x.position).ToList());

            return;
        }
        
        //Pathfind to destination
        bool hasPath = _pathFinder.FindPath(
            PathFinderDestination.position,
            _pathFindResult);
        if (hasPath)
        {
            _pathFollower.FollowPath(_pathFindResult);
        }
    }
}
