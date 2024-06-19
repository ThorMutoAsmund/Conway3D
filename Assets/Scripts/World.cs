using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// https://simple.wikipedia.org/wiki/Conway%27s_Game_of_Life

public class World : MonoBehaviour
{
    public bool is2D = true;
    public GameObject cellPrefab2D;
    public GameObject cellPrefab3D;
    public int cellCount = 0;
    public int candidateCount = 0;
    public int neighborMin2D = 2;
    public int neighborMax2D = 4;
    public int neighborSpawn2D = 3;

    public int NeighborMin2D => this.neighborMin2D;
    public int NeighborMax2D => this.neighborMax2D;
    public bool IsRunning { get; private set; }
    public bool AllowSenescentCells { get; private set; } = false;

    private int span = 10;
    private bool stopAfterNextUpdate;
    private Dictionary<Vector3Int, ICell> cells = new();
    private List<Vector3Int> candidateLocations = new();
    private List<Vector3Int> deathRow = new ();

    private void LateUpdate()
    {
        if (this.IsRunning)
        {
            if (this.is2D)
            {
                // Any dead cell touching exactly three alive neighbours becomes alive.
                var spawnList = new List<Vector3Int>();

                foreach (var candidateLocation in new List<Vector3Int>(this.candidateLocations))
                {
                    var cellsAround = CountAliveCellsAround(candidateLocation);
                    if (cellsAround == this.neighborSpawn2D)
                    {
                        spawnList.Add(candidateLocation);
                    }
                    else if (cellsAround < this.neighborSpawn2D)
                    {
                        this.candidateLocations.Remove(candidateLocation);
                    }
                }
                foreach (var spawnLocation in spawnList)
                {
                    Spawn2D(spawnLocation, true);
                }

                // Kill
                if (this.deathRow.Count > 0)
                {
                    foreach (var cellLocation in this.deathRow)
                    {
                        Kill(cellLocation);
                    }
                    this.deathRow.Clear();
                }

                if (this.stopAfterNextUpdate)
                {
                    this.stopAfterNextUpdate = false;
                    Stop();
                }
            }

            Report();
        }
    }

    public void ClearAll()
    {
        Debug.Log($"Clearing all in world");

        foreach (var cellLocation in new List<Vector3Int>(this.cells.Keys))
        {
            Kill(cellLocation);
        }
        this.candidateLocations.Clear();
    }

    public bool HasCellAt(Vector3Int location)
    {
        return this.cells.ContainsKey(location);
    }

    public bool HasAliveCellAt(Vector3Int location)
    {
        return this.cells.ContainsKey(location) && this.cells[location].IsAlive;
    }

    public int CountAliveCells(params Vector3Int[] locations)
    {
        return locations.Count(l => HasAliveCellAt(l));
    }

    public int CountAliveCellsAround(Vector3Int location)
    {
        return location.Surrounding2D().Count(l => HasAliveCellAt(l));
    }
    

    public void SpawnRandom(int maxAmount)
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Application must be running");
            return;
        }

        Debug.Log($"Spawning {maxAmount} cells");
        for (int i = 0; i < maxAmount; ++i)
        {
            if (this.is2D)
            {
                var location = new Vector3Int(Random.Range(-this.span, this.span + 1), Random.Range(-this.span, this.span + 1), 0);
                if (!HasCellAt(location))
                {
                    Spawn2D(location, true);
                }
            }
            else
            {
                var location = new Vector3Int(Random.Range(-this.span, this.span + 1), Random.Range(-this.span, this.span + 1), Random.Range(-this.span, this.span + 1));
                if (!HasCellAt(location))
                {
                    Spawn3D(location);
                }
            }
        }

        Report();
    }

    public void SpawnGlider()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Application must be running");
            return;
        }

        if (this.is2D)
        {
            Debug.Log($"Spawning glider");

            Spawn2D(new Vector3Int(-1, -1, 0), true);
            Spawn2D(new Vector3Int(0, -1, 0), true);
            Spawn2D(new Vector3Int(1, -1, 0), true);
            Spawn2D(new Vector3Int(1, 0, 0), true);
            Spawn2D(new Vector3Int(0, 1, 0), true);

            Report();
        }
    }

    private void Spawn2D(Vector3Int location, bool is2D = false)
    {
        var cell = Instantiate(this.cellPrefab2D, location, Quaternion.identity, this.transform).GetComponent<Cell2D>();
        this.cells[location] = cell;

        if (this.candidateLocations.Contains(location))
        {
            this.candidateLocations.Remove(location);
        }

        foreach (var candidateLocation in location.Surrounding2D())
        {
            if (!this.candidateLocations.Contains(candidateLocation) && (!this.cells.ContainsKey(candidateLocation) || this.deathRow.Contains(candidateLocation)))
            {
                this.candidateLocations.Add(candidateLocation);
            }
        }
    }

    private void Spawn3D(Vector3Int location, bool is2D = false)
    {
        var cell = Instantiate(this.cellPrefab3D, location, Quaternion.identity, this.transform).GetComponent<Cell3D>();
        this.cells[location] = cell;

        if (this.candidateLocations.Contains(location))
        {
            this.candidateLocations.Remove(location);
        }

        foreach (var candidateLocation in location.Surrounding3D())
        {
            if (!this.candidateLocations.Contains(candidateLocation) && (!this.cells.ContainsKey(candidateLocation) || this.deathRow.Contains(candidateLocation)))
            {
                this.candidateLocations.Add(candidateLocation);
            }
        }
    }

    public void ScheduleKill(ICell cell)
    {
        this.deathRow.Add(cell.Location);
    }

    private void Kill(Vector3Int cellLocation)
    {
        var cell = this.cells[cellLocation];
        this.cells.Remove(cellLocation);
        GameObject.Destroy(cell.gameObject);
    }

    private void Report()
    {
        // Reporting
        this.candidateCount = this.candidateLocations.Count;
        this.cellCount = this.cells.Count;
    }

    public void Run()
    {
        Debug.Log("Starting world");
        this.IsRunning = true;
    }

    public void Stop()
    {
        Debug.Log("Stopping world");
        this.IsRunning = false;
    }

    public void Step()
    {
        Debug.Log($"Step");

        if (this.IsRunning)
        {
            this.IsRunning = false;
        }

        this.IsRunning = true;
        this.stopAfterNextUpdate = true;
    }
}
