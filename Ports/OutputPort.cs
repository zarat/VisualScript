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
    public class OutputPort : Port
    {

        public OutputPort() { }

        public OutputPort(BasicNode ownerNode, string name) : base(ownerNode, name) { }

        public override Point Location => new Point(OwnerNode.CalculateBoundingBox().X + OwnerNode.CalculateBoundingBox().Width, OwnerNode.CalculateBoundingBox().Y + (OwnerNode.OutputPorts.IndexOf(this)) * portSpacing);
    
    }

}
