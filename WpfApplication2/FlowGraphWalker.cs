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
        public List<List<int>> CalculateAllPathes(List<FlowGraphNode> nodes)
        {
            List<List<int>> Pathes= new List<List<int>>();
            foreach (FlowGraphNode node in nodes)
            {

                switch (node.getNodeType())
                {
                    case NodeType.E_WHILE:
                        {
                            WhileNode whilenode = (WhileNode)node;
                            if (Pathes.Count == 0)
                            {
                                Pathes.Add(new List<int>() { whilenode.getId() });
                            }
                            else
                            {
                                for (int i = 0; i < Pathes.Count; i++)
                                {
                                    Pathes[i].Add(whilenode.getId());
                                }
                            }
                            List<List<int>> dublicate = new List<List<int>>();
                            foreach (List<int> ls in Pathes)
                            {
                                List<int> tmp = new List<int>();
                                tmp.AddRange(ls);
                                dublicate.Add(tmp);
                            }
                            if (whilenode.loop != null)
                            {
                                List<List<int>> recursive = CalculateAllPathes(whilenode.loop);

                                foreach (List<int> ls in recursive)
                                {
                                    for (int j = 0; j < Pathes.Count; j++)
                                    {
                                        Pathes[j].AddRange(ls);
                                    }
                                }
                            }
                            foreach (List<int> ls in dublicate)
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
                                    Pathes.Add(new List<int>() { ifnode.getId() });
                                }
                                else
                                {
                                    for (int i = 0; i < Pathes.Count; i++)
                                    {
                                        Pathes[i].Add(node.getId());
                                    }
                                }
                            List<List<int>> dublicate = new List<List<int>>();
                            foreach (List<int> ls in Pathes)
                            {
                                List<int> tmp = new List<int>();
                                tmp.AddRange(ls);
                                dublicate.Add(tmp);
                            }
                            if (ifnode.left != null)
                            {
                                List<List<int>> recursive = CalculateAllPathes(ifnode.left);
                              
                                foreach (List<int> ls in recursive)
                                {
                                    for(int j=0;j<Pathes.Count;j++)
                                    {
                                        Pathes[j].AddRange(ls);
                                    }
                                }
                            }
                            if (ifnode.right != null)
                            {
                                List<List<int>> recursive = CalculateAllPathes(ifnode.right);

                                foreach (List<int> ls in recursive)
                                {
                                    for (int j = 0; j < dublicate.Count; j++)
                                    {
                                        dublicate[j].AddRange(ls);
                                    }
                                }
                            }
                            foreach (List<int> ls in dublicate)
                            {
                                Pathes.Add(ls);
                            }
                            break;
                        }
                    default:
                        {
                            if(Pathes.Count == 0)
                            {
                                Pathes.Add(new List<int>() { node.getId()});
                            }
                            else
                            {
                                for(int i=0;i<Pathes.Count;i++)
                                {
                                    Pathes[i].Add(node.getId());
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
