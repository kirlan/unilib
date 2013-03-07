using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace TestCubePlanet
{
    public class TreeModel
    {
        public readonly Model m_pModel;
        public readonly Matrix worldMatrix;
        public Matrix[] boneTransforms;

        public static int Size { get { return sizeof(float)*16; } }

        public void Save(BinaryWriter bw)
        {
            bw.Write(worldMatrix.M11);
            bw.Write(worldMatrix.M12);
            bw.Write(worldMatrix.M13);
            bw.Write(worldMatrix.M14);
            bw.Write(worldMatrix.M21);
            bw.Write(worldMatrix.M22);
            bw.Write(worldMatrix.M23);
            bw.Write(worldMatrix.M24);
            bw.Write(worldMatrix.M31);
            bw.Write(worldMatrix.M32);
            bw.Write(worldMatrix.M33);
            bw.Write(worldMatrix.M34);
            bw.Write(worldMatrix.M41);
            bw.Write(worldMatrix.M42);
            bw.Write(worldMatrix.M43);
            bw.Write(worldMatrix.M44);
        }

        public TreeModel(BinaryReader br, Model pModel, Texture2D pTexture)
        {
            m_pModel = pModel;

            worldMatrix = new Matrix(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

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

        public TreeModel(Vector3 pPosition, float fAngle, float fScale, Model pModel, Texture2D pTexture)
        {
            m_pModel = pModel;

            Vector3 pUp = Vector3.Normalize(pPosition);
            Vector3 pAxis = Vector3.Cross(Vector3.Up, pUp);
            pAxis.Normalize();
            float fRotAngle = (float)Math.Acos(Vector3.Dot(Vector3.Up, pUp));
            worldMatrix = Matrix.CreateScale(fScale * 10) * Matrix.CreateRotationY(fAngle) * Matrix.CreateFromAxisAngle(pAxis, fRotAngle) * Matrix.CreateTranslation(pPosition);

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
            return string.Format("{0}:{1}:{2}", worldMatrix.Translation.X, worldMatrix.Translation.Y, worldMatrix.Translation.Z);
        }
    }
}
