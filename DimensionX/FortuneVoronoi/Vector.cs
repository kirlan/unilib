using System;
using System.Collections;
using BenTools.Data;

namespace BenTools.Mathematics
{
	/// <summary>
	/// A vector class, implementing all interesting features of vectors
	/// </summary>
	public class BTVector : IEnumerable, IComparable
	{
		/// <summary>
		/// Global precision for any calculation
		/// </summary>
		public static int Precision = 10;
		public double[] data;
		public object Tag=null;
		/// <summary>
		/// Build a new vector
		/// </summary>
		/// <param name="dim">The dimension</param>
		public BTVector(int dim)
		{
			data = new double[dim];
		}
		/// <summary>
		/// Build a new vector
		/// </summary>
		/// <param name="X">The elements of the vector</param>
		public BTVector(params double[] X)
		{
			data = new double[X.Length];
			X.CopyTo(data,0);
		}
		/// <summary>
		/// Build a new vector as a copy of an existing one
		/// </summary>
		/// <param name="O">The existing vector</param>
		public BTVector(BTVector O)
			: this(O.data)
		{}
		/// <summary>
		/// Build a new vector from a string
		/// </summary>
		/// <param name="S">A string, as produced by ToString</param>
		public BTVector(string S)
		{
			if(S[0]!='(' || S[S.Length-1]!=')')
				throw new Exception("Formatfehler!");
			string[] P = MathTools.HighLevelSplit(S.Substring(1,S.Length-2),';');
			data = new double[P.Length];
			int i;
			for(i=0;i<data.Length;i++)
			{
				data[i] = Convert.ToDouble(P[i]);
			}
		}
		/// <summary>
		/// Gets or sets the value of the vector at the given index
		/// </summary>
        //public double this [int i]
        //{
        //    get
        //    {
        //        return data[i];
        //    }
        //    set
        //    {
        //        data[i] = Math.Round(value,Precision);
        //    }
        //}

        public void Set(int i, double value)
        {
            data[i] = Math.Round(value, Precision);
        }

		/// <summary>
		/// The dimension of the vector
		/// </summary>
		public int Dim
		{
			get
			{
				return data.Length;
			}
		}

		/// <summary>
		/// The squared length of the vector
		/// </summary>
		public double SquaredLength
		{
			get
			{
				return this*this;
			}
		}

		/// <summary>
		/// The sum of all elements in the vector
		/// </summary>
		public double ElementSum
		{
			get
			{
				int i;
				double E = 0;
				for(i=0;i<Dim;i++)
					E+=data[i];
				return E;
			}
		}
		/// <summary>
		/// Reset all elements with ransom values from the given range
		/// </summary>
		/// <param name="Min">Min</param>
		/// <param name="Max">Max</param>
		public void Randomize(double Min, double Max)
		{
			int i;
			for(i=0;i<data.Length;i++)
			{
				Set(i, Min + (Max-Min)*MathTools.R.NextDouble());
			}
		}
		/// <summary>
		/// Reset all elements with ransom values from the given range
		/// </summary>
		/// <param name="MinMax">MinMax[0] - Min
		/// MinMax[1] - Max</param>
		public void Randomize(BTVector[] MinMax)
		{
			int i;
			for(i=0;i<data.Length;i++)
			{
                Set(i, MinMax[0].data[i] + (MinMax[1].data[i] - MinMax[0].data[i]) * MathTools.R.NextDouble());
			}
		}
		/// <summary>
		/// Scale all elements by r
		/// </summary>
		/// <param name="r">The scalar</param>
		public void Multiply(double r)
		{
			int i;
			for(i=0;i<data.Length;i++)
			{
				Set(i, data[i] * r);
			}
		}
		/// <summary>
		/// Add another vector
		/// </summary>
		/// <param name="V">V</param>
		public void Add(BTVector V)
		{
			int i;
			for(i=0;i<data.Length;i++)
			{
                Set(i, data[i] + V.data[i]);
			}
		}
		/// <summary>
		/// Add a constant to all elements
		/// </summary>
		/// <param name="d">The constant</param>
		public void Add(double d)
		{
			int i;
			for(i=0;i<data.Length;i++)
			{
				Set(i, data[i] + d);
			}
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return data.GetEnumerator();
		}

		/// <summary>
		/// Convert the vector into a reconstructable string representation
		/// </summary>
		/// <returns>A string from which the vector can be rebuilt</returns>
		public override string ToString()
		{
			string S = "(";
			int i;
			for(i=0;i<data.Length;i++)
			{
				S += data[i].ToString("G4");
				if(i<data.Length-1)
					S+=";";
			}
			S+=")";
			return S;
		}

		/// <summary>
		/// Compares this vector with another one
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			BTVector B = obj as BTVector;
			if(B==null || data.Length != B.data.Length)
				return false;
			int i;
			for(i=0;i<data.Length;i++)
			{
				if(/*!data[i].Equals(B.data[i]) && */Math.Abs(data[i]-B.data[i])>1e-10)
					return false;
			}
			return true;
		}

		/// <summary>
		/// Retrieves a hashcode that is dependent on the elements
		/// </summary>
		/// <returns>The hashcode</returns>
		public override int GetHashCode()
		{
			int Erg = 0;
			foreach(double D in data)
				Erg = Erg ^ Math.Round(D,Precision).GetHashCode();
			return Erg;
		}

		/// <summary>
		/// Subtract two vectors
		/// </summary>
		public static BTVector operator - (BTVector A, BTVector B)
		{
			if(A.Dim!=B.Dim)
				throw new Exception("Vectors of different dimension!");
			BTVector Erg = new BTVector(A.Dim);
			int i;
			for(i=0;i<A.Dim;i++)
                Erg.Set(i, A.data[i] - B.data[i]);
			return Erg;
		}

		/// <summary>
		/// Add two vectors
		/// </summary>
		public static BTVector operator + (BTVector A, BTVector B)
		{
			if(A.Dim!=B.Dim)
				throw new Exception("Vectors of different dimension!");
			BTVector Erg = new BTVector(A.Dim);
			int i;
			for(i=0;i<A.Dim;i++)
				Erg.Set(i, A.data[i]+B.data[i]);
			return Erg;
		}

		/// <summary>
		/// Get the scalar product of two vectors
		/// </summary>
		public static double operator * (BTVector A, BTVector B)
		{
			if(A.Dim!=B.Dim)
				throw new Exception("Vectors of different dimension!");
			double Erg = 0;
			int i;
			for(i=0;i<A.Dim;i++)
                Erg += A.data[i] * B.data[i];
			return Erg;
		}

		/// <summary>
		/// Scale one vector
		/// </summary>
		public static BTVector operator * (BTVector A, double B)
		{
			BTVector Erg = new BTVector(A.Dim);
			int i;
			for(i=0;i<A.Dim;i++)
				Erg.Set(i, A.data[i]*B);
			return Erg;
		}

		/// <summary>
		/// Scale one vector
		/// </summary>
		public static BTVector operator * (double A, BTVector B)
		{
			return B*A;
		}
		/// <summary>
		/// Interprete the vector as a double-array
		/// </summary>
		public static explicit operator double[](BTVector A)
		{
			return A.data;
		}
		/// <summary>
		/// Get the distance of two vectors
		/// </summary>
		public static double Dist(BTVector V1, BTVector V2)
		{
			if(V1.Dim != V2.Dim)
				return -1;
			int i;
			double E = 0,D;
			for(i=0;i<V1.Dim;i++)
			{
                D = (V1.data[i] - V2.data[i]);
				E+=D*D;
			}
			return E;

		}

		/// <summary>
		/// Compare two vectors
		/// </summary>
		public int CompareTo(object obj)
		{
			BTVector A = this;
			BTVector B = obj as BTVector;
			if(A==null || B==null)
				return 0;
			double Al,Bl;
			Al = A.SquaredLength;
			Bl = B.SquaredLength;
			if(Al>Bl)
				return 1;
			if(Al<Bl)
				return -1;
			int i;
			for(i=0;i<A.Dim;i++)
			{
                if (A.data[i] > B.data[i])
					return 1;
                if (A.data[i] < B.data[i])
					return -1;
			}
			return 0;
		}
		/// <summary>
		/// Get a copy of one vector
		/// </summary>
		/// <returns></returns>
		public virtual BTVector Clone()
		{
			return new BTVector(data);
		}
	}

}
