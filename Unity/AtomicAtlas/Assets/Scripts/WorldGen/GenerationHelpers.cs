using System.Collections.Generic;
using UnityEngine;

namespace Atlas.WorldGen
{
    public static class GenerationHelper
    {
        /// <summary>
        /// Create a randomized world plane with nodes set up in a tesselating pattern
        /// </summary>
        public static WorldPlane GenerateRandomizedWorldPlane(string name, bool isCave, int gridSizeX, int gridSizeY, float maxJitter)
        {
            var nodeGrid = new Dictionary<Vector2, Node>();
            var plane = new WorldPlane(name, isCave);
            float cellSizeX = 1f / gridSizeX;
            float cellSizeY = 1f / gridSizeY;
            float offsetX = cellSizeX * 0.5f;
            float offsetY = cellSizeY * 0.5f;

            // Generate nodes
            for (int i = 0; i < gridSizeX; i++)
            {
                for (int j = 0; j < gridSizeY; j++)
                {
                    // Create a point centered in each grid cell then jitter it (jitter is a normalized value from 0-1)
                    var vector = new Vector2(i * cellSizeX + offsetX, j * cellSizeY + offsetY);
                    vector.x += Random.Range(-maxJitter * 0.5f, maxJitter * 0.5f) * cellSizeX;
                    vector.y += Random.Range(-maxJitter * 0.5f, maxJitter * 0.5f) * cellSizeY;

                    var node = plane.CreateNode(vector);
                    nodeGrid.Add(new Vector2(i, j), node);
                }
            }

            // Standard connections and randomized diagonal triangulation connections
            for (int i = 0; i < gridSizeX; i++)
            {
                for (int j = 0; j < gridSizeY; j++)
                {
                    int right = i + 1 >= gridSizeX ? 0 : i + 1;
                    int up = j + 1 >= gridSizeY ? 0 : j + 1;

                    var vector = new Vector2(i, j);
                    var node = nodeGrid[vector];
                    var rightVector = new Vector2(right, j);
                    var rightNode = nodeGrid[rightVector];
                    var upVector = new Vector2(i, up);
                    var upNode = nodeGrid[upVector];

                    bool isRightWrap = Vector2.Distance(vector, rightVector) >= 2;
                    bool isUpWrap = Vector2.Distance(vector, upVector) >= 2;

                    plane.CreateConnection(node, upNode, isUpWrap);
                    plane.CreateConnection(node, rightNode, isRightWrap);

                    // One of the diagonal directions needs a connection
                    if (Random.Range(0, 2) == 0)
                    {
                        var diagVector = new Vector2(right, up);
                        var upperNode = nodeGrid[diagVector];
                        bool isWrap = Vector2.Distance(vector, diagVector) >= 2;

                        plane.CreateConnection(node, upperNode, isWrap);
                    }
                    else
                    {
                        bool isWrap = Vector2.Distance(rightVector, upVector) >= 2;

                        plane.CreateConnection(rightNode, upNode, isWrap);
                    }
                }
            }

            return plane;
        }
    }
}