using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualScript.Nodes;

namespace VisualScript.Ports
{
    [Serializable]
    public class InputPort : Port
    {

        public InputPort() { }

        public InputPort(BasicNode ownerNode, string name) : base(ownerNode, name)
        {
        }

        //public override Point Location => new Point( OwnerNode.CalculateBoundingBox().X - 10, OwnerNode.CalculateBoundingBox().Y + (OwnerNode.InputPorts.IndexOf(this)) * portSpacing );
        public override Point Location => new Point(OwnerNode.CalculateBoundingBox().X - Width, OwnerNode.CalculateBoundingBox().Y + (OwnerNode.InputPorts.IndexOf(this)) * portSpacing);

    }

}
