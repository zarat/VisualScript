using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualScript.Nodes;
using VisualScript.Connectors;
using System.Xml.Serialization;

namespace VisualScript.Ports
{

    [Serializable]
    public abstract class Port
    {

        /// <summary>
        /// The parent node of the port. 
        /// </summary>
        public BasicNode OwnerNode { get; }

        /// <summary>
        /// A List of <seealso cref="Connector"/>.
        /// Should be only a single connector.
        /// </summary>
        /// \todo
        public List<Connector> Connectors { get; set; } = new List<Connector>();
        public string Name { get; set; }

        /// <summary>
        /// Returns true if the port is connected.
        /// </summary>
        public bool Connected { get; set; }

        public int Width { get; set; } = 10;
        public int Height { get; set; } = 10;

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

        /// <summary>
        /// The location of the port.
        /// </summary>
        public abstract Point Location { get; }

        /// <summary>
        /// The boundingbox of the port.
        /// </summary>
        public Rectangle Bounds => new Rectangle(Location, new Size(Width, Height));

        /// <summary>
        /// A helper variable to place multiple lines of text.
        /// </summary>
        [Browsable(false)]
        public int portSpacing = 15;

    }

}
