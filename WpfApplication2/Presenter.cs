using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication2

{
    public class Presenter
    {
        int depth;
        const int linelenght = 20;
        const int smallLineshift = 5;
        public Presenter()
        {

        }
        System.Windows.UIElement getNextRect(int x,int y,FlowGraphNode node)
        {
            var grid = new MyGrid();
            Rectangle rect = new Rectangle { Width = 70, Height = 30, Fill = Brushes.Azure };
            grid.Children.Add(rect);
            grid.Children.Add(new TextBlock { Text = node.ToString() });
            grid.xpos = x - 35;
            grid.ypos = y;
            return grid;
        }
        List<System.Windows.UIElement> getNextLine(int x,int y)
        {
            List<System.Windows.UIElement> l = new List<System.Windows.UIElement>();
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine.X1 = x;
            myLine.X2 = x;
            myLine.Y1 = y;
            myLine.Y2 = y+20;
            myLine.StrokeThickness = 2;
            l.Add(myLine);
            Line myLine1 = new Line();
            myLine1.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine1.X1 = x;
            myLine1.X2 = x-10;
            myLine1.Y1 = y+20;
            myLine1.Y2 = y+15;
            myLine1.StrokeThickness = 2;
            l.Add(myLine1);
            Line myLine2 = new Line();
            myLine2.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine2.X1 = x;
            myLine2.X2 = x+10;
            myLine2.Y1 = y+20;
            myLine2.Y2 = y+15;
            myLine2.StrokeThickness = 2;
            return l;
        }
        public List<System.Windows.UIElement> BuildFlowControlGraph(string json)
        {
            List<System.Windows.UIElement> shapes = new List<System.Windows.UIElement>();
            JsonParser parser = new JsonParser();
            SyntaxTree tree = new SyntaxTree();
            tree.Create(parser.Deserialize(json));
            List<FlowGraphNode> Nodes=tree.CreateFlowControlGraph(0);
            shapes = generateShapes(Nodes, 165, 0);





            return shapes;
        }

        public List<System.Windows.UIElement> generateShapes(List<FlowGraphNode> nodes, int x, int y)
        {
            List<System.Windows.UIElement> shapes = new List<System.Windows.UIElement>();
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
                            shapes.Add(getNextRect(x,y,node));
                            y += 30;
                            shapes.AddRange(getNextLine(x, y));
                            y += 20;
                            break;
                        }
                }
            }
            return shapes;
        }
    }

    class MyGrid : Grid
    {
        public int xpos { get; set; }
        public int ypos { get; set; }
    }
}
