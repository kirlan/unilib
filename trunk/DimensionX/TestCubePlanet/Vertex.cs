using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCubePlanet
{
    public class Vertex
    {
        public float m_fX;
        public float m_fY;
        public float m_fZ;

        public Vertex(float fX, float fY, float fR, Cube.Face3D eFace)
        {
            //Формула взята на http://mathproofs.blogspot.ru/2005/07/mapping-cube-to-sphere.html
            float x = 1;
            float y = 1;
            float z = 1;
            switch (eFace)
            {
                case Cube.Face3D.Forward:
                        x = fX / fR;
                        y = fY / fR;
                        z = 1;
                    break;
                case Cube.Face3D.Right:
                        x = 1;
                        y = fY / fR;
                        z = -fX / fR;
                    break;
                case Cube.Face3D.Backward:
                        x = -fX / fR;
                        y = fY / fR;
                        z = -1;
                    break;
                case Cube.Face3D.Left:
                        x = -1;
                        y = fY / fR;
                        z = fX / fR;
                    break;
                case Cube.Face3D.Top:
                        x = fX / fR;
                        y = 1;
                        z = -fY / fR;
                    break;
                case Cube.Face3D.Bottom:
                        x = fX / fR;
                        y = -1;
                        z = fY / fR;
                    break;
            }
            m_fX = fR * x * (float)Math.Sqrt(1 - y * y / 2 - z * z / 2 + y * y * z * z / 3);
            m_fY = fR * y * (float)Math.Sqrt(1 - x * x / 2 - z * z / 2 + x * x * z * z / 3);
            m_fZ = fR * z * (float)Math.Sqrt(1 - x * x / 2 - y * y / 2 + x * x * y * y / 3);
        }
    }
}
