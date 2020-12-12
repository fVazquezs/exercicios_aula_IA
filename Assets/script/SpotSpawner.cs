using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotSpawner : MonoBehaviour
{
    
    public static SpotSpawner Instance { get; private set; }

    public Spot SpotTemplate;
    public Vector2Int MazeSize;
    public float VerticalOffSet = 0.1f;
    public LayerMask WallLayers;

    private readonly Dictionary<Vector2Int, Spot> _spawnedSpots = new Dictionary<Vector2Int, Spot>(); 
        
    public Spot GetNearestSpot(Vector3 worldPosition)
    {
        var spotPosition = new Vector2Int(Mathf.RoundToInt(worldPosition.x), -Mathf.RoundToInt(worldPosition.z));

        spotPosition.x = Mathf.Clamp(spotPosition.x, 0, MazeSize.x - 1);
        spotPosition.y = Mathf.Clamp(spotPosition.y, 0, MazeSize.y - 1);

        return GetSpot(spotPosition);
    }

    public Spot GetSpot(Vector2Int position)
    {
        return _spawnedSpots.TryGetValue(position, out var spot) ? spot : null;
    }
    
    private void Awake()
    {
        Instance = this;
        for (int x = 0; x < MazeSize.x; x++)
        {
            for (int y = 0; y < MazeSize.y; y++)
            {
                var spotPosition = new Vector3(x, VerticalOffSet, -y);
                var spot = Instantiate(SpotTemplate, spotPosition, Quaternion.identity, transform);
            
                spot.GridPosition = new Vector2Int(x, y);
                spot.IsWalkable = CheckWalkable(spotPosition);
                
                _spawnedSpots.Add(spot.GridPosition, spot);
            }
        }
    }

    private bool CheckWalkable(Vector3 spotPosition)
    {
        // return !Physics.CheckSphere(spotPosition, 0.1f, (1 << 8));
        return !Physics.CheckSphere(spotPosition, 0.1f, WallLayers);
    }
}
