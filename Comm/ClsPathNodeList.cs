using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace WinRobots.Comm
{
    //Dijkstra算法求最短路径(C#版)
    //图的路径(V0是起点):
    class ClsPathNodeList
    {
        private int[] distance;
        private int row;
        private ArrayList ways = new ArrayList();

        public ClsPathNodeList(int n, params int[] d)
        {
            this.row = n;
            distance = new int[row * row];
            for (int i = 0; i < row * row; i++)
            {
                this.distance[i] = d[i];
            }
            for (int i = 0; i < this.row; i++)  //有row个点，则从中心到各点的路有row-1条
            {
                ArrayList w = new ArrayList();
                int j = 0;
                w.Add(j);
                ways.Add(w);
            }
        }
        //------------------------------
        public void Find_way()
        {
            ArrayList S = new ArrayList(1);
            ArrayList Sr = new ArrayList(1);
            int[] Indexof_distance = new int[this.row];

            for (int i = 0; i < row; i++)
            {
                Indexof_distance[i] = i;
            }

            S.Add(Indexof_distance[0]);

            for (int i = 0; i < this.row; i++)
            {
                Sr.Add(Indexof_distance[i]);
            }
            Sr.RemoveAt(0);
            int[] D = new int[this.row];    //存放中心点到每个点的距离

            //---------------以上已经初始化了，S和Sr(里边放的都是点的编号)------------------
            int Count = this.row - 1;
            while (Count > 0)
            {
                //假定中心点的编号是0的贪吃法求路径
                for (int i = 0; i < row; i++)
                    D[i] = this.distance[i];

                int min_num = (int)Sr[0];  //距中心点的最小距离点编号

                foreach (int s in Sr)
                {
                    if (D[s] < D[min_num]) min_num = s;
                }

                //以上可以排序优化
                S.Add(min_num);
                Sr.Remove(min_num);
                //-----------把最新包含进来的点也加到路径中-------------
                ((ArrayList)ways[min_num]).Add(min_num);
                //-----------------------------------------------
                foreach (int element in Sr)
                {
                    int position = element * (this.row) + min_num;
                    bool exchange = false;      //有交换标志

                    if (D[element] < D[min_num] + this.distance[position])
                        D[element] = D[element];
                    else
                    {
                        D[element] = this.distance[position] + D[min_num];
                        exchange = true;
                    }
                    //修改距离矩阵                   
                    this.distance[element] = D[element];
                    position = element * this.row;
                    this.distance[position] = D[element];

                    //修改路径---------------
                    if (exchange == true)
                    {
                        ((ArrayList)ways[element]).Clear();
                        foreach (int point in (ArrayList)ways[min_num])
                            ((ArrayList)ways[element]).Add(point);
                    }
                }
                --Count;
            }
        }

        public void Display()
        {
          //------中心到各点的最短路径----------
          //  Console.WriteLine("中心到各点的最短路径如下: \n\n");
            int sum_d_index = 0;
            foreach (ArrayList mother in ways)
            {
                foreach (int child in mother)
                    Console.Write("V{0} -- ", child + 1);
              //  Console.WriteLine("    路径长 {0}", distance[sum_d_index++]);
            }
        }
    }
}
