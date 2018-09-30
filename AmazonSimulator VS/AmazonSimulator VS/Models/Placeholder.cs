using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Placeholder : Model3D
    {
        public Placeholder(double x, double y, double z, double rotationX, double rotationY, double rotationZ) :
            base("placeholder", x, y, z, rotationX, rotationY, rotationZ)
        {
        }
    }
}
