using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public enum NodeType
    {
        E_ASSIGMENT,
        E_DECL,
        E_IF,
        E_FUNC_CALL,
        E_CYCLE,
        E_COSNT,
        E_ID,
        E_ARRAY_REF,
        E_BIN_OP,
        E_UN_OP,
        E_FOR,
        E_WHILE,
        E_RETURN,
        
    }
    public interface FlowGraphNode
    {
        NodeType getNodeType();
        void setNodeType(NodeType type);
    }

    public class OperationNode : FlowGraphNode
    {
        public FlowGraphNode left { get; set; }
        public FlowGraphNode right { get; set; }
        NodeType type;
        public OperationNode()
        {
            this.type = NodeType.E_ASSIGMENT;
        }

        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }
        override public string ToString()
        {
            return this.left + "=" + this.right;
        }
    }
    public class DeclNode : FlowGraphNode
    {
        public DeclNode()
        {
            this.type = NodeType.E_DECL;
        }
        NodeType type { get; set; }
        public bool isArray { get; set; }
        public bool isPointer { get; set; }
        public bool isInited { get; set; }

        private List<String> InitValues;
        public string DeclType { get; set; }
        public string DeclName { get; set; }

        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }
        override public string ToString()
        {
            string array = "";
            string pointer = "";
            string inited="Declaration";
            if(this.isArray)
            {
                array = "[]";
            }
            if(this.isPointer)
            {
                pointer = "*";
            }
            if(this.isInited)
            {
                inited = "initialized";
            }
            return inited+"  variable  " + this.DeclName +"\n of type "+ this.DeclType+array+pointer;
        }
    }

    public class FuncCallNode : FlowGraphNode
    {
        NodeType type { get; set; }
        public string FunctionName { get; set; }
        public List<FlowGraphNode> args { get; set; }
        public FuncCallNode()
        {
            this.type = NodeType.E_FUNC_CALL;
        }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }
        override public string ToString()
        {
            StringBuilder str= new StringBuilder();
            foreach(var arg in this.args)
                {
                str.Append("\n"+arg.ToString() + "  ");
            }
            return "call  " + this.FunctionName + " with " + str.ToString();
        }
    }

    public class IfNode : FlowGraphNode
    {
        NodeType type { get; set; }
        public IfNode()
        {
            this.type = NodeType.E_IF;

        }
        public FlowGraphNode condition { get; set; }

        public List<FlowGraphNode> left { get; set; }
        public List<FlowGraphNode> right { get; set; }

        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }

        override public string ToString()
        {
            return "IF(" +condition.ToString()+ ")";
        }
    }
    public class WhileNode : FlowGraphNode
    {
        NodeType type { get; set; }
        public WhileNode()
        {
            this.type = NodeType.E_WHILE;

        }
        public FlowGraphNode condition { get; set; }

        public List<FlowGraphNode> loop { get; set; }

        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }
    }
    public class ID : FlowGraphNode
    {
        NodeType type;
        public ID()
        {
            type = NodeType.E_ID;
        }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }
        override public string ToString()
        {
            return this.Name;
        }
        public string Name { get; set; }
    }
    public class ConstantNode : FlowGraphNode
    {

        NodeType type;
        public ConstantNode()
        {
            type = NodeType.E_COSNT;
        }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }
        public string ValType { get; set; }
        public string Value { get; set; }

        override public string ToString()
        {
            return this.Value.ToString();
        }
    }
    public class ArrayRef: FlowGraphNode
    {
        NodeType type;
        public ArrayRef()
        {
            this.type = NodeType.E_ARRAY_REF;
        }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }

        public string Name { get; set; }
        public FlowGraphNode index { get; set; }

        override public string ToString()
        {
            return this.Name + "["+index.ToString()+"]";
        }
    }
    public class BinaryOp : FlowGraphNode
    {
        NodeType type;
        public BinaryOp()
        {
            this.type = NodeType.E_BIN_OP;
        }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }
        override public string ToString()
        {
            return this.left.ToString() +this.OP+this.right.ToString();
        }
        public string OP { get; set; }
        public FlowGraphNode left;
        public FlowGraphNode right;
    }
    public class UnaryOp : FlowGraphNode
    {
        NodeType type;
        public UnaryOp()
        {
            type = NodeType.E_UN_OP;
        }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }

        public string OP { get; set; }
        public FlowGraphNode left;
        override public string ToString()
        {
            return this.OP + this.left.ToString();
        }

    }


    public class ReturnNode : FlowGraphNode
    {
        NodeType type;
        public ReturnNode()
        {
            type = NodeType.E_RETURN;
        }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }

        public FlowGraphNode expr;

        override public string ToString()
        {
            return "return " + this.expr.ToString();
        }

    }
}
