using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public class CheckAssigment
    {

    }
    public class CheckReturn
    {
        public Variable Cheker { get; set; }
        public Variable ToCheck { get; set; }
        public bool intcheck { get; set; }
        public bool pointercheck { get; set; }
        public bool boolchecl { get; set; }
        public bool charcheck { get; set; }
        public bool memcheck { get; set; }
    }
    public class Variable
    {
        public string type { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string isarray{ get; set; }
        public string ispointer { get; set; }
        public string comma { get; set; }
    }
    public class FuncCallChecker
    {
        public string Name { get; set; }
        public List<Variable> args;
    }
    public class TestCase
    {
        public string result { get; set; }
        public string function_name { get; set; }
        public Variable function_type { get; set; }
        public int id { get; set; }
        public IEnumerable<Variable> CheckVars { get; set; }
        public IEnumerable<Variable> Arguments { get; set; }
        public IEnumerable<FuncCallChecker> FuncCalls { get; set; }
        public IEnumerable<FlowGraphNode> path { get; set; }
        public List<FlowGraphNode> function_nodes { get; set; }
        public CheckReturn returnchecker { get; set; }
        public IEnumerable<CheckReturn> AssigmentChecker { get; set; }
    }

    public class TestCases
    {
        public string filename { get; set; }
        public IEnumerable<TestCase> testcases { get; set; }

    }
    public class TestCaseBuilder
    {
        static int id;
        public TestCaseBuilder()
        {
            id = 0;
        }
        string GetTypeOfTypeDef(String type,List<FlowGraphNode> GlobalScope)
        {
            string result = null;
            foreach(FlowGraphNode glob in GlobalScope)
            {
                if(glob.getNodeType() == NodeType.E_TYPEDEF)
                {
                    TypeDef node = (TypeDef)glob;
                    if (node.name == type)
                    {
                        TypeDecl decl = (TypeDecl)node.TypeDecl;
                        result = decl.Type.getNodeType().ToString();
                    }
                }
            }
            return result;
        }
        Variable DeclNodeToVar(DeclNode node,Function f)
        {
            Variable v = new Variable();
            v.name = node.DeclName;
            v.type = node.DeclType;
            v.comma = "";
            if (f != null)
            {
                if (f.Getparams().Count > 0)
                {
                    if (node == f.Getparams().Last())
                    {
                        v.comma = "";
                    }
                    else
                    {
                        v.comma = ",";
                    }
                }
            }
            if (true == node.isPointer)
            {
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < node.PointerLevel; i++)
                {
                    str.Append("*");

                }
                v.ispointer = str.ToString();
            }
            else
            {
                v.ispointer = "";
            }
            if (true == node.isArray)
            {
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < node.ArrayLevel; i++)
                {
                    str.Append("[]");

                }
                v.isarray = str.ToString();
            }
            else
            {
                v.isarray = "";
            }
            v.value = "{0}";
            //calc var value
            return v;

        }
        private FlowGraphNode resolveTypedef(List<FlowGraphNode> Scope,string typename)
        {
            foreach(FlowGraphNode node in Scope)
            {
                if(node.getNodeType() == NodeType.E_TYPEDEF)
                {
                    TypeDef tmp = (TypeDef)node;
                    if(tmp.name == typename)
                    {
                        return ((TypeDecl)tmp.TypeDecl).Type;
                    }
                }
            }
            return null;
        }
        private FlowGraphNode getStructByType(List<FlowGraphNode> Scope, string typename)
        {
            foreach (FlowGraphNode node in Scope)
            {
                if (node.getNodeType() == NodeType.E_STRUCT)
                {
                    StructNode tmp = (StructNode)node;
                    //if (tmp. == typename)
                   // {
                     //   return tmp.TypeDecl;
                    //}
                }
            }
            return null;
        }
        private DeclNode resovleTypeID(List<FlowGraphNode> Scope, string name)
        {
            foreach (FlowGraphNode g in Scope)
            {

                if (g.getNodeType() == NodeType.E_DECL)
                {
                    DeclNode tmp = (DeclNode)g;
                    if (tmp.DeclName == name)
                    {
                        return tmp;
                    }
                }
            }
            return null;
        }
        private DeclNode resovleType(List<FlowGraphNode> Scope, string name)
        {
            foreach (FlowGraphNode g in Scope)
            {

                if (g.getNodeType() == NodeType.E_DECL)
                {
                    DeclNode tmp = (DeclNode)g;
                    if(tmp.DeclName == name)
                    {
                        return tmp;
                    }
                }
                if(g.getNodeType() == NodeType.E_STRUCT)
                {
                    StructNode tmp = (StructNode)g;
                    foreach(FlowGraphNode node in tmp.Decl)
                    {
                        if(null!= resovleType(new List<FlowGraphNode>() { node},name))
                        {
                            return resovleType(new List<FlowGraphNode>() { node }, name);
                        }
                    }
                }
                if(g.getNodeType() == NodeType.E_UNION)
                {
                    UnionNode tmp = (UnionNode)g;
                    foreach (FlowGraphNode node in tmp.Decl)
                    {
                        if (null != resovleType(new List<FlowGraphNode>() { node }, name))
                        {
                            return resovleType(new List<FlowGraphNode>() { node }, name);
                        }
                    }
                }
            }
                    return null;
        }
        public List<TestCase> BuildTestCases(Function f,List<FlowGraphNode> GlobalScope,List<Function> functions)
        {
            List<TestCase> testcases = new List<TestCase>();
            FlowGraphWalker graphwalker = new FlowGraphWalker();
            List<List<FlowGraphNode>> pathes = graphwalker.CalculateAllPathes(f.nodes);
            foreach (List<FlowGraphNode> path in pathes)
            {
                TestCase NewCase = new TestCase();
                NewCase.id = TestCaseBuilder.id++;
                List<Variable> args = new List<Variable>();
                foreach(DeclNode node in f.Getparams())
                {
                    args.Add(DeclNodeToVar(node, f));
                }

                NewCase.Arguments = args.AsEnumerable();
                NewCase.function_name = f.name;
                NewCase.function_type = DeclNodeToVar(f.returntype, f);
                NewCase.function_nodes = f.nodes;
                NewCase.path = path;
                List<CheckReturn> AssigmentChecks = new List<CheckReturn>();
                List<FuncCallChecker> FuncCalls = new List<FuncCallChecker>(); 
                foreach (FlowGraphNode testnode in path)
                {
                    List<DeclNode> insideVars = new List<DeclNode>();
                    switch (testnode.getNodeType())
                    {
                        case NodeType.E_DECL:
                            {
                                DeclNode insidevar = (DeclNode)testnode;
                                insideVars.Add(insidevar);
                                break;
                            }
                        case NodeType.E_IF:
                            {
                                break;
                            }
                        case NodeType.E_WHILE:
                            {
                                break;
                            }
                        case NodeType.E_FOR:
                            {
                                break;
                            }
                        case NodeType.E_FUNC_CALL:
                            {
                                FuncCallNode node = (FuncCallNode)testnode;
                                Boolean IsLocal = false;
                                foreach(Function f1 in functions)
                                {
                                    if(f1.name == node.FunctionName)
                                    {
                                        IsLocal = true;
                                    }
                                }
                                if(!IsLocal)
                                {
                                    FuncCallChecker checker = new FuncCallChecker();
                                    checker.Name = node.FunctionName;
                                    FuncCalls.Add(checker);
                                }
                                break;
                            }
                        case NodeType.E_ASSIGMENT:
                            {
                                OperationNode node = (OperationNode)testnode;
                                bool InGlobalScope = false;
                                CheckReturn checker = new CheckReturn();
                                Variable toCheck = new Variable();
                                Variable CheckValue = new Variable();
                                foreach (FlowGraphNode g in GlobalScope)
                                {
                                    if (g.getNodeType() == NodeType.E_DECL)
                                    {
                                        if (node.left.getNodeType() == NodeType.E_STRUCTREF)
                                        {
                                            StructRef tmp = (StructRef)node.left;
                                            while (tmp.structname.getNodeType() != NodeType.E_ID)
                                            {
                                                if (tmp.structname.getNodeType() == NodeType.E_UN_OP)
                                                {
                                                    UnaryOp pp = (UnaryOp)tmp.structname;
                                                    while(pp.left.getNodeType() == NodeType.E_UN_OP)
                                                    {
                                                        pp = (UnaryOp)pp.left;
                                                    }
                                                    if(pp.left.getNodeType() == NodeType.E_STRUCTREF)
                                                    {
                                                        tmp = (StructRef)pp.left;
                                                    }
                                                    if(pp.left.getNodeType() == NodeType.E_ID)
                                                    {
                                                        tmp.structname = pp.left;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    tmp = (StructRef)tmp.structname;
                                                }
                                            }
                                            ID name = (ID)tmp.structname;
                                            if (name.Name == ((DeclNode)g).DeclName)
                                            {

                                                DeclNode r = resovleType( new List<FlowGraphNode>(){ resolveTypedef(GlobalScope, ((DeclNode)g).DeclType) }, ((StructRef)(node.left)).structfield.ToString());
                                                CheckValue = DeclNodeToVar(r, null);
                                                CheckValue.name = r.DeclName + "Checker";
                                                CheckValue.type = r.DeclType;
                                                InGlobalScope = true;
                                            }
                                        }
                                        if (node.left.getNodeType() == NodeType.E_ID)
                                        {
                                            ID tmp = (ID)node.left;
                                            if (tmp.Name == ((DeclNode)g).DeclName)
                                            {

                                                DeclNode r = resovleTypeID(GlobalScope, tmp.Name);
                                                CheckValue = DeclNodeToVar(r, null);
                                                CheckValue.name = r.DeclName + "Checker";
                                                CheckValue.type = r.DeclType;
                                                InGlobalScope = true;
                                            }
                                        }
                                        if (node.left.getNodeType() == NodeType.E_ARRAY_REF)
                                        {

                                        }


                                    }
                                }
                                if (InGlobalScope)
                                {
                                    if (node.left.getNodeType() == NodeType.E_STRUCTREF)
                                    {
                                        StructRef Sref = (StructRef)node.left;
                                        toCheck.name = Sref.structname.ToString() + Sref.reftype.ToString() + Sref.structfield;
                                        if (node.right.getNodeType() == NodeType.E_COSNT)
                                        {
                                            ConstantNode tmp = (ConstantNode)node.right;
                                            toCheck.value = tmp.Value;
                                        }
                                        checker.memcheck = true;
                                        checker.Cheker = CheckValue;
                                        checker.ToCheck = toCheck;
                                        AssigmentChecks.Add(checker);
                                    }
                                    if (node.left.getNodeType() == NodeType.E_ID)
                                    {
                                        ID Identifier = (ID)node.left;
                                        toCheck.name = Identifier.Name;
                                        if (node.right.getNodeType() == NodeType.E_COSNT)
                                        {
                                            ConstantNode tmp = (ConstantNode)node.right;
                                            toCheck.value = tmp.Value;
                                        }
                                        checker.Cheker = CheckValue;
                                        checker.ToCheck = toCheck;
                                        AssigmentChecks.Add(checker);
                                        checker.memcheck = true;
                                    }
                                    if (node.left.getNodeType() == NodeType.E_ARRAY_REF)
                                    {

                                    }
                                    if(node.right.getNodeType() == NodeType.E_COSNT)
                                    {
                                        checker.Cheker.value = node.right.ToString();
                                    }
                                }
                                    
                                
                                break;
                            }
                        case NodeType.E_RETURN:
                            {
                                ReturnNode ret = (ReturnNode)testnode;
                                if(NewCase.function_type.type =="void")
                                {
                                    break;
                                }
                                NewCase.returnchecker = new CheckReturn() { ToCheck= new Variable { type = NewCase.function_type.type,value="{0}"} };
                                //if(ret.expr.getNodeType() == NodeType.E_COSNT)
                                {
                                    NewCase.returnchecker.ToCheck.value = ret.expr.ToString();
                                }
                                if (NewCase.function_type.ispointer != "" ||
                                     NewCase.function_type.isarray != "")
                                {
                                    NewCase.returnchecker.memcheck = true;
                                }
                                
                                    if (NewCase.function_type.type.Contains("int"))
                                {
                                    NewCase.returnchecker.ToCheck.type = "int";
                                    NewCase.returnchecker.intcheck = true;

                                }
                                if (NewCase.function_type.type.Contains("char"))
                                {
                                    NewCase.returnchecker.ToCheck.type = "char";
                                    NewCase.returnchecker.charcheck = true;

                                }
                                else
                                {
                                    string type = GetTypeOfTypeDef(f.returntype.DeclType,GlobalScope);
                                        if(type == "E_ENUM")
                                        {
                                            NewCase.returnchecker.ToCheck.type = f.returntype.DeclType;
                                            NewCase.returnchecker.intcheck = true;
                                        }
                                }
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }

                }
                NewCase.FuncCalls = FuncCalls;
                NewCase.AssigmentChecker = AssigmentChecks;
                testcases.Add(NewCase);

            }
                    return testcases;
        }
    }
}
