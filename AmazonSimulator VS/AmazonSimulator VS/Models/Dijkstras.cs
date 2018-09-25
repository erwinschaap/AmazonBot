using System;
using System.Collections.Generic;

namespace Models
{
    class Graph : Calculations
    {
        Dictionary<Node, List<Node>> allNodes = new Dictionary<Node, List<Node>>();
        Dictionary<char, Dictionary<char, int>> vertices = new Dictionary<char, Dictionary<char, int>>();

        public void AddVertex(Node node)
        {
            List<Node> nodesList = node.nodes;
            allNodes.Add(node, nodesList);

            char name = node.name;
            Dictionary<char, int> edges = new Dictionary<char, int>();
            foreach(Node node1 in nodesList)
            {
                int dist = CalculateDistance(node, node1);
                edges.Add(node1.name, dist);
            }
            vertices[name] = edges;
        }

        public List<Node> ShortestPath(char start, char finish, List<Node> nodes)
        {
            var previous = new Dictionary<char, char>();
            var distances = new Dictionary<char, int>();
            var nodesCharList = new List<char>();

            List<char> path = null;
            List<Node> nodesPath = null;

            foreach (var vertex in vertices)
            {
                if (vertex.Key == start)
                {
                    distances[vertex.Key] = 0;
                }
                else
                {
                    distances[vertex.Key] = int.MaxValue;
                }

                nodesCharList.Add(vertex.Key);
            }

            while (nodesCharList.Count != 0)
            {
                nodesCharList.Sort((x, y) => distances[x] - distances[y]);

                char smallest = nodesCharList[0];
                nodesCharList.Remove(smallest);

                if (smallest == finish)
                {
                    path = new List<char>();
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }

                foreach (var neighbor in vertices[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }
            path.Add(start);
            path.Reverse();
            for (int i = 0; i < nodes.Count; i++)
            {
                foreach (Node item in nodes)
                {
                    nodes.Add(item);
                }
            }
            return nodesPath;
            //return path;
        }
    }
}