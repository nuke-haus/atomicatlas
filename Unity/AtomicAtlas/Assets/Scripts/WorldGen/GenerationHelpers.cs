using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Atlas.WorldGen
{
    public static class GenerationHelper
    {
        private static bool InsertPointInGrid(Vector2[,] grid, float cellsize, Vector2 point)
        {
            int x = Mathf.FloorToInt(point.x / cellsize);
            int y = Mathf.FloorToInt(point.y / cellsize);

            if (grid[x, y] == Vector2.zero)
            {
                grid[x, y] = point;
                return true;
            }

            return false;
        }

        private static bool IsValidPoint(Vector2[,] grid, float cellsize, int gwidth, int gheight, Vector2 p, float radius, float min, float max)
        {
            if (p.x < min || p.x >= max || p.y < min || p.y >= max)
            {
                return false;
            }
                
            int xindex = Mathf.FloorToInt(p.x / cellsize);
            int yindex = Mathf.FloorToInt(p.y / cellsize);
            int i0 = Mathf.Max(xindex - 1, 0);
            int i1 = Mathf.Max(xindex + 1, gwidth - 1);
            int j0 = Mathf.Max(yindex - 1, 0);
            int j1 = Mathf.Max(yindex + 1, gheight - 1);

            for (int i = i0; i <= i1; i++)
            {
                for (int j = j0; j <= j1; j++)
                {
                    if (grid[i, j] != Vector2.zero && Vector2.Distance(grid[i, j], p) < radius)
                    {
                        return false;
                    }
                }
            }
        
            return true;
        }

        private static readonly float MAX_POINT_GENERATION_ATTEMPTS = 50;
        private static readonly float MAX_RECOVERY_ATTEMPTS = 50;

        private static List<Vector2> GeneratePoissonDiskSamples(int numPoints)
        {
            float radius = (1f / Mathf.Sqrt(numPoints));
            float min = 0 + radius * 0.4f;
            float max = 1f - radius * 0.4f;
            float cellsize = radius / Mathf.Sqrt(2);
            int width = (int)Mathf.Ceil(1f / cellsize) + 1;
            int height = (int)Mathf.Ceil(1f / cellsize) + 1;
            int recoveryAttempts = 0;

            var finalPoints = new List<Vector2>();
            var activePoints = new List<Vector2>();

            // Generate a grid of 9 points in a box formation so we get a decent distribution
            Vector2[,] grid = new Vector2[width, height];
            Vector2 p0 = new Vector2(0.5f, 0.5f);
            Vector2 p1 = new Vector2(0.1f, 0.1f);
            Vector2 p2 = new Vector2(0.1f, 0.9f);
            Vector2 p3 = new Vector2(0.9f, 0.101f);
            Vector2 p4 = new Vector2(0.9f, 0.901f);
            Vector2 p5 = new Vector2(0.5f, 0.902f);
            Vector2 p6 = new Vector2(0.5f, 0.102f);
            Vector2 p7 = new Vector2(0.9f, 0.501f);
            Vector2 p8 = new Vector2(0.1f, 0.502f);

            InsertPointInGrid(grid, cellsize, p0);
            finalPoints.Add(p0);
            activePoints.Add(p0);

            InsertPointInGrid(grid, cellsize, p1);
            finalPoints.Add(p1);
            activePoints.Add(p1);

            InsertPointInGrid(grid, cellsize, p2);
            finalPoints.Add(p2);
            activePoints.Add(p2);

            InsertPointInGrid(grid, cellsize, p3);
            finalPoints.Add(p3);
            activePoints.Add(p3);

            InsertPointInGrid(grid, cellsize, p4);
            finalPoints.Add(p4);
            activePoints.Add(p4);

            InsertPointInGrid(grid, cellsize, p5);
            finalPoints.Add(p5);
            activePoints.Add(p5);

            InsertPointInGrid(grid, cellsize, p6);
            finalPoints.Add(p6);
            activePoints.Add(p6);

            InsertPointInGrid(grid, cellsize, p7);
            finalPoints.Add(p7);
            activePoints.Add(p7);

            InsertPointInGrid(grid, cellsize, p8);
            finalPoints.Add(p8);
            activePoints.Add(p8);

            //Debug.Log($"Width {width}, height {height}, wxh {width * height}, num points {numPoints}");

            while (activePoints.Any() && finalPoints.Count < numPoints)
            {
                int randomIndex = UnityEngine.Random.Range(0, activePoints.Count);
                Vector2 point = activePoints[randomIndex];

                bool found = false;
                for (int tries = 0; tries < MAX_POINT_GENERATION_ATTEMPTS; tries++)
                {
                    float theta = UnityEngine.Random.Range(0f, 360f);
                    float newRadius = UnityEngine.Random.Range(radius, radius * 3f);
                    float newX = point.x + newRadius * Mathf.Cos(Mathf.Deg2Rad * theta);
                    float newY = point.y + newRadius * Mathf.Sin(Mathf.Deg2Rad * theta);
                    Vector2 newPoint = new Vector2(newX, newY);

                    if (!IsValidPoint(grid, cellsize, width, height, newPoint, radius * 0.75f, min, max))
                    {
                        continue;
                    }

                    bool validInsertion = InsertPointInGrid(grid, cellsize, newPoint);

                    if (validInsertion)
                    {
                        finalPoints.Add(newPoint);
                        activePoints.Add(newPoint);
                        found = true;
                    }

                    break;
                }

                if (!found)
                {
                    activePoints.Remove(point);
                }

                // Edge case, we ran our of active points and we don't have enough final points
                if (!activePoints.Any() && finalPoints.Count < numPoints && recoveryAttempts < MAX_RECOVERY_ATTEMPTS)
                {
                    Vector2 randVector = new Vector2(UnityEngine.Random.Range(0.1f, 0.9f), UnityEngine.Random.Range(0.1f, 0.9f));
                    activePoints.Add(randVector);
                    radius *= 0.98f;
                    recoveryAttempts++;
                }
            }

            return finalPoints;
        }

        public static WorldPlane GenerateRandomizedWorldPlane(string name, bool isCave, int nodeCount)
        {
            var plane = new WorldPlane(name, isCave);
            var positions = GeneratePoissonDiskSamples(nodeCount);

            // Generate all nodes
            foreach (var position in positions)
            {
                plane.CreateNode(position);
            }

            // Generate triangles
            var triangles = Triangulator.GetBowyerWatsonTriangles(plane.Nodes).ToList();

            // TODO: Remove triangles with inner angle below a threshold

            var completed = new List<Edge>();

            while (triangles.Any())
            {
                var triangle = triangles[0];
                
                foreach (var edge in triangle.Edges)
                {
                    if (edge.Point1.Node == null || edge.Point2.Node == null || completed.Any(completedEdge => completedEdge.Equals(edge)))
                    {
                        continue;
                    }

                    plane.CreateConnection(edge.Point1.Node, edge.Point2.Node, false);
                    completed.Add(edge);
                }

                triangles.Remove(triangle);
            }

            return plane;
        }
    }
}
