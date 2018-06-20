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
        E_TYPEDEF,
        E_TYPEDECL,
        E_STRUCT,
        E_ENUM,
        E_ENUMVALUE,
        E_STRUCTREF,
        E_TYPENAME,
        E_TYPEDECL_ID,
        E_CAST,
        E_UNION,
    }
    public interface FlowGraphNode
    {
        NodeType getNodeType();
        void setNodeType(NodeType type);
        int getId();
        
    }

    public class OperationNode : FlowGraphNode
    {
        int id;
        public FlowGraphNode left { get; set; }
        public FlowGraphNode right { get; set; }
        NodeType type;
        public OperationNode(int id)
        {
            this.type = NodeType.E_ASSIGMENT;
            this.id = id;
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

        public int getId()
        {
            return this.id;
        }

    }
    public class DeclNode : FlowGraphNode
    {
        int id;
        public DeclNode(int id)
        {
            this.id = id;
            this.type = NodeType.E_DECL;
            this.ArrayLevel = 0;
            this.PointerLevel = 0;
        }
        NodeType type { get; set; }
        public int ArrayLevel { get; set; }
        public int PointerLevel { get; set; }
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
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < this.ArrayLevel; i++)
                {
                    str.Append("[]");
                   
                }
                array = str.ToString();
            }
            if(this.isPointer)
            {
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < this.PointerLevel; i++)
                {
                    str.Append("*");

                }
                pointer = str.ToString();
            }
            if(this.isInited)
            {
                inited = "initialized";
            }
            return inited+"  variable  " + this.DeclName +"\n of type "+ this.DeclType+array+pointer;
        }
        public int getId()
        {
            return this.id;
        }
    }

    public class FuncCallNode : FlowGraphNode
    {
        int id;
        NodeType type { get; set; }
        public string FunctionName { get; set; }
        public List<FlowGraphNode> args { get; set; }
        public FuncCallNode(int id)
        {
            this.id = id;
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
                if (args.Last() == arg)
                {
                    str.Append(arg.ToString());
                }
                else
                {
                    str.Append(arg.ToString() + " , ");
                }
            }
            return " " + this.FunctionName + "(" + str.ToString()+")";
        }
        public int getId()
        {
            return this.id;
        }
    }

    public class IfNode : FlowGraphNode
    {
        int id;
        NodeType type { get; set; }
        public IfNode(int id )
        {
            this.id = id;
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
        public int getId()
        {
            return this.id;
        }
    }
    public class WhileNode : FlowGraphNode
    {
        int id;
        NodeType type { get; set; }
        public WhileNode(int id)
        {
            this.type = NodeType.E_WHILE;
            this.id = id;
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
        public int getId()
        {
            return this.id;
        }

        override public string ToString()
        {
            return "While "+this.condition.ToString();
        }
    }
    public class ForNode : FlowGraphNode
    {
        int id;
        NodeType type { get; set; }
        public ForNode(int id)
        {
            this.type = NodeType.E_FOR;
            this.id = id;
        }
        public FlowGraphNode condition { get; set; }

        public List<FlowGraphNode> loop { get; set; }
        public FlowGraphNode init { get; set; }
        public FlowGraphNode next { get; set; }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }
        public int getId()
        {
            return this.id;
        }

        override public string ToString()
        {
            StringBuilder rez = new StringBuilder();
            if(this.init != null)
            {
                rez.Append(this.init.ToString() + ";\n");
            }
            if (this.condition != null)
            {
                rez.Append(this.condition.ToString() + ";\n");
            }
            if (this.next != null)
            {
                rez.Append(this.next.ToString() + ")");
            }
            return "For(" +rez.ToString();
        }
    }

    public class ID : FlowGraphNode
    {
        int id;
        NodeType type;
        public ID(int id)
        {
            this.id = id;
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
        public int getId()
        {
            return this.id;
        }
    }
    public class ConstantNode : FlowGraphNode
    {
        int id;
        NodeType type;
        public ConstantNode(int id)
        {
            this.id = id;
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
        public int getId()
        {
            return this.id;
        }
    }
    public class ArrayRef: FlowGraphNode
    {
        int id;
        NodeType type;
        public ArrayRef(int id)
        {
            this.id=id;
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

        public FlowGraphNode Name { get; set; }
        public FlowGraphNode index { get; set; }

        override public string ToString()
        {
            return this.Name + "["+index.ToString()+"]";
        }
        public int getId()
        {
            return this.id;
        }
    }
    public class StructRef : FlowGraphNode
    {
        int id;
        NodeType type;
        public StructRef(int id)
        {
            this.id = id;
            this.type = NodeType.E_STRUCTREF;
        }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }

        public string reftype { get; set; }
        public FlowGraphNode structfield { get; set; }
        public FlowGraphNode structname { get; set; }
        override public string ToString()
        {
            return this.structname.ToString() + reftype + structfield.ToString();
        }
        public int getId()
        {
            return this.id;
        }
    }

    public class BinaryOp : FlowGraphNode
    {
        int id;
        NodeType type;
        public BinaryOp(int id)
        {
            this.id = id;
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
        public int getId()
        {
            return this.id;
        }
    }
    public class UnaryOp : FlowGraphNode
    {
        int id;
        NodeType type;
        public UnaryOp(int id)
        {
            this.id = id;
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
        public int getId()
        {
            return this.id;
        }

    }

    public class Cast : FlowGraphNode
    {
        int id;
        NodeType type;
        public Cast(int id)
        {
            this.id = id;
            type = NodeType.E_CAST;
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
        public FlowGraphNode expr;
        public FlowGraphNode typetocast;
        override public string ToString()
        {
            return this.typetocast.ToString() + this.expr.ToString();
        }
        public int getId()
        {
            return this.id;
        }

    }
    public class ReturnNode : FlowGraphNode
    {
        int id;
        NodeType type;
        public ReturnNode(int id)
        {
            this.id = id;
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
            if (expr != null)
            {
                return "return " + this.expr.ToString();
            }
            else
            {
                return "return";
            }
        }
        public int getId()
        {
            return this.id;
        }
    }
    public class TypeName:FlowGraphNode
    {
        int id;
        NodeType type;
        public TypeName(int id)
        {
            this.id = id;
            type = NodeType.E_TYPENAME;
        }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }

        public FlowGraphNode Type;

        override public string ToString()
        {
            return " ";
        }
        public int getId()
        {
            return this.id;
        }
    }
    public class TypeDef : FlowGraphNode
    {
        int id;
        NodeType type;
        public TypeDef(int id)
        {
            this.id = id;
            type = NodeType.E_TYPEDEF;
        }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }

        public string name;
        public FlowGraphNode TypeDecl;

        override public string ToString()
        {
            return "type " + this.name.ToString();
        }
        public int getId()
        {
            return this.id;
        }
    }


    public class TypeDecl : FlowGraphNode
    {
        int id;
        NodeType type;
        public TypeDecl(int id)
        {
            this.id = id;
            type = NodeType.E_TYPEDEF;
        }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }

        public string name;
        public FlowGraphNode Type;

        override public string ToString()
        {
            return "type " + this.Type.ToString();
        }
        public int getId()
        {
            return this.id;
        }
    }

    public class StructNode : FlowGraphNode
    {
        int id;
        NodeType type;
        public StructNode(int id)
        {
            this.id = id;
            type = NodeType.E_STRUCT;
        }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }

        
        public List<FlowGraphNode> Decl;
        public string name;
        override public string ToString()
        {
            
            return "struct ";
        }
        public int getId()
        {
            return this.id;
        }
    }


    public class UnionNode : FlowGraphNode
    {
        int id;
        NodeType type;
        public UnionNode(int id)
        {
            this.id = id;
            type = NodeType.E_UNION;
        }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }

        public string name;
        public List<FlowGraphNode> Decl;

        override public string ToString()
        {

            return "struct ";
        }
        public int getId()
        {
            return this.id;
        }
    }

    public class EnumNode : FlowGraphNode
    {
        int id;
        NodeType type;
        public EnumNode(int id)
        {
            this.id = id;
            type = NodeType.E_ENUM;
        }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }


        public List<FlowGraphNode> Values;
        public string name;
        override public string ToString()
        {

            return this.name;
        }
        public int getId()
        {
            return this.id;
        }
    }
    public class EnumValue : FlowGraphNode
    {
        int id;
        NodeType type;
        public EnumValue(int id)
        {
            this.id = id;
            type = NodeType.E_ENUMVALUE;
        }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }


        public string value;

        override public string ToString()
        {

            return value;
        }
        public int getId()
        {
            return this.id;
        }
    }

    public class TypeDeclIdentifierType : FlowGraphNode
    {
        int id;
        NodeType type;
        public TypeDeclIdentifierType(int id)
        {
            this.id = id;
            type = NodeType.E_TYPEDECL_ID;
        }
        public NodeType getNodeType()
        {
            return this.type;
        }

        public void setNodeType(NodeType type)
        {
            this.type = type;
        }

        public string name;

        override public string ToString()
        {
            return "type " + this.name.ToString();
        }
        public int getId()
        {
            return this.id;
        }
    }
}
