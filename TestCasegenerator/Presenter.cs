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
        private static Presenter instance;
        SyntaxTree tree;
        public string preprocessorpath { get; set; }
        public string prepargs { get; set;}
        #region pidor
        int depth;
        const int linelenght = 20;
        const int smallLineshift = 5;
        public List<MyGrid> grids;
        #endregion
        public static List<String> chosen { get; set; }
        private  Presenter()
        {
            tree = new SyntaxTree();
            grids = new List<MyGrid>();
            preprocessorpath = "C:\\Users\\corov\\Desktop\\Dyplom\\LLVM\\bin\\clang.exe";
            prepargs = "-E"; 
        }
        public static Presenter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Presenter();
                }
                return instance;
            }
        }
        MyGrid getNextRect(int x,int y,FlowGraphNode node, List<FlowGraphNode> path)
        {
            var grid = new MyGrid();
            Rectangle rect = null;
            if (path.Contains(node))
            { rect = new Rectangle { Width = Double.NaN, Height = Double.NaN, Fill = Brushes.LightBlue }; }
            else
            {
                rect = new Rectangle { Width = Double.NaN, Height = Double.NaN, Fill = Brushes.Azure };
            }
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
        MyGrid getNextDiamond(int x, int y, FlowGraphNode node, List<FlowGraphNode> path)
        {
            var grid = new MyGrid();
            Rectangle rect = null;
            if (path.Contains(node))
            {  rect = new Rectangle { Width = 50, Height = 50, Fill = Brushes.LightBlue }; }
            else
            {
                 rect = new Rectangle { Width = 50, Height = 50, Fill = Brushes.Azure };
            }
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
        enum Direction
        {
            E_RIGHT,
            E_LEFT,
            E_TOP,
            E_DOWN
        }
        List<System.Windows.UIElement> getArrows(int x,int y, Direction dir)
        {
            List<System.Windows.UIElement> l = new List<System.Windows.UIElement>();
            Line myLine1 = new Line();
            Line myLine2 = new Line();

            switch (dir)
            {
                case Direction.E_RIGHT:
                    {
                        myLine1.X1 = x;
                        myLine1.X2 = x + 10;
                        myLine1.Y1 = y;
                        myLine1.Y2 = y - 10;
                        myLine2.X1 = x;
                        myLine2.X2 = x + 10;
                        myLine2.Y1 = y;
                        myLine2.Y2 = y + 10;
                        break;
                    }
                case Direction.E_LEFT:
                    {

                        break;
                    }
                case Direction.E_TOP:
                    {
        
                        break;
                    }
                case Direction.E_DOWN:
                    {

                        break;
                    }
            }
            myLine1.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
          
            myLine1.StrokeThickness = 2;
            l.Add(myLine1);

            myLine2.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            
            myLine2.StrokeThickness = 2;
            l.Add(myLine2);
            return l;
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
        public List<string> ParseFuncNames(string json)
        {
            JsonParser parser = new JsonParser();
            chosen= tree.Create(parser.Deserialize(json));
            return chosen;
        }
        public TestCases generateCases(List<string> funcnames)
        {
            List<TestCase> ls = new List<TestCase>();
            TestCases cases = new TestCases();
            tree.ParseFunctions();
            List<Function> funcs = tree.GetFunctionByNames(funcnames);
            TestCaseBuilder builder = new TestCaseBuilder();
            foreach(Function f in funcs)
            {
                ls.AddRange(builder.BuildTestCases(f ,tree.GlobalScope,tree.ParsedFunctions));
            }
            cases.testcases = ls.AsEnumerable();
            return cases;
        }
        public List<System.Windows.UIElement> BuildFlowControlGraph(ref int y, List<FlowGraphNode> Nodes, List<FlowGraphNode> path)
        {
            List<System.Windows.UIElement> shapes = new List<System.Windows.UIElement>();
            grids = new List<MyGrid>();
            //GetFunctionByNames
            //List<FlowGraphNode> Nodes=tree.CreateFlowControlGraph(1);
            //TestCaseBuilder builder = new TestCaseBuilder();
            //builder.BuildTestCases(Nodes);
            shapes = generateShapes(Nodes, 400, ref y ,path);
            return shapes;
        }
        public List<System.Windows.UIElement> generateShapes(List<FlowGraphNode> nodes, int x,ref int y, List<FlowGraphNode> path)
        {
            
            List<System.Windows.UIElement> shapes = new List<System.Windows.UIElement>();
            foreach (FlowGraphNode node in nodes)
            {

                switch (node.getNodeType())
                {
                    case NodeType.E_WHILE:
                        {
                            int whiley = 0;
                            WhileNode whilenode = (WhileNode)node;
                            grids.Add(getNextDiamond(x, y, node,path));
                            System.Windows.UIElement el = getNextDiamond(x, y, node,path);
                            el.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                            System.Windows.Size size = el.DesiredSize;

                            y += (int)size.Height+20;
                            whiley = y - ((int)size.Height + 20)/2;
                            shapes.AddRange(getNextLine(x, y));
                            y += 20;
                            shapes.AddRange(generateShapes(whilenode.loop, x, ref y,path));
                            shapes.Add(new Line() { X1 = x, X2 = x-150, Y1 = y+20, Y2 = y+20, Stroke = Brushes.LightBlue });
                            shapes.Add(new Line() { X1 = x-150, X2 = x - 150, Y1 = y+20, Y2 = whiley, Stroke = Brushes.LightBlue });
                            shapes.Add(new Line() { X1 = x - 150, X2 = x, Y1 = whiley, Y2 = whiley, Stroke = Brushes.LightBlue });

                            shapes.Add(new Line() { X1 = x, X2 = x + 150, Y1 = y-10, Y2 = y- 10, Stroke = Brushes.LightBlue });
                            shapes.Add(new Line() { X1 = x + 150, X2 = x + 150, Y1 = y  -10, Y2 = whiley, Stroke = Brushes.LightBlue });
                            shapes.Add(new Line() { X1 = x + 150, X2 = x, Y1 = whiley, Y2 = whiley, Stroke = Brushes.LightBlue });
                            shapes.AddRange(getArrows(x + (int)size.Width/2-20, whiley, Direction.E_RIGHT));
                            if (NodeType.E_FOR == whilenode.loop.Last().getNodeType()||
                                NodeType.E_WHILE == whilenode.loop.Last().getNodeType())
                            {

                            }
                            else
                            {
                                shapes.AddRange(getNextLine(x, y));
                                y += 20;
                            }
                            break;
                        }
                    case NodeType.E_FOR:
                        {
                            int whiley = 0;
                            ForNode whilenode = (ForNode)node;
                            grids.Add(getNextDiamond(x, y, node,path));
                            System.Windows.UIElement el = getNextDiamond(x, y, node,path);
                            el.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                            System.Windows.Size size = el.DesiredSize;

                            y += (int)size.Height + 20;
                            whiley = y - ((int)size.Height + 20) / 2;
                            shapes.AddRange(getNextLine(x, y));
                            y += 20;
                            shapes.AddRange(generateShapes(whilenode.loop, x, ref y,path));
                            shapes.Add(new Line() { X1 = x, X2 = x - 150, Y1 = y + 20, Y2 = y + 20, Stroke = Brushes.LightBlue });
                            shapes.Add(new Line() { X1 = x - 150, X2 = x - 150, Y1 = y + 20, Y2 = whiley, Stroke = Brushes.LightBlue });
                            shapes.Add(new Line() { X1 = x - 150, X2 = x, Y1 = whiley, Y2 = whiley, Stroke = Brushes.LightBlue });

                            shapes.Add(new Line() { X1 = x, X2 = x + 150, Y1 = y - 10, Y2 = y - 10, Stroke = Brushes.LightBlue });
                            shapes.Add(new Line() { X1 = x + 150, X2 = x + 150, Y1 = y - 10, Y2 = whiley, Stroke = Brushes.LightBlue });
                            shapes.Add(new Line() { X1 = x + 150, X2 = x, Y1 = whiley, Y2 = whiley, Stroke = Brushes.LightBlue });
                            shapes.AddRange(getArrows(x + (int)size.Width / 2, whiley, Direction.E_RIGHT));
                            if (NodeType.E_FOR == whilenode.loop.Last().getNodeType() ||
                                    NodeType.E_WHILE == whilenode.loop.Last().getNodeType())
                            {

                            }
                            else
                            {
                                shapes.AddRange(getNextLine(x, y));
                                y += 20;
                            }
                            break;
                        }
                    case NodeType.E_IF:
                        {
                            IfNode ifnode = (IfNode)node;
                            grids.Add(getNextDiamond(x, y, node,path));
                            int x1 = x;
                            int x2 = x;
                            y += 50;
                            int y1 = y;
                            int y2 = y;
                            if (ifnode.left.Count>0)
                            {
                                x1 = x + 300;
                                shapes.AddRange(getNextifLine(x, y, x + 300));
                            }
                            if(ifnode.right.Count>0)
                            {
                                x2 = x - 300;
                                shapes.AddRange(getNextifLine(x, y, x - 300));
                            }
                            y += 20;
                            int tmp = y;
                            if (ifnode.left.Count > 0)
                            {
                                shapes.AddRange(generateShapes(ifnode.left, x + 300, ref y,path));
                                y1 = y;
                                y += 20;
                            }
                            if (ifnode.right.Count > 0)
                            {
                                shapes.AddRange(generateShapes(ifnode.right, x - 300, ref tmp,path));
                                y2 = tmp;
                                tmp += 20;
                            }
                            if (y < tmp)
                            {

                                y = tmp;
                            }
                            if (ifnode.left.Count>0)
                            {
                                if (NodeType.E_RETURN != ifnode.left.Last().getNodeType())
                                {

                                    shapes.Add(new Line() { X1 = x1, X2 = x, Y1 = y1, Y2 = y, Stroke = Brushes.LightBlue });
                                }
                            }
                            else
                            {
                                shapes.Add(new Line() { X1 = x1, X2 = x, Y1 = y1, Y2 = y, Stroke = Brushes.LightBlue });
                            }
                            if (ifnode.right.Count>0)
                            {
                                if (NodeType.E_RETURN != ifnode.right.Last().getNodeType())
                                {
                                    shapes.Add(new Line() { X1 = x2, X2 = x, Y1 = y2, Y2 = y, Stroke = Brushes.LightBlue });
                                }
                            }
                            else
                            {
                                shapes.Add(new Line() { X1 = x2, X2 = x, Y1 = y2, Y2 = y, Stroke = Brushes.LightBlue });
                            }
                            break;
                        }
                    default:
                        {
                            grids.Add(getNextRect(x, y, node,path));
                            System.Windows.UIElement el = getNextRect(x, y, node,path);
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
