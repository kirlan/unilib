using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace MiscControls
{
	/// <summary>
	/// Control name: Analog Clock Control
	/// Description: A customizable and resizable clock control
	/// Developed by: Syed Mehroz Alam
	/// Email: smehrozalam@yahoo.com
	/// URL: Programming Home "http://www.geocities.com/smehrozalam/"
	/// </summary>
	public class AnalogClock : System.Windows.Forms.UserControl
	{
		const float PI=3.141592654F;

		DateTime dateTime;

		float fRadius;
		float fCenterX;
		float fCenterY;
		float fCenterCircleRadius;

		float fHourLength;
		float fMinLength;
		float fSecLength;

		float fHourThickness;
		float fMinThickness;
		float fSecThickness;

		bool bDraw5MinuteTicks=true;
		bool bDraw1MinuteTicks=true;
        bool bShowMinutesFlow = false;
        bool bDrawSecondHand = false;
        float fTicksThickness = 1;

		Color hrColor=Color.DarkMagenta ;
		Color minColor=Color.Green ;
		Color secColor=Color.Red ;
		Color circleColor=Color.Red;
        private Timer timer1;
        private IContainer components;
        Color ticksColor = Color.Black;
//		private System.ComponentModel.IContainer components;

		public AnalogClock()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                //if(components != null)
                //{
                //    components.Dispose();
                //}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // AnalogClock
            // 
            this.DoubleBuffered = true;
            this.Name = "AnalogClock";
            this.Load += new System.EventHandler(this.AnalogClock_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.AnalogClock_Paint);
            this.Resize += new System.EventHandler(this.AnalogClock_Resize);
            this.ResumeLayout(false);

		}
		#endregion

		private void AnalogClock_Load(object sender, System.EventArgs e)
		{
			//dateTime=DateTime.Now;
			this.AnalogClock_Resize(sender,e);
		}

		private void DrawLine(float fThickness, float fLength, Color color, float fRadians, System.Windows.Forms.PaintEventArgs e)
		{
			e.Graphics.DrawLine(new Pen( color, fThickness ),
				fCenterX - (float)(fLength/9*System.Math.Sin(fRadians)), 
				fCenterY + (float)(fLength/9*System.Math.Cos(fRadians)), 
				fCenterX + (float)(fLength*System.Math.Sin(fRadians)), 
				fCenterY - (float)(fLength*System.Math.Cos(fRadians)));
		}

		private void DrawPolygon(float fThickness, float fLength, Color color, float fRadians, System.Windows.Forms.PaintEventArgs e)
		{

			PointF A=new PointF( (float)(fCenterX+ fThickness*2*System.Math.Sin(fRadians+PI/2)), 
				(float)(fCenterY - fThickness*2*System.Math.Cos(fRadians+PI/2)) );
			PointF B=new PointF( (float)(fCenterX+ fThickness*2*System.Math.Sin(fRadians-PI/2)),
				(float)(fCenterY - fThickness*2*System.Math.Cos(fRadians-PI/2)) );
			PointF C=new PointF( (float)(fCenterX+ fLength*System.Math.Sin(fRadians)), 
				(float) (fCenterY - fLength*System.Math.Cos(fRadians)) );
			PointF D=new PointF( (float)(fCenterX- fThickness*4*System.Math.Sin(fRadians)), 
				(float)(fCenterY + fThickness*4*System.Math.Cos(fRadians) ));
			PointF[] points={A,D,B,C};
			e.Graphics.FillPolygon( new SolidBrush(color), points );
		}

		private void AnalogClock_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			float fRadHr=(dateTime.Hour%12+dateTime.Minute/60F) *30*PI/180;
			float fRadMin=(dateTime.Minute)*6*PI/180;
			float fRadSec=(dateTime.Second)*6*PI/180;

            float fRadius2 = fRadius / 1.35F;
            e.Graphics.DrawEllipse(new Pen(Color.Black), fCenterX - fRadius2 - 1, fCenterY - fRadius2 - 1, 2 * fRadius2 + 2, 2 * fRadius2 + 2);
            e.Graphics.FillEllipse(new SolidBrush(Color.White), fCenterX - fRadius2, fCenterY - fRadius2, 2 * fRadius2, 2 * fRadius2);
            
            DrawPolygon(this.fHourThickness, this.fHourLength, hrColor, fRadHr, e);
            if (bShowMinutesFlow)
                DrawPolygon(this.fMinThickness, this.fMinLength, minColor, fRadMin, e);
            else
                DrawPolygon(this.fMinThickness, this.fMinLength, minColor, 0, e);
            if (bDrawSecondHand)
                DrawLine(this.fSecThickness, this.fSecLength, secColor, fRadSec, e);


			for(int i=0;i<60;i++)
			{
				if ( this.bDraw5MinuteTicks==true && i%5==0 ) // Draw 5 minute ticks
				{
					e.Graphics.DrawLine( new Pen( ticksColor, fTicksThickness ),
						fCenterX + (float)( this.fRadius/1.450F*System.Math.Sin(i*6*PI/180) ) , 
						fCenterY - (float)( this.fRadius/1.450F*System.Math.Cos(i*6*PI/180) ),
						fCenterX + (float)( this.fRadius/1.65F*System.Math.Sin(i*6*PI/180) ), 
						fCenterY - (float)( this.fRadius/1.65F*System.Math.Cos(i*6*PI/180)) );
				}
				else if ( this.bDraw1MinuteTicks==true ) // draw 1 minute ticks
				{
					e.Graphics.DrawLine( new Pen( ticksColor, fTicksThickness ),
						fCenterX + (float) ( this.fRadius/1.50F*System.Math.Sin(i*6*PI/180) ), 
						fCenterY - (float) ( this.fRadius/1.50F*System.Math.Cos(i*6*PI/180) ),
						fCenterX + (float) ( this.fRadius/1.55F*System.Math.Sin(i*6*PI/180) ), 
						fCenterY - (float) ( this.fRadius/1.55F*System.Math.Cos(i*6*PI/180) ) );
				}
			}

			//draw circle at center
			e.Graphics.FillEllipse( new SolidBrush( circleColor ), fCenterX-fCenterCircleRadius/2, fCenterY-fCenterCircleRadius/2, fCenterCircleRadius, fCenterCircleRadius);
		}

		private void AnalogClock_Resize(object sender, System.EventArgs e)
		{
			this.Width = this.Height;
			this.fRadius = this.Height/2;
			this.fCenterX = this.ClientSize.Width/2;
			this.fCenterY = this.ClientSize.Height/2;
			this.fHourLength = (float)this.Height/3/1.65F;
			this.fMinLength = (float)this.Height/3/1.20F;
			this.fSecLength = (float)this.Height/3/1.15F;
			this.fHourThickness = (float)this.Height/100;
			this.fMinThickness = (float)this.Height/150;
			this.fSecThickness = (float)this.Height/200;
			this.fCenterCircleRadius = this.Height/50;
			this.Refresh();
		}

        public int Hours
        {
            get { return dateTime.Hour; }
            set 
            { 
                dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, value, dateTime.Minute, dateTime.Second);
                this.Refresh();
            }
        }

        public int Minutes
        {
            get { return dateTime.Minute; }
            set 
            { 
                dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, value, dateTime.Second);
                this.Refresh();
            }
        }

        private int m_iMinutesAdded = 0;

        public void AddMinutesAnimated(int minutes)
        {
            m_iMinutesAdded += minutes;
            timer1.Enabled = true;
        }

        public void AddMinutes(int minutes)
        {
            int oldDayTime = GetDayTime();
            int hour = dateTime.Hour;
            int day = dateTime.DayOfYear;
            dateTime = dateTime.AddMinutes((double)minutes);
            if(dateTime.Hour != hour || bShowMinutesFlow)
                this.Refresh();

            if (dateTime.DayOfYear != day)
            {
                if (Midnight != null)
                    Midnight(this, new EventArgs());
            }

            int newDayTime = GetDayTime();

            if (oldDayTime != newDayTime)
            {
                switch (newDayTime)
                {
                    case 0:
                        if (GoodMorning != null)
                            GoodMorning(this, new EventArgs());
                        timer1.Interval = 50;
                        break;
                    case 1:
                        if (GoodAfternoon != null)
                            GoodAfternoon(this, new EventArgs());
                        timer1.Interval = 50;
                        break;
                    case 2:
                        if (GoodEvening != null)
                            GoodEvening(this, new EventArgs());
                        timer1.Interval = 50;
                        break;
                    case 3:
                        if (GoodNight != null)
                            GoodNight(this, new EventArgs());
                        timer1.Interval = 10;
                        break;
                }
            }
        }

        public void AddHours(int hours)
        {
            AddMinutes(hours * 60);
        }

        public Color HourHandColor
		{
			get { return this.hrColor; }
			set { this.hrColor=value; }
		}

		public Color MinuteHandColor
		{
			get { return this.minColor; }
			set { this.minColor=value; }
		}

		public Color SecondHandColor
		{
			get { return this.secColor; }
			set { this.secColor=value;
				  this.circleColor=value; }
		}

		public Color TicksColor
		{
			get { return this.ticksColor; }
			set { this.ticksColor=value; }
		}

		public bool Draw1MinuteTicks
		{
			get { return this.bDraw1MinuteTicks; }
			set { this.bDraw1MinuteTicks=value; }
		}

		public bool Draw5MinuteTicks
		{
			get { return this.bDraw5MinuteTicks; }
			set { this.bDraw5MinuteTicks=value; }
		}

        /// <summary>
        /// Показывать ход минутной стрелки.
        /// Если false, то часовая стрелка будет двигаться с цифры на цыфру, а минутная всегда показывать на 12
        /// </summary>
        public bool ShowMinutesFlow
        {
            get { return this.bShowMinutesFlow; }
            set { this.bShowMinutesFlow = value; }
        }

        public bool DrawSecondHand
        {
            get { return this.bDrawSecondHand; }
            set { this.bDrawSecondHand = value; }
        }

        private int m_iMorningHour = 8;
        public int MorningHour
        {
            get { return m_iMorningHour; }
            set { m_iMorningHour = value; }
        }
        private int m_iMorningMinute = 6;
        public int MorningMinute
        {
            get { return m_iMorningMinute; }
            set { m_iMorningMinute = value; }
        }

        private int m_iNoonHour = 13;
        public int NoonHour
        {
            get { return m_iNoonHour; }
            set { m_iNoonHour = value; }
        }
        private int m_iNoonMinute = 0;
        public int NoonMinute
        {
            get { return m_iNoonMinute; }
            set { m_iNoonMinute = value; }
        }

        private int m_iEveningHour = 19;
        public int EveningHour
        {
            get { return m_iEveningHour; }
            set { m_iEveningHour = value; }
        }
        private int m_iEveningMinute = 0;
        public int EveningMinute
        {
            get { return m_iEveningMinute; }
            set { m_iEveningMinute = value; }
        }

        private int m_iNightHour = 0;
        public int NightHour
        {
            get { return m_iNightHour; }
            set { m_iNightHour = value; }
        }
        private int m_iNightMinute = 0;
        public int NightMinute
        {
            get { return m_iNightMinute; }
            set { m_iNightMinute = value; }
        }

        public event EventHandler GoodMorning;

        public event EventHandler GoodAfternoon;

        public event EventHandler GoodEvening;

        public event EventHandler GoodNight;

        /// <summary>
        /// Вычисляет текущее время суток
        /// </summary>
        /// <returns>0 - утро, 1 - день, 2 - вечер, 3 - ночь, 4 - непонятно что</returns>
        private int GetDayTime()
        {
            int morningMin = m_iMorningHour * 60 + m_iMorningMinute;
            int noonMin = m_iNoonHour * 60 + m_iNoonMinute;
            int eveningMin = m_iEveningHour * 60 + m_iEveningMinute;
            int nightMin = m_iNightHour * 60 + m_iNightMinute;

            int currentMin = dateTime.Hour * 60 + dateTime.Minute;

            if (morningMin < noonMin)
            {
                if (currentMin >= morningMin && currentMin < noonMin)
                    return 0;
            }
            else
            {
                if (currentMin >= morningMin || currentMin < noonMin)
                    return 0;
            }

            if (noonMin < eveningMin)
            {
                if (currentMin >= noonMin && currentMin < eveningMin)
                    return 1;
            }
            else
            {
                if (currentMin >= noonMin || currentMin < eveningMin)
                    return 1;
            }

            if (eveningMin < nightMin)
            {
                if (currentMin >= eveningMin && currentMin < nightMin)
                    return 2;
            }
            else
            {
                if (currentMin >= eveningMin || currentMin < nightMin)
                    return 2;
            }

            if (nightMin < morningMin)
            {
                if (currentMin >= nightMin && currentMin < morningMin)
                    return 3;
            }
            else
            {
                if (currentMin >= nightMin || currentMin < morningMin)
                    return 3;
            }

            return 4;
        }

        public event EventHandler Midnight;

        public event EventHandler AnimationFinished;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_iMinutesAdded > 0)
            {
                int inc = m_iMinutesAdded < 3 ? m_iMinutesAdded : 3;
                m_iMinutesAdded -= inc;
                AddMinutes(inc);
            }
            else
            {
                timer1.Enabled = false;
                if (AnimationFinished != null)
                    AnimationFinished(this, new EventArgs());
            }
        }

        public void Stop()
        {
            timer1.Enabled = false;
            m_iMinutesAdded = 0;
        }
	}
}
