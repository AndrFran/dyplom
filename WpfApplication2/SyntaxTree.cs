using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfApplication2
{

    class SyntaxTree
    {
        static int Id;
        List<Dictionary<string, object>> globals;
        List<Dictionary<string,object>> functions;
        List<Function> ParsedFunctions;
        List<FlowGraphNode> graph = null;
        public SyntaxTree()
        {
            Id = 0;
            globals = new List<Dictionary<string, object>>();
            functions = new List<Dictionary<string, object>>();
            ParsedFunctions = new List<Function>();
        }

        public List<string> Create(Dictionary<string, object> dic)
        {
            List<string> names = new List<string>();
            ArrayList val=(ArrayList) dic["ext"];
            foreach(Dictionary<string, object> value in val)
            {
                if("FuncDef"== value["_nodetype"].ToString())
                {
                    names.Add(((Dictionary<string, object>)(value["decl"]))["name"].ToString());
                    functions.Add(value);
                }
                else
                {
                    globals.Add(value);
                }
            }
            return names;
        }
        public List<FlowGraphNode> CreateFlowControlGraph(int id)
        {
            Function func = new Function();

            func.name = ((Dictionary<string, object>)(this.functions[id]["decl"]))["name"].ToString();


            Dictionary<string, object> type = (Dictionary<string, object>)((Dictionary<string, object>)(this.functions[id]["decl"]))["type"];
            func.returntype = ((ArrayList)((Dictionary<string, object>)((Dictionary<string, object>)type["type"])["type"])["names"])[0].ToString();

            ArrayList functionflow = (ArrayList)((Dictionary<string, object>)(this.functions[id]["body"]))["block_items"];
            foreach (Dictionary<string, object> item in functionflow)
            {
                addNewNode(ParseNode(item));
            }
            Dictionary<string, object> tmp = (((Dictionary<string, object>)(((Dictionary<string, object>)(((Dictionary<string, object>)(this.functions[id]["decl"]))["type"]))["args"])));
            if (tmp != null)
            { 
            ArrayList prameters = (ArrayList)tmp["params"];
                foreach (Dictionary<string, object> paremeter in prameters)
                {
                    func.AddParam((DeclNode)ParseDeclaration(paremeter));
                }
            }
            
            ParsedFunctions.Add(func);
            return graph;
        }
        FlowGraphNode ParseNode(Dictionary<string, object> item)
        {
            string NodeType = item["_nodetype"].ToString();
            FlowGraphNode node = null;
            if ("Decl" == NodeType)
            {
                node = ParseDeclaration(item);
            }
            if ("FuncCall" == NodeType)
            {
                node = ParseFuncCall(item);
            }
            if("If" == NodeType)
            {
                node = ParseIf(item);
            }
            if ("Constant" == NodeType)
            {
                node = ParseConstant(item);
            }
            if("ArrayRef" == NodeType)
            {
                node = ParseArrayRef(item);
            }
            if("ID" == NodeType)
            {
                node = ParseID(item);
            }
            if("BinaryOp" == NodeType)
            {
                node = ParseBinOp(item);
            }
            if("UnaryOp" == NodeType)
            {
                node = ParseUnaryOp(item);
            }
            if("Assignment" == NodeType)
            {
                node = ParseAssigmet(item);
            }
            if("Return" == NodeType)
            {
                node = ParseReturn(item);
            }
            if("While" == NodeType)
            {
                node = ParseWhile(item);
            }
            return node;
        }

        FlowGraphNode ParseDeclaration(Dictionary<string, object> item)
        {
            DeclNode node = new DeclNode(Id++);
            Dictionary<string, object> type = null;
            node.DeclName = item["name"].ToString();
            if(null == item["init"])
            {
                node.isInited = false;
            }
            else
            {
                node.isInited = true;
            }
            do
            {
                if (null == type)
                {
                    type = (Dictionary<string, object>)item["type"];
                }
                else
                {
                    type = (Dictionary<string, object>)type["type"];
                }
                string var = type["_nodetype"].ToString();
                if("ArrayDecl" == var)
                {
                    node.isArray = true;
                }
                if("PtrDecl" == var)
                {
                    node.isPointer = true;
                }
                if("TypeDecl" == var)
                {

                }
                if("IdentifierType" == var)
                {
                    node.DeclType = ((ArrayList)(type["names"]))[0].ToString();
                }
            } while (type.ContainsKey("type"));

            return node;
        }
        FlowGraphNode ParseFuncCall(Dictionary<string, object> item)
        {
            FuncCallNode node = new FuncCallNode(Id++);
            node.FunctionName = ((Dictionary<string, object>)(item["name"]))["name"].ToString();
            ArrayList arguments = (ArrayList)((Dictionary<string, object>)item["args"])["exprs"];
            List<FlowGraphNode> args = new List<FlowGraphNode>();
            foreach ( Dictionary < string, object> argument in arguments)
            {
                args.Add(ParseNode(argument));
            }
            node.args = args;
            return node;

        }
        FlowGraphNode ParseConstant(Dictionary<string, object> item)
        {
            ConstantNode node = new ConstantNode(Id++);
            node.ValType = item["type"].ToString();
            node.Value = item["value"].ToString();
            return node;
        }
        FlowGraphNode ParseID(Dictionary<string, object> item)
        {
            ID node = new ID(Id++);
            node.Name = item["name"].ToString();
            return node;
        }
        FlowGraphNode ParseArrayRef(Dictionary<string, object> item)
        {
            ArrayRef node = new ArrayRef(Id++);
            node.Name = ((Dictionary<string,object>)item["name"])["name"].ToString();
            node.index = ParseNode(((Dictionary<string, object>)item["subscript"]));
            return node;
        }
        FlowGraphNode ParseBinOp(Dictionary<string, object> item)
        {
            BinaryOp node = new BinaryOp(Id++);
            node.OP = item["op"].ToString();
            node.left = ParseNode(((Dictionary<string, object>)item["left"]));
            node.right = ParseNode(((Dictionary<string, object>)item["right"]));
            return node;
        }
        FlowGraphNode ParseAssigmet(Dictionary<string, object> item)
        {
            OperationNode node = new OperationNode(Id++);
            node.left = ParseNode(((Dictionary<string, object>)item["lvalue"]));
            node.right = ParseNode(((Dictionary<string, object>)item["rvalue"]));
            return node;
        }
        FlowGraphNode ParseUnaryOp(Dictionary<string, object> item)
        {
            UnaryOp node = new UnaryOp(Id++);
            node.OP = item["op"].ToString();
            node.left = ParseNode(((Dictionary<string, object>)item["expr"]));
            return node;
        }
        FlowGraphNode ParseReturn(Dictionary<string, object> item)
        {
            ReturnNode node = new ReturnNode(Id++);
            node.expr = ParseNode(((Dictionary<string, object>)item["expr"]));
            return node;
        }
        FlowGraphNode ParseIf(Dictionary<string, object> item)
        {
            IfNode node = new IfNode(Id++);
            List<FlowGraphNode> left = new List<FlowGraphNode>();
            List<FlowGraphNode> right = new List<FlowGraphNode>();
            Dictionary<string, object> leftarr = (Dictionary<string, object>)item["iffalse"];
            Dictionary<string, object> rightarr = (Dictionary<string, object>)item["iftrue"];
            if (leftarr != null)
                {
                if ("Compound" == leftarr["_nodetype"].ToString())
                {
                    ArrayList functionflow = (ArrayList)leftarr["block_items"];
                    if (functionflow != null)
                    {
                        foreach (Dictionary<string, object> argument in functionflow)
                        {
                            left.Add(ParseNode(argument));
                        }
                    }
                }
                else
                {
                    left.Add(ParseNode(leftarr));
                }
            }
            if (rightarr != null)
            {
                if ("Compound" == rightarr["_nodetype"].ToString())
                {
                    ArrayList functionflow = (ArrayList)rightarr["block_items"];
                    if (functionflow != null)
                    {
                        foreach (Dictionary<string, object> argument in functionflow)
                        {
                            right.Add(ParseNode(argument));
                        }
                    }
                }
                else
                {
                    right.Add(ParseNode(rightarr));
                }
            }
            node.left = left;
            node.right = right;
            node.condition = ParseNode((Dictionary<string, object>)item["cond"]);
            return node;
        }
        void addNewNode(FlowGraphNode node)
        {
            if(null == graph)
            {
                graph = new List<FlowGraphNode>();
                graph.Add(node);
            }
            else
            {
                graph.Add(node);
            }
        }
        FlowGraphNode ParseWhile(Dictionary<string, object> item)
        {
            WhileNode node = new WhileNode(Id++);
            List<FlowGraphNode> stmt = new List<FlowGraphNode>();
            Dictionary<string, object> leftarr = (Dictionary<string, object>)item["stmt"];
            if (leftarr != null)
            {
                if ("Compound" == leftarr["_nodetype"].ToString())
                {
                    ArrayList functionflow = (ArrayList)leftarr["block_items"];
                    if (functionflow != null)
                    {
                        foreach (Dictionary<string, object> argument in functionflow)
                        {
                            stmt.Add(ParseNode(argument));
                        }
                    }
                }
                else
                {
                    stmt.Add(ParseNode(leftarr));
                }
            }
           
            node.loop = stmt;
            node.condition = ParseNode((Dictionary<string, object>)item["cond"]);
            return node;
        }
    }
    public class Variable
    {
        public string type { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }
    public class Function
    {
        List<DeclNode> parametrs;
        List<Variable> insidevars;
        public string returntype { get; set; }
        public List<FlowGraphNode> nodes { get; set; }
        public string name { get; set; }
        public void AddParam(DeclNode var)
        {
            parametrs.Add(var);
        }
        public void AddInternalParam(Variable var)
        {
            insidevars.Add(var);
        }
        public Function()
        {
            parametrs = new List<DeclNode>();
            insidevars = new List<Variable>();
            nodes = null;
        }
    }
   
    public class Operation
    {
        void Oparation()
        {

        }
        public bool Evaluate()
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("", typeof(Boolean));
            table.Columns[0].Expression = "true and false or true";

            System.Data.DataRow r = table.NewRow();
            table.Rows.Add(r);
            Boolean result = (Boolean)r[0];
            return result;
        }
    }
}
