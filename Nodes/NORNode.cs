﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

using VisualScript.Connectors;
using VisualScript.Ports;

namespace VisualScript.Nodes
{

    /// <summary>
    /// All Inputs müssen negativ sein
    /// </summary>
    [Serializable]
    public class NORNode : Node
    {

        public NORNode()
        {

            Name = "Not OR";
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

            bool i = false;
            int allInputs = 0;
            int positiveInputs = 0;
            int negativeInputs = 0;

            foreach (Connector c in Manager.Instance.connectors)
            {

                if (c.EndPort.OwnerNode == this)
                {

                    allInputs++;

                    if (!string.IsNullOrEmpty(c.StartPort.OwnerNode.Value.ToString()) && c.StartPort.OwnerNode.Value.ToString() != "0")
                    {
                        positiveInputs++;

                    }
                    else
                    {
                        negativeInputs++;
                    }

                }

            }

            if (allInputs > 0 && positiveInputs == 0)
            {
                i = true;
            }

            Value = i == true ? "1" : "0";

        }
        public override void Paint(object sender, PaintEventArgs e)
        {

            if (Value.ToString() == "0")
                e.Graphics.FillRectangle(Brushes.Red, Bounds);
            else
                e.Graphics.FillRectangle(Brushes.Green, Bounds);
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
