using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LandscapeGeneration;

namespace MapDrawXNAEngine
{
    protected class TreeModel
    {
        public readonly Vector3 m_pPosition;
        public readonly Model m_pModel;
        public readonly Matrix worldMatrix;
        public Matrix[] boneTransforms;

        public TreeModel(Vector3 pPosition, float fAngle, float fScale, Model pModel, WorldShape eWorldShape, Texture2D pTexture)
        {
            m_pPosition = pPosition;
            m_pModel = pModel;

            if (eWorldShape == WorldShape.Ringworld)
            {
                Vector3 pUp = Vector3.Transform(-Vector3.Normalize(m_pPosition), Matrix.CreateScale(1, 1, 0));
                //                    Vector3 pUp = -Vector3.Normalize(m_pPosition);
                Vector3 pAxis = Vector3.Cross(Vector3.Up, pUp);
                pAxis.Normalize();
                float fRotAngle = (float)Math.Acos(Vector3.Dot(Vector3.Up, pUp));
                worldMatrix = Matrix.CreateScale(fScale * 10) * Matrix.CreateRotationY(fAngle) * Matrix.CreateFromAxisAngle(pAxis, fRotAngle) * Matrix.CreateTranslation(pPosition);
            }
            else if (eWorldShape == WorldShape.Planet)
            {
                Vector3 pUp = Vector3.Normalize(m_pPosition);
                Vector3 pAxis = Vector3.Cross(Vector3.Up, pUp);
                pAxis.Normalize();
                float fRotAngle = (float)Math.Acos(Vector3.Dot(Vector3.Up, pUp));
                worldMatrix = Matrix.CreateScale(fScale * 10) * Matrix.CreateRotationY(fAngle) * Matrix.CreateFromAxisAngle(pAxis, fRotAngle) * Matrix.CreateTranslation(pPosition);
            }
            else
                worldMatrix = Matrix.CreateScale(fScale * 10) * Matrix.CreateRotationY(fAngle) * Matrix.CreateTranslation(pPosition);

            boneTransforms = new Matrix[m_pModel.Bones.Count];
            m_pModel.CopyAbsoluteBoneTransformsTo(boneTransforms);
            foreach (ModelMesh mesh in m_pModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Tree"];
                    currentEffect.Parameters["xTextureModel"].SetValue(pTexture);
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}:{2}", m_pPosition.X, m_pPosition.Y, m_pPosition.Z);
        }
    }
}
