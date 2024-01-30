using System;
using System.Drawing;
using System.Windows.Forms;

using VisualScript.Connectors;
using VisualScript.Ports;

namespace VisualScript.Nodes
{

    [Serializable]
    public class EqualNode : BasicNode
    {

        public string testVal { get; set; } = "0";

        public EqualNode()
        {

            Name = "Equals";
            Type = VariableType.Integer;
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

            bool i = false;

            foreach (Connector c in Manager.Instance.connectors)
            {

                if (c.EndPort.OwnerNode == this)
                {

                    if (!string.IsNullOrEmpty(c.StartPort.OwnerNode.Value.ToString()))
                    {

                        int x = 0;
                        bool check1 = int.TryParse(c.StartPort.OwnerNode.Value.ToString(), out x);
                        int y = 0;
                        bool check2 = int.TryParse(testVal, out y);

                        // both integer
                        if (check1 && check2)
                        {
                            if (x == y)
                                i = true;

                        }

                        else
                        {
                            if (c.StartPort.OwnerNode.Value.Equals(testVal))
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
