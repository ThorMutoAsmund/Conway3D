using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell3D : MonoBehaviour, ICell
{
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
    }

    private void Update()
    {
        if (this.world.IsRunning && this.IsAlive)
        {
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
