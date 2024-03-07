
using System.Collections.Generic;
using UnityEngine;

// The world consists of several planes, though typically just a main plane and a cave plane
public class World
{
    public List<WorldPlane> Planes => planes;
    public Vector2 WorldSize => worldSize;

    private List<WorldPlane> planes;
    private Vector2 worldSize = new Vector2(2048, 1024);

    public World(Vector2 size)
    {
        worldSize = size;
        planes = new List<WorldPlane>();
    }

    public void AddPlane(WorldPlane plane)
    {
        planes.Add(plane);
    }
}
