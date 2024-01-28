using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using VisualScript.Ports;

namespace VisualScript.Connectors
{
    [Serializable]
    [XmlInclude(typeof(Port))]
    public class Connector
    {
        [Browsable(false)]
        public Port StartPort { get; set; }
        [Browsable(false)]
        public Port EndPort { get; set; }

        [Browsable(true)]
        [Category("Connector")]
        public string Test
        {
            get { return StartPort.OwnerNode.Type + "::" + StartPort.OwnerNode.Value + " -> " + EndPort.OwnerNode.Type + "::" + EndPort.OwnerNode.Value; }
        }
    }
}
