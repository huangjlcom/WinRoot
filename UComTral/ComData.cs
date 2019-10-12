using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using WinRobots.Comm;

namespace WinRobots.UComTral
{
   public   class ComData
    {

       public string GetModelID(string ID)
       {
           string rVal = "";
           switch (ID)
           {
               case "A":
                   rVal = "12";
                   break;
               case "B":
                   rVal = "11";
                   break;
               case "C":
                   rVal = "8";
                   break;
               case "D":
                   rVal = "9";
                   break;
               case "E":
                   rVal = "10";
                   break;
               case "F":
                   rVal = "4";
                   break;
               case "G":
                   rVal = "5";
                   break;
               case "I":
                   rVal = "2";
                   break;
               case "H":
                   rVal = "1";
                   break;

           }
           return rVal;
       }
       public string GetModelIDs(string ID)
       {
           string rVal = "";
           switch (ID)
           {
               case "12":
                   rVal = "A";
                   break;
               case "11":
                   rVal = "B";
                   break;
               case "8":
                   rVal = "C";
                   break;
               case "9":
                   rVal = "D";
                   break;
               case "10":
                   rVal = "E";
                   break;
               case "4":
                   rVal = "F";
                   break;
               case "5":
                   rVal = "G";
                   break;
               case "2":
                   rVal = "I";
                   break;
               case "1":
                   rVal = "H";
                   break;


           }

           return rVal;
       }

       //初始化节点
       public ArrayList nodeList = new ArrayList();
       RoutePlanner planner = new RoutePlanner();
       public void initNode()
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

      

       }

       public void initFCKNode()
       {
           Node aNode = new Node("A");
           nodeList.Add(aNode);
           Edge aEdge1 = new Edge();
           aEdge1.StartNodeID = aNode.ID;
           aEdge1.EndNodeID = "B";
           aEdge1.Weight = 10;
           aNode.EdgeList.Add(aEdge1);

           Node bNode = new Node("B"); 
           Edge abEdge1 = new Edge();
           abEdge1.StartNodeID = bNode.ID;
           abEdge1.EndNodeID = "A";
           abEdge1.Weight = 10;
           bNode.EdgeList.Add(abEdge1);
           nodeList.Add(bNode);

           Edge bEdge1 = new Edge();
           bEdge1.StartNodeID = bNode.ID;
           bEdge1.EndNodeID = "C";
           bEdge1.Weight = 10;
           bNode.EdgeList.Add(bEdge1);


           Node cNode = new Node("C");
           nodeList.Add(cNode);
           Edge cEdge1 = new Edge();
           cEdge1.StartNodeID = cNode.ID;
           cEdge1.EndNodeID = "D";
           cEdge1.Weight = 10;
           cNode.EdgeList.Add(cEdge1);

           Edge DEdge1 = new Edge();
           DEdge1.StartNodeID = cNode.ID;
           DEdge1.EndNodeID = "B";
           DEdge1.Weight = 10;
           cNode.EdgeList.Add(DEdge1);

           Edge EEdge1 = new Edge();
           EEdge1.StartNodeID = cNode.ID;
           EEdge1.EndNodeID = "E";
           EEdge1.Weight = 10;
           cNode.EdgeList.Add(EEdge1);

           Node dNode = new Node("D");
           nodeList.Add(dNode);

           Edge dEdge1 = new Edge();
           dEdge1.StartNodeID = dNode.ID;
           dEdge1.EndNodeID = "C";
           dEdge1.Weight = 10;
           dNode.EdgeList.Add(dEdge1);


           Node eNode = new Node("E");
           nodeList.Add(eNode);

           Edge eEdge1 = new Edge();
           eEdge1.StartNodeID = eNode.ID;
           eEdge1.EndNodeID = "C";
           eEdge1.Weight = 10;
           eNode.EdgeList.Add(eEdge1);

       }

       public string[] GetString()
       {
           string[] strl = new string[2] { "1_2_3_4_5_6_9_8_7_10_11_12", "1_2_3_4_5_6_14_13" };

           return strl;
       }
       public void initFNode()
       {
           string strpath = "A";
           int n = 14;
           Node aNode;
           for (int i = 0; i < n; i++)
           {
               aNode = new Node(strpath+(i+1).ToString());
               nodeList.Add(aNode);
           }
           Edge aEdge;
           foreach (Node atemp in nodeList)
           {
               switch (atemp.ID)
               {
                   case "A1":
                        aEdge = new Edge();
                        aEdge.StartNodeID = atemp.ID;
                        aEdge.EndNodeID = "A2";
                        aEdge.Weight = 10;
                        atemp.EdgeList.Add(aEdge);
                       break;
                   case "A2":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A1";
                       aEdge.Weight = 10;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A3";
                       aEdge.Weight = 10;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A3":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A2";
                       aEdge.Weight = 10;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A4";
                       aEdge.Weight = 10;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A4":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A3";
                       aEdge.Weight = 10;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A5";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A5":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A4";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A6";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A6":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A5";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A9";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A7":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A8";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A10";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A8":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A7";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A9";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A9":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A6";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A14";
                       aEdge.Weight = 18;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A8";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A10":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A7";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A11";
                       aEdge.Weight = 10;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A11":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A10";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A12";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A12":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A11";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);

                      
                       break;
                   case "A13":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A14";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);

                    
                       break;
                   case "A14":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A13";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A9";
                       aEdge.Weight = 18;
                       atemp.EdgeList.Add(aEdge);
                       break;

               }
           }
          //int sl =  nodeList.Count;
          //sl++;
           //Node aNode = new Node("A");
           //nodeList.Add(aNode);
           //Edge aEdge1 = new Edge();
           //aEdge1.StartNodeID = aNode.ID;
           //aEdge1.EndNodeID = "B";
           //aEdge1.Weight = 10;
           //aNode.EdgeList.Add(aEdge1);

           //Node bNode = new Node("B");
           //nodeList.Add(bNode);
           //Edge bEdge1 = new Edge();
           //bEdge1.StartNodeID = bNode.ID;
           //bEdge1.EndNodeID = "C";
           //bEdge1.Weight = 5;
           //bNode.EdgeList.Add(bEdge1);

           //Edge bEdge2 = new Edge();
           //bEdge2.StartNodeID = bNode.ID;
           //bEdge2.EndNodeID = "D";
           //bEdge2.Weight = 5;
           //bNode.EdgeList.Add(bEdge2);



           //Node cNode = new Node("C");
           //nodeList.Add(cNode);
           //Edge cEdge1 = new Edge();
           //cEdge1.StartNodeID = cNode.ID;
           //cEdge1.EndNodeID = "D";
           //cEdge1.Weight = 5;
           //cNode.EdgeList.Add(cEdge1);

           //Edge cEdge2 = new Edge();
           //cEdge2.StartNodeID = cNode.ID;
           //cEdge2.EndNodeID = "B";
           //cEdge2.Weight = 5;
           //cNode.EdgeList.Add(cEdge2);

           //Edge cEdge3 = new Edge();
           //cEdge3.StartNodeID = cNode.ID;
           //cEdge3.EndNodeID = "F";
           //cEdge3.Weight = 10;
           //cNode.EdgeList.Add(cEdge3);


           //Node dNode = new Node("D");
           //nodeList.Add(dNode);

           //Edge dEdge1 = new Edge();
           //dEdge1.StartNodeID = dNode.ID;
           //dEdge1.EndNodeID = "B";
           //dEdge1.Weight = 5;
           //dNode.EdgeList.Add(dEdge1);

           //Edge dEdge2 = new Edge();
           //dEdge2.StartNodeID = dNode.ID;
           //dEdge2.EndNodeID = "C";
           //dEdge2.Weight = 5;
           //dNode.EdgeList.Add(dEdge2);

           //Edge dEdge3 = new Edge();
           //dEdge3.StartNodeID = dNode.ID;
           //dEdge3.EndNodeID = "E";
           //dEdge3.Weight = 10;
           //dNode.EdgeList.Add(dEdge3);


           //Node eNode = new Node("E");
           //nodeList.Add(eNode);

           //Edge eEdge1 = new Edge();
           //eEdge1.StartNodeID = eNode.ID;
           //eEdge1.EndNodeID = "D";
           //eEdge1.Weight = 10;
           //eNode.EdgeList.Add(eEdge1);


           //Node fNode = new Node("F");
           //nodeList.Add(fNode);
           //Edge fEdge1 = new Edge();
           //fEdge1.StartNodeID = fNode.ID;
           //fEdge1.EndNodeID = "C";
           //fEdge1.Weight = 10;
           //fNode.EdgeList.Add(fEdge1);

           //Edge fEdge2 = new Edge();
           //fEdge2.StartNodeID = fNode.ID;
           //fEdge2.EndNodeID = "H";
           //fEdge2.Weight = 5;
           //fNode.EdgeList.Add(fEdge2);

           //Edge fEdge3 = new Edge();
           //fEdge3.StartNodeID = fNode.ID;
           //fEdge3.EndNodeID = "G";
           //fEdge3.Weight = 5;
           //fNode.EdgeList.Add(fEdge3);


           //Node gNode = new Node("G");
           //nodeList.Add(gNode);
           //Edge gEdge1 = new Edge();
           //gEdge1.StartNodeID = gNode.ID;
           //gEdge1.EndNodeID = "F";
           //gEdge1.Weight = 5;
           //gNode.EdgeList.Add(gEdge1);

           //Edge gEdge2 = new Edge();
           //gEdge2.StartNodeID = gNode.ID;
           //gEdge2.EndNodeID = "I";
           //gEdge2.Weight = 6;
           //gNode.EdgeList.Add(gEdge2);

           //Node hNode = new Node("H");
           //nodeList.Add(hNode);
           //Edge hEdge1 = new Edge();
           //hEdge1.StartNodeID = hNode.ID;
           //hEdge1.EndNodeID = "F";
           //hEdge1.Weight = 5;
           //hNode.EdgeList.Add(hEdge1);

           //Edge hEdge2 = new Edge();
           //hEdge2.StartNodeID = hNode.ID;
           //hEdge2.EndNodeID = "I";
           //hEdge2.Weight = 5;
           //hNode.EdgeList.Add(hEdge2);

           //Node iNode = new Node("I");
           //nodeList.Add(iNode);
           //Edge iEdge1 = new Edge();
           //iEdge1.StartNodeID = iNode.ID;
           //iEdge1.EndNodeID = "H";
           //iEdge1.Weight = 5;
           //iNode.EdgeList.Add(iEdge1);

           //Edge iEdge2 = new Edge();
           //iEdge2.StartNodeID = iNode.ID;
           //iEdge2.EndNodeID = "G";
           //iEdge2.Weight = 6;
           //iNode.EdgeList.Add(iEdge2);



       }



       public void Clear()
       {
           nodeList.Clear();
       }

       public void MovNode(string _point)
       {
           for (int i = 0; i < nodeList.Count; i++)
           {
               if (((Node)nodeList[i]).ID == "A" + _point)
               {
                   nodeList.Remove(nodeList[i]);
                   break;
               }
           }

       }

       public void initFCNode()
       {
           string strpath = "A";
           int n = 10;
           Node aNode;
           for (int i = 0; i < n; i++)
           {
               if (i == 3) continue;
               aNode = new Node(strpath + (i + 1).ToString());
               nodeList.Add(aNode);
           }
           Edge aEdge;
           foreach (Node atemp in nodeList)
           {
               switch (atemp.ID)
               {
                   case "A1":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A2";
                       aEdge.Weight = 10;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A5";
                       aEdge.Weight = 20;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A2":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A1";
                       aEdge.Weight = 10;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A3";
                       aEdge.Weight = 10;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A3":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A2";
                       aEdge.Weight = 10;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A7";
                       aEdge.Weight = 15;
                       atemp.EdgeList.Add(aEdge);
                       
                         aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A8";
                       aEdge.Weight = 15;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A5":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A1";
                       aEdge.Weight = 20;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A6";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A6":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A5";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A7";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A7":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A3";
                       aEdge.Weight = 15;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A6";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A8";
                       aEdge.Weight = 30;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A8":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A7";
                       aEdge.Weight = 30;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A9";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);
                          aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A3";
                       aEdge.Weight = 15;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A9":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A10";
                       aEdge.Weight = 18;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A8";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A10":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A9";
                       aEdge.Weight = 18;
                       atemp.EdgeList.Add(aEdge);
                       break;
               }
           }
       }


       //虚拟设置
       public void initRNode()
       {
           string strpath = "A";
           int n = 13;
           Node aNode;
           for (int i = 0; i < n; i++)
           {
               if (i == 3) continue;
               aNode = new Node(strpath + (i + 1).ToString());
               nodeList.Add(aNode);
           }
           Edge aEdge;
           foreach (Node atemp in nodeList)
           {
               switch (atemp.ID)
               {
                   case "A1":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A2";
                       aEdge.Weight = 10;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A5";
                       aEdge.Weight = 20;
                       atemp.EdgeList.Add(aEdge);
                      
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A11";
                       aEdge.Weight = 5;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A2":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A1";
                       aEdge.Weight = 10;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A3";
                       aEdge.Weight = 10;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A3":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A2";
                       aEdge.Weight = 10;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A7";
                       aEdge.Weight = 15;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A8";
                       aEdge.Weight = 15;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A5":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A1";
                       aEdge.Weight = 20;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A6";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);

                         aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A11";
                       aEdge.Weight = 6;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A6":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A5";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A7";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A7":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A3";
                       aEdge.Weight = 15;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A6";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A8";
                       aEdge.Weight = 30;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A8":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A7";
                       aEdge.Weight = 30;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A9";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A3";
                       aEdge.Weight = 15;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A9":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A10";
                       aEdge.Weight = 18;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A8";
                       aEdge.Weight = 8;
                       atemp.EdgeList.Add(aEdge);
                       break;
                   case "A10":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A9";
                       aEdge.Weight = 18;
                       atemp.EdgeList.Add(aEdge);

                        aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A13";
                       aEdge.Weight = 5;
                       atemp.EdgeList.Add(aEdge);
                       break;

                   case "A11":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A1";
                       aEdge.Weight = 5;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A5";
                       aEdge.Weight = 6;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A12";
                       aEdge.Weight = 5;
                       atemp.EdgeList.Add(aEdge);
                       break;

                   case "A12":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A11";
                       aEdge.Weight = 5;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A13";
                       aEdge.Weight = 5;
                       atemp.EdgeList.Add(aEdge);
                       break;

                   case "A13":
                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A12";
                       aEdge.Weight = 5;
                       atemp.EdgeList.Add(aEdge);

                       aEdge = new Edge();
                       aEdge.StartNodeID = atemp.ID;
                       aEdge.EndNodeID = "A10";
                       aEdge.Weight = 5;
                       atemp.EdgeList.Add(aEdge);
                       break;
               }
           }
       }
    }
}
