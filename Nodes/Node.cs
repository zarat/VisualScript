using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;
using System.Drawing.Design;
using System.Xml.Serialization;

using VisualScript;
using VisualScript.Ports;
using VisualScript.Connectors;

namespace VisualScript.Nodes
{

    [Serializable]
    [XmlInclude(typeof(AddNode))]
    [XmlInclude(typeof(VariableNode))]
    public abstract class Node
    {

        [Browsable(false)]
        public virtual int X { get; set; } = 10;

        [Browsable(false)]
        public virtual int Y { get; set; } = 10;

        [Browsable(false)]
        public virtual int Width { get; set; } = 100;

        [Browsable(false)]
        public virtual int Height { get; set; } = 40;

        [Browsable(false)]
        public Rectangle Bounds => new Rectangle(X, Y, Width, Height);

        [Browsable(true)]
        [Description("Eingehende Ports")]
        [Editor(typeof(PortListEditor), typeof(UITypeEditor))]
        public virtual List<InputPort> InputPorts { get; set; } = new List<InputPort>();

        [Browsable(true)]
        [Description("Ausgehende Ports")]
        [Editor(typeof(PortListEditor), typeof(UITypeEditor))]
        public virtual List<OutputPort> OutputPorts { get; set; } = new List<OutputPort>();

        [Browsable(true)]
        [Description("Der Name des Node")]
        public virtual string Name { get; set; }

        [Browsable(true)]
        public virtual string Type { get; set; } = "Undefined";

        [Browsable(true)]
        public virtual string Value { get; set; } = null;

        public abstract void Paint(object sender, PaintEventArgs e);

        public abstract void UpdateValue();

    }

}
