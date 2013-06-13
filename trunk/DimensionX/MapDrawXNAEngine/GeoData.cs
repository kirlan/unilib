using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration;
using Microsoft.Xna.Framework;

namespace MapDrawXNAEngine
{
    internal class GeoData
    {
#region Static functions
        /// <summary>
        /// Вычисляет реальные 3d координаты центра локации с учётом её собственной высоты над сеткой и формы мира
        /// </summary>
        /// <param name="pLoc">локация</param>
        /// <param name="eShape">форма мира</param>
        /// <param name="fMultiplier">множитель высоты</param>
        /// <returns></returns>
        public static Vector3 GetPosition(IPointF pLoc, WorldShape eShape, float fMultiplier)
        {
            return GetPosition(pLoc, eShape, pLoc.H, fMultiplier);
        }

        /// <summary>
        /// Вычисляет реальные 3d координаты центра локации с учётом указанной высоты над сеткой и формы мира
        /// </summary>
        /// <param name="pLoc">локация</param>
        /// <param name="eShape">форма мира</param>
        /// <param name="fHeight">высота</param>
        /// <param name="fMultiplier">множитель высоты</param>
        /// <returns></returns>
        public static Vector3 GetPosition(IPointF pLoc, WorldShape eShape, float fHeight, float fMultiplier)
        {
            Vector3 pPosition = new Vector3(pLoc.X / 1000, pLoc.Z / 1000, pLoc.Y / 1000);
            Vector3 pUp = Vector3.Up;
            if (eShape == WorldShape.Ringworld)
                pUp = Vector3.Transform(-Vector3.Normalize(pPosition), Matrix.CreateScale(1, 1, 0));
            if (eShape == WorldShape.Planet)
                pUp = Vector3.Normalize(pPosition);

            pPosition += pUp * fHeight * fMultiplier;

            return pPosition;
        }

        /// <summary>
        /// Вычисляет нормаль к плоскости.ю образованной векторами v1-v2 и v1-v3
        /// </summary>
        /// <param name="v1">точка, в которой вычисляем нормаль</param>
        /// <param name="v2">вторая вершина треугольника</param>
        /// <param name="v3">третья вершина треугольника</param>
        /// <returns></returns>
        public static Vector3 GetNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Vector3 side1 = v1 - v3;
            Vector3 side2 = v1 - v2;
            return Vector3.Cross(side1, side2);
        }

        /// <summary>
        /// Вычисляет текстурную ось U в вершине (тангент, aka вектор 0X в пространстве касательных)
        /// </summary>
        /// <param name="v1">точка, в которой вычисляем тангент</param>
        /// <param name="v2">вторая вершина треугольника</param>
        /// <param name="v3">третья вершина треугольника</param>
        /// <param name="t1">текстурные координаты первой точки</param>
        /// <param name="t2">текстурные координаты второй точки</param>
        /// <param name="t3">текстурные координаты третьей точки</param>
        /// <returns></returns>
        public static Vector3 GetTangent(Vector3 v1, Vector3 v2, Vector3 v3, Vector4 t1, Vector4 t2, Vector4 t3)
        {
            Vector3 side1 = v1 - v3;
            Vector3 side2 = v1 - v2;

            Vector2 lT1 = new Vector2(t1.X - t2.X, t1.Y - t2.Y);
            Vector2 lT2 = new Vector2(t1.X - t3.X, t1.Y - t3.Y);

            float tmp = 0.0f;
            if (Math.Abs(lT1.X * lT2.Y - lT2.X * lT1.Y) <= 0.0001f)
            {
                tmp = 1.0f;
            }
            else
            {
                tmp = 1.0f / (lT1.X * lT2.Y - lT2.X * lT1.Y);
            }

            Vector3 tangent = new Vector3();
            tangent.X = (lT1.Y * side1.X - lT2.Y * side2.X);
            tangent.Y = (lT1.Y * side1.Y - lT2.Y * side2.Y);
            tangent.Z = (lT1.Y * side1.Z - lT2.Y * side2.Z);

            tangent = -tangent * tmp;
            tangent.Normalize();

            return tangent;
        }
#endregion

        public VoronoiVertex m_pOwner;

        public Vector3 m_pPosition = new Vector3(0);
        public Vector3 m_pNormal = new Vector3(0);
        public Vector3 m_pTangent = new Vector3(0);

        public Vector4 TexWeights = new Vector4(0);
        public Vector4 TexWeights2 = new Vector4(0);
        public Vector4 TextureCoordinate = new Vector4(0);

        public Microsoft.Xna.Framework.Color m_pColor = Microsoft.Xna.Framework.Color.Black;

        public GeoData(VoronoiVertex pOwner, WorldShape eShape, float fMultiplier)
        {
            m_pOwner = pOwner;
            m_pPosition = GetPosition(pOwner, eShape, fMultiplier);
        }

        public GeoData(VoronoiVertex pOwner, WorldShape eShape, float fHeight, float fMultiplier)
        {
            m_pOwner = pOwner;
            m_pPosition = GetPosition(pOwner, eShape, fHeight, fMultiplier);
        }
    }

    internal class GeoData2 : GeoData
    {
        public GeoData[] m_aLinked;

        public GeoData2(VoronoiVertex pOwner, WorldShape eShape, float fMultiplier)
            : base(pOwner, eShape, fMultiplier)
        {
        }

        public GeoData2(VoronoiVertex pOwner, WorldShape eShape, float fHeight, float fMultiplier)
            : base(pOwner, eShape, fHeight, fMultiplier)
        {
        }
    }
}
