using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public class FlowGraphWalker
    {

        public FlowGraphWalker()
        {

        }
        public List<List<FlowGraphNode>> CalculateAllPathes(List<FlowGraphNode> nodes)
        {
            List<List<FlowGraphNode>> Pathes= new List<List<FlowGraphNode>>();
            foreach (FlowGraphNode node in nodes)
            {
                switch (node.getNodeType())
                {
                    case NodeType.E_FOR:
                        {
                            ForNode whilenode = (ForNode)node;
                            if (Pathes.Count == 0)
                            {
                                Pathes.Add(new List<FlowGraphNode>() { whilenode });
                            }
                            else
                            {
                                for (int i = 0; i < Pathes.Count; i++)
                                {
                                    if (NodeType.E_RETURN != Pathes[i].Last().getNodeType())
                                    {
                                        Pathes[i].Add(whilenode);
                                    }
                                    }
                            }
                            List<List<FlowGraphNode>> dublicate = new List<List<FlowGraphNode>>();
                            foreach (List<FlowGraphNode> ls in Pathes)
                            {
                                if (NodeType.E_RETURN != ls.Last().getNodeType())
                                {
                                    List<FlowGraphNode> tmp = new List<FlowGraphNode>();
                                    tmp.AddRange(ls);
                                    dublicate.Add(tmp);
                                }
                            }
                            if (whilenode.loop != null)
                            {
                                List<List<FlowGraphNode>> recursive = CalculateAllPathes(whilenode.loop);

                                foreach (List<FlowGraphNode> ls in recursive)
                                {
                                    for (int j = 0; j < Pathes.Count; j++)
                                    {
                                        if (NodeType.E_RETURN != Pathes[j].Last().getNodeType())
                                        {
                                            Pathes[j].AddRange(ls);
                                        }
                                    }
                                }
                            }
                            foreach (List<FlowGraphNode> ls in dublicate)
                            {

                                Pathes.Add(ls);
                            }
                            break;
                        }
                    case NodeType.E_WHILE:
                        {
                            WhileNode whilenode = (WhileNode)node;
                            if (Pathes.Count == 0)
                            {
                                Pathes.Add(new List<FlowGraphNode>() { whilenode });
                            }
                            else
                            {
                                for (int i = 0; i < Pathes.Count; i++)
                                {
                                    if (NodeType.E_RETURN != Pathes[i].Last().getNodeType())
                                    {
                                        Pathes[i].Add(whilenode);
                                    }
                                }
                            }
                            List<List<FlowGraphNode>> dublicate = new List<List<FlowGraphNode>>();
                            foreach (List<FlowGraphNode> ls in Pathes)
                            {
                                if (NodeType.E_RETURN != ls.Last().getNodeType())
                                {
                                    List<FlowGraphNode> tmp = new List<FlowGraphNode>();
                                    tmp.AddRange(ls);
                                    dublicate.Add(tmp);
                                }
                            }
                            if (whilenode.loop != null)
                            {
                                List<List<FlowGraphNode>> recursive = CalculateAllPathes(whilenode.loop);

                                foreach (List<FlowGraphNode> ls in recursive)
                                {
                                    for (int j = 0; j < Pathes.Count; j++)
                                    {
                                        if (NodeType.E_RETURN != Pathes[j].Last().getNodeType())
                                        {
                                            Pathes[j].AddRange(ls);
                                        }
                                    }
                                }
                            }
                            foreach (List<FlowGraphNode> ls in dublicate)
                            {
                                Pathes.Add(ls);
                            }
                            break;
                        }
                    case NodeType.E_IF:
                        {
                                IfNode ifnode = (IfNode)node;
                                if (Pathes.Count == 0)
                                {
                                    Pathes.Add(new List<FlowGraphNode>() { ifnode});
                                }
                                else
                                {
                                    for (int i = 0; i < Pathes.Count; i++)
                                    {
                                    if (NodeType.E_RETURN != Pathes[i].Last().getNodeType())
                                    {
                                        Pathes[i].Add(node);
                                    }
                                    }
                                }
                            List<List<FlowGraphNode>> dublicate = new List<List<FlowGraphNode>>();
                            foreach (List<FlowGraphNode> ls in Pathes)
                            {
                                if (NodeType.E_RETURN != ls.Last().getNodeType())
                                {
                                    List<FlowGraphNode> tmp = new List<FlowGraphNode>();
                                    tmp.AddRange(ls);
                                    dublicate.Add(tmp);
                                }
                            }
                            if (ifnode.left != null)
                            {
                                List<List<FlowGraphNode>> recursive = CalculateAllPathes(ifnode.left);
                              
                                foreach (List<FlowGraphNode> ls in recursive)
                                {
                                    for(int j=0;j<Pathes.Count;j++)
                                    {
                                        if (NodeType.E_RETURN != Pathes[j].Last().getNodeType())
                                        {
                                            Pathes[j].AddRange(ls);
                                        }
                                    }
                                }
                            }
                            if (ifnode.right != null)
                            {
                                List<List<FlowGraphNode>> recursive = CalculateAllPathes(ifnode.right);

                                foreach (List<FlowGraphNode> ls in recursive)
                                {
                                    for (int j = 0; j < dublicate.Count; j++)
                                    {
                                        if (NodeType.E_RETURN != dublicate[j].Last().getNodeType())
                                        {
                                            dublicate[j].AddRange(ls);
                                        }
                                    }
                                }
                            }
                            foreach (List<FlowGraphNode> ls in dublicate)
                            {
                                Pathes.Add(ls);
                            }
                            break;
                        }

                    default:
                        {
                            if(Pathes.Count == 0)
                            {
                                Pathes.Add(new List<FlowGraphNode>() { node});
                            }
                            else
                            {
                                for(int i=0;i<Pathes.Count;i++)
                                {
                                    if (NodeType.E_RETURN != Pathes[i].Last().getNodeType())
                                    {
                                        Pathes[i].Add(node);
                                    }
                                }
                            }
                            break;
                        }
                }
            }
                        return Pathes;
        }
    }
}
