using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Task : IRobotTask
    {
        private bool Complete = false;

        public void MoveRobot()
        {
        }

        public void StartTask(Robot robot)
        {
            throw new NotImplementedException();
        }

        public bool TaskComplete(Robot robot)
        {
            throw new NotImplementedException();
        }
    }
}
