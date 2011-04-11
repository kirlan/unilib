using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using nsUniLibControls;

namespace MiscControls
{
    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    /// <summary>
    /// Represents a glass ProgressBar control.
    /// </summary>
    [ToolboxBitmap(typeof(System.Windows.Forms.ProgressBar)), ToolboxItem(true), ToolboxItemFilter("System.Windows.Forms"), Description("Raises an event when the user clicks it.")]
    public partial class GlassProgressBar : ProgressBar
    {
        # region " Global Vareables "
        
        //Image backgroundImage;

        /// <summary>
        /// The ToolTip of the Control.
        /// </summary>
        ToolTip toolTip = new ToolTip();

        /// <summary>
        /// If false, the shine isn't drawn (-> symbolizes an disabled control).
        /// </summary>
        bool drawShine = true;

        # endregion

        # region " Colors "
        private Color m_eColor00 = SystemColors.Control;
        /// <summary>
        /// Backcolor
        /// </summary>
        public Color Color00
        {
            get { return m_eColor00; }
            set
            {
                m_eColor00 = value;
                MyRepaint();
            }
        }

        private Color m_eColor01 = Color.Crimson;
        /// <summary>
        /// Forecolor1
        /// </summary>
        public Color Color01
        {
            get { return m_eColor01; }
            set
            {
                m_eColor01 = value;
                MyRepaint();
            }
        }

        private Color m_eColor02 = Color.Red;
        /// <summary>
        /// Forecolor2
        /// </summary>
        public Color Color02
        {
            get { return m_eColor02; }
            set
            {
                m_eColor02 = value;
                MyRepaint();
            }
        }

        private Color m_eColor03 = Color.OrangeRed;
        /// <summary>
        /// Forecolor3
        /// </summary>
        public Color Color03
        {
            get { return m_eColor03; }
            set
            {
                m_eColor03 = value;
                MyRepaint();
            }
        }

        private Color m_eColor04 = Color.Orange;
        /// <summary>
        /// Forecolor4
        /// </summary>
        public Color Color04
        {
            get { return m_eColor04; }
            set
            {
                m_eColor04 = value;
                MyRepaint();
            }
        }

        private Color m_eColor05 = Color.Gold;
        /// <summary>
        /// Forecolor5
        /// </summary>
        public Color Color05
        {
            get { return m_eColor05; }
            set
            {
                m_eColor05 = value;
                MyRepaint();
            }
        }

        private Color m_eColor06 = Color.Yellow;
        /// <summary>
        /// Forecolor6
        /// </summary>
        public Color Color06
        {
            get { return m_eColor06; }
            set
            {
                m_eColor06 = value;
                MyRepaint();
            }
        }

        private Color m_eColor07 = Color.GreenYellow;
        /// <summary>
        /// Forecolor7
        /// </summary>
        public Color Color07
        {
            get { return m_eColor07; }
            set
            {
                m_eColor07 = value;
                MyRepaint();
            }
        }

        private Color m_eColor08 = Color.Chartreuse;
        /// <summary>
        /// Forecolor8
        /// </summary>
        public Color Color08
        {
            get { return m_eColor08; }
            set
            {
                m_eColor08 = value;
                MyRepaint();
            }
        }

        private Color m_eColor09 = Color.LawnGreen;
        /// <summary>
        /// Forecolor9
        /// </summary>
        public Color Color09
        {
            get { return m_eColor09; }
            set
            {
                m_eColor09 = value;
                MyRepaint();
            }
        }

        private Color m_eColor10 = Color.Lime;
        /// <summary>
        /// Forecolor10
        /// </summary>
        public Color Color10
        {
            get { return m_eColor10; }
            set
            {
                m_eColor10 = value;
                MyRepaint();
            }
        }

        # endregion

        # region " Vareables for Drawing "

        GraphicsPath outerBorderPath;
        GraphicsPath ContentPath;
        GraphicsPath ShinePath;
        GraphicsPath BorderPath;

        LinearGradientBrush ShineBrush;

        Pen outerBorderPen;
        Pen BorderPen;

        Brush ContentBrush;

        Rectangle rect;
        Rectangle rect2;

        # endregion

        #region " Constructors "

        /// <summary>
        /// Initializes a new instance of the <see cref="Glass.GlassButton" /> class.
        /// </summary>
        public GlassProgressBar()
        {
            DoubleBuffered = true;

            InitializeComponent();
            
            timer.Interval = animationLength / framesCount;
            //base.BackColor = Color.Transparent;
            //BackColor = Color.White;
            ForeColor = Color.White;
            OuterBorderColor = Color.Black;
            InnerBorderColor = Color.Black;
            animateGlow = false;
            toolTipText = "";
            roundCorner = 6;

            RecalcRect((float)currentFrame / (framesCount - 1f));

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, false);

            this.SizeChanged += new EventHandler(ProgressBar_SizeChanged);
        }

        #endregion

        #region " Fields and Properties "

        private string m_sToolTip = "";
        public string ToolTip
        {
            get { return m_sToolTip; }
            set
            {
                m_sToolTip = value;
                toolTip1.SetToolTip(this, m_sToolTip);
            }
        }

        public new int Maximum
        {
            get { return base.Maximum; }
            set { base.Maximum = value; SetValue(base.Value, 0); }
        }

        public new int Minimum
        {
            get { return base.Minimum; }
            set { base.Minimum = value; SetValue(base.Value, 0); }
        }

        public new int Value
        {
            get { return base.Value; }
            set { SetValue(value, 0); }
        }

        public void SetValue(int iValue, int iTime)
        {
            if (iValue <= base.Maximum && iValue >= base.Minimum)
            {
                base.Value = iValue;
            }
            else 
            {
                if (iValue > base.Maximum)
                    base.Value = base.Maximum;

                if (iValue < base.Minimum)
                    base.Value = base.Minimum;
            }

//            m_iShouldBeWidth = (int)((float)(panel1.ClientRectangle.Width) / (float)(m_iMaxValue - m_iMinValue) * (float)iValue);

            MyRepaint();
        }

        private Color innerBorderColor;
        /// <summary>
        /// Gets or sets the inner border color of the control.
        /// </summary>
        /// <returns>A <see cref="T:System.Drawing.Color" /> value representing the color of the inner border.</returns>
        [DefaultValue(typeof(Color), "Black"), Category("Appearance"), Description("The inner border color of the control.")]
        public Color InnerBorderColor
        {
            get { return innerBorderColor; }
            set
            {
                if (innerBorderColor != value)
                {
                    innerBorderColor = value;
                    MyRepaint();
                }
            }
        }

        private int roundCorner;
        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        /// <value>The corner radius.</value>
        [DefaultValue(6), Category("Appearance"), Description("The radius of the corners.")]
        public int CornerRadius
        {
            get { return roundCorner; }
            set
            {
                if (roundCorner != value)
                {
                    roundCorner = value;
                    MyRepaint();
                }
            }
        }
        
        string toolTipText;
        /// <summary>
        /// Gets or sets the tool tip text.
        /// </summary>
        /// <value>The tool tip text.</value>
        [DefaultValue(""), Category("Appearance"), Description("The ToolTip-Text of the Progress Bar. Leave blank to not show a ToolTip.")]
        public string ToolTipText
        {
            get { return toolTipText; }
            set
            {
                if (toolTipText != value)
                {
                    toolTipText = value;

                    if (toolTipText.Length > 0)
                        toolTip.SetToolTip(this, toolTipText);

                    if (IsHandleCreated)
                    {
                        Invalidate();
                    }
                }
            }
        }

        private bool animateGlow;
        /// <summary>
        /// Gets or sets a value indicating whether the glow is animated.
        /// </summary>
        /// <value><c>true</c> if glow is animated; otherwise, <c>false</c>.</value>
        [DefaultValue(false), Category("Appearance"), Description("If true the glow is animated.")]
        public bool AnimateGlow
        {
            get { return animateGlow; }
            set
            {
                if (animateGlow != value)
                {
                    animateGlow = value;
                    if (IsHandleCreated)
                    {
                        Invalidate();
                    }
                }
            }
        }

        private Color outerBorderColor;
        /// <summary>
        /// Gets or sets the outer border color of the control.
        /// </summary>
        /// <returns>A <see cref="T:System.Drawing.Color" /> value representing the color of the outer border.</returns>
        [DefaultValue(typeof(Color), "White"), Category("Appearance"), Description("The outer border color of the control.")]
        public Color OuterBorderColor
        {
            get { return outerBorderColor; }
            set
            {
                if (outerBorderColor != value)
                {
                    outerBorderColor = value;
                    MyRepaint();
                }
            }
        }

        #endregion

        #region " Painting "

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)" /> event.
        /// </summary>
        /// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            SmoothingMode sm = pevent.Graphics.SmoothingMode;
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            DrawProgressBarBackground(pevent.Graphics);

            pevent.Graphics.SmoothingMode = sm;
        }

        private void MyRepaint()
        {
            RecalcRect((float)currentFrame / (framesCount - 1f));

            if (IsHandleCreated)
            {
                Invalidate();
            }
        }

        /// <summary>
        /// Draws the component background.
        /// </summary>
        /// <param name="g">The graphics to draw on.</param>
        private void DrawProgressBarBackground(Graphics g)
        {
            //white border
            g.DrawPath(outerBorderPen, outerBorderPath);

            //content
            g.FillPath(ContentBrush, ContentPath);

            //glow
            //if ((isHovered || isAnimating) && !isPressed)
            //{
            //    g.SetClip(GlowClip, CombineMode.Intersect);
            //    g.FillPath(GlowRadialPath, GlowBottomRadial);

            //    g.ResetClip();
            //}

            //shine
            if (drawShine && Enabled)
            {
                g.FillPath(ShineBrush, ShinePath);
            }

            //black border
            g.DrawPath(BorderPen, BorderPath);
        }

        /// <summary>
        /// Creates the round rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="radius">The radius.</param>
        /// <returns></returns>
        private GraphicsPath CreateRoundRectangle(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int l = rectangle.Left;
            int t = rectangle.Top;
            int w = rectangle.Width;
            int h = rectangle.Height;
            int d = radius << 1;

            if (d > 0)
            {
                path.AddArc(l, t, d, d, 180, 90); // topleft
                path.AddLine(l + radius, t, l + w - radius, t); // top
                path.AddArc(l + w - d, t, d, d, 270, 90); // topright
                path.AddLine(l + w, t + radius, l + w, t + h - radius); // right
                path.AddArc(l + w - d, t + h - d, d, d, 0, 90); // bottomright
                path.AddLine(l + w - radius, t + h, l + radius, t + h); // bottom
                path.AddArc(l, t + h - d, d, d, 90, 90); // bottomleft
                path.AddLine(l, t + h - radius, l, t + radius); // left
            }
            else
            {
                path.AddRectangle(rectangle);
            }

            path.CloseFigure();

            return path;
        }

        /// <summary>
        /// Creates the top round rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="radius">The radius.</param>
        /// <returns></returns>
        private GraphicsPath CreateTopRoundRectangle(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int l = rectangle.Left;
            int t = rectangle.Top;
            int w = rectangle.Width;
            int h = rectangle.Height;
            int d = radius << 1;

            if (d > 0)
            {
                path.AddArc(l, t, d, d, 180, 90); // topleft
                path.AddLine(l + radius, t, l + w - radius, t); // top
                path.AddArc(l + w - d, t, d, d, 270, 90); // topright
                path.AddLine(l + w, t + radius, l + w, t + h); // right
                path.AddLine(l + w, t + h, l, t + h); // bottom
                path.AddLine(l, t + h, l, t + radius); // left
            }
            else
            {
                path.AddRectangle(rectangle);
            }

            path.CloseFigure();

            return path;
        }

        /// <summary>
        /// Creates the bottom radial path.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns></returns>
        private GraphicsPath CreateBottomRadialPath(Rectangle rectangle)
        {
            GraphicsPath path = new GraphicsPath();
            RectangleF rect = rectangle;
            rect.X -= rectangle.Width * .35f;
            rect.Y -= rectangle.Height * .15f;
            rect.Width *= 1.7f;
            rect.Height *= 2.3f;
            path.AddEllipse(rect);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Handles the SizeChanged event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ProgressBar_SizeChanged(object sender, EventArgs e)
        {
            //SuspendLayout();

            //if (this.BackgroundImage != null && this.Height > 0 && this.Width > 0)
            //{
            //    if (this.backgroundImage == null)
            //        this.backgroundImage = this.BackgroundImage;

            //    Bitmap renderBmp = new Bitmap(this.Width, this.Height,
            //        System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            //    Graphics g = Graphics.FromImage(renderBmp);
            //    g.DrawImage(this.backgroundImage, 0, 0, this.Width, this.Height);
            //    this.BackgroundImage = renderBmp;
            //    g.Dispose();
            //}

            //ResumeLayout(); 
            RecalcRect((float)currentFrame / (framesCount - 1f));
        }

        /// <summary>
        /// Recalcs the rectangles for drawing.
        /// </summary>
        /// <param name="progress">The animation progress.</param>
        private void RecalcRect(float progress)
        {
            try
            {
                int rCorner = roundCorner;

                if (roundCorner > Height / 2)
                    rCorner = Height / 2;

                if (roundCorner > Width / 2)
                    rCorner = Width / 2;

                rect = RecalcOuterBorder();

                rect = RecalcContent(rect, out rect2);

                rect2 = RecalcShine(rect2);

                BorderPath = CreateRoundRectangle(rect, rCorner);

                BorderPen = new Pen(innerBorderColor);
            }
            catch { }
        }

        /// <summary>
        /// Recalcs the shine.
        /// </summary>
        /// <param name="rect2">The rect2.</param>
        /// <returns></returns>
        private Rectangle RecalcShine(Rectangle rect2)
        {
            KColor shineColor = new KColor();
            shineColor.RGB = baseColor;
            shineColor.Lightness = 1.0;// -(1.0 - shineColor.Lightness) / 2.0;

            int rCorner = roundCorner;

            if (roundCorner > Height / 2)
                rCorner = Height / 2;

            if (roundCorner > Width / 2)
                rCorner = Width / 2;

            if (rect2.Width > 0 && rect2.Height > 0)
            {
                rect2.Height++;
                ShinePath = CreateTopRoundRectangle(rect2, rCorner);

                rect2.Height++;
                int opacity = 0x99;
                //if (isPressed)
                //    opacity = (int)(.4f * opacity + .5f);
//                ShineBrush = new LinearGradientBrush(rect2, Color.FromArgb(opacity, shineColor), Color.FromArgb(opacity / 3, shineColor), LinearGradientMode.Vertical);
                ShineBrush = new LinearGradientBrush(rect2, Color.FromArgb(opacity, shineColor.RGB), Color.FromArgb(opacity / 3, baseColor), LinearGradientMode.Vertical);

                rect2.Height -= 2;

                drawShine = true;
            }
            else
                drawShine = false;
            return rect2;
        }

        private Color baseColor = Color.Black;

        private Orientation m_eOrientation;

        /// <summary>
        /// Gets or sets the orientation of the control.
        /// </summary>
        /// <returns>An <see cref="T:MiscControls.Orientation" /> value representing the direction of progress bar.</returns>
        [DefaultValue(typeof(Orientation), "Horizontal"), Category("Appearance"), Description("The orientation of the control.")]
        public Orientation Orientation
        {
            get { return m_eOrientation; }
            set 
            { 
                if (m_eOrientation != value)
                {
                    m_eOrientation = value;
                    MyRepaint();
                }
            }
        }

        /// <summary>
        /// Recalcs the content.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="rect2">The rect2.</param>
        /// <returns></returns>
        private Rectangle RecalcContent(Rectangle rect, out Rectangle rect2)
        {
            int rCorner = roundCorner;

            if (roundCorner > Height / 2)
                rCorner = Height / 2;

            if (roundCorner > Width / 2)
                rCorner = Width / 2;

            int rWidth = rCorner * 2;
            if (rect.Width <= rWidth)
                rWidth = rect.Width / 2;

            int rHeight = rCorner * 2;
            if (rect.Height <= rHeight)
                rHeight = rect.Height / 2;

            int iShouldBeValue = 0;
            double fIntervalLength = 0;
            if (m_eOrientation == Orientation.Horizontal)
            {
                iShouldBeValue = (int)((float)(rect.Width - rWidth) / (float)(base.Maximum - base.Minimum) * (float)base.Value);
                fIntervalLength = (double)rect.Width / 9.0;
            }
            else
            {
                iShouldBeValue = (int)((float)(rect.Height - rHeight) / (float)(base.Maximum - base.Minimum) * (float)base.Value);
                fIntervalLength = (double)rect.Height / 9.0;
            }
            int iIntervalNumber = (int)((float)iShouldBeValue / fIntervalLength);
            int iDelta = iShouldBeValue - (int)(iIntervalNumber * fIntervalLength);

            KColor color1 = new KColor();
            KColor color2 = new KColor();
            switch (iIntervalNumber)
            {
                case 0:
                    color1.RGB = m_eColor01;
                    color2.RGB = m_eColor02;
                    break;
                case 1:
                    color1.RGB = m_eColor02;
                    color2.RGB = m_eColor03;
                    break;
                case 2:
                    color1.RGB = m_eColor03;
                    color2.RGB = m_eColor04;
                    break;
                case 3:
                    color1.RGB = m_eColor04;
                    color2.RGB = m_eColor05;
                    break;
                case 4:
                    color1.RGB = m_eColor05;
                    color2.RGB = m_eColor06;
                    break;
                case 5:
                    color1.RGB = m_eColor06;
                    color2.RGB = m_eColor07;
                    break;
                case 6:
                    color1.RGB = m_eColor07;
                    color2.RGB = m_eColor08;
                    break;
                case 7:
                    color1.RGB = m_eColor08;
                    color2.RGB = m_eColor09;
                    break;
                case 8:
                    color1.RGB = m_eColor09;
                    color2.RGB = m_eColor10;
                    break;
                case 9:
                    color1.RGB = m_eColor10;
                    color2.RGB = m_eColor10;
                    break;
            }

            KColor color3 = new KColor();
            if (Math.Abs(color2.Hue - color1.Hue) < 180)
                color3.Hue = color1.Hue + (int)((float)(color2.Hue - color1.Hue) * (float)iDelta / fIntervalLength);
            else
            {
                if (color2.Hue > color1.Hue)
                    color3.Hue = color1.Hue + (int)((float)(color2.Hue - color1.Hue - 360) * (float)iDelta / fIntervalLength);
                else
                    color3.Hue = color1.Hue + (int)((float)(color2.Hue - color1.Hue + 360) * (float)iDelta / fIntervalLength);
            }
            color3.Lightness = color1.Lightness + (int)((float)(color2.Lightness - color1.Lightness) * (float)iDelta / fIntervalLength);
            color3.Saturation = color1.Saturation + (int)((float)(color2.Saturation - color1.Saturation) * (float)iDelta / fIntervalLength);

            baseColor = color3.RGB;

            if (m_eOrientation == Orientation.Horizontal)
            {
                rect.X += 1;
                rect.Y += 1;
                rect.Width = rWidth + iShouldBeValue - 2;
                rect.Height -= 2;
            }
            else
            {
                rect.X += 1;
                rect.Y += rect.Height - (rHeight + iShouldBeValue - 1);
                rect.Width -= 2;
                rect.Height = rHeight + iShouldBeValue - 2;
            }

            rect2 = rect;
            rect2.Height >>= 1;

            ContentPath = CreateRoundRectangle(rect, rCorner);
            int opacity = /*isPressed ? 0xcc :*/ 0x7f;
            ContentBrush = new SolidBrush(Color.FromArgb(opacity, Enabled ? baseColor : Color.Black));
            return rect;
        }

        /// <summary>
        /// Recalcs the outer border.
        /// </summary>
        /// <returns></returns>
        private Rectangle RecalcOuterBorder()
        {
            int rCorner = roundCorner;

            if (roundCorner > Height / 2)
                rCorner = Height / 2;

            if (roundCorner > Width / 2)
                rCorner = Width / 2;

            Rectangle rect;
            rect = ClientRectangle;
            rect.Width--;
            rect.Height--;
            outerBorderPath = CreateRoundRectangle(rect, rCorner);
            rect.Inflate(1, 1);
            GraphicsPath region = CreateRoundRectangle(rect, rCorner);
            this.Region = new Region(region);
            rect.Inflate(-1, -1);

            Color col = outerBorderColor;

            outerBorderPen = new Pen(col);
            return rect;
        }

        #endregion

        #region " Animation Support "

        private const int animationLength = 300;
        private const int framesCount = 10;
        private int currentFrame;
        private int direction;

        private bool isAnimating
        {
            get
            {
                return direction != 0;
            }
        }

        private void FadeIn()
        {
            direction = 1;
            timer.Enabled = true;
        }

        private void FadeOut()
        {
            direction = -1;
            timer.Enabled = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!timer.Enabled || !animateGlow)
            {
                return;
            }

            MyRepaint();

            currentFrame += direction;
            if (currentFrame == -1)
            {
                currentFrame = 0;
                timer.Enabled = false;
                direction = 0;
                return;
            }
            if (currentFrame == framesCount)
            {
                currentFrame = framesCount - 1;
                timer.Enabled = false;
                direction = 0;
            }
        }

        #endregion
    }
}
