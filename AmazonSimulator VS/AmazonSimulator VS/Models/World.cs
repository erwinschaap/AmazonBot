using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;

namespace Models
{
    public class World : IObservable<Command>, IUpdatable
    {
        private List<Model3D> worldObjects = new List<Model3D>();
        private List<IObserver<Command>> observers = new List<IObserver<Command>>();
        private double MoveTruck = 0;

        public World()
        {
            Truck truck = CreateTruck(0, 0, 0);
            truck.Move(10, 0, 0);
            Robot robot = CreateRobot(0, 0, 0);
            robot.Move(8, 0, 0);

        }

        private Scaffholding CreateScaffholding(double x, double y, double z)
        {
            Scaffholding scaffholding = new Scaffholding(x, y, z, 0, 0, 0);
            worldObjects.Add(scaffholding);
            return scaffholding;
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
                MoveTruck += 0.05;
                if (u is IUpdatable)
                {
                    if (u is Truck)
                    {
                        u.Move(MoveTruck, 0, 0);
                    }
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