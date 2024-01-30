using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

using VisualScript.Connectors;
using VisualScript.Ports;

namespace VisualScript.Nodes
{

    [Serializable]
    public class ANDNode : Node
    {

        [Description("Alle Inputs müssen positiv sein.")]
        public override string Name { get; set; } = "AND";
        public override string Type { get; set; }
        public override string Value { get; set; }

        public ANDNode()
        {

            // Create a quad by default
            points = new Point[]
            {
                new Point(-50, -50),
                new Point(-50, 50),
                new Point(50, 50),
                new Point(50, -50)
            };

            InputPorts.Add(new InputPort(this, "Input 1"));
            OutputPorts.Add(new OutputPort(this, "Output 1"));
        }
        public override void UpdateValue()
        {

            bool result = false;

            int allConnectorsCount = 0;
            int connectorCount = 0;

            foreach (Connector c in Manager.Instance.connectors)
            {

                if (c.EndPort.OwnerNode == this)
                {
                    allConnectorsCount++;

                    if (!string.IsNullOrEmpty(c.StartPort.OwnerNode.Value) && c.StartPort.OwnerNode.Value != "0")
                    {
                        connectorCount++;
                    }

                }
            }

            if (connectorCount == allConnectorsCount && allConnectorsCount > 0)
                result = true;

            Value = result ? "1" : "0";

        }

        public override void Paint(object sender, PaintEventArgs e)
        {

            base.Paint(sender, e);

            e.Graphics.DrawString(Name, Control.DefaultFont, Brushes.Black, CalculateBoundingBox());
            Point newLocation = CalculateBoundingBox().Location;
            newLocation.Y += 15;
            e.Graphics.DrawString("Ergebnis: " + Value, Control.DefaultFont, Brushes.Black, newLocation);

        }

    }
}
