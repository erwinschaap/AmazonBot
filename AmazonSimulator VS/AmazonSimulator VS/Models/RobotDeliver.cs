using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class RobotDeliver : IRobotTask
    {
        private bool complete = false;

        public void StartTask(Robot robot)
        {
            robot.DropScaffholding();
            complete = true;
        }

        public bool TaskComplete(Robot robot)
        {
            return complete;
        }
    }
}
