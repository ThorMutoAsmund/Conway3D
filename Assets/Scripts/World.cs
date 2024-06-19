using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class World : MonoBehaviour
{
    public GameObject cellPrefab;
    public int cellCount = 0;
    public int candidateCount = 0;

    public bool IsRunning { get; private set; }
    public bool AllowSenescentCells { get; private set; } = false;

    private int span = 10;
    private bool stopAfterNextUpdate;
    private Dictionary<Vector3Int, Cell> cells = new();
    private List<Vector3Int> candidateLocations = new();
    private List<Cell> deathRow = new ();

    private void LateUpdate()
    {
        if (this.IsRunning)
        {
            // Reporting
            this.candidateCount = this.candidateLocations.Count;
            this.cellCount = this.cells.Count;

            // Any dead cell touching exactly three alive neighbours becomes alive.
            var spawnList = new List<Vector3Int>();

            foreach (var candidateLocation in new List<Vector3Int>(this.candidateLocations))
            {
                var cellsAround = CountAliveCellsAround(candidateLocation);
                if (cellsAround == 3)
                {
                    spawnList.Add(candidateLocation);
                }
                else if (cellsAround < 3)
                {
                    this.candidateLocations.Remove(candidateLocation);
                }
            }
            foreach (var spawnLocation in spawnList)
            {
                Spawn(spawnLocation, true);
            }


            // Kill
            if (this.deathRow.Count > 0)
            {
                foreach (var cell in this.deathRow)
                {
                    Kill(cell);
                }
                this.deathRow.Clear();
            }

            if (this.stopAfterNextUpdate)
            {
                this.stopAfterNextUpdate = false;
                Stop();
            }
        }
    }

    public void ClearAll()
    {
        Debug.Log($"Clearing {this.name}");

        foreach (var cell in new List<Cell>(this.cells.Values))
        {
            Kill(cell);
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
            var location = new Vector3Int(Random.Range(-this.span, this.span + 1), Random.Range(-this.span, this.span + 1), Random.Range(-this.span, this.span + 1));
            if (!HasCellAt(location))
            {
                Spawn(location);
            }
        }
    }

    public void SpawnRandom2D(int maxAmount)
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Application must be running");
            return;
        }

        Debug.Log($"Spawning {maxAmount} cells");
        for (int i = 0; i < maxAmount; ++i)
        {
            var location = new Vector3Int(Random.Range(-this.span, this.span + 1), Random.Range(-this.span, this.span + 1), 0);
            if (!HasCellAt(location))
            {
                Spawn(location, true);
            }
        }
    }

    public void SpawnGlider2D()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Application must be running");
            return;
        }

        Debug.Log($"Spawning glider");

        Spawn(new Vector3Int(-1, -1, 0), true);
        Spawn(new Vector3Int(0, -1, 0), true);
        Spawn(new Vector3Int(1, -1, 0), true);
        Spawn(new Vector3Int(1, 0, 0), true);
        Spawn(new Vector3Int(0, 1, 0), true);
    }

    private void Spawn(Vector3Int location, bool is2D = false)
    {
        var cell = Instantiate(this.cellPrefab, location, Quaternion.identity, this.transform).GetComponent<Cell>();
        cell.is2D = is2D;
        this.cells[location] = cell;

        if (this.candidateLocations.Contains(location))
        {
            this.candidateLocations.Remove(location);
        }

        if (is2D)
        {
            foreach (var candidateLocation in location.Surrounding2D())
            {
                if (!this.candidateLocations.Contains(candidateLocation) && !this.cells.ContainsKey(candidateLocation))
                {
                    this.candidateLocations.Add(candidateLocation);
                }
            }
        }
    }

    public void ScheduleKill(Cell cell)
    {
        this.deathRow.Add(cell);
    }

    private void Kill(Cell cell)
    {
        this.cells.Remove(cell.Location);

        GameObject.Destroy(cell.gameObject);
    }

    public void Run()
    {
        Debug.Log($"Starting world {this.name}");
        this.IsRunning = true;
    }

    public void Stop()
    {
        Debug.Log($"Stopping world {this.name}");
        this.IsRunning = false;
    }

    public void Step()
    {
        Debug.Log($"Step {this.name}");

        if (this.IsRunning)
        {
            this.IsRunning = false;
        }

        this.IsRunning = true;
        this.stopAfterNextUpdate = true;
    }
}
