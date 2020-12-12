using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    struct SpotAndCost
    {
        public Spot Spot;
        public Spot Parent;
        public int Cost;
        public float Heuristic;

        public SpotAndCost(Spot spot, Spot parent, int cost, float heuristic)
        {
            Spot = spot;
            Parent = parent;
            Cost = cost;
            Heuristic = heuristic;
        }
    }

    private readonly List<SpotAndCost> _nodesToOpen = new List<SpotAndCost>();
    private readonly HashSet<Spot> _nodesOpened = new HashSet<Spot>();
    private readonly Dictionary<Spot, Spot> _finalParents = new Dictionary<Spot, Spot>();

    public bool FindPath(Vector3 destPosition, List<Vector3> outResult)
    {
        Vector3 sourcePosition = transform.position;
        Spot sourceSpot = SpotSpawner.Instance.GetNearestSpot(sourcePosition);
        Spot destSpot = SpotSpawner.Instance.GetNearestSpot(destPosition);

        _finalParents.Clear();
        _nodesToOpen.Clear();
        _nodesOpened.Clear();
        destPosition.y = 0f;

        _nodesToOpen.Add(new SpotAndCost(sourceSpot, sourceSpot, 0, 0));

        while (_nodesToOpen.Count > 0)
        {
            SpotAndCost nodeToOpen = _nodesToOpen[0];
            for (int i = 0; i < _nodesToOpen.Count; i++)
            {
                if (_nodesToOpen[i].Cost + _nodesToOpen[i].Heuristic < nodeToOpen.Cost + nodeToOpen.Heuristic)
                {
                    nodeToOpen = _nodesToOpen[i];
                }
            }

            _nodesToOpen.Remove(nodeToOpen);

            //Marca como visitado
            if (_nodesOpened.Contains(nodeToOpen.Spot)) continue;
            _nodesOpened.Add(nodeToOpen.Spot);
            _finalParents.Add(nodeToOpen.Spot, nodeToOpen.Parent);


            if (nodeToOpen.Spot == destSpot)
            {
                //Encontrou o caminho e passar qual é 
                Spot currentSpot = destSpot;
                while (true)
                {
                    outResult.Add(currentSpot.transform.position);
                    Spot parent = _finalParents[currentSpot];
                    if (parent == currentSpot) break;

                    currentSpot = parent;
                }

                outResult.Reverse();

                Debug.LogFormat("Found path, notes visited {0} / pathlength {1}", _nodesOpened.Count, outResult.Count);
                return true;
            }

            //Achar vizinhos
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    bool isOrthogonal = Mathf.Abs(dx) + Mathf.Abs(dy) == 1;
                    if (!isOrthogonal) continue;

                    //calcula posição do no vizinho 
                    Vector2Int neighbourPosition = nodeToOpen.Spot.GridPosition;
                    neighbourPosition.x += dx;
                    neighbourPosition.y += dy;
                    Spot neighbourSpot = SpotSpawner.Instance.GetSpot(neighbourPosition);

                    if (neighbourSpot == null || _nodesOpened.Contains(neighbourSpot) ||
                        !neighbourSpot.IsWalkable) continue;

                    Vector3 spotPosition = nodeToOpen.Spot.transform.position;
                    spotPosition.y = 0f;
                    Vector3 distanceDelta = spotPosition - destPosition;
                    float heuristic = 
                        // Mathf.Abs(distanceDelta.x) + Mathf.Abs(distanceDelta.z);
                    Vector3.Distance(spotPosition, destPosition);

                    _nodesToOpen.Add(new SpotAndCost(neighbourSpot, nodeToOpen.Spot, nodeToOpen.Cost + 1, heuristic));
                }
            }
        }

        return false;
    }
}