using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class RobotPickUpScaffholding : IRobotTask
    {
        private Scaffholding scaffholding;
        private bool complete = false;

        public RobotPickUpScaffholding(Scaffholding scaffholding)
        {
            this.scaffholding = scaffholding;
        }

        public void StartTask(Robot robot)
        {
            robot.SetScaffholding(scaffholding);
            complete = true;
        }

        public bool TaskComplete(Robot robot)
        {
            return complete;
        }
    }
}
