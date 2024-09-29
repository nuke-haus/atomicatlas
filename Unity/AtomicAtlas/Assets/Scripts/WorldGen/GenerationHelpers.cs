using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

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
            float borderTolerance = radius * 0.25f;
            float min = 0 + borderTolerance;
            float max = 1f - borderTolerance;
            float cellsize = radius / Mathf.Sqrt(2);
            int width = (int)Mathf.Ceil(1f / cellsize) + 1;
            int height = (int)Mathf.Ceil(1f / cellsize) + 1;
            int recoveryAttempts = 0;

            var finalPoints = new List<Vector2>();
            var activePoints = new List<Vector2>();

            borderTolerance *= 2;

            // Generate a grid of 9 points in a box formation so we get a decent distribution
            Vector2[,] grid = new Vector2[width, height];
            Vector2 p0 = new Vector2(0.5f, 0.5f);
            Vector2 p1 = new Vector2(borderTolerance, borderTolerance);
            Vector2 p2 = new Vector2(borderTolerance, 1f - borderTolerance);
            Vector2 p3 = new Vector2(1f - borderTolerance, borderTolerance + 0.001f);
            Vector2 p4 = new Vector2(1f - borderTolerance, 1f - borderTolerance + 0.001f);
            Vector2 p5 = new Vector2(0.5f, 1f - borderTolerance + 0.002f);
            Vector2 p6 = new Vector2(0.5f, borderTolerance + 0.002f);
            Vector2 p7 = new Vector2(1f - borderTolerance, 0.501f);
            Vector2 p8 = new Vector2(borderTolerance, 0.502f);

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

        /// <summary>
        /// Get node closest to vector
        /// </summary>
        private static Node GetClosestNode(IEnumerable<Node> nodes, Vector2 point)
        {
            float dist = float.MaxValue;
            Node result = null;

            foreach (var node in nodes)
            {
                var nodeDist = Vector2.Distance(node.NormalizedPosition, point);

                if (nodeDist < dist)
                {
                    dist = nodeDist;
                    result = node;
                }
            }

            return result;
        }

        private static IEnumerable<Node> GetEdgeNodes(IEnumerable<Node> nodes, Vector2 startPoint, Vector2 endPoint, int numChecks)
        {
            Vector2 dir = (endPoint - startPoint).normalized;
            float distance = Vector2.Distance(startPoint, endPoint);
            float spacing = distance / numChecks;
            var result = new HashSet<Node>();

            for (int i = 0; i < numChecks; i++)
            {
                var vector = startPoint + dir * (i * spacing);
                var closestNode = GetClosestNode(nodes, vector);
                result.Add(closestNode);
            }

            return result;
        }

        private static void GenerateWrapNodes(WorldPlane plane, IEnumerable<Node> nodes, Vector2 offset)
        {
            foreach (var node in nodes)
            {
                plane.CreatePlaceholderNode(node.NormalizedPosition + offset, node);
            }
        }

        private static void StitchEdge(IEnumerable<Node> nodes, WorldPlane plane)
        {
            foreach (var node in nodes)
            {
                if (!node.Connections.Any(c => c.IsWrapConnection))
                {
                    var n1 = node.GetConnectedNodes().FirstOrDefault(n => n.HasWrapConnection);
                    var n2 = node.GetConnectedNodes().FirstOrDefault(n => n.HasWrapConnection && n != n1);

                    if (n1 != null && n2 != null)
                    {
                        plane.CreateConnection(n1, n2, false);
                    }
                }
            }
        }

        public static WorldPlane GenerateRandomizedWorldPlane(string name, bool isCave, int nodeCount)
        {
            var plane = new WorldPlane(name, isCave);
            var positions = GeneratePoissonDiskSamples(nodeCount);

            // Generate all nodes and placeholder nodes
            foreach (var position in positions)
            {
                var node = plane.CreateNode(position);
            }

            // Get the 2 edges of the node collection
            int numChecks = Mathf.CeilToInt(Mathf.Sqrt(plane.Nodes.Count) * 1.5f);
            var botEdge = GetEdgeNodes(plane.Nodes.Where(node => node.NormalizedPosition.y < 0.1f), Vector2.zero, new Vector2(1f, 0f), numChecks);
            var leftEdge = GetEdgeNodes(plane.Nodes.Where(node => node.NormalizedPosition.x < 0.1f), Vector2.zero, new Vector2(0f, 1f), numChecks);

            // Get the corner
            var botLeft = GetClosestNode(leftEdge, Vector2.zero);

            Debug.Log($"Bot edge has {botEdge.Count()} nodes");
            Debug.Log($"Left edge has {leftEdge.Count()} nodes");

            // Set up wrap nodes 
            GenerateWrapNodes(plane, botEdge, new Vector2(0f, 1f));
            GenerateWrapNodes(plane, leftEdge, new Vector2(1f, 0f));

            // Diagonal corner wrap
            plane.CreatePlaceholderNode(botLeft.NormalizedPosition + new Vector2(1f, 1f), botLeft);

            // Generate triangles
            var triangles = Triangulator.GetBowyerWatsonTriangles(plane).ToList();
            var completed = new List<Edge>();

            while (triangles.Any())
            {
                var triangle = triangles[0];

                if (botEdge.Contains(triangle.Vertices[0].Node) && botEdge.Contains(triangle.Vertices[1].Node) && botEdge.Contains(triangle.Vertices[2].Node))
                {
                    // Invalid triangle, since it messes with wrap connections
                    triangles.Remove(triangle);
                }
                else if (leftEdge.Contains(triangle.Vertices[0].Node) && leftEdge.Contains(triangle.Vertices[1].Node) && leftEdge.Contains(triangle.Vertices[2].Node))
                {
                    // Invalid triangle, since it messes with wrap connections
                    triangles.Remove(triangle);
                }
                else
                { 
                    foreach (var edge in triangle.Edges)
                    {
                        if (edge.Point1.Node == null
                            || edge.Point2.Node == null
                            || (edge.Point1.Node.IsPlaceholderNode && edge.Point2.Node.IsPlaceholderNode)
                            || completed.Any(completedEdge => completedEdge.Equals(edge)))
                        {
                            continue;
                        }

                        if (edge.Point1.Node.IsPlaceholderNode)
                        {
                            plane.CreateConnection(edge.Point1.Node.ParentNode, edge.Point2.Node, true);
                        }
                        else if (edge.Point2.Node.IsPlaceholderNode)
                        {
                            plane.CreateConnection(edge.Point1.Node, edge.Point2.Node.ParentNode, true);
                        }
                        else
                        {
                            plane.CreateConnection(edge.Point1.Node, edge.Point2.Node, false);
                        }

                        completed.Add(edge);
                    }

                    triangles.Remove(triangle);
                }
            }

            // Stitch any edge nodes that are missing a connection due to deleting bad triangles
            StitchEdge(leftEdge, plane);
            StitchEdge(botEdge, plane);

            plane.DeletePlaceholderNodes();
            return plane;
        }
    }
}
