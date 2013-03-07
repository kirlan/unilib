using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestCubePlanet
{
    public enum SettlementSize
    {
        Hamlet,
        Village,
        Fort,
        Town,
        City,
        Capital
    }

    public class SettlementModel
    {
        public readonly Vector3 m_pPosition;
        public readonly Vector3 m_pUp;
        public readonly float m_fAngle;
        public readonly Model m_pModel;
        public readonly float m_fScale;
        public readonly Matrix worldMatrix;
        public readonly string m_sName;
        /// <summary>
        /// 0 - деревни и хутора
        /// 1 - города и форты
        /// 2 - большие города и столицы
        /// </summary>
        public readonly int m_iSize;

        public SettlementModel(Vector3 pPosition, float fAngle, float fScale, Model pModel, Texture2D pTexture, string sName, int iSize)
        {
            m_pPosition = pPosition;
            m_fAngle = fAngle;
            m_fScale = fScale;
            m_pModel = pModel;
            m_sName = sName;
            m_iSize = iSize;

            m_pUp = Vector3.Normalize(pPosition);
            Vector3 pAxis = Vector3.Cross(Vector3.Up, m_pUp);
            pAxis.Normalize();
            float fRotAngle = (float)Math.Acos(Vector3.Dot(Vector3.Up, m_pUp));
            worldMatrix = Matrix.CreateScale(m_fScale) * Matrix.CreateRotationY(m_fAngle) * Matrix.CreateFromAxisAngle(pAxis, fRotAngle) * Matrix.CreateTranslation(m_pPosition);

            foreach (ModelMesh mesh in m_pModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Model"];
                    currentEffect.Parameters["xTextureModel"].SetValue(pTexture);
                }
            }
        }
    }
}
