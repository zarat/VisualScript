using System.Collections.Generic;

using VisualScript.Nodes;
using VisualScript.Connectors;

namespace VisualScript
{

    public class Manager
    {

        private static Manager instance;

        public List<Node> nodes;
        public List<Connector> connectors;

        public Manager()
        {

            nodes = new List<Node>();
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
        public Node self;

        public CustomForm(object value, Node ownerNode)
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
            
            addButton.Location = new Point(10, 10);
            removeButton.Location = new Point(100, 10);
            listBox.Location = new Point(10, 40);
            listBox.Size = new Size(200, 150);

        }

        private void AddButton_Click(object sender, EventArgs e)
        {
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

            Node ownerNode = context.Instance as Node;

            if (editorService != null)
            {
                using (CustomForm customForm = new CustomForm(value, ownerNode))
                {
                    if (editorService.ShowDialog(customForm) == DialogResult.OK)
                    {
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
    public class PortListTypeConverter : TypeConverter
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

                List<InputPort> ports = (List<InputPort>)value;
                return string.Join(", ", ports.Select(port => $"{port.OwnerNode.Name} ({port.Name})"));

            }
            else if (value is List<OutputPort>)
            {

                List<OutputPort> ports = (List<OutputPort>)value;
                return string.Join(", ", ports.Select(port => $"{port.OwnerNode.Name} ({port.Name})"));

            }

            return base.ConvertTo(context, culture, value, destinationType);

        }

    }

    #endregion


}
