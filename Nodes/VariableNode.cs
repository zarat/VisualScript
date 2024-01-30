using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

using VisualScript.Ports;

namespace VisualScript.Nodes
{

    [Serializable]
    public class VariableNode : Node
    {

        //[Browsable(true)]
        //public virtual VariableType variableType { get; set; } = VariableType.Default;
        public override string Name { get; set; } = "Neue Variable";

        public override string Type { get; set; } = "";
        public override string Value { get; set; } = "";

        public VariableNode()
        {

            // Create a quad by default
            points = new Point[]
            {
                new Point(-50, -50),
                new Point(-50, 50),
                new Point(50, 50),
                new Point(50, -50)
            };

            OutputPorts.Add(new OutputPort(this, "Output 1"));
        }

        public override void UpdateValue()
        {

        }

        public override void Paint(object sender, PaintEventArgs e)
        {

            base.Paint(sender, e);

            e.Graphics.DrawString(Name, Control.DefaultFont, Brushes.Black, CalculateBoundingBox().Location);

            Rectangle newLocation = CalculateBoundingBox();
            newLocation.Y += 15;
            e.Graphics.DrawString(Type.ToString() + ": " + Value, Control.DefaultFont, Brushes.Black, newLocation);

            /*
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
            */

        }

    }

}
