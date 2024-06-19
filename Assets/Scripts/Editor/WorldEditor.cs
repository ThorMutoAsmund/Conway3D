using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(World), true)]
public class WorldEditor : Editor
{
    private string runStopText = "Run";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Clear"))
        {
            World world = (World)target;
            world.ClearAll();
        }

        if (GUILayout.Button("Spawn 3D"))
        {
            World world = (World)target;
            world.SpawnRandom(20);
        }

        if (GUILayout.Button("Spawn 2D"))
        {
            World world = (World)target;
            world.SpawnRandom2D(20);
        }

        if (GUILayout.Button("Spawn 2D Glider"))
        {
            World world = (World)target;
            world.SpawnGlider2D();
        }

        if (GUILayout.Button(this.runStopText))
        {
            World world = (World)target;

            if (world.IsRunning)
            {
                world.Stop();
                this.runStopText = "Run";
            }
            else
            {
                world.Run();
                this.runStopText = "Stop";
            }
        }

        if (GUILayout.Button("Step"))
        {
            World world = (World)target;

            world.Step();
        }
    }
}
