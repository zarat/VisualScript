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
    public class Connector
    {

        /// <summary>
        /// The start port of the connector.
        /// </summary>
        [Browsable(false)]
        public Port StartPort { get; set; }

        /// <summary>
        /// The end port of the connector.
        /// </summary>
        [Browsable(false)]
        public Port EndPort { get; set; }

        /// <summary>
        /// To indicate the starting node.
        /// </summary>
        [Browsable(true)]
        [Category("Connector")]
        public string From
        {
            get { return StartPort.OwnerNode.Name + " => " + StartPort.OwnerNode.Value; }
        }

        /// <summary>
        /// To indicate the end node.
        /// </summary>
        [Browsable(true)]
        [Category("Connector")]
        public string To
        {
            get { return EndPort.OwnerNode.Name + " => " + EndPort.OwnerNode.Value; }
        }

        /// <summary>
        /// Paint the connector to the forms graphics.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Paint(object sender, PaintEventArgs e)
        {
            // Offset
            Point start = StartPort.Location;
            start.X += StartPort.Bounds.Width/2;
            start.Y += StartPort.Bounds.Height/2;
            Point end = EndPort.Location;
            end.X += EndPort.Bounds.Width/2;
            end.Y += EndPort.Bounds.Height/2;
            e.Graphics.DrawLine(Pens.Black, start, end);
        }
    }
}
