using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

using VisualScript.Ports;

namespace VisualScript.Connectors
{
    [Serializable]
    [XmlInclude(typeof(Port))]
    public class Connector
    {
        [Browsable(false)]
        public Port StartPort { get; set; }
        [Browsable(false)]
        public Port EndPort { get; set; }

        [Browsable(true)]
        [Category("Connector")]
        public string From
        {
            get { return StartPort.OwnerNode.Name + "::" + StartPort.OwnerNode.Value; }
        }
        [Browsable(true)]
        [Category("Connector")]
        public string To
        {
            get { return EndPort.OwnerNode.Name + "::" + EndPort.OwnerNode.Value; }
        }

        public void Paint(object sender, PaintEventArgs e)
        {
            // Offset
            Point start = StartPort.Location;
            start.X += 5;
            start.Y += 5;
            Point end = EndPort.Location;
            end.X += 5;
            end.Y += 5;
            e.Graphics.DrawLine(Pens.Black, start, end);
        }
    }
}
