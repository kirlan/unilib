﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TestCubePlanet
{
    class Face
    {
        public Square[] m_aSquares;

        public Face(GraphicsDevice pDevice, CubeFace pFace)
        {
            m_aSquares = new Square[pFace.Resolution * pFace.Resolution];

            int index = 0;
            foreach (var chunk in pFace.m_cChunk)
            {
                m_aSquares[index++] = new Square(pDevice, chunk, pFace.Resolution);
            }
        }
    }
}
