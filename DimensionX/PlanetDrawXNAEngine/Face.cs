﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Socium;
using LandscapeGeneration.PlanetBuilder;

namespace PlanetDrawXNAEngine
{
    public class Face
    {
        public Square[] m_aSquares;

        public Face(GraphicsDevice pDevice, CubeFace<LocationX> pFace, float fR, float fMaxHeight, Model[] treeModel, Model[] palmModel, Model[] pineModel, Texture2D pTreeTexture)
        {
            m_aSquares = new Square[pFace.Resolution * pFace.Resolution];

            int index = 0;
            foreach (var chunk in pFace.m_cChunk)
            {
                m_aSquares[index++] = new Square(pDevice, chunk, fR, fMaxHeight, treeModel, palmModel, pineModel, pTreeTexture);
            }
        }
    }
}
