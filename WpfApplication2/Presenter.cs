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
        public List<MyGrid> grids;
        public Presenter()
        {
            grids = new List<MyGrid>();
        }
        MyGrid getNextRect(int x,int y,FlowGraphNode node)
        {
            var grid = new MyGrid();
            
            Rectangle rect = new Rectangle { Width = Double.NaN, Height = Double.NaN, Fill = Brushes.Azure };
            rect.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            rect.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            grid.Children.Add(rect);
            TextBlock tex = new TextBlock { Text = node.ToString(), Foreground = Brushes.Black };
            grid.Children.Add(tex);
            tex.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            System.Windows.Size size = tex.DesiredSize;
            grid.xpos =x-(int)(size.Width/2);
            grid.ypos = y;
            return grid;
        }
        MyGrid getNextDiamond(int x, int y, FlowGraphNode node)
        {
            var grid = new MyGrid();

            Rectangle rect = new Rectangle { Width = 50, Height = 50, Fill = Brushes.Azure };
            rect.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            rect.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            RotateTransform rotation = new RotateTransform();

            rotation.Angle = 45;
            rect.RenderTransform = rotation;
            grid.Children.Add(rect);
            TextBlock tex = new TextBlock { Text = "\n"+node.ToString(), Foreground = Brushes.Black };
            grid.Children.Add(tex);
            tex.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            System.Windows.Size size = tex.DesiredSize;
            grid.xpos = x -(int)(size.Width/2)+25;
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
            l.Add(myLine2);
            return l;
        }
        List<System.Windows.UIElement> getNextifLine(int x, int y,int x2)
        {
            List<System.Windows.UIElement> l = new List<System.Windows.UIElement>();
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine.X1 = x;
            myLine.X2 = x2;
            myLine.Y1 = y;
            myLine.Y2 = y + 20;
            myLine.StrokeThickness = 2;
            l.Add(myLine);
            //Line myLine1 = new Line();
            //myLine1.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            //myLine1.X1 = x2;
            //myLine1.X2 = x2 - 10;
            //myLine1.Y1 = y + 20;
            //myLine1.Y2 = y + 15;
            //myLine1.StrokeThickness = 2;
            //l.Add(myLine1);
            //Line myLine2 = new Line();
            //myLine2.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            //myLine2.X1 = x2;
            //myLine2.X2 = x2 + 10;
            //myLine2.Y1 = y + 20;
            //myLine2.Y2 = y + 15;
            //myLine2.StrokeThickness = 2;
            //l.Add(myLine2);
            return l;
        }
        public List<System.Windows.UIElement> BuildFlowControlGraph(string json,ref int y)
        {
            List<System.Windows.UIElement> shapes = new List<System.Windows.UIElement>();
            JsonParser parser = new JsonParser();
            SyntaxTree tree = new SyntaxTree();
            tree.Create(parser.Deserialize(json));
            List<FlowGraphNode> Nodes=tree.CreateFlowControlGraph(0);
            shapes = generateShapes(Nodes, 300, ref y );
            return shapes;
        }
        public List<System.Windows.UIElement> generateShapes(List<FlowGraphNode> nodes, int x,ref int y)
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
                            IfNode ifnode = (IfNode)node;
                            grids.Add(getNextDiamond(x, y, node));
                            int x1 = x;
                            int x2 = x;
                            y += 50;
                            int y1 = y;
                            int y2 = y;
                            if (ifnode.left.Count>0)
                            {
                                x1 = x + 200;
                                shapes.AddRange(getNextifLine(x, y, x + 200));
                            }
                            if(ifnode.right.Count>0)
                            {
                                x2 = x - 200;
                                shapes.AddRange(getNextifLine(x, y, x - 200));
                            }
                            y += 20;
                            int tmp = y;
                            if (ifnode.left.Count > 0)
                            {
                                shapes.AddRange(generateShapes(ifnode.left, x + 200, ref y));
                                y1 = y;
                                y += 20;
                            }
                            if (ifnode.right.Count > 0)
                            {
                                shapes.AddRange(generateShapes(ifnode.right, x - 200, ref tmp));
                                y2 = tmp;
                                tmp += 20;
                            }
                            if (y < tmp)
                            {

                                y = tmp;
                            }
                            shapes.Add(new Line() {X1 = x1, X2 =x, Y1 = y1, Y2 =y , Stroke= Brushes.LightBlue});
                            shapes.Add(new Line() { X1 = x2, X2 = x, Y1 = y2, Y2 = y , Stroke = Brushes.LightBlue });
                            
                            break;
                        }
                    default:
                        {
                            grids.Add(getNextRect(x, y, node));
                            System.Windows.UIElement el = getNextRect(x, y, node);
                            el.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                            System.Windows.Size size = el.DesiredSize;
                            y += (int)size.Height;
                            if (node != nodes.Last())
                            {
                                shapes.AddRange(getNextLine(x, y));
                                y += 20;

                            }
                            break;
                        }
                }
            }
            return shapes;
        }
    }

    public class MyGrid : Grid
    {
        public int xpos { get; set; }
        public int ypos { get; set; }
    }
}
