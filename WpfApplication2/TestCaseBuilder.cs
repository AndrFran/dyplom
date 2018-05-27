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
    public class TestCase
    {
        string name { get; set; }
        List<int> path;
        public TestCaseType type { get; set; }
        int id { get; set; }
        IEnumerable<Variable> checkvars;
        IEnumerable<Variable> SetUpVariables;
        IEnumerable<Function> FuncCalls;
        IEnumerable<FlowGraphNode> operations { get; set; }
    }
    public class TestCaseBuilder
    {
        public TestCaseBuilder()
        {

        }
        public List<TestCase> BuildTestCases(List<FlowGraphNode> nodes)
        {
            List<TestCase> testcases = new List<TestCase>();
            FlowGraphWalker graphwalker = new FlowGraphWalker();
            List<List<int>> pathes = graphwalker.CalculateAllPathes(nodes);

            return testcases;
        }
    }
}
