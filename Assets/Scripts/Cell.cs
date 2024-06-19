using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool is2D = false;

    public bool IsAlive { get; private set; } = true;
    private World world;
    public Vector3Int Location { get; private set; }

    private void Awake()
    {
        this.Location = new Vector3Int((int)this.transform.position.x, (int)this.transform.position.y, (int)this.transform.position.z);
    }

    private void Start()
    {
        this.world = GetComponentInParent<World>();

        if (this.is2D)
        {
            this.transform.localScale = new Vector3(1.0f, 1.0f, 0.1f); // <- pancake
        }
    }

    private void Update()
    {
        if (this.world.IsRunning && this.IsAlive)
        {
            if (this.is2D)
            {
                // https://simple.wikipedia.org/wiki/Conway%27s_Game_of_Life

                var neighbors = this.world.CountAliveCells(this.Location.Surrounding2D());
                // Any alive cell that is touching less than two alive neighbours dies.
                // Any alive cell touching four or more alive neighbours dies.
                if (neighbors < 2 || neighbors >= 4)
                {
                    Die();
                }
                // Any alive cell touching two or three alive neighbours does nothing.
            }
        }
    }

    private void Die()
    {
        if (this.world.AllowSenescentCells)
        {
            this.IsAlive = false;
        }
        else
        {
            this.world.ScheduleKill(this);
        }
    }
}
