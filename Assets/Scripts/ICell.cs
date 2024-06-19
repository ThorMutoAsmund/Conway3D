using UnityEngine;

public interface ICell
{
    bool IsAlive { get; }
    Vector3Int Location { get; }
    GameObject gameObject { get; }
}
