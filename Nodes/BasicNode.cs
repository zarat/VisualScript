using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;
using System.Drawing.Design;
using System.Xml.Serialization;

using VisualScript;
using VisualScript.Ports;
using VisualScript.Connectors;

namespace VisualScript.Nodes
{

    /// <summary>
    /// The type of a variable.
    /// </summary>
    public enum VariableType
    {
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined,
        /// <summary>
        /// Boolean
        /// </summary>
        Boolean,
        /// <summary>
        /// Char
        /// </summary>
        Char,
        /// <summary>
        /// Integer
        /// </summary>
        Integer,
        /// <summary>
        /// Float
        /// </summary>
        Float,
        /// <summary>
        /// Double
        /// </summary>
        Double,
        /// <summary>
        /// String
        /// </summary>
        String
    }

    /// <summary>
    /// An abstract basic node. Allmost all other nodes inherit from this node.
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(AddNode))]
    [XmlInclude(typeof(VariableNode))]
    public abstract class BasicNode 
    {

        [Browsable(false)]
        public virtual int X { get; set; } = 10;

        [Browsable(false)]
        public virtual int Y { get; set; } = 10;

        [Browsable(false)]
        public virtual int Width { get; set; } = 100;

        [Browsable(false)]
        public virtual int Height { get; set; } = 40;

        [Browsable(false)]
        public Rectangle Bounds => CalculateBoundingBox(); // new Rectangle(X, Y, Width, Height);

        /// <summary>
        /// The <seealso cref="InputPort"/>s of the node. 
        /// </summary>
        /// \todo Move this to <seealso cref="Node"/> instead on <seealso cref="BasicNode"/>.
        [Browsable(true)]
        [Description("Eingehende Ports")]
        [Editor(typeof(PortListEditor), typeof(UITypeEditor))]
        //[TypeConverter(typeof(PortListConverter))]
        public virtual List<InputPort> InputPorts { get; set; } = new List<InputPort>();

        /// <summary>
        /// The <seealso cref="OutputPort"/>s of the node. 
        /// </summary>
        /// /// \todo Move this to <seealso cref="Node"/> instead on <seealso cref="BasicNode"/>.
        [Browsable(true)]
        [Description("Ausgehende Ports")]
        [Editor(typeof(PortListEditor), typeof(UITypeEditor))]
        //[TypeConverter(typeof(PortListConverter))]
        public virtual List<OutputPort> OutputPorts { get; set; } = new List<OutputPort>();

        /// <summary>
        /// The name of the node.
        /// </summary>
        private string _name = "Neuer Node";
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The type of this nodes value.
        /// </summary>
        /// \todo
        private VariableType _type = VariableType.Undefined;
        public virtual VariableType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// This nodes value.
        /// </summary>
        private string _value;
        public virtual string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private Point origin;

        public Point Origin { get { return origin; } set { origin = value; } }

        public Point[] points { get; set; }

        public Point[] TranslatedPoints()
        {
            Point[] translatedPoints = new Point[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                translatedPoints[i] = new Point(points[i].X + Origin.X, points[i].Y + Origin.Y);
            }
            return translatedPoints;
        }

        public Rectangle CalculateBoundingBox()
        {
            int minX = int.MaxValue, minY = int.MaxValue;
            int maxX = int.MinValue, maxY = int.MinValue;
            // Verschiebe die Punkte relativ zum Ursprung
            Point[] translatedPoints = new Point[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                translatedPoints[i] = new Point(points[i].X + Origin.X, points[i].Y + Origin.Y);
            }
            foreach (Point point in translatedPoints)
            {
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);
                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
            }
            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        public bool Hit(Point location)
        {
            // Prüfe, ob der Klickpunkt innerhalb des Polygons liegt
            if (null != points)
            {
                // Verschiebe die Punkte relativ zum Ursprung
                Point[] translatedPoints = new Point[points.Length];
                for (int i = 0; i < points.Length; i++)
                {
                    translatedPoints[i] = new Point(points[i].X + Origin.X, points[i].Y + Origin.Y);
                }
                // Überprüfe, ob der Klickpunkt innerhalb des Polygons liegt
                return IsPointInPolygon(location);
            }
            return false;
        }

        private bool IsPointInPolygon(Point location)
        {
            int crossings = 0;
            for (int i = 0; i < points.Length; i++)
            {
                int j = (i + 1) % points.Length;
                if ((points[i].Y <= location.Y && location.Y < points[j].Y) ||
                    (points[j].Y <= location.Y && location.Y < points[i].Y))
                {
                    if (location.X < points[i].X + (points[j].X - points[i].X) * (location.Y - points[i].Y) / (points[j].Y - points[i].Y))
                    {
                        crossings++;
                    }
                }
            }
            return crossings % 2 != 0;
        }

        public void Move(Point delta)
        {
            // Aktualisiere die Position des Ursprungs
            Origin = new Point(Origin.X + delta.X, Origin.Y + delta.Y);
        }

        public void MoveTo(Point delta)
        {
            // Aktualisiere die Position des Ursprungs
            Origin = delta;
        }

        public abstract void Paint(object sender, PaintEventArgs e);

        public abstract void UpdateValue();

        /// <summary>
        /// To manually clean up the node before deleting
        /// </summary>
        public virtual void Dispose()
        {
            Value = "0";
            // get connected connectors and remove them
            foreach(InputPort ip in InputPorts)
            {
                foreach(Connector c in ip.Connectors)
                {
                    c.StartPort.Connected = false;
                    Manager.Instance.connectors.Remove(c);
                }
            }
            foreach (OutputPort op in OutputPorts)
            {
                foreach (Connector c in op.Connectors)
                {
                    c.EndPort.Connected = false;
                    Manager.Instance.connectors.Remove(c);
                }
            }

        }

    }

}
