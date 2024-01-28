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

            if (Value == "0")
                e.Graphics.FillRectangle(Brushes.Red, Bounds);
            else
                e.Graphics.FillRectangle(Brushes.Green, Bounds);

            e.Graphics.DrawString(Name, Control.DefaultFont, Brushes.Black, Bounds.Location);

            Rectangle newLocation = Bounds;
            newLocation.Y += 15;
            e.Graphics.DrawString("Ergebnis: " + (Value == "1" ? "Wahr" : "Falsch"), Control.DefaultFont, Brushes.Black, newLocation);

            // Draw Input Ports
            foreach (var inputPort in InputPorts)
            {
                e.Graphics.FillEllipse(Brushes.Blue, inputPort.Bounds);
            }

            // Draw Output Ports
            foreach (var outputPort in OutputPorts)
            {
                e.Graphics.FillEllipse(Brushes.Red, outputPort.Bounds);
            }
        }

    }
}
