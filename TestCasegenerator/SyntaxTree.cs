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
        public List<Function> ParsedFunctions;
        public List<FlowGraphNode> GlobalScope { get; set; }
        List<FlowGraphNode> graph = null;
        public SyntaxTree()
        {
            Id = 0;
            globals = new List<Dictionary<string, object>>();
            functions = new List<Dictionary<string, object>>();
            ParsedFunctions = new List<Function>();
            GlobalScope = new List<FlowGraphNode>();
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
            foreach(Dictionary<string,object> glob in globals)
            {
                GlobalScope.Add(ParseNode(glob));
            }
            return names;
        }
        public List<Function> GetFunctionByNames(List<string> names)
        {
            List<Function> functions = new List<Function>();
            foreach (Function func in this.ParsedFunctions)
                {
                if (names.Contains(func.name))
                {
                    functions.Add(func);
                }
            }
            return functions;
        }
        public void ParseFunctions()
        {
            for(int i =0;i<this.functions.Count;i++)
            {
                CreateFlowControlGraph(i);
            } 
        }
        public List<FlowGraphNode> CreateFlowControlGraph(int id)
        {
            if (graph != null)
            { 
            graph.Clear();
        }
            Function func = new Function();

            func.name = ((Dictionary<string, object>)(this.functions[id]["decl"]))["name"].ToString();


            Dictionary<string, object> type = ((Dictionary<string, object>)(this.functions[id]["decl"]));
           
            func.returntype = (DeclNode)ParseDeclaration(type);
            ArrayList functionflow = (ArrayList)((Dictionary<string, object>)(this.functions[id]["body"]))["block_items"];
            foreach (Dictionary<string, object> item in functionflow)
            {
                func.nodes.Add(ParseNode(item));
            }
            if(NodeType.E_RETURN != func.nodes.Last().getNodeType())
            {
                ReturnNode node = new ReturnNode(Id++);
                func.nodes.Add(node);
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
            return func.nodes;
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
            if ("For" == NodeType)
            {
                node = ParseFor(item);
            }
            if ("Typedef" == NodeType)
            {
                node = ParseTypeDef(item);
            }
            if ("TypeDecl" == NodeType)
            {
                node = ParseTypeDecl(item);
            }
            if ("Enum" == NodeType)
            {
                node = ParseEnum(item);
            }
            if ("Enumerator" == NodeType)
            {
                node = ParseEnumValue(item);
            }
            if("Struct" == NodeType)
            {
                node = ParseStruct(item);
            }
            if ("Union" == NodeType)
            {
                node = ParseUnion(item);
            }
            if ("StructRef" == NodeType)
            {
                node = ParseStructRef(item);
            }
            if("Typename" == NodeType)
            {
                node = ParseTypeName(item);
            }
            if("IdentifierType" == NodeType)
            {
                node = ParseTypeDeclIdentifierType(item);
            }
            if("Cast" == NodeType)
            {
                node = ParseCast(item);
            }
            return node;
        }
        FlowGraphNode ParseTypeDeclIdentifierType(Dictionary<string, object> item)
        {
            TypeDeclIdentifierType node = new TypeDeclIdentifierType(Id++);
            StringBuilder str = new StringBuilder();

            ArrayList type1 = ((ArrayList)(item["names"]));
            for (int i = 0; i < type1.Count; i++)
            {
                if (i == type1.Count - 1)
                {
                    str.Append(type1[i].ToString());
                }
                else
                {
                    str.Append(type1[i].ToString() + " ");
                }
            }
            node.name = str.ToString();
            return node;
        }
        FlowGraphNode ParseDeclaration(Dictionary<string, object> item)
        {
            DeclNode node = new DeclNode(Id++);
            Dictionary<string, object> type = null;
            if (item.ContainsKey("name"))
            {
                if(item["name"]!=null)
                node.DeclName = item["name"].ToString();
            }
            if (item.ContainsKey("init"))
            {
                if (null == item["init"])
                {
                    node.isInited = false;
                }
                else
                {
                    node.isInited = true;
                }
            }
            do
            {
                string var;
                if (item.ContainsKey("type"))
                {
                    if (null == type)
                    {
                        type = (Dictionary<string, object>)item["type"];
                    }
                    else
                    {
                        type = (Dictionary<string, object>)type["type"];
                    }
                    var = type["_nodetype"].ToString();

                }
                else
                {
                    type = item;
                    var =item["_nodetype"].ToString();
                }
                if("Union" == var)
                {
                    if(node.isArray || node.isPointer)
                    {
                        UnionNode r = (UnionNode)ParseNode(type);
                        node.DeclType = r.name;
                    }
                    else
                    {
                        return ParseNode(type);
                    }

                }
                if("Struct"==var)
                {
                    if (node.isArray || node.isPointer)
                    {
                        StructNode r = (StructNode)ParseNode(type);
                        node.DeclType = r.name;
                    }
                    else
                    {
                        return ParseNode(type);
                    }
                }
                    if ("ArrayDecl" == var)
                {
                    node.ArrayLevel++;
                    node.isArray = true;
                }
                if("PtrDecl" == var)
                {
                    node.PointerLevel++;
                    node.isPointer = true;
                }
                if("TypeDecl" == var)
                {

                }
                if("IdentifierType" == var)
                {
                    StringBuilder str = new StringBuilder();

                    ArrayList type1 = ((ArrayList)(type["names"]));
                    for (int i = 0; i < type1.Count; i++)
                    {
                        if (i == type1.Count - 1)
                        {
                            str.Append(type1[i].ToString());
                        }
                        else
                        {
                            str.Append(type1[i].ToString() + " ");
                        }
                    }
                    node.DeclType = str.ToString();
                }
            } while (type.ContainsKey("type"));

            return node;
        }
        FlowGraphNode ParseFuncCall(Dictionary<string, object> item)
        {
            FuncCallNode node = new FuncCallNode(Id++);
            node.FunctionName = ((Dictionary<string, object>)(item["name"]))["name"].ToString();
            if (item["args"] != null)
            {
                ArrayList arguments = (ArrayList)((Dictionary<string, object>)item["args"])["exprs"];
                List<FlowGraphNode> args = new List<FlowGraphNode>();
                foreach (Dictionary<string, object> argument in arguments)
                {
                    args.Add(ParseNode(argument));
                }
                node.args = args;
            }
            else
            {
                node.args = new List<FlowGraphNode>();
            }
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
            node.Name = ParseNode((Dictionary<string,object>)item["name"]);
            node.index = ParseNode(((Dictionary<string, object>)item["subscript"]));
            return node;
        }
        FlowGraphNode ParseStructRef(Dictionary<string, object> item)
        {
            StructRef node = new StructRef(Id++);
            node.reftype = item["type"].ToString();
            node.structfield = ParseNode(((Dictionary<string, object>)item["field"]));
            node.structname = ParseNode(((Dictionary<string, object>)item["name"]));
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
            Dictionary<string, object> tmp = (Dictionary<string, object>)(item["expr"]);
                if (tmp.ContainsKey("expr"))
                {
                node.left = ParseNode(((Dictionary<string, object>)tmp["expr"]));
            }
            else
            {
                node.left = ParseNode(((Dictionary<string, object>)item["expr"]));
            }
            return node;
        }
        FlowGraphNode ParseCast(Dictionary<string, object> item)
        {
            Cast node = new Cast(Id++);
            Dictionary<string, object> tmp = (Dictionary<string, object>)(item["expr"]);
            if (tmp.ContainsKey("expr"))
            {
                node.expr = ParseNode(((Dictionary<string, object>)tmp["expr"]));
            }
            else
            {
                node.expr = ParseNode(((Dictionary<string, object>)item["expr"]));
            }
            node.typetocast = ParseNode(((Dictionary<string, object>)item["to_type"]));
            return node;
        }
        FlowGraphNode ParseTypeDef(Dictionary<string, object> item)
        {
            TypeDef node = new TypeDef(Id++);
            node.name = item["name"].ToString();
            node.TypeDecl = ParseNode(((Dictionary<string, object>)item["type"]));
            return node;
        }
        FlowGraphNode ParseTypeDecl(Dictionary<string, object> item)
        {
            TypeDecl node = new TypeDecl(Id++);
            if (item.ContainsKey("name"))
            {
                node.name = item["name"].ToString();
            }
                node.Type = ParseNode(((Dictionary<string, object>)item["type"]));
            return node;
        }
        FlowGraphNode ParseTypeName(Dictionary<string, object> item)
        {
            TypeName node = new TypeName(Id++);
            node.Type = ParseNode(((Dictionary<string, object>)item["type"]));
            return node;
        }
        FlowGraphNode ParseStruct(Dictionary<string, object> item)
        {
            StructNode node = new StructNode(Id++);
            node.Decl = new List<FlowGraphNode>();
            ArrayList decls = (ArrayList)item["decls"];
            if(decls!=null)
            foreach (Dictionary<string, object> decl in decls)
            {
                node.Decl.Add(ParseNode(decl));
            }
            if (item.ContainsKey("name"))
            {
                if(item["name"]!= null)
                node.name = item["name"].ToString();
            }
            return node;
        }
        FlowGraphNode ParseUnion(Dictionary<string, object> item)
        {
            UnionNode node = new UnionNode(Id++);
            node.Decl = new List<FlowGraphNode>();
            ArrayList decls = (ArrayList)item["decls"];
            if(decls!=null)
            foreach (Dictionary<string, object> decl in decls)
            {
                node.Decl.Add(ParseNode(decl));
            }
            if (item.ContainsKey("name"))
            {
                if (item["name"] != null)
                    node.name = item["name"].ToString();

            }
            return node;
        }
        FlowGraphNode ParseEnum(Dictionary<string, object> item)
        {
            EnumNode node = new EnumNode(Id++);
            node.name = item["name"].ToString();
            node.Values = new List<FlowGraphNode>();
            ArrayList values = (ArrayList)((Dictionary<string,object>)item["values"])["enumerators"];
            foreach (Dictionary<string, object> val in values)
            {
                node.Values.Add(ParseNode(val));
            }
            return node;
        }
        FlowGraphNode ParseEnumValue(Dictionary<string, object> item)
        {
            EnumNode node = new EnumNode(Id++);
            node.name = item["name"].ToString();
            return node;
        }
        FlowGraphNode ParseReturn(Dictionary<string, object> item)
        {
            ReturnNode node = new ReturnNode(Id++);
            if(item["expr"]!=null)
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

    FlowGraphNode ParseFor(Dictionary<string, object> item)
    {
        ForNode node = new ForNode(Id++);
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
            if(item["cond"]!=null)
        node.condition = ParseNode((Dictionary<string, object>)item["cond"]);
            if (item["init"] != null)
                node.init = ParseNode((Dictionary<string, object>)item["init"]);
            if (item["next"] != null)
                node.next = ParseNode((Dictionary<string, object>)item["next"]);
            return node;
    }
    }
    public class Function
    {
        List<DeclNode> parametrs;
        public DeclNode returntype { get; set; }
        public List<FlowGraphNode> nodes { get; set; }
        public string name { get; set; }
        public void AddParam(DeclNode var)
        {
            parametrs.Add(var);
        }
        public List<DeclNode> Getparams()
        {
            return this.parametrs;
        }
        public Function()
        {
            parametrs = new List<DeclNode>();
            nodes = new List<FlowGraphNode>();
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
