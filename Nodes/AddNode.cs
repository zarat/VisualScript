using System;
using System.Drawing;
using System.Windows.Forms;

using VisualScript.Connectors;
using VisualScript.Ports;
using VisualScript;

namespace VisualScript.Nodes
{
    [Serializable]
    public class AddNode : Node
    {

        public AddNode()
        {

            Name = "Addition";
            Type = VariableType.Integer;
            Value = "";

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

            int i = 0;
            int counter = 0;

            foreach (Connector c in Manager.Instance.connectors)
            {

                if (c.EndPort.OwnerNode == this)
                {

                    if (!string.IsNullOrEmpty(c.StartPort.OwnerNode.Value.ToString()))
                    {

                        int x;
                        if (!int.TryParse(c.StartPort.OwnerNode.Value.ToString(), out x))
                            continue;

                        if (counter == 0)
                            i = x;
                        else
                            i += x;

                    }

                    counter++;

                }

            }

            Value = i.ToString();

        }

        public override void Paint(object sender, PaintEventArgs e)
        {

            base.Paint(sender, e);

            e.Graphics.DrawString(Name, Control.DefaultFont, Brushes.Black, CalculateBoundingBox());
            Point newLocation = CalculateBoundingBox().Location;
            newLocation.Y += 15;
            e.Graphics.DrawString(Type.ToString() + ": " + Value, Control.DefaultFont, Brushes.Black, newLocation);

            return;
            /*
            e.Graphics.DrawRectangle(Pens.Black, Bounds);
            e.Graphics.DrawString(Name, Control.DefaultFont, Brushes.Black, Bounds.Location);

            Rectangle newLocation = Bounds;
            newLocation.Y += 15;
            e.Graphics.DrawString("Ergebnis: " + Value, Control.DefaultFont, Brushes.Black, newLocation);

            // Draw Output Ports
            foreach (var inputPort in InputPorts)
            {
                e.Graphics.FillEllipse(Brushes.Blue, inputPort.Bounds);
            }

            // Draw Output Ports
            foreach (var outputPort in OutputPorts)
            {
                e.Graphics.FillEllipse(Brushes.Red, outputPort.Bounds);
            }
            */
        }

    }

}
