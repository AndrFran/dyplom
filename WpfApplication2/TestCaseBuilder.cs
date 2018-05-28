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
    public class Variable
    {
        public string type { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string isarray{ get; set; }
        public string ispointer { get; set; }
}
    public class TestCase
    {

        public string function_name { get; set; }
        public TestCaseType type { get; set; }
        public int id { get; set; }
        public IEnumerable<Variable> CheckVars { get; set; }
        public IEnumerable<Variable> Arguments { get; set; }
        public IEnumerable<Function> FuncCalls { get; set; }
        public IEnumerable<FlowGraphNode> path { get; set; }
    }

    public class TestCases
    {
        public string filename { get; set; }
        IEnumerable<TestCase> testcases { get; set; }

    }
    public class TestCaseBuilder
    {
        static int id;
        public TestCaseBuilder()
        {

        }
        public List<TestCase> BuildTestCases(Function f)
        {
            List<TestCase> testcases = new List<TestCase>();
            FlowGraphWalker graphwalker = new FlowGraphWalker();
            List<List<FlowGraphNode>> pathes = graphwalker.CalculateAllPathes(f.nodes);
            foreach (List<FlowGraphNode> path in pathes)
            {
                TestCase NewCase = new TestCase();
                List<Variable> args = new List<Variable>();
                foreach(DeclNode node in f.Getparams())
                {
                    Variable v = new Variable();
                    v.name = node.DeclName;
                    v.type = node.DeclType;
                    if(true == node.isPointer)
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
                    args.Add(v);
                }
                NewCase.Arguments = args.AsEnumerable();
                NewCase.function_name = f.name;
                if(f.returntype != null)
                {
                    NewCase.type = TestCaseType.E_RETURN_CHECK_INT;
                }
            }
                    return testcases;
        }
    }
}
