using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace MiscControls
{
    /// <summary>
    /// Represents a glass panel control.
    /// </summary>
    [ToolboxBitmap(typeof(System.Windows.Forms.Panel)), ToolboxItem(true), ToolboxItemFilter("System.Windows.Forms"), Description("Raises an event when the user clicks it.")]
    public partial class GlassPanel : Panel
    {
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

        public GlassPanel()
        {
            InitializeComponent();
        
            base.BackColor = Color.Transparent;
            BackColor = Color.Black;
            ForeColor = Color.White;
            OuterBorderColor = Color.White;
            InnerBorderColor = Color.Black;
            ShineColor = Color.White;
            roundCorner = 6;

            RecalcRect();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, false);

            this.SizeChanged += new EventHandler(GlassPanel_SizeChanged);
        }

        public GlassPanel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            base.BackColor = Color.Transparent;
            BackColor = Color.Black;
            ForeColor = Color.White;
            OuterBorderColor = Color.White;
            InnerBorderColor = Color.Black;
            ShineColor = Color.White;
            roundCorner = 6;

            RecalcRect();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, false);

            this.SizeChanged += new EventHandler(GlassPanel_SizeChanged);
        }
        #endregion

        #region " Fields and Properties "

        private Color backColor;
        /// <summary>
        /// Gets or sets the background color of the control.
        /// </summary>
        /// <returns>A <see cref="T:System.Drawing.Color" /> value representing the background color.</returns>
        [DefaultValue(typeof(Color), "Black")]
        public new Color BackColor
        {
            get { return backColor; }
            set
            {
                if (!backColor.Equals(value))
                {
                    backColor = value;

                    RecalcRect();

                    OnBackColorChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the foreground color of the control.
        /// </summary>
        /// <returns>The foreground <see cref="T:System.Drawing.Color" /> of the control.</returns>
        [DefaultValue(typeof(Color), "White")]
        public new Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;

                RecalcRect();
            }
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

                    RecalcRect();

                    if (IsHandleCreated)
                    {
                        Invalidate();
                    }
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

                    RecalcRect();

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

                    RecalcRect();

                    if (IsHandleCreated)
                    {
                        Invalidate();
                    }
                }
            }
        }

        private Color shineColor;
        /// <summary>
        /// Gets or sets the shine color of the control.
        /// </summary>
        /// <returns>A <see cref="T:System.Drawing.Color" /> value representing the shine color.</returns>
        [DefaultValue(typeof(Color), "White"), Category("Appearance"), Description("The shine color of the control.")]
        public Color ShineColor
        {
            get { return shineColor; }
            set
            {
                if (shineColor != value)
                {
                    shineColor = value;

                    RecalcRect();

                    if (IsHandleCreated)
                    {
                        Invalidate();
                    }
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
            //pevent.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            //pevent.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            //pevent.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            //pevent.Graphics.CompositingMode = CompositingMode.SourceOver;

            RecalcRect();

            DrawPanelBackground(pevent.Graphics);
            ////DrawForegroundFromPanel(pevent);

            pevent.Graphics.SmoothingMode = sm;
        }

        /// <summary>
        /// Draws the panel background.
        /// </summary>
        /// <param name="g">The graphics to draw on.</param>
        private void DrawPanelBackground(Graphics g)
        {
            //white border
            g.DrawPath(outerBorderPen, outerBorderPath);

            //content
            g.FillPath(ContentBrush, ContentPath);

            //shine
            if (Enabled)
            {
                g.FillPath(ShineBrush, ShinePath);
            }

            //black border
            g.DrawPath(BorderPen, BorderPath);
        }

        private Panel imagePanel;
        /// <summary>
        /// Draws the foreground from panel.
        /// </summary>
        /// <param name="pevent">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        private void DrawForegroundFromPanel(PaintEventArgs pevent)
        {
            if (imagePanel == null)
            {
                imagePanel = new Panel();
                imagePanel.Parent = new TransparentControl();
                imagePanel.BackColor = Color.Transparent;
            }
            Font font;
            if (Enabled)
            {
                imagePanel.ForeColor = ForeColor;
                font = Font;
            }
            else
            {
                imagePanel.ForeColor = Color.DarkGray;
                font = new Font(Font, FontStyle.Bold);
            }
            imagePanel.Font = font;
            imagePanel.RightToLeft = RightToLeft;
            imagePanel.Padding = Padding;
            imagePanel.Size = Size;
            imagePanel.Text = Text;
            InvokePaint(imagePanel, pevent);
            //foreach (Control comp in Controls)
            //{
            //    //imagePanel.Controls.Add(comp);
            //}
        }

        class TransparentControl : Control
        {
            protected override void OnPaintBackground(PaintEventArgs pevent) { }
            protected override void OnPaint(PaintEventArgs e) { }
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
        /// Handles the SizeChanged event of the GlassButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void GlassPanel_SizeChanged(object sender, EventArgs e)
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
            RecalcRect();
        }

        /// <summary>
        /// Recalcs the rectangles for drawing.
        /// </summary>
        private void RecalcRect()
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
                //                ShineBrush = new LinearGradientBrush(rect2, Color.FromArgb(opacity, shineColor), Color.FromArgb(opacity / 3, shineColor), LinearGradientMode.Vertical);
                ShineBrush = new LinearGradientBrush(rect2, Color.FromArgb(opacity, shineColor), Color.FromArgb(opacity / 3, backColor), LinearGradientMode.Vertical);

                rect2.Height -= 2;
            }
            return rect2;
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

            rect.X++;
            rect.Y++;
            rect.Width -= 2;
            rect.Height -= 2;

            rect2 = rect;
            rect2.Height >>= 1;

            ContentPath = CreateRoundRectangle(rect, rCorner);
            int opacity = 0x7f;
            ContentBrush = new SolidBrush(Color.FromArgb(opacity, backColor));//Enabled ? backColor : Color.Black));
            //ContentBrush = new SolidBrush(backColor);//Enabled ? backColor : Color.Black));
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


    }
}
