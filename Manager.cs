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

}
