using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Atlas.WorldGen
{
    public static class Triangulator
    {
        public static IEnumerable<Triangle> GetBowyerWatsonTriangles(IEnumerable<Node> nodes)
        {
            var b0 = new Point(0f, 0f);
            var b1 = new Point(0f, 1f);
            var b2 = new Point(1f, 0f);
            var b3 = new Point(1f, 1f);
            var tri1 = new Triangle(b0, b1, b2);
            var tri2 = new Triangle(b1, b2, b3);
            var border = new List<Triangle>() { tri1, tri2 };
            var points = NodesToPoints(nodes);
            var triangulation = new HashSet<Triangle>(border);

            foreach (var point in points)
            {
                var badTriangles = FindBadTriangles(point, triangulation);
                var polygon = FindHoleBoundaries(badTriangles);

                foreach (var triangle in badTriangles)
                {
                    foreach (var vertex in triangle.Vertices)
                    {
                        vertex.AdjacentTriangles.Remove(triangle);
                    }
                }

                triangulation.RemoveWhere(o => badTriangles.Contains(o));

                foreach (var edge in polygon.Where(possibleEdge => possibleEdge.Point1 != point && possibleEdge.Point2 != point))
                {
                    var triangle = new Triangle(point, edge.Point1, edge.Point2);
                    triangulation.Add(triangle);
                }
            }

            triangulation.Remove(tri1);
            triangulation.Remove(tri2);

            return triangulation;
        }

        private static IEnumerable<Point> NodesToPoints(IEnumerable<Node> nodes)
        {
            return nodes.Select(node => new Point(node));
        }

        private static List<Edge> FindHoleBoundaries(ISet<Triangle> badTriangles)
        {
            var edges = new List<Edge>();
            foreach (var triangle in badTriangles)
            {
                edges.Add(new Edge(triangle.Vertices[0], triangle.Vertices[1]));
                edges.Add(new Edge(triangle.Vertices[1], triangle.Vertices[2]));
                edges.Add(new Edge(triangle.Vertices[2], triangle.Vertices[0]));
            }
            var grouped = edges.GroupBy(o => o);
            var boundaryEdges = edges.GroupBy(o => o).Where(o => o.Count() == 1).Select(o => o.First());
            return boundaryEdges.ToList();
        }

        private static ISet<Triangle> FindBadTriangles(Point point, HashSet<Triangle> triangles)
        {
            var badTriangles = triangles.Where(o => o.IsPointInsideCircumcircle(point));
            return new HashSet<Triangle>(badTriangles);
        }
    }

    public class Triangle
    {
        public Point[] Vertices { get; } = new Point[3];
        public Edge[] Edges { get; } = new Edge[3];
        public Vector2 Circumcenter { get; private set; }
        public float RadiusSquared;

        public IEnumerable<Triangle> TrianglesWithSharedEdge
        {
            get
            {
                var neighbors = new HashSet<Triangle>();

                foreach (var vertex in Vertices)
                {
                    var trianglesWithSharedEdge = vertex.AdjacentTriangles.Where(o =>
                    {
                        return o != this && SharesEdgeWith(o);
                    });
                    neighbors.UnionWith(trianglesWithSharedEdge);
                }

                return neighbors;
            }
        }

        public Edge GetSharedEdge(Triangle other)
        {
            return Edges.FirstOrDefault(edge => other.Edges.Any(otherEdge => otherEdge.Equals(edge)));
        }

        public Triangle(Point point1, Point point2, Point point3)
        {
            // In theory this shouldn't happen, but it was at one point so this at least makes sure we're getting a
            // relatively easily-recognised error message, and provides a handy breakpoint for debugging.
            if (point1 == point2 || point1 == point3 || point2 == point3)
            {
                throw new ArgumentException("Must be 3 distinct points");
            }

            if (!IsCounterClockwise(point1, point2, point3))
            {
                Vertices[0] = point1;
                Vertices[1] = point3;
                Vertices[2] = point2;
            }
            else
            {
                Vertices[0] = point1;
                Vertices[1] = point2;
                Vertices[2] = point3;
            }

            Vertices[0].AdjacentTriangles.Add(this);
            Vertices[1].AdjacentTriangles.Add(this);
            Vertices[2].AdjacentTriangles.Add(this);

            Edges[0] = new Edge(point1, point2);
            Edges[1] = new Edge(point2, point3);
            Edges[2] = new Edge(point3, point1);

            UpdateCircumcircle();
        }

        private void UpdateCircumcircle()
        {
            var p0 = Vertices[0];
            var p1 = Vertices[1];
            var p2 = Vertices[2];
            var dA = p0.X * p0.X + p0.Y * p0.Y;
            var dB = p1.X * p1.X + p1.Y * p1.Y;
            var dC = p2.X * p2.X + p2.Y * p2.Y;

            var aux1 = (dA * (p2.Y - p1.Y) + dB * (p0.Y - p2.Y) + dC * (p1.Y - p0.Y));
            var aux2 = -(dA * (p2.X - p1.X) + dB * (p0.X - p2.X) + dC * (p1.X - p0.X));
            var div = (2 * (p0.X * (p2.Y - p1.Y) + p1.X * (p0.Y - p2.Y) + p2.X * (p1.Y - p0.Y)));

            if (div == 0)
            {
                throw new DivideByZeroException();
            }

            var center = new Vector2(aux1 / div, aux2 / div);
            Circumcenter = center;
            RadiusSquared = (center.x - p0.X) * (center.x - p0.X) + (center.y - p0.Y) * (center.y - p0.Y);
        }

        private bool IsCounterClockwise(Point point1, Point point2, Point point3)
        {
            var result = (point2.X - point1.X) * (point3.Y - point1.Y) - (point3.X - point1.X) * (point2.Y - point1.Y);
            return result > 0;
        }

        public bool SharesEdgeWith(Triangle triangle)
        {
            var sharedVertices = Vertices.Where(o => triangle.Vertices.Contains(o)).Count();
            return sharedVertices == 2;
        }

        public bool IsPointInsideCircumcircle(Point point)
        {
            var squared = (point.X - Circumcenter.x) * (point.X - Circumcenter.x) + (point.Y - Circumcenter.y) * (point.Y - Circumcenter.y);
            return squared < RadiusSquared;
        }
    }

    public class Edge
    {
        public Point Point1 { get; }
        public Point Point2 { get; }

        public Edge(Point point1, Point point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            var edge = obj as Edge;

            var samePoints = Point1 == edge.Point1 && Point2 == edge.Point2;
            var samePointsReversed = Point1 == edge.Point2 && Point2 == edge.Point1;
            return samePoints || samePointsReversed;
        }

        public override int GetHashCode()
        {
            int hCode = (int)Point1.X ^ (int)Point1.Y ^ (int)Point2.X ^ (int)Point2.Y;
            return hCode.GetHashCode();
        }
    }

    public class Point
    {
        public float X => Node == null ? x : Node.NormalizedPosition.x;
        public float Y => Node == null ? y : Node.NormalizedPosition.y;
        public Node Node { get; private set; }
        public HashSet<Triangle> AdjacentTriangles { get; } = new HashSet<Triangle>();

        private float x;
        private float y;

        public Point(Node n)
        {
            Node = n;
        }

        public Point(float xPos, float yPos)
        {
            Node = null;
            x = xPos;
            y = yPos;
        }
    }
}