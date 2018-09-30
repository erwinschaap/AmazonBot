using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;

namespace Models
{
    public class World : Calculations, IObservable<Command>, IUpdatable
    {
        private List<Model3D> worldObjects = new List<Model3D>();
        private List<Node> nodes = new List<Node>();
        private Graph graph = new Graph();
        private List<IObserver<Command>> observers = new List<IObserver<Command>>();
        private double moveTruck = 0;
        public char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private int alphabetIndex;

        public World()
        {
            GeneratePathNodes(4, 10, 0, 5, 3);

            foreach (Node node in nodes)
            {
                Console.WriteLine("Node " + node.name + ": " + node.x + ", " + node.y + ", " + node.z);
                //foreach (Node node1 in node.nodes)
                //{
                //    Console.WriteLine(" " + node1.name);
                //}
            }
            Console.WriteLine("Nodes Count :" + nodes.Count());

            Console.WriteLine("Node Path");
            foreach (Node node in graph.ShortestPath('A', 'C', nodes))
            {
                Console.WriteLine("Node " + node.name + ": " + node.x + ", " + node.y + ", " + node.z);
            }


            Truck truck = CreateTruck(0, 0, 0);
            Robot robot = CreateRobot(0, 0, 0);
        }

        private void GeneratePathNodes(int nrOfNodesLeftAndRight, double startXPositionLeftNodes, double startXPositionRightsNodes, double laneHight, int nrOfScaffholdingsInARow)
        {
            double laneHightLeft = laneHight;
            Node n = new Node(alphabet[0], ((startXPositionLeftNodes - startXPositionRightsNodes) / 2), 0, 0);
            nodes.Add(n);

            //Nodes in left row
            for (int i = 0; i < nrOfNodesLeftAndRight; i++)
            {
                foreach (Node node in nodes)
                {
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        if (node.name == alphabet[j])
                        {
                            alphabetIndex = j + 1;
                        }
                    }
                }
                Node n2 = new Node(alphabet[alphabetIndex], startXPositionLeftNodes, 0, laneHightLeft);
                nodes.Add(n2);
                laneHightLeft += laneHight;
            }

            double laneHightRight = laneHight;
            //Nodes in right row
            for (int i = 0; i < nrOfNodesLeftAndRight; i++)
            {
                foreach (Node node in nodes)
                {
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        if (node.name == alphabet[j])
                        {
                            alphabetIndex = j + 1;
                        }
                    }
                }
                Node node2 = new Node(alphabet[alphabetIndex], startXPositionRightsNodes, 0, laneHightRight);
                nodes.Add(node2);
                laneHightRight += laneHight;
            }

            if (nrOfScaffholdingsInARow != 0)
            {
                for (int i = (nrOfScaffholdingsInARow + 1); i > 1; i--)
                {
                    double laneHightMiddle = laneHight;
                    for (int j = 0; j < nrOfNodesLeftAndRight; j++)
                    {
                        foreach (Node node in nodes)
                        {
                            for (int h = 0; h < alphabet.Length; h++)
                            {
                                if (node.name == alphabet[h])
                                {
                                    alphabetIndex = h + 1;
                                }
                            }
                        }
                        Node node2 = new Node(alphabet[alphabetIndex], ((startXPositionRightsNodes / (nrOfScaffholdingsInARow + 1)) * i), 0, laneHightMiddle);
                        Console.WriteLine("Node2 " + node2.name + ": " + node2.x + ", " + node2.y + ", " + node2.z);
                        nodes.Add(node2);
                        laneHightMiddle += laneHight;
                    }
                }
            }


            ///Adding Node Connections
            //for (int i = 0; i < nodes.Count(); i++)
            //{
            //    Node n3 = nodes[i];
            //    List<Node> nodeConnections = new List<Node>();
            //    if (i == 0)
            //    {
            //        nodeConnections.Add(nodes[i + 1]);
            //        nodeConnections.Add(nodes[nrOfNodesLeftAndRight + 1]);
            //        n3.SetNodeConnection(nodeConnections);
            //    }
            //    else if (i == 1)//Left under corner node
            //    {
            //        nodeConnections.Add(nodes[i + 1]);
            //        nodeConnections.Add(nodes[0]);
            //        nodeConnections.Add(nodes[i + (nrOfNodesLeftAndRight * 2)]);
            //        n3.SetNodeConnection(nodeConnections);
            //    }
            //    else if (i == (nrOfNodes - nrOfNodesLeftAndRight))//Right under corner node
            //    {
            //        nodeConnections.Add(nodes[i + 1]);
            //        nodeConnections.Add(nodes[0]);
            //        nodeConnections.Add(nodes[i + (nrOfNodesLeftAndRight * 3)]);
            //        n3.SetNodeConnection(nodeConnections);
            //    }
            //    else if (i == nrOfNodesLeftAndRight)//Left upper corner node
            //    {
            //        nodeConnections.Add(nodes[i - 1]);
            //        nodeConnections.Add(nodes[i + (nrOfNodesLeftAndRight * 2)]);
            //        n3.SetNodeConnection(nodeConnections);
            //    }
            //    else if (i == nrOfNodes)//Right upper corner node 
            //    {
            //        nodeConnections.Add(nodes[i - 1]);
            //        nodeConnections.Add(nodes[i + (nrOfNodesLeftAndRight * 3)]);
            //        n3.SetNodeConnection(nodeConnections);
            //    }
            //    else if (i < nrOfNodesLeftAndRight && i != 0 && i != nrOfNodesLeftAndRight)//All nodes on the left side whom are no corners
            //    {
            //        nodeConnections.Add(nodes[i + 1]);
            //        nodeConnections.Add(nodes[i - 1]);
            //        nodeConnections.Add(nodes[i + (nrOfNodesLeftAndRight * 2)]);
            //        n3.SetNodeConnection(nodeConnections);
            //    }
            //    else if (i > nrOfNodesLeftAndRight && i != (nrOfNodesLeftAndRight + 1) && i != ((nrOfNodesLeftAndRight * 2) + 1))//All nodes on the right side whom are no corners
            //    {
            //        nodeConnections.Add(nodes[i + 1]);
            //        nodeConnections.Add(nodes[i - 1]);
            //        nodeConnections.Add(nodes[i + (nrOfNodesLeftAndRight * 3)]);
            //        n3.SetNodeConnection(nodeConnections);
            //    }
            //    else if (i > (nrOfNodesLeftAndRight * 2) && i < ((nrOfNodesLeftAndRight * 3) + 1))//All nodes to the right of the left side
            //    {
            //        nodeConnections.Add(nodes[i - (nrOfNodesLeftAndRight * 2)]);
            //        nodeConnections.Add(nodes[i + nrOfNodesLeftAndRight]);
            //        n3.SetNodeConnection(nodeConnections);
            //    }
            //    else if (i > (nrOfNodesLeftAndRight * 4) && i < ((nrOfNodesLeftAndRight * 5) + 1))//All nodes to the left of the right side
            //    {
            //        nodeConnections.Add(nodes[i - (nrOfNodesLeftAndRight * 3)]);
            //        nodeConnections.Add(nodes[i - nrOfNodesLeftAndRight]);
            //        n3.SetNodeConnection(nodeConnections);
            //    }
            //    else if (i > (nrOfNodesLeftAndRight * 3) && i < (nrOfNodesLeftAndRight * 4))//All nodes whom are not corners, left row, right row, second left or second right
            //    {
            //        nodeConnections.Add(nodes[i + nrOfNodesLeftAndRight]);
            //        nodeConnections.Add(nodes[i - nrOfNodesLeftAndRight]);
            //        n3.SetNodeConnection(nodeConnections);
            //    }

            //}

            foreach (Node node in nodes)
            {
                //graph.AddVertex(node);
                CreatePlaceholder(node.x, node.y, node.z);
            }
        }

        private Scaffholding CreateScaffholding(double x, double y, double z)
        {
            Scaffholding scaffholding = new Scaffholding(x, y, z, 0, 0, 0);
            worldObjects.Add(scaffholding);
            return scaffholding;
        }

        private Placeholder CreatePlaceholder(double x, double y, double z)
        {
            Placeholder placeholder = new Placeholder(x, y, z, 0, 0, 0);
            worldObjects.Add(placeholder);
            return placeholder;
        }

        private Truck CreateTruck(double x, double y, double z)
        {
            Truck truck = new Truck(x, y, z, 0, 0, 0);
            worldObjects.Add(truck);
            return truck;
        }

        private Robot CreateRobot(double x, double y, double z)
        {
            Robot robot = new Robot(x, y, z, 0, 0, 0);
            worldObjects.Add(robot);
            return robot;
        }

        public IDisposable Subscribe(IObserver<Command> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);

                SendCreationCommandsToObserver(observer);
            }
            return new Unsubscriber<Command>(observers, observer);
        }

        private void SendCommandToObservers(Command command)
        {
            for (int i = 0; i < this.observers.Count; i++)
            {
                this.observers[i].OnNext(command);
            }
        }

        private void SendCreationCommandsToObserver(IObserver<Command> obs)
        {
            foreach (Model3D m3d in worldObjects)
            {
                obs.OnNext(new UpdateModel3DCommand(m3d));
            }

        }

        public bool Update(int tick)
        {
            for (int i = 0; i < worldObjects.Count; i++)
            {
                Model3D u = worldObjects[i];

                if (u is IUpdatable)
                {
                    bool needsCommand = ((IUpdatable)u).Update(tick);

                    if (needsCommand)
                    {
                        SendCommandToObservers(new UpdateModel3DCommand(u));
                    }
                }
            }

            return true;
        }
    }

    internal class Unsubscriber<Command> : IDisposable
    {
        private List<IObserver<Command>> _observers;
        private IObserver<Command> _observer;

        internal Unsubscriber(List<IObserver<Command>> observers, IObserver<Command> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}