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

        public ANDNode()
        {

            Name = "AND";
            Type = VariableType.Boolean;
            Value = "0";

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

            /*
            bool result = false;

            int allConnectorsCount = 0;
            int connectorCount = 0;

            foreach (Connector c in Manager.Instance.connectors)
            {

                if (c.EndPort.OwnerNode == this)
                {
                    allConnectorsCount++;

                    if (!string.IsNullOrEmpty(c.StartPort.OwnerNode.Value.ToString()) && c.StartPort.OwnerNode.Value.ToString() != "0")
                    {
                        connectorCount++;
                    }

                }
            }

            if (connectorCount == allConnectorsCount && allConnectorsCount > 0)
                result = true;

            Value = result ? "1" : "0";
            */

            bool result = true;
            int connectedPorts = 0;

            foreach(InputPort ip in InputPorts)
            {
                if(ip.Connected)
                {
                    if (ip.Connectors[0].StartPort.OwnerNode.Value == "0" || string.IsNullOrEmpty(ip.Connectors[0].StartPort.OwnerNode.Value))
                    {
                        result = false;
                        break;
                    }  
                    connectedPorts++;
                }
            }

            if(connectedPorts == 0)
            {
                result = false;
            }

            Value = result ? "1" : "0";

        }

        public override void Paint(object sender, PaintEventArgs e)
        {

            base.Paint(sender, e);

            if(Value == "1")
            {
                e.Graphics.FillPolygon(Brushes.Green, TranslatedPoints());
            }
            else
            {
                e.Graphics.FillPolygon(Brushes.Red, TranslatedPoints());
            }

            e.Graphics.DrawString(Name, Control.DefaultFont, Brushes.Black, CalculateBoundingBox());
            Point newLocation = CalculateBoundingBox().Location;
            newLocation.Y += 15;
            e.Graphics.DrawString(Type.ToString() + ": " + Value, Control.DefaultFont, Brushes.Black, newLocation);

        }

    }
}
