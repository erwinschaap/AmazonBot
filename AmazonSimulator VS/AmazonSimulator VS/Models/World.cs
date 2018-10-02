using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;

namespace Models
{
    public class World : IObservable<Command>, IUpdatable
    {
        private List<Model3D> worldObjects = new List<Model3D>();
        private List<Node> nodes = new List<Node>();
        private Graph graph = new Graph();
        private List<IObserver<Command>> observers = new List<IObserver<Command>>();
        private double moveTruck = 0;
        private char[] alphabetPathNodes = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private char[] alphabetScaffholdingNodes = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        private int alphabetPathIndex = 0;
        private int alphabetScaffholdingIndex = 0;


        public World()
        {
            List<Robot> robots = new List<Robot>();
            robots.Add(CreateRobot(0, 0, 0));

            //this.worldManager = new WorldManager(robots,...,...);

            GeneratePathNodes(4, 30, 5, 5, 5, 3);

            Console.WriteLine("Nodes Count :" + nodes.Count());

            int aap = 0;
            foreach (Node node in nodes)
            {
                Console.WriteLine("Node " + aap + " " + node.name + ": " + node.x + ", " + node.y + ", " + node.z + ", ");
                aap++;
                foreach (Node node1 in node.nodes)
                {
                    Console.WriteLine(node1.name);
                }
            }
            Console.WriteLine("Nodes Count :" + nodes.Count());

            Truck truck = CreateTruck(0, 0, 0);
            Robot robot = CreateRobot(0, 0, 0);
            
        }
        private void CreateNode(char name, double x, double y, double z)
        {
            Node node = new Node(name, x, y, z);
            nodes.Add(node);
        }
        private void GeneratePathNodes(int nrOfNodesLeftAndRight, double startXPositionLeftNodes, double startXPositionRightsNodes, double startZPosition, double laneHight, int nrOfScaffholdingsInARow)
        {
            double laneHightLeft = laneHight;
            Node n = new Node(alphabetPathNodes[0], (((startXPositionLeftNodes - startXPositionRightsNodes) / (2 + nrOfScaffholdingsInARow)) * 3), 0, startZPosition);
            alphabetPathIndex++;
            nodes.Add(n);

            for (int i = 1; i < ((2 + nrOfScaffholdingsInARow) + 1); i++)
            {
                double laneHightCount = laneHight;
                if (i == 1 || i == (2 + nrOfScaffholdingsInARow))
                {
                    for (int j = 0; j < nrOfNodesLeftAndRight; j++)
                    {
                        Node node2 = new Node(alphabetPathNodes[alphabetPathIndex], (((startXPositionLeftNodes - startXPositionRightsNodes) / (2 + nrOfScaffholdingsInARow)) * i), 0, startZPosition + laneHightCount);
                        nodes.Add(node2);
                        laneHightCount += laneHight;
                        alphabetPathIndex++;
                    }

                }
                else
                {
                    for (int j = 0; j < (nrOfNodesLeftAndRight * 2); j++)
                    {
                        if (j % 2 == 0)
                        {
                            Node node2 = new Node(alphabetPathNodes[alphabetPathIndex], (((startXPositionLeftNodes - startXPositionRightsNodes) / (2 + nrOfScaffholdingsInARow)) * i), 0, startZPosition + laneHightCount);
                            nodes.Add(node2);
                            laneHightCount += (laneHight / 2);
                            alphabetPathIndex++;
                        }
                        else
                        {
                            Node node2 = new Node(alphabetScaffholdingNodes[alphabetScaffholdingIndex], (((startXPositionLeftNodes - startXPositionRightsNodes) / (2 + nrOfScaffholdingsInARow)) * i), 0, startZPosition + laneHightCount);
                            nodes.Add(node2);
                            laneHightCount += (laneHight / 2);
                            alphabetScaffholdingIndex++;
                        }
                    }
                }
            }
            Node n2 = new Node(alphabetPathNodes[22], (((startXPositionLeftNodes - startXPositionRightsNodes) / (2 + nrOfScaffholdingsInARow)) * 5), 0, startZPosition);
            nodes.Add(n2);
            Node n4 = new Node(alphabetPathNodes[23], (((startXPositionLeftNodes - startXPositionRightsNodes) / (2 + nrOfScaffholdingsInARow)) * 1), 0, startZPosition);
            nodes.Add(n4);

            // Adding Node Connections
            int nrOfNodes = nodes.Count() - 1;

            for (int i = 0; i < nodes.Count(); i++)
            {
                Node n3 = nodes[i];
                List<Node> nodeConnections = new List<Node>();
                if (i == 0)
                {
                    nodeConnections.Add(nodes[33]);
                    nodeConnections.Add(nodes[34]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 1)//Left under corner node
                {
                    nodeConnections.Add(nodes[i + 1]);
                    nodeConnections.Add(nodes[33]);
                    nodeConnections.Add(nodes[5]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 2)//Left second node
                {
                    nodeConnections.Add(nodes[3]);
                    nodeConnections.Add(nodes[1]);
                    nodeConnections.Add(nodes[7]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 3)//Left third node
                {
                    nodeConnections.Add(nodes[4]);
                    nodeConnections.Add(nodes[2]);
                    nodeConnections.Add(nodes[9]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 4)//Left upper corner node
                {
                    nodeConnections.Add(nodes[11]);
                    nodeConnections.Add(nodes[3]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 29)//Right under corner node
                {
                    nodeConnections.Add(nodes[30]);
                    nodeConnections.Add(nodes[21]);
 
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 30)//Right second node
                {
                    nodeConnections.Add(nodes[31]);
                    nodeConnections.Add(nodes[29]);
                    nodeConnections.Add(nodes[23]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 31)//Right third node
                {
                    nodeConnections.Add(nodes[32]);
                    nodeConnections.Add(nodes[30]);
                    nodeConnections.Add(nodes[25]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 32)//Right upper corner node
                {
                    nodeConnections.Add(nodes[27]);
                    nodeConnections.Add(nodes[31]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i > 12 && i < 21 && i % 2 != 0)//Middle row Path node
                {
                    nodeConnections.Add(nodes[i + 1]);
                    nodeConnections.Add(nodes[i + (nrOfNodesLeftAndRight * 2)]);
                    nodeConnections.Add(nodes[i - (nrOfNodesLeftAndRight * 2)]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i > 5 && i < 29 && i % 2 == 0)//Scaffholding nodes
                {
                    nodeConnections.Add(nodes[i - 1]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 5)//Second row first node
                {
                    nodeConnections.Add(nodes[i + 1]);
                    nodeConnections.Add(nodes[1]);
                    nodeConnections.Add(nodes[13]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 7)//Second row second node
                {
                    nodeConnections.Add(nodes[2]);
                    nodeConnections.Add(nodes[15]);
                    nodeConnections.Add(nodes[i + 1]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 9)//Second row third node
                {
                    nodeConnections.Add(nodes[3]);
                    nodeConnections.Add(nodes[17]);
                    nodeConnections.Add(nodes[i + 1]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 11)//Second row Last node
                {
                    nodeConnections.Add(nodes[2]);
                    nodeConnections.Add(nodes[15]);
                    nodeConnections.Add(nodes[i + 1]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 21)//Fourth row first node
                {
                    nodeConnections.Add(nodes[29]);
                    nodeConnections.Add(nodes[13]);
                    nodeConnections.Add(nodes[i + 1]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 23)//Fourth row second node
                {
                    nodeConnections.Add(nodes[30]);
                    nodeConnections.Add(nodes[15]);
                    nodeConnections.Add(nodes[i + 1]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 25)//Fourth row third node
                {
                    nodeConnections.Add(nodes[31]);
                    nodeConnections.Add(nodes[17]);
                    nodeConnections.Add(nodes[i + 1]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 27)//Fourth row third node
                {
                    nodeConnections.Add(nodes[32]);
                    nodeConnections.Add(nodes[19]);
                    nodeConnections.Add(nodes[i + 1]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 33)//Fourth row third node
                {
                    nodeConnections.Add(nodes[0]);
                    nodeConnections.Add(nodes[1]);
                    n3.SetNodeConnection(nodeConnections);
                }
                else if (i == 34)//Fourth row third node
                {
                    nodeConnections.Add(nodes[0]);
                    nodeConnections.Add(nodes[29]);
                    n3.SetNodeConnection(nodeConnections);
                }

                //else if (i == (nrOfNodes - nrOfNodesLeftAndRight))//Right under corner node
                //{
                //    nodeConnections.Add(nodes[i + 1]);
                //    nodeConnections.Add(nodes[0]);
                //    nodeConnections.Add(nodes[i + (nrOfNodesLeftAndRight * 3)]);
                //    n3.SetNodeConnection(nodeConnections);
                //}
                //else if (i == nrOfNodesLeftAndRight)//Left upper corner node
                //{
                //    nodeConnections.Add(nodes[i - 1]);
                //    nodeConnections.Add(nodes[i + (nrOfNodesLeftAndRight * 2)]);
                //    n3.SetNodeConnection(nodeConnections);
                //}
                //else if (i == nrOfNodes)//Right upper corner node 
                //{
                //    nodeConnections.Add(nodes[i - 1]);
                //    nodeConnections.Add(nodes[i + (nrOfNodesLeftAndRight * 3)]);
                //    n3.SetNodeConnection(nodeConnections);
                //}
                //else if (i < nrOfNodesLeftAndRight && i != 0 && i != nrOfNodesLeftAndRight)//All nodes on the left side whom are no corners
                //{
                //    nodeConnections.Add(nodes[i + 1]);
                //    nodeConnections.Add(nodes[i - 1]);
                //    nodeConnections.Add(nodes[i + (nrOfNodesLeftAndRight * 2)]);
                //    n3.SetNodeConnection(nodeConnections);
                //}
                //else if (i > nrOfNodesLeftAndRight && i != (nrOfNodesLeftAndRight + 1) && i != ((nrOfNodesLeftAndRight * 2) + 1))//All nodes on the right side whom are no corners
                //{
                //    nodeConnections.Add(nodes[i + 1]);
                //    nodeConnections.Add(nodes[i - 1]);
                //    nodeConnections.Add(nodes[i + (nrOfNodesLeftAndRight * 3)]);
                //    n3.SetNodeConnection(nodeConnections);
                //}
                //else if (i > (nrOfNodesLeftAndRight * 2) && i < ((nrOfNodesLeftAndRight * 3) + 1))//All nodes to the right of the left side
                //{
                //    nodeConnections.Add(nodes[i - (nrOfNodesLeftAndRight * 2)]);
                //    nodeConnections.Add(nodes[i + nrOfNodesLeftAndRight]);
                //    n3.SetNodeConnection(nodeConnections);
                //}
                //else if (i > (nrOfNodesLeftAndRight * 4) && i < ((nrOfNodesLeftAndRight * 5) + 1))//All nodes to the left of the right side
                //{
                //    nodeConnections.Add(nodes[i - (nrOfNodesLeftAndRight * 3)]);
                //    nodeConnections.Add(nodes[i - nrOfNodesLeftAndRight]);
                //    n3.SetNodeConnection(nodeConnections);
                //}
                //else if (i > (nrOfNodesLeftAndRight * 3) && i < (nrOfNodesLeftAndRight * 4))//All nodes whom are not corners, left row, right row, second left or second right
                //{
                //    nodeConnections.Add(nodes[i + nrOfNodesLeftAndRight]);
                //    nodeConnections.Add(nodes[i - nrOfNodesLeftAndRight]);
                //    n3.SetNodeConnection(nodeConnections);
                //}
            }

            foreach (Node node in nodes)
            {
                //graph.AddVertex(node);
                CreatePlaceholder(node.x, node.y, node.z);
                for (int i = 0; i < alphabetScaffholdingNodes.Length; i++)
                {
                    if (node.name.Equals(alphabetScaffholdingNodes[i]))
                    {
                        CreateScaffholding(node.x, node.y, node.z);
                    }
                }
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