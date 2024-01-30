using System;
using System.Drawing;
using System.Windows.Forms;

using VisualScript.Connectors;
using VisualScript.Ports;
using VisualScript;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace VisualScript.Nodes
{
    [Serializable]
    public class Node : BasicNode
    {

        public override string Name { get; set; } = "Empty Node";

        public override string Type { get; set; }

        public override string Value { get; set; }

        public override void UpdateValue()
        {
            
        }

        public Node()
        {

            // Create a quad by default
            points = new Point[]
            {
                new Point(-50, -50),
                new Point(-50, 50),
                new Point(50, 50),
                new Point(50, -50)
            };

        }

        public override void Paint(object sender, PaintEventArgs e)
        {

            // Zeichnen des Vielecks relativ zum Ursprung
            e.Graphics.DrawPolygon(Pens.Black, TranslatedPoints());
            
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

            // Umschließendes Quadrat zeichnen
            if (points.Length > 0)
            {
                //Rectangle boundingBox = CalculateBoundingBox();
                //e.Graphics.DrawRectangle(Pens.Red, boundingBox);
            }

        }
  
        
    }

}
