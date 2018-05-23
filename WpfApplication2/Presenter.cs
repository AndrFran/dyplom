using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace WpfApplication2

{
    public class Presenter
    {
        public Presenter()
        {

        }
        public List<Shape> BuildFlowControlGraph(string json)
        {
            List<Shape> shapes = new List<Shape>();
            JsonParser parser = new JsonParser();
            SyntaxTree tree = new SyntaxTree();
            tree.Create(parser.Deserialize(json));
            List<FlowGraphNode> Nodes=tree.CreateFlowControlGraph(0);
            


            

            return shapes;
        }

        public List<Shape> generateShapes(List<FlowGraphNode> nodes)
        {
            List<Shape> shapes = new List<Shape>();
            foreach (FlowGraphNode node in nodes)
            {

                switch (node.getNodeType())
                {
                    case NodeType.E_WHILE:
                        {
                            break;
                        }
                    case NodeType.E_IF:
                        {
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            return shapes;
        }
    }
}
