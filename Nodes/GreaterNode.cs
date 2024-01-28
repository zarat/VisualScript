using System;
using System.Drawing;
using System.Windows.Forms;

using VisualScript.Connectors;
using VisualScript.Ports;

namespace VisualScript.Nodes
{

    [Serializable]
    public class GreaterNode : Node
    {

        public override string Name { get; set; } = "Greater";

        public override string Type { get; set; }
        public override string Value { get; set; } = "0";

        public string testVal { get; set; } = "0";

        public GreaterNode()
        {
            InputPorts.Add(new InputPort(this, "Input 1"));
            OutputPorts.Add(new OutputPort(this, "Output 1"));
        }

        public override void UpdateValue()
        {

            bool i = false;

            foreach (Connector c in Manager.Instance.connectors)
            {

                if (c.EndPort.OwnerNode == this)
                {

                    if (!string.IsNullOrEmpty(c.StartPort.OwnerNode.Value))
                    {

                        int x = 0;
                        bool check = int.TryParse(c.StartPort.OwnerNode.Value, out x);
                        if (check)
                        {
                            int y = 0;
                            check = int.TryParse(testVal, out y);

                            if (x > y)
                                i = true;
                        }


                    }


                }

            }

            Value = i == true ? "1" : "0";

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
            e.Graphics.DrawString("Ergebnis: " + Value, Control.DefaultFont, Brushes.Black, newLocation);

            // Draw Output Ports
            foreach (var inputPort in InputPorts)
            {
                e.Graphics.FillEllipse(Brushes.Red, inputPort.Bounds);
            }

            // Draw Output Ports
            foreach (var outputPort in OutputPorts)
            {
                e.Graphics.FillEllipse(Brushes.Red, outputPort.Bounds);
            }
        }

    }
}
