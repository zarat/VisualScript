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
        public InputPort(Node ownerNode, string name) : base(ownerNode, name)
        {
        }

        public override Point Location => new Point(
           OwnerNode.X - 10,
           OwnerNode.Y + (OwnerNode.InputPorts.IndexOf(this)) * portSpacing
       );
    }

}
