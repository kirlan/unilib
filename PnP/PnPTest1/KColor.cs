using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PnPTest1
{
    public class KColor
    {
        private int m_iRed = 0;
        /// <summary>
        /// Red component of RGB (0..255)
        /// </summary>
        public int Red
        {
            get { return m_iRed; }
            set { m_iRed = value; Convert2HLS(); }
        }

        private int m_iGreen = 0;
        /// <summary>
        /// Green component of RGB (0..255)
        /// </summary>
        public int Green
        {
            get { return m_iGreen; }
            set { m_iGreen = value; Convert2HLS(); }
        }

        private int m_iBlue = 0;
        /// <summary>
        /// Blue component of RGB (0..255)
        /// </summary>
        public int Blue
        {
            get { return m_iBlue; }
            set { m_iBlue = value; Convert2HLS(); }
        }

        private double m_fHue = 0;
        /// <summary>
        /// Hue component of HSL (0..360)
        /// </summary>
        public double Hue
        {
            get { return m_fHue; }
            set 
            { 
                m_fHue = value;
                while (m_fHue > 360)
                    m_fHue -= 360;
                while (m_fHue < 0)
                    m_fHue += 360;
                Convert2RGB(); 
            }
        }

        private double m_fSaturation = 0;
        /// <summary>
        /// Saturation component of HSL (0..1)
        /// </summary>
        public double Saturation
        {
            get { return m_fSaturation; }
            set 
            { 
                m_fSaturation = Math.Min(Math.Max(value, 0), 1); 
                Convert2RGB(); 
            }
        }

        private double m_fLightness = 0;
        /// <summary>
        /// Lightness component of HSL (0..1)
        /// </summary>
        public double Lightness
        {
            get { return m_fLightness; }
            set 
            {
                m_fLightness = Math.Min(Math.Max(value, 0), 1);
                Convert2RGB(); 
            }
        }

        public Color RGB
        {
            get
            {
                Convert2RGB();
                return Color.FromArgb(m_iRed, m_iGreen, m_iBlue);
            }
            set
            {
                m_iRed = value.R;
                m_iGreen = value.G;
                m_iBlue = value.B;
                Convert2HLS();
            }
        }

        private void Convert2HLS()
        {
            double mn, mx;
            int major = 0;

            if (m_iRed < m_iGreen)
            {
                mn = m_iRed;
                mx = m_iGreen;
                major = 2;// Green;
            }
            else
            {
                mn = m_iGreen;
                mx = m_iRed;
                major = 1;// Red;
            }

            if (m_iBlue < mn)
            {
                mn = m_iBlue;
            }
            else
            {
                if (m_iBlue > mx)
                {
                    mx = m_iBlue;
                    major = 3;// Blue;
                }
            }

            if (mn == mx)
            {
                m_fLightness = mn / 255;
                m_fSaturation = 0;
                m_fHue = 0;
            }
            else
            {
                m_fLightness = (mn + mx) / 510;

                if (m_fLightness <= 0.5)
                {
                    m_fSaturation = (mx - mn) / (mn + mx);
                }
                else
                {
                    m_fSaturation = (mx - mn) / (510 - mn - mx);
                }

                switch (major)
                {
                    case 1:
                        m_fHue = (m_iGreen - m_iBlue) * 60 / (mx - mn) + 360;
                        break;
                    case 2:
                        m_fHue = (m_iBlue - m_iRed) * 60 / (mx - mn) + 120;
                        break;
                    case 3:
                        m_fHue = (m_iRed - m_iGreen) * 60 / (mx - mn) + 240;
                        break;
                }

                if (m_fHue >= 360)
                {
                    m_fHue = m_fHue - 360;
                }
            }
        }

        private void Convert2RGB()
        { 
	        if(m_fSaturation == 0)
	        {
		        m_iRed   = (byte)(m_fLightness*255);
		        m_iGreen = (byte)(m_fLightness*255);
		        m_iBlue  = (byte)(m_fLightness*255);
	        }
	        else
	        {
		        double m1, m2;

                if (m_fLightness <= 0.5)
		        {
                    m2 = m_fLightness + m_fLightness * m_fSaturation;
		        }
		        else
		        {
                    m2 = m_fLightness + m_fSaturation - m_fLightness * m_fSaturation;
		        }

                m1 = 2 * m_fLightness - m2;

                m_iRed = Value(m1, m2, m_fHue + 120);
                m_iGreen = Value(m1, m2, m_fHue);
                m_iBlue = Value(m1, m2, m_fHue - 120);
	        }
        }

        private byte Value(double m1, double m2, double h)
        {
	        if(h >= 360)
	        {
		        h-= 360;
	        }
	        else
	        {
		        if(h < 0)
		        {
			        h+= 360;
		        }
	        }

	        if(h < 60)
	        {
		        m1 = m1 + (m2 - m1)*h/60;
	        }
	        else
	        {
		        if(h < 180)
		        {
			        m1 = m2;
		        }
		        else
		        {
			        if(h < 240)
			        {
				        m1 = m1 + (m2 - m1)*(240 - h)/60;
			        }
		        }
	        }

	        return (byte)(m1*255);
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}-{2} [{3}-{4}-{5}]", m_iRed, m_iGreen, m_iBlue, m_fHue, m_fLightness, m_fSaturation);
        }
    }
}
