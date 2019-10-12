using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using WinRobots.Comm;

namespace WinRobots.UComTral
{
    public partial class UCPath : UserControl
    {
        public UCPath()
        {
            InitializeComponent();
          //  iniv();
        }

        private void btnRW_Click(object sender, EventArgs e)
        {
            Node aNode = new Node("A");
            nodeList.Add(aNode);
            Edge aEdge1 = new Edge();
            aEdge1.StartNodeID = aNode.ID;
            aEdge1.EndNodeID = "B";
            aEdge1.Weight = 10;
            aNode.EdgeList.Add(aEdge1);

            Node bNode = new Node("B");
            nodeList.Add(bNode);
            Edge bEdge1 = new Edge();
            bEdge1.StartNodeID = bNode.ID;
            bEdge1.EndNodeID = "C";
            bEdge1.Weight = 5;
            bNode.EdgeList.Add(bEdge1);

            Edge bEdge2 = new Edge();
            bEdge2.StartNodeID = bNode.ID;
            bEdge2.EndNodeID = "D";
            bEdge2.Weight = 5;
            bNode.EdgeList.Add(bEdge2);

            Node cNode = new Node("C");
            nodeList.Add(cNode);
            Edge cEdge1 = new Edge();
            cEdge1.StartNodeID = cNode.ID;
            cEdge1.EndNodeID = "D";
            cEdge1.Weight = 5;
            cNode.EdgeList.Add(cEdge1);

            Edge cEdge2 = new Edge();
            cEdge2.StartNodeID = cNode.ID;
            cEdge2.EndNodeID = "B";
            cEdge2.Weight = 5;
            cNode.EdgeList.Add(cEdge2);

            Edge cEdge3 = new Edge();
            cEdge3.StartNodeID = cNode.ID;
            cEdge3.EndNodeID = "F";
            cEdge3.Weight = 10;
            cNode.EdgeList.Add(cEdge3);


            Node dNode = new Node("D");
            nodeList.Add(dNode);

            Edge dEdge1 = new Edge();
            dEdge1.StartNodeID = dNode.ID;
            dEdge1.EndNodeID = "B";
            dEdge1.Weight = 5;
            dNode.EdgeList.Add(dEdge1);

            Edge dEdge2 = new Edge();
            dEdge2.StartNodeID = dNode.ID;
            dEdge2.EndNodeID = "C";
            dEdge2.Weight = 5;
            dNode.EdgeList.Add(dEdge2);

            Edge dEdge3 = new Edge();
            dEdge3.StartNodeID = dNode.ID;
            dEdge3.EndNodeID = "E";
            dEdge3.Weight = 10;
            dNode.EdgeList.Add(dEdge3);


            Node eNode = new Node("E");
            nodeList.Add(eNode);

            Edge eEdge1 = new Edge();
            eEdge1.StartNodeID = eNode.ID;
            eEdge1.EndNodeID = "D";
            eEdge1.Weight = 10;
            eNode.EdgeList.Add(eEdge1);


            Node fNode = new Node("F");
            nodeList.Add(fNode);
            Edge fEdge1 = new Edge();
            fEdge1.StartNodeID = fNode.ID;
            fEdge1.EndNodeID = "C";
            fEdge1.Weight = 10;
            fNode.EdgeList.Add(fEdge1);

            Edge fEdge2 = new Edge();
            fEdge2.StartNodeID = fNode.ID;
            fEdge2.EndNodeID = "H";
            fEdge2.Weight = 5;
            fNode.EdgeList.Add(fEdge2);

            Edge fEdge3 = new Edge();
            fEdge3.StartNodeID = fNode.ID;
            fEdge3.EndNodeID = "G";
            fEdge3.Weight = 5;
            fNode.EdgeList.Add(fEdge3);


            Node gNode = new Node("G");
            nodeList.Add(gNode);
            Edge gEdge1 = new Edge();
            gEdge1.StartNodeID = gNode.ID;
            gEdge1.EndNodeID = "F";
            gEdge1.Weight = 5;
            gNode.EdgeList.Add(gEdge1);

            Edge gEdge2 = new Edge();
            gEdge2.StartNodeID = gNode.ID;
            gEdge2.EndNodeID = "I";
            gEdge2.Weight = 6;
            gNode.EdgeList.Add(gEdge2);

            Node hNode = new Node("H");
            nodeList.Add(hNode);
            Edge hEdge1 = new Edge();
            hEdge1.StartNodeID = hNode.ID;
            hEdge1.EndNodeID = "F";
            hEdge1.Weight = 5;
            hNode.EdgeList.Add(hEdge1);

            Edge hEdge2 = new Edge();
            hEdge2.StartNodeID = hNode.ID;
            hEdge2.EndNodeID = "I";
            hEdge2.Weight = 5;
            hNode.EdgeList.Add(hEdge2);

            Node iNode = new Node("I");
            nodeList.Add(iNode);
            Edge iEdge1 = new Edge();
            iEdge1.StartNodeID = iNode.ID;
            iEdge1.EndNodeID = "H";
            iEdge1.Weight = 5;
            iNode.EdgeList.Add(iEdge1);

            Edge iEdge2 = new Edge();
            iEdge2.StartNodeID = iNode.ID;
            iEdge2.EndNodeID = "G";
            iEdge2.Weight = 6;
            iNode.EdgeList.Add(iEdge2);

            RoutePlanner planner = new RoutePlanner();
            RoutePlanResult result = planner.Paln(nodeList, "E", "C");
        }

        private void btnSendCmd_Click(object sender, EventArgs e)
        {
            n = 0;
        }



        //计算最优路径
        private void button25_Click(object sender, EventArgs e)
        {
            n = 1;
            int v = 0;
            //int  v = int.Parse(toolStripTextBox1.Text);
            string str = "s";
            switch (str)
            {
                case "A":
                    v = 0; break;
                case "B":
                    v = 1; break;
                case "C":
                    v = 2; break;
                case "D":
                    v = 3; break;
                case "E":
                    v = 4; break;
                default:
                    MessageBox.Show("请输入正确的源点");
                    break;
            }
            int[] d;//最短路径
            shortpan(t, v, out d);
            for (int i = 0; i < 5; i++)
            {
               
            }

        }

        //初始化节点
        ArrayList nodeList = new ArrayList();
        RoutePlanner planner = new RoutePlanner();


        /// <summary>
        /// 最小值
        /// </summary>
        /// <param name="Q"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private int MIN(int[] Q, out int j)
        {
            int a = 10000;
            j = 0;
            for (int i = 0; i < Q.Length; i++)
            {
                if (a >= Q[i])
                {
                    a = Q[i];
                    j = i;
                    this.richTextBox2.Text += i.ToString();
                }
            }
            return a;
        }
        /// <summary>
        /// 最短路径
        /// </summary>
        /// <param name="t"></param>      
        /// <param name="v"></param>
        /// <param name="d"></param>
        private void shortpan(int[,] t, int v, out int[] d)
        {
            int[,] h;
            int[] Q;
            h = new int[t.GetLength(0), t.GetLength(0)];
            d = new int[t.GetLength(0)];
            Q = new int[t.GetLength(0)];
            d[v] = 0;//源点到源点为0;
            int u = v;//记录移除的节点                    
            int w = v;//记录前一个节点
            int max = 10000;
            //判断是否存在边，初始化Q
            for (int i = 0; i < t.GetLength(0); i++)
            {
                for (int j = 0; j < t.GetLength(0); j++)
                {
                    if (t[i, j] < max && t[i, j] != 0)
                    {
                        h[i, j] = 1;
                    }
                }
                Q[i] = max;
            }
            //更新最短路径
            for (int j = 1; j < d.Length; j++)
            {
                int l = 0;
                for (int i = 0; i < Q.Length; i++)
                {
                    if (h[u, i] == 1)
                    {
                        if (t[u, i] + d[u] <= Q[i])
                        {
                            g.DrawLine(new Pen(Brushes.Red, 2),new Point( S[u].X+i, S[u].Y), S[i]);
                            System.Threading.Thread.Sleep(500);
                            if (Q[i] != max && h[w, i] != 0)
                                g.DrawLine(new Pen(Brushes.Yellow, 2), S[w], S[i]);
                            Q[i] = t[u, i] + d[u];
                            this.richTextBox2.Text += ">>" + t[u, i].ToString();
                        }
                        else
                        {
                            g.DrawLine(new Pen(Brushes.Red, 2), new Point(S[u].X + i, S[u].Y), S[i]);
                            System.Threading.Thread.Sleep(500);
                            g.DrawLine(new Pen(Brushes.Yellow, 2), S[u], S[i]);
                            System.Threading.Thread.Sleep(500);

                            this.richTextBox2.Text += "\n >>" + u.ToString();
                        }
                        h[i, u] = 2;
                    }
                }
                w = u;
                int r = MIN(Q, out u);
                d[u] = r;
                Q[u] = max;
            }
        }

        Point[] S;
        Graphics g;
        int[,] t;
        int n = 0;
        List<int> W;
        public void initAB()
        {
            iniv();

        }

        public void iniv()
        {
            W = new List<int>();
            t = new int[12, 12];
            S = new Point[12];

            Point a1 = new Point(24, 183);
            S[0] = a1;
            Point a2 = new Point(24, 88);
            S[1] = a2;
            Point a3 = new Point(105, 197);
            S[2] = a3;
            Point a4 = new Point(105, 130);
            S[3] = a4;
            Point a5 = new Point(105, 75);
            S[4] = a5;
            Point a6 = new Point(0, 0);
            S[5] = a6;
            Point a7 = new Point(175, 244);
            S[6] = a7;
            Point a8 = new Point(202, 196);
            S[7] = a8;
            Point a9 = new Point(197, 140);
            S[8] = a9;
            Point a10 = new Point(197, 66);
            S[9] = a10;
            Point a11 = new Point(235, 176);
            S[10] = a11;
            Point a12= new Point(345, 176);
            S[11] = a12;
            for (int i = 0; i < 12; i++)
            {             
                for (int j = 0; j < 12; j++)
                {
                    t[j, i] = 0;
                }
            }
            t[0, 1] = 20;
            t[0, 2] = 8;
            t[1, 0] = 20;
            t[1, 4] = 8;
            t[4, 1] = 8;
            t[4, 3] = 5;
            t[3, 4] = 5;
            t[3, 2] = 5;
            t[2, 3] = 5;
            t[2, 0] = 8;
            t[2, 6] = 20;
            t[6, 2] = 20;
            t[6, 7] = 5;
            t[7, 6] = 5;
            t[7, 8] = 5;
            t[7, 10] = 8;
            t[8, 7] = 5;
            t[8, 10] = 8;
            t[8, 9] = 10;
            t[9, 8] = 10;
            t[10, 7] = 8;
            t[10, 8] = 8;
            t[10, 11] = 10;
            t[11, 10] = 10;

            if (n == 0)
            {
                g = pictureBox2.CreateGraphics();
                Pen pen = new Pen(Brushes.Yellow, 2);
                for (int m = 0; m < 12; m++)
                {
                    if (m == 5) continue;
                    g.DrawString("a"+(m+1).ToString(), new Font("宋体", 18), Brushes.Red, S[m].X , S[m].Y);
                }
                g.DrawLine(pen, S[0], S[1]);
                g.DrawLine(pen, S[0], S[2]);
                g.DrawLine(pen, S[1], S[0]);
                g.DrawLine(pen, S[1], S[4]);
                g.DrawLine(pen, S[4], S[1]);
                g.DrawLine(pen, S[4], S[3]);
                g.DrawLine(pen, S[3], S[4]);
                g.DrawLine(pen, S[3], S[2]);
                g.DrawLine(pen, S[2], S[3]);
                g.DrawLine(pen, S[2], S[0]);
                g.DrawLine(pen, S[2], S[6]);
                g.DrawLine(pen, S[6], S[2]);
                g.DrawLine(pen, S[6], S[7]);
                g.DrawLine(pen, S[7], S[6]);
                g.DrawLine(pen, S[7], S[8]);
                g.DrawLine(pen, S[7], S[10]);
                g.DrawLine(pen, S[8], S[7]);
                g.DrawLine(pen, S[8], S[10]);
                g.DrawLine(pen, S[8], S[9]);
                g.DrawLine(pen, S[9], S[8]);
                g.DrawLine(pen, S[10], S[7]);
                g.DrawLine(pen, S[10], S[8]);
                g.DrawLine(pen, S[10], S[11]);
                g.DrawLine(pen, S[11], S[10]);
                
            }

       
        }

        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            iniv();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text +="\n"+ Cursor.Position.X.ToString() + "_" + Cursor.Position.Y.ToString();
        }
    }
}
