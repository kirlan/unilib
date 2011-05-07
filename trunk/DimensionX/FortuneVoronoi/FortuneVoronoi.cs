using System;
using System.Collections;
using BenTools.Data;

namespace BenTools.Mathematics
{
	public class VoronoiGraph
	{
		public HashSet Vertizes = new HashSet();
		public HashSet Edges = new HashSet();
	}
	public class VoronoiEdge
	{
        /// <summary>
        /// Cell center at right from edge
        /// </summary>
        public BTVector RightData;
        /// <summary>
        /// Cell center at left of edge
        /// </summary>
        public BTVector LeftData;

        /// <summary>
        /// Start of edge
        /// </summary>
        public BTVector VVertexA = Fortune.VVUnkown;
        /// <summary>
        /// End of edge
        /// </summary>
        public BTVector VVertexB = Fortune.VVUnkown;

		public void AddVertex(BTVector V)
		{
			if(VVertexA==Fortune.VVUnkown)
				VVertexA = V;
			else if(VVertexB==Fortune.VVUnkown)
				VVertexB = V;
			else throw new Exception("Tried to add third vertex!");
		}
	}
	
	// VoronoiVertex or VoronoiDataPoint are represented as Vector

	internal abstract class VNode
	{
		private VNode _Parent = null;
		private VNode _Left = null, _Right = null;
		public VNode Left
		{
			get{return _Left;}
			set
			{
				_Left = value;
				value.Parent = this;
			}
		}
		public VNode Right
		{
			get{return _Right;}
			set
			{
				_Right = value;
				value.Parent = this;
			}
		}
		public VNode Parent
		{
			get{return _Parent;}
			set{_Parent = value;}
		}


		public void Replace(VNode ChildOld, VNode ChildNew)
		{
			if(Left==ChildOld)
				Left = ChildNew;
			else if(Right==ChildOld)
				Right = ChildNew;
			else throw new Exception("Child not found!");
			ChildOld.Parent = null;
		}

		public static VDataNode FirstDataNode(VNode Root)
		{
			VNode C = Root;
			while(C.Left!=null)
				C = C.Left;
			return (VDataNode)C;
		}
		public static VDataNode LeftDataNode(VDataNode Current)
		{
			VNode C = Current;
			//1. Up
			do
			{
				if(C.Parent==null)
					return null;
				if(C.Parent.Left == C)
				{
					C = C.Parent;
					continue;
				}
				else
				{
					C = C.Parent;
					break;
				}
			}while(true);
			//2. One Left
			C = C.Left;
			//3. Down
			while(C.Right!=null)
				C = C.Right;
			return (VDataNode)C; // Cast statt 'as' damit eine Exception kommt
		}
		public static VDataNode RightDataNode(VDataNode Current)
		{
			VNode C = Current;
			//1. Up
			do
			{
				if(C.Parent==null)
					return null;
				if(C.Parent.Right == C)
				{
					C = C.Parent;
					continue;
				}
				else
				{
					C = C.Parent;
					break;
				}
			}while(true);
			//2. One Right
			C = C.Right;
			//3. Down
			while(C.Left!=null)
				C = C.Left;
			return (VDataNode)C; // Cast statt 'as' damit eine Exception kommt
		}

		public static VEdgeNode EdgeToRightDataNode(VDataNode Current)
		{
			VNode C = Current;
			//1. Up
			do
			{
				if(C.Parent==null)
					throw new Exception("No Left Leaf found!");
				if(C.Parent.Right == C)
				{
					C = C.Parent;
					continue;
				}
				else
				{
					C = C.Parent;
					break;
				}
			}while(true);
			return (VEdgeNode)C;
		}

		public static VDataNode FindDataNode(VNode Root, double ys, double x)
		{
			VNode C = Root;
			do
			{
				if(C is VDataNode)
					return (VDataNode)C;
				if(((VEdgeNode)C).Cut(ys,x)<0)
					C = C.Left;
				else
					C = C.Right;
			}while(true);
		}

		/// <summary>
        /// Will return the new root (unchanged except in start-up)
		/// </summary>
		/// <param name="e">Processing event</param>
		/// <param name="Root"></param>
		/// <param name="VG"></param>
		/// <param name="ys"></param>
		/// <param name="CircleCheckList"></param>
		/// <returns></returns>
        public static VNode ProcessDataEvent(VDataEvent e, VNode Root, VoronoiGraph VG, double ys, out VDataNode[] CircleCheckList)
		{
			if(Root==null)
			{
				Root = new VDataNode(e.DataPoint);
				CircleCheckList = new VDataNode[] {(VDataNode)Root};
				return Root;
			}
			//1. Find the node to be replaced
			VNode C = VNode.FindDataNode(Root, ys, e.DataPoint.data[0]);
			//2. Create the subtree (ONE Edge, but two VEdgeNodes)
			VoronoiEdge VE = new VoronoiEdge();
			VE.LeftData = ((VDataNode)C).DataPoint;
			VE.RightData = e.DataPoint;
			VE.VVertexA = Fortune.VVUnkown;
			VE.VVertexB = Fortune.VVUnkown;
			VG.Edges.Add(VE);

			VNode SubRoot;
            if (Math.Abs(VE.LeftData.data[1] - VE.RightData.data[1]) < 1e-10)
			{
                if (VE.LeftData.data[0] < VE.RightData.data[0])
				{
					SubRoot = new VEdgeNode(VE,false);
					SubRoot.Left = new VDataNode(VE.LeftData);
					SubRoot.Right = new VDataNode(VE.RightData);
				}
				else
				{
					SubRoot = new VEdgeNode(VE,true);
					SubRoot.Left = new VDataNode(VE.RightData);
					SubRoot.Right = new VDataNode(VE.LeftData);
				}
				CircleCheckList = new VDataNode[] {(VDataNode)SubRoot.Left,(VDataNode)SubRoot.Right};
			}
			else
			{
				SubRoot = new VEdgeNode(VE,false);
				SubRoot.Left = new VDataNode(VE.LeftData);
				SubRoot.Right = new VEdgeNode(VE,true);
				SubRoot.Right.Left = new VDataNode(VE.RightData);
				SubRoot.Right.Right = new VDataNode(VE.LeftData);
				CircleCheckList = new VDataNode[] {(VDataNode)SubRoot.Left,(VDataNode)SubRoot.Right.Left,(VDataNode)SubRoot.Right.Right};
			}

			//3. Apply subtree
			if(C.Parent == null)
				return SubRoot;
			C.Parent.Replace(C,SubRoot);
			return Root;
		}
		public static VNode ProcessCircleEvent(VCircleEvent e, VNode Root, VoronoiGraph VG, double ys, out VDataNode[] CircleCheckList)
		{
			VDataNode a,b,c;
			VEdgeNode eu,eo;
			b = e.NodeN;
			a = VNode.LeftDataNode(b);
			c = VNode.RightDataNode(b);
			if(a==null || b.Parent==null || c==null || !a.DataPoint.Equals(e.NodeL.DataPoint) || !c.DataPoint.Equals(e.NodeR.DataPoint))
			{
				CircleCheckList = new VDataNode[]{};
				return Root; // Abbruch da sich der Graph verändert hat
			}
			eu = (VEdgeNode)b.Parent;
			CircleCheckList = new VDataNode[] {a,c};
			//1. Create the new Vertex
            BTVector VNew = new BTVector(e.Center.data[0], e.Center.data[1]);
//			VNew[0] = Fortune.ParabolicCut(a.DataPoint[0],a.DataPoint[1],c.DataPoint[0],c.DataPoint[1],ys);
//			VNew[1] = (ys + a.DataPoint[1])/2 - 1/(2*(ys-a.DataPoint[1]))*(VNew[0]-a.DataPoint[0])*(VNew[0]-a.DataPoint[0]);
			VG.Vertizes.Add(VNew);
			//2. Find out if a or c are in a distand part of the tree (the other is then b's sibling) and assign the new vertex
			if(eu.Left==b) // c is sibling
			{
				eo = VNode.EdgeToRightDataNode(a);

				// replace eu by eu's Right
				eu.Parent.Replace(eu,eu.Right);
			}
			else // a is sibling
			{
				eo = VNode.EdgeToRightDataNode(b);

				// replace eu by eu's Left
				eu.Parent.Replace(eu,eu.Left);
			}
			eu.Edge.AddVertex(VNew);
//			///////////////////// uncertain
//			if(eo==eu)
//				return Root;
//			/////////////////////
			eo.Edge.AddVertex(VNew);
			//2. Replace eo by new Edge
			VoronoiEdge VE = new VoronoiEdge();
			VE.LeftData = a.DataPoint;
			VE.RightData = c.DataPoint;
			VE.AddVertex(VNew);
			VG.Edges.Add(VE);

			VEdgeNode VEN = new VEdgeNode(VE, false);
			VEN.Left = eo.Left;
			VEN.Right = eo.Right;
			if(eo.Parent == null)
				return VEN;
			eo.Parent.Replace(eo,VEN);
			return Root;
		}
		public static VCircleEvent CircleCheckDataNode(VDataNode n, double ys)
		{
			VDataNode l = VNode.LeftDataNode(n);
			VDataNode r = VNode.RightDataNode(n);
			if(l==null || r==null || l.DataPoint==r.DataPoint || l.DataPoint==n.DataPoint || n.DataPoint==r.DataPoint)
				return null;
            if (MathTools.ccw(l.DataPoint.data[0], l.DataPoint.data[1], n.DataPoint.data[0], n.DataPoint.data[1], r.DataPoint.data[0], r.DataPoint.data[1], false) <= 0)
				return null;
			BTVector Center = Fortune.CircumCircleCenter(l.DataPoint,n.DataPoint,r.DataPoint);
			VCircleEvent VC = new VCircleEvent(n, l, r, Center, true);
            //VC.NodeN = n;
            //VC.NodeL = l;
            //VC.NodeR = r;
            //VC.Center = Center;
            //VC.Valid = true;
			if(VC.Y>=ys)
				return VC;
			return null;
		}
	}

	internal class VDataNode : VNode
	{
		public VDataNode(BTVector DP)
		{
			this.DataPoint = DP;
		}
		public BTVector DataPoint;
	}

	internal class VEdgeNode : VNode
	{
		public VEdgeNode(VoronoiEdge E, bool Flipped)
		{
			this.Edge = E;
			this.Flipped = Flipped;
		}
		public VoronoiEdge Edge;
		public bool Flipped;
		public double Cut(double ys, double x)
		{
			if(!Flipped)
                return Math.Round(x - Fortune.ParabolicCut(Edge.LeftData.data[0], Edge.LeftData.data[1], Edge.RightData.data[0], Edge.RightData.data[1], ys), 10);
            return Math.Round(x - Fortune.ParabolicCut(Edge.RightData.data[0], Edge.RightData.data[1], Edge.LeftData.data[0], Edge.LeftData.data[1], ys), 10);
		}
	}


	internal abstract class VEvent : IComparable
	{
		public abstract double Y {get;}
		public abstract double X {get;}
		#region IComparable Members

		public int CompareTo(object obj)
		{
			if(!(obj is VEvent))
				throw new ArgumentException("obj not VEvent!");
			int i = Y.CompareTo(((VEvent)obj).Y);
			if(i!=0)
				return i;
			return X.CompareTo(((VEvent)obj).X);
		}

		#endregion
	}

    /// <summary>
    /// The point event is handled when a new parabolic arc appears on parabolic front. 
    /// This event happens exactly when a new arc appears on the parabolic front, or more precise, 
    /// when the scanning line  encounteres a point pi from set P. Then, the scanning plane   
    /// hits the coin defined above point pi for the first time. Because the coin and the scanning 
    /// plane are slanted for the same angle, the intersection between them appears as a line on 
    /// the coin surface. This line is actually a half-edge, but if it is projected onto xy-plane 
    /// we can consider it a degenerated parabola with zero width. This degenerated parabola 
    /// starts to open as the scanning plane  slides over the space. When a new arc comes across, 
    /// it splits the existed arc into two parts and becomes a new member of the parabolic front.
    /// 
    /// What happens with Voronoi diagram when a point event is encountered? Lets remember, that a 
    /// joint point of two arcs on parabolic front describes the Voronoi edge.
    /// </summary>
	internal class VDataEvent : VEvent
	{
        /// <summary>
        /// Cell center
        /// </summary>
		public BTVector DataPoint;
		/// <summary>
		/// The only constructor
		/// </summary>
		/// <param name="DP">Cell center</param>
        public VDataEvent(BTVector DP)
		{
			this.DataPoint = DP;
		}
		public override double Y
		{
			get
			{
                return DataPoint.data[1];
			}
		}

		public override double X
		{
			get
			{
                return DataPoint.data[0];
			}
		}

	}

	internal class VCircleEvent : VEvent
	{
		public VDataNode NodeN, NodeL, NodeR;
		public BTVector Center;

        public double optY;

        public override double Y
        {
            get
            {
                return optY;// Math.Round(Center.data[1] + MathTools.Dist(NodeN.DataPoint.data[0], NodeN.DataPoint.data[1], Center.data[0], Center.data[1]), 10);
            }
        }

		public override double X
		{
			get
			{
                return Center.data[0];
			}
		}

		public bool Valid = true;
        private VDataNode n;
        private VDataNode l;
        private VDataNode r;
        private BTVector Center_2;
        private bool p;

        public VCircleEvent(VDataNode n, VDataNode l, VDataNode r, BTVector center, bool valid)
        {
            // TODO: Complete member initialization
            NodeN = n;
            NodeL = l;
            NodeR = r;
            Center = center;
            Valid = valid;

            optY = Math.Round(Center.data[1] + MathTools.Dist(NodeN.DataPoint.data[0], NodeN.DataPoint.data[1], Center.data[0], Center.data[1]), 10);
        }
	}

	public abstract class Fortune
	{
		public static readonly BTVector VVInfinite = new BTVector(double.PositiveInfinity, double.PositiveInfinity);
		public static readonly BTVector VVUnkown = new BTVector(double.NaN, double.NaN);
		internal static double ParabolicCut(double x1, double y1, double x2, double y2, double ys)
		{
//			y1=-y1;
//			y2=-y2;
//			ys=-ys;
//			
			if(Math.Abs(x1-x2)<1e-10 && Math.Abs(y1-y2)<1e-10)
			{
//				if(y1>y2)
//					return double.PositiveInfinity;
//				if(y1<y2)
//					return double.NegativeInfinity;
//				return x;
				throw new Exception("Identical datapoints are not allowed!");
			}

			if(Math.Abs(y1-ys)<1e-10 && Math.Abs(y2-ys)<1e-10)
				return (x1+x2)/2;
			if(Math.Abs(y1-ys)<1e-10)
				return x1;
			if(Math.Abs(y2-ys)<1e-10)
				return x2;
			double a1 = 1/(2*(y1-ys));
			double a2 = 1/(2*(y2-ys));
			if(Math.Abs(a1-a2)<1e-10)
				return (x1+x2)/2;
			double xs1 = 0.5/(2*a1-2*a2)*(4*a1*x1-4*a2*x2+2*Math.Sqrt(-8*a1*x1*a2*x2-2*a1*y1+2*a1*y2+4*a1*a2*x2*x2+2*a2*y1+4*a2*a1*x1*x1-2*a2*y2));
			double xs2 = 0.5/(2*a1-2*a2)*(4*a1*x1-4*a2*x2-2*Math.Sqrt(-8*a1*x1*a2*x2-2*a1*y1+2*a1*y2+4*a1*a2*x2*x2+2*a2*y1+4*a2*a1*x1*x1-2*a2*y2));
			xs1=Math.Round(xs1,10);
			xs2=Math.Round(xs2,10);
			if(xs1>xs2)
			{
				double h = xs1;
				xs1=xs2;
				xs2=h;
			}
			if(y1>=y2)
				return xs2;
			return xs1;
		}
		internal static BTVector CircumCircleCenter(BTVector A, BTVector B, BTVector C)
		{
			if(A==B || B==C || A==C)
				throw new Exception("Need three different points!");
            double tx = (A.data[0] + C.data[0]) / 2;
            double ty = (A.data[1] + C.data[1]) / 2;

            double vx = (B.data[0] + C.data[0]) / 2;
            double vy = (B.data[1] + C.data[1]) / 2;

			double ux,uy,wx,wy;

            if (A.data[0] == C.data[0])
			{
				ux = 1;
				uy = 0;
			}
			else
			{
                ux = (C.data[1] - A.data[1]) / (A.data[0] - C.data[0]);
				uy = 1;
			}

            if (B.data[0] == C.data[0])
			{
				wx = -1;
				wy = 0;
			}
			else
			{
                wx = (B.data[1] - C.data[1]) / (B.data[0] - C.data[0]);
				wy = -1;
			}

			double alpha = (wy*(vx-tx)-wx*(vy - ty))/(ux*wy-wx*uy);

			return new BTVector(tx+alpha*ux,ty+alpha*uy);
		}	

        /// <summary>
        /// The algorithm itself (Fortune.ComputeVoronoiGraph(IEnumerable)) takes any IEnumerable 
        /// containing only two dimensional vectors. It will return a VoronoiGraph. The algorithm's 
        /// complexity is O(n ld(n)) with a factor of about 10 microseconds on my machine (2GHz).
        /// </summary>
        /// <param name="Datapoints">IEnumerable collection of Vector, representing cells' centers</param>
        /// <returns>VoronoiGraph - collections ov vertexes and edges</returns>
		public static VoronoiGraph ComputeVoronoiGraph(IEnumerable Datapoints)
		{
            //The event queue Q is arranged regarding the events y-coordinates. 
            //It stores the upcoming events that are already known. For the point event, the point 
            //is simply stored. For the circle event, the lowest point of the circle is stored. 
            //In the last case, the pointer to the leaf in the tree of parabolic front that 
            //represents the arc disappearing in the event is stored, too.
			BinaryPriorityQueue PQ = new BinaryPriorityQueue();
			
            Hashtable CurrentCircles = new Hashtable();
			VoronoiGraph VG = new VoronoiGraph();
			VNode RootNode = null;
            //Adds all centers to events queue
			foreach(BTVector V in Datapoints)
			{
				PQ.Push(new VDataEvent(V));
			}

            //Run through events queue (sorted by vector length?)
			while(PQ.Count>0)
			{
				VEvent VE = PQ.Pop() as VEvent;
				VDataNode[] CircleCheckList;
				if(VE is VDataEvent)
				{
					RootNode = VNode.ProcessDataEvent(VE as VDataEvent,RootNode,VG,VE.Y,out CircleCheckList);
				}
				else if(VE is VCircleEvent)
				{
					CurrentCircles.Remove(((VCircleEvent)VE).NodeN);
					if(!((VCircleEvent)VE).Valid)
						continue;
					RootNode = VNode.ProcessCircleEvent(VE as VCircleEvent,RootNode,VG,VE.Y,out CircleCheckList);
				}
				else throw new Exception("Got event of type "+VE.GetType().ToString()+"!");
				foreach(VDataNode VD in CircleCheckList)
				{
					if(CurrentCircles.ContainsKey(VD))
					{
						((VCircleEvent)CurrentCircles[VD]).Valid=false;
						CurrentCircles.Remove(VD);
					}
					VCircleEvent VCE = VNode.CircleCheckDataNode(VD,VE.Y);
					if(VCE!=null)
					{
						PQ.Push(VCE);
						CurrentCircles[VD]=VCE;
					}
				}
				if(VE is VDataEvent)
				{
					BTVector DP = ((VDataEvent)VE).DataPoint;

                    //VCircleEvent[] aCEs = new VCircleEvent[CurrentCircles.Values.Count];
                    //CurrentCircles.Values.CopyTo(aCEs, 0);

					foreach(VCircleEvent VCE in CurrentCircles.Values)
                    //for (int i=0; i< aCEs.Length; i++)
                    {
                        //VCircleEvent VCE = aCEs[i];
                        double fDist1Squared = (VCE.Center.data[0] - DP.data[0]) * (VCE.Center.data[0] - DP.data[0]) + (VCE.Center.data[1] - DP.data[1]) * (VCE.Center.data[1] - DP.data[1]);
                        double fDist2Squared = (VCE.optY - VCE.Center.data[1]) * (VCE.optY - VCE.Center.data[1]);
                        if (fDist1Squared < fDist2Squared &&
                            Math.Abs(fDist1Squared - fDist2Squared) > 1e-20 + 2 * 1e-10 * (VCE.optY - VCE.Center.data[1]))
							VCE.Valid = false;
					}
				}
			}
			return VG;
		}
	}
}
