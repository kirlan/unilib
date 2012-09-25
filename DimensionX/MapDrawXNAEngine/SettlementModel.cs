using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LandscapeGeneration;

namespace MapDrawXNAEngine
{
    internal class SettlementModel
    {
        public readonly Vector3 m_pPosition;
        public readonly Vector3 m_pUp;
        public readonly float m_fAngle;
        public readonly Model m_pModel;
        public readonly float m_fScale;
        public readonly Matrix worldMatrix;
        public readonly string m_sName;
        public readonly int m_iSize;

        public SettlementModel(Vector3 pPosition, float fAngle, float fScale, Model pModel, WorldShape eWorldShape, Texture2D pTexture, string sName, int iSize)
        {
            m_pPosition = pPosition;
            m_fAngle = fAngle;
            m_fScale = fScale;
            m_pModel = pModel;
            m_sName = sName;
            m_iSize = iSize;

            m_pUp = Vector3.Up; 
            
            if (eWorldShape == WorldShape.Ringworld)
            {
                m_pUp = Vector3.Transform(-Vector3.Normalize(m_pPosition), Matrix.CreateScale(1, 1, 0));
                Vector3 pAxis = Vector3.Cross(Vector3.Up, m_pUp);
                pAxis.Normalize();
                float fRotAngle = (float)Math.Acos(Vector3.Dot(Vector3.Up, m_pUp));
                worldMatrix = Matrix.CreateScale(m_fScale) * Matrix.CreateRotationY(m_fAngle) * Matrix.CreateFromAxisAngle(pAxis, fRotAngle) * Matrix.CreateTranslation(m_pPosition);
            }
            else if (eWorldShape == WorldShape.Planet)
            {
                m_pUp = Vector3.Normalize(pPosition);
                Vector3 pAxis = Vector3.Cross(Vector3.Up, m_pUp);
                pAxis.Normalize();
                float fRotAngle = (float)Math.Acos(Vector3.Dot(Vector3.Up, m_pUp));
                worldMatrix = Matrix.CreateScale(m_fScale) * Matrix.CreateRotationY(m_fAngle) * Matrix.CreateFromAxisAngle(pAxis, fRotAngle) * Matrix.CreateTranslation(m_pPosition);
            }
            else
                worldMatrix = Matrix.CreateScale(m_fScale) * Matrix.CreateRotationY(m_fAngle) * Matrix.CreateTranslation(m_pPosition);

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
