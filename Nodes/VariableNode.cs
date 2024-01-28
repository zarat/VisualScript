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
        public override string Value { get; set; } = "1";

        public VariableNode()
        {
            OutputPorts.Add(new OutputPort(this, "Output 1"));
        }

        public override void UpdateValue()
        {

        }

        public override void Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.DrawRectangle(Pens.Black, Bounds);
            e.Graphics.DrawString(Name, Control.DefaultFont, Brushes.Black, Bounds.Location);


            Rectangle newLocation = Bounds;
            newLocation.Y += 15;
            e.Graphics.DrawString(Type.ToString() + ": " + Value, Control.DefaultFont, Brushes.Black, newLocation);

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
