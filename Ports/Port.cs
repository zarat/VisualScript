using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualScript.Nodes;

namespace VisualScript.Ports
{

    [Serializable]
    public abstract class Port
    {

        public BasicNode OwnerNode { get; }
        public string Name { get; set; }
        public bool Connected { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// \deprecated Just for serialization
        protected Port() { }

        protected Port(BasicNode ownerNode, string name)
        {
            OwnerNode = ownerNode;
            Name = name;
        }

        public abstract Point Location { get; }

        public Rectangle Bounds => new Rectangle(Location, new Size(10, 10));

        [Browsable(false)]
        public int portSpacing = 10;

    }

}
