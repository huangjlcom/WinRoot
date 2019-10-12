using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace WinRobots.Comm
{
    public class ClsPathName
    {
        public string PathStr = "";

    }


    public class Edge
    {
        public string StartNodeID;
        public string EndNodeID;
        public double Weight; //权值，代价        
    }


    public class Node
    {
        private string iD;
        private ArrayList edgeList;//Edge的集合－－出边表

        public Node(string id)
        {
            this.iD = id;
            this.edgeList = new ArrayList();
        }

        #region property
        public string ID
        {
            get
            {
                return this.iD;
            }
        }

        public ArrayList EdgeList
        {
            get
            {
                return this.edgeList;
            }
        }
        #endregion
    }

    /// <summary>
    /// PassedPath 用于缓存计算过程中的到达某个节点的权值最小的路径
    /// </summary>
    public class PassedPath
    {
        private string curNodeID;
        private bool beProcessed;   //是否已被处理
        private double weight;        //累积的权值
        private ArrayList passedIDList; //路径

        public PassedPath(string ID)
        {
            this.curNodeID = ID;
            this.weight = double.MaxValue;
            this.passedIDList = new ArrayList();
            this.beProcessed = false;
        }

        #region property
        public bool BeProcessed
        {
            get
            {
                return this.beProcessed;
            }
            set
            {
                this.beProcessed = value;
            }
        }

        public string CurNodeID
        {
            get
            {
                return this.curNodeID;
            }
        }

        public double Weight
        {
            get
            {
                return this.weight;
            }
            set
            {
                this.weight = value;
            }
        }

        public ArrayList PassedIDList
        {
            get
            {
                return this.passedIDList;
            }
        }
        #endregion
    }

    /// <summary>
    /// PlanCourse 缓存从源节点到其它任一节点的最小权值路径＝》路径表
    /// </summary>
    public class PlanCourse
    {
        private Hashtable htPassedPath;

        #region ctor
        public PlanCourse(ArrayList nodeList, string originID)
        {
            this.htPassedPath = new Hashtable();
            Node originNode = null;
            foreach (Node node in nodeList)
            {
                if (node.ID == originID)
                {
                    originNode = node;
                }
                else
                {
                    PassedPath pPath = new PassedPath(node.ID);
                    this.htPassedPath.Add(node.ID, pPath);
                }
            }

            if (originNode == null)
            {
                throw new Exception("The origin node is not exist !");
            }

            this.InitializeWeight(originNode);
        }

        private void InitializeWeight(Node originNode)
        {
            if ((originNode.EdgeList == null) || (originNode.EdgeList.Count == 0))
            {
                return;
            }

            foreach (Edge edge in originNode.EdgeList)
            {
                PassedPath pPath = this[edge.EndNodeID];
                if (pPath == null)
                {
                    continue;
                }

                pPath.PassedIDList.Add(originNode.ID);
                pPath.Weight = edge.Weight;
            }
        }
        #endregion

        public PassedPath this[string nodeID]
        {
            get
            {
                return (PassedPath)this.htPassedPath[nodeID];
            }
        }
    }

    //
    public class RoutePlanResult
    {
        private string[] passedNodeIDs;

        private double weight;

        public RoutePlanResult(string[] strings, double d)
        {
            this.passedNodeIDs = strings;
            this.weight = d;
        }

        /**
        * @return Returns the passedNodeIDs.
        */
        public string[] getPassedNodeIDs()
        {
            return passedNodeIDs;
        }

        /**
        * @param passedNodeIDs The passedNodeIDs to set.
        */
        public void setPassedNodeIDs(string[] passedNodeIDs)
        {
            this.passedNodeIDs = passedNodeIDs;
        }

        /**
        * @return Returns the weight.
        */
        public double getWeight()
        {
            return weight;
        }

        /**
        * @param weight The weight to set.
        */
        public void setWeight(double weight)
        {
            this.weight = weight;
        }
    }

    /// <summary>
    /// RoutePlanner 提供图算法中常用的路径规划功能。
    /// 2005.09.06
    /// </summary>
    public class RoutePlanner
    {
        public RoutePlanner()
        {
        }

        #region Paln
        //获取权值最小的路径
        public RoutePlanResult Paln(ArrayList nodeList, string originID, string destID)
        {
            PlanCourse planCourse = new PlanCourse(nodeList, originID);

            Node curNode = this.GetMinWeightRudeNode(planCourse, nodeList, originID);

            #region 计算过程
            while (curNode != null)
            {
                PassedPath curPath = planCourse[curNode.ID];
                foreach (Edge edge in curNode.EdgeList)
                {
                    

                    PassedPath targetPath = planCourse[edge.EndNodeID];
                    if (targetPath == null) continue;
                    double tempWeight = curPath.Weight + edge.Weight;

                    if (tempWeight < targetPath.Weight)
                    {
                        targetPath.Weight = tempWeight;
                        targetPath.PassedIDList.Clear();

                        for (int i = 0; i < curPath.PassedIDList.Count; i++)
                        {
                            targetPath.PassedIDList.Add(curPath.PassedIDList[i].ToString());
                        }

                        targetPath.PassedIDList.Add(curNode.ID);
                    }
                }

                //标志为已处理
                planCourse[curNode.ID].BeProcessed = true;
                //获取下一个未处理节点
                curNode = this.GetMinWeightRudeNode(planCourse, nodeList, originID);
            }
            #endregion

            //表示规划结束
            return this.GetResult(planCourse, destID);
        }
        #endregion

        #region private method
        #region GetResult
        //从PlanCourse表中取出目标节点的PassedPath，这个PassedPath即是规划结果
        private RoutePlanResult GetResult(PlanCourse planCourse, string destID)
        {
            PassedPath pPath = planCourse[destID];

            if (pPath.Weight == int.MaxValue)
            {
                RoutePlanResult result1 = new RoutePlanResult(null, int.MaxValue);
                return result1;
            }

            string[] passedNodeIDs = new string[pPath.PassedIDList.Count];
            for (int i = 0; i < passedNodeIDs.Length; i++)
            {
                passedNodeIDs[i] = pPath.PassedIDList[i].ToString();
            }
            RoutePlanResult result = new RoutePlanResult(passedNodeIDs, pPath.Weight);

            return result;
        }
        #endregion

        #region GetMinWeightRudeNode
        //从PlanCourse取出一个当前累积权值最小，并且没有被处理过的节点
        private Node GetMinWeightRudeNode(PlanCourse planCourse, ArrayList nodeList, string originID)
        {
            double weight = double.MaxValue;
            Node destNode = null;

            foreach (Node node in nodeList)
            {
                if (node.ID == originID)
                {
                    continue;
                }

                PassedPath pPath = planCourse[node.ID];
                if (pPath.BeProcessed)
                {
                    continue;
                }

                if (pPath.Weight < weight)
                {
                    weight = pPath.Weight;
                    destNode = node;
                }
            }

            return destNode;
        }
        #endregion
        #endregion
    }
}
