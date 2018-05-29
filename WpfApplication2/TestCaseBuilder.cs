using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public enum TestCaseType
    {
        E_RETURN_CHECK_INT,
        E_RETURN_CHECK_CHAR,
        E_RETURN_CHECK_BOOL,
        E_FUNC_CALL_CHECK,
        E_POINTER_CHECK,
        E_GLOBAL_CHECK
    }
    public class CheckReturn
    {
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
    public class TestCase
    {

        public string function_name { get; set; }
        public Variable function_type { get; set; }
        public int id { get; set; }
        public IEnumerable<Variable> CheckVars { get; set; }
        public IEnumerable<Variable> Arguments { get; set; }
        public IEnumerable<FuncCallNode> FuncCalls { get; set; }
        public IEnumerable<FlowGraphNode> path { get; set; }
        public List<FlowGraphNode> function_nodes { get; set; }
        public CheckReturn returnchecker { get; set; }
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
            if (node == f.Getparams().Last())
            {
                v.comma = "";
            }
            else
            {
                v.comma = ",";
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
        public List<TestCase> BuildTestCases(Function f,List<FlowGraphNode> GlobalScope)
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
                                break;
                            }
                        case NodeType.E_ASSIGMENT:
                            {
                                break;
                            }
                        case NodeType.E_RETURN:
                            {
                                ReturnNode ret = (ReturnNode)testnode;
                                NewCase.returnchecker = new CheckReturn() { ToCheck= new Variable { type = NewCase.function_type.type,value=ret.expr.ToString()} };
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
                testcases.Add(NewCase);

            }
                    return testcases;
        }
    }
}
