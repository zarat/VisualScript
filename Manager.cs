using System.Collections.Generic;

using VisualScript.Nodes;
using VisualScript.Connectors;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System;
using VisualScript.Ports;
using System.Drawing;

namespace VisualScript
{

    public class Manager
    {

        /// <summary>
        /// The manager is a singleton. Only a single instance exists.
        /// </summary>
        private static Manager instance;

        /// <summary>
        /// A List of <seealso cref="BasicNode"/>s that are present in the current scene. 
        /// </summary>
        public List<BasicNode> nodes;

        /// <summary>
        /// A List of <seealso cref="Connector"/>s that are present in the current scene. 
        /// </summary>
        public List<Connector> connectors;

        public Manager()
        {

            nodes = new List<BasicNode>();
            connectors = new List<Connector>();

        }

        public static Manager Instance
        {
            get
            {
                // Falls keine Instanz vorhanden ist, erstelle eine
                if (instance == null)
                {
                    instance = new Manager();
                }
                return instance;
            }
        }

        /// <summary>
        /// Render all the <seealso cref="Connector"/>s and <seealso cref="BasicNode"/>s that are present in the current scene.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Render(object sender, PaintEventArgs e)
        {
            foreach (var connector in connectors)
            {
                connector.Paint(sender, e);
            }
            foreach (BasicNode node in nodes)
            {
                node.Paint(sender, e);
            }
        }

        /// <summary>
        /// Connect 2 <seealso cref="Port"/>s together.
        /// </summary>
        /// <param name="startPort"></param>
        /// <param name="endPort"></param>
        public void ConnectPorts(Port startPort, Port endPort)
        {
            if (endPort is OutputPort || startPort is InputPort)
            {
                MessageBox.Show("Eine Verbindung muss von einem blauen Input starten und bei einem roten Output enden.");
                return;
            }

            if(endPort.Connected)
            {
                MessageBox.Show("Nur eine Verbindung pro Port.");
                return;
            }

            Connector c = new Connector { StartPort = startPort, EndPort = endPort };

            connectors.Add(c);

            startPort.Connected = true;
            endPort.Connected = true;

            startPort.Connectors.Add(c);
            endPort.Connectors.Add(c);

            UpdateValues();
            UpdateConnectors();
        }

        /// <summary>
        /// Legacy
        /// </summary>
        /// \todo 
        public void UpdateConnectors()
        {

            /*
            foreach (var node in nodes)
            {
                foreach (var outputPort in node.OutputPorts)
                {
                    foreach (var inputPort in node.InputPorts)
                    {
                        if (outputPort.Connected && inputPort.Connected)
                        {
                            connectors.Add(new Connector { StartPort = outputPort, EndPort = inputPort });
                        }
                    }
                }
            }
            */

        }

        /// <summary>
        /// Call the <seealso cref="Node.UpdateValue"/> method on each nodes in the current scene.
        /// </summary>
        public void UpdateValues()
        {
            foreach (BasicNode n in nodes)
            {
                n.UpdateValue();
            }
        }

        /// <summary>
        /// Get a List of all <seealso cref="BasicNode"/>s at a specific <seealso cref="Point"/> on screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public List<BasicNode> MatchNode(object sender, MouseEventArgs e)
        {
            List<BasicNode> list = new List<BasicNode>();
            foreach (var node in nodes)
            {
                if (node.Bounds.Contains(e.Location))
                {
                    list.Add(node);
                }
            }
            return list;
        }
        
        public List<Port> MatchPort(object sender, MouseEventArgs e)
        {
            List<Port> list = new List<Port>();
            foreach (var node in nodes)
            {
                var clickedPort = node.InputPorts.Concat<Port>(node.OutputPorts).FirstOrDefault(port => port.Bounds.Contains(e.Location));
                if (clickedPort != null)
                {
                    list.Add(clickedPort);
                }
            }
            return list;
        }
        
        public List<Connector> MatchConnector(object sender, MouseEventArgs e)
        {
            List<Connector> list = new List<Connector>();
            foreach (var connector in Manager.Instance.connectors)
            {
                //var connectorBounds = GetConnectorBounds(connector);
                //if (connectorBounds.Contains(e.Location))
                if (IsPointOnConnector(e.Location, connector))
                {
                    list.Add(connector);
                }
            }
            return list;
        }

        public bool IsPointOnConnector(Point clickPoint, Connector connector)
        {
            var _startPoint = connector.StartPort.Location;
            var _endPoint = connector.EndPort.Location;
            var startPoint = _startPoint;
            var endPoint = _endPoint;
            startPoint.X += 5;
            startPoint.Y += 5;
            endPoint.X += 5;
            endPoint.Y += 5;

            // Überprüfen, ob der Klickpunkt in der Nähe der Linie liegt
            float distance = DistancePointToLine(clickPoint, startPoint, endPoint);
            return distance < 5; // Ändern Sie den Schwellenwert nach Bedarf
        }

        public  float DistancePointToLine(Point point, Point lineStart, Point lineEnd)
        {
            float a = point.X - lineStart.X;
            float b = point.Y - lineStart.Y;
            float c = lineEnd.X - lineStart.X;
            float d = lineEnd.Y - lineStart.Y;

            float dot = a * c + b * d;
            float len_sq = c * c + d * d;
            float param = dot / len_sq;

            float xx, yy;

            if (param < 0)
            {
                xx = lineStart.X;
                yy = lineStart.Y;
            }
            else if (param > 1)
            {
                xx = lineEnd.X;
                yy = lineEnd.Y;
            }
            else
            {
                xx = lineStart.X + param * c;
                yy = lineStart.Y + param * d;
            }

            float dx = point.X - xx;
            float dy = point.Y - yy;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }


    }

    #region Editor Stuff

    /// <summary>
    /// Das Form das statt dem Editorwindow geöffnet wird weil im EditorWindow der Hinzufuegen Button nicht geht.
    /// </summary>
    /// \todo
    public class CustomForm : Form
    {

        private ListBox listBox;
        private object selectedPort;
        private List<InputPort> _inputPorts;
        private List<OutputPort> _outputPorts;
        public object SelectedPort => selectedPort;
        public BasicNode self;

        public CustomForm(object value, BasicNode ownerNode)
        {

            self = ownerNode;

            this.Text = "PortList Editor";

            Button addButton = new Button();
            addButton.Text = "Hinzufügen";
            addButton.Click += AddButton_Click;

            Button removeButton = new Button();
            removeButton.Text = "Entfernen";
            removeButton.Click += RemoveButton_Click;

            this.Controls.Add(addButton);
            this.Controls.Add(removeButton);

            listBox = new ListBox();

            // Fülle die ListBox mit den Ports
            if (value is List<OutputPort> outputPorts)
            {
                _outputPorts = outputPorts;

                foreach (Port c in outputPorts)
                {
                    listBox.Items.Add(c);
                }

            }
            else if (value is List<InputPort> inputPorts)
            {
                _inputPorts = inputPorts;

                foreach (Port c in inputPorts)
                {
                    listBox.Items.Add(c);
                }

            }

            listBox.SelectedIndexChanged += (sender, e) =>
            {
                selectedPort = listBox.SelectedItem;
            };

            this.Controls.Add(listBox);

            // Setze die Position und Größe der Steuerelemente
            addButton.Location = new Point(10, 10);
            removeButton.Location = new Point(100, 10);
            listBox.Location = new Point(10, 40);
            listBox.Size = new Size(200, 150);

        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            // Hier können Sie Logik zum Hinzufügen von Ports implementieren
            // Zum Beispiel: Fügen Sie einen neuen Port zur Liste hinzu
            if (_inputPorts != null)
            {
                _inputPorts.Add(new InputPort(self, "Neues Input"));
                listBox.Items.Add("Neues Input");
            }
            else if (_outputPorts != null)
            {
                _outputPorts.Add(new OutputPort(self, "Neues Output"));
                listBox.Items.Add("Neues Output");
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            // Hier können Sie Logik zum Entfernen von Ports implementieren
            // Zum Beispiel: Entfernen Sie den ausgewählten Port aus der Liste
            if (listBox.SelectedItem != null)
            {
                if (_inputPorts != null)
                {
                    var selectedPort = listBox.SelectedItem as InputPort;
                    if (selectedPort != null)
                    {
                        _inputPorts.Remove(selectedPort);
                        listBox.Items.Remove(selectedPort);
                    }
                }
                else if (_outputPorts != null)
                {
                    var selectedPort = listBox.SelectedItem as OutputPort;
                    if (selectedPort != null)
                    {
                        _outputPorts.Remove(selectedPort);
                        listBox.Items.Remove(selectedPort);
                    }
                }
            }
        }

    }

    /// <summary>
    /// Zeigt ein Dropdown statt dem Editorwindow weil im EditorWindow der Hinzufuegen Button nicht geht.
    /// </summary>
    /// \todo
    public class PortListEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            System.Windows.Forms.Design.IWindowsFormsEditorService editorService = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));

            BasicNode ownerNode = context.Instance as BasicNode;

            if (editorService != null)
            {
                // Öffne das benutzerdefinierte Formular, wenn auf die Eigenschaft geklickt wird
                using (CustomForm customForm = new CustomForm(value, ownerNode))
                {
                    if (editorService.ShowDialog(customForm) == DialogResult.OK)
                    {
                        // Aktualisiere den Wert nach der Auswahl im Formular
                        value = customForm.SelectedPort; // Du musst entsprechend dein Formular anpassen
                    }
                }
            }

            return value;
        }
    }

    /// <summary>
    /// Macht die Anzeige einer List(Port) zu einem String
    /// </summary>
    /// \todo
    public class PortListConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {

            if (value is List<InputPort>)
            {

                // Konvertiere die Liste von Ports in eine Zeichenkette
                List<InputPort> ports = (List<InputPort>)value;
                return string.Join(", ", ports.Select(port => $"{port.OwnerNode.Name} ({port.Name})"));

            }
            else if (value is List<OutputPort>)
            {

                // Konvertiere die Liste von Ports in eine Zeichenkette
                List<OutputPort> ports = (List<OutputPort>)value;
                return string.Join(", ", ports.Select(port => $"{port.OwnerNode.Name} ({port.Name})"));

            }

            return base.ConvertTo(context, culture, value, destinationType);

        }

    }

    #endregion

}
