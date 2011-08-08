using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleVectors
{
    public class SimpleVector3d
    {
        private double[] xyz = new double[3];

        public double Z
        {
          get { return xyz[2]; }
          set { xyz[2] = value; }
        }

        public double Y
        {
          get { return xyz[1]; }
          set { xyz[2] = value; }
        }

        public double X
        {
          get { return xyz[0]; }
          set { xyz[0] = value; }
        }

        public SimpleVector3d () {}
        public SimpleVector3d ( double vx, double vy, double vz ) 
        { 
            xyz[0] = vx; 
            xyz[1] = vy; 
            xyz[2] = vz; 
        }
        public SimpleVector3d ( double v ) 
        { 
            xyz[0] = xyz[1] = xyz[2] = v; 
        }
        public SimpleVector3d ( SimpleVector3d v ) 
        { 
            xyz[0] = v.X; 
            xyz[1] = v.Y; 
            xyz[2] = v.Z; 
        }

        public static SimpleVector3d operator + ( SimpleVector3d u, SimpleVector3d v )
        {
            return new SimpleVector3d( u.X + v.X, u.Y + v.Y, u.Z + v.Z );
        }
        public static SimpleVector3d operator - ( SimpleVector3d u , SimpleVector3d v )
        {
            return new SimpleVector3d( u.X - v.X, u.Y - v.Y, u.Z - v.Z );
        }
        public static SimpleVector3d operator * ( double f, SimpleVector3d v  )
        {
            return new SimpleVector3d( f * v.X, f * v.Y, f * v.Z );
        }
        public static SimpleVector3d operator * ( SimpleVector3d u, double f )
        {
            return new SimpleVector3d( u.X * f, u.Y * f, u.Z * f );
        }
        public static SimpleVector3d operator * ( SimpleVector3d u, SimpleVector3d v )
        {
            return new SimpleVector3d( u.X * v.X, u.Y * v.Y, u.Z * v.Z );
        }
        public static SimpleVector3d operator / ( SimpleVector3d v, double f )
        {
            return new SimpleVector3d( v.X / f, v.Y / f, v.Z / f );
        }
        public static SimpleVector3d operator / ( SimpleVector3d u, SimpleVector3d v )
        {
            return new SimpleVector3d( u.X / v.X, u.Y / v.Y, u.Z / v.Z );
        }
        public static double operator & ( SimpleVector3d u, SimpleVector3d v ) 
        { 
            return u.X*v.X + u.Y*v.Y + u.Z*v.Z; 
        }
        public static SimpleVector3d operator ^ ( SimpleVector3d u, SimpleVector3d v )
        {
            return new SimpleVector3d( u.Y * v.Z - u.Z * v.Y,
                        u.Z * v.X - u.X * v.Z,
                        u.X * v.Y - u.Y * v.X );
        }

        public static double operator ! (SimpleVector3d v) 
        { 
            return Math.Sqrt( v.X*v.X + v.Y*v.Y + v.Z*v.Z ); 
        }
        public static bool operator < ( SimpleVector3d v, double f ) 
        {
            return v.X < f && v.Y < f && v.Z < f; 
        }
        public static bool operator > ( SimpleVector3d v, double f ) 
        {
            return v.X > f && v.Y > f && v.Z > f; 
        }
    }
}
