using System;
using System.Drawing;
using System.Windows.Forms;
using OpenGL;

namespace myOpenGL
{
    public partial class Form1 : Form
    {
        cOGL cGL;
        Panel panel1;
        Timer timer1;
        private Button lightButton1;
        private Button lightButton2;
        private Button btnRedLight;
        private Button btnYellowLight;
        private Button btnGreenLight;
        private Button btnAutoLight;
        private Label lblTrafficStatus;
        private Button colorBtn;

        //  לשליטה בצל
        private HScrollBar hScrollBar11; // מיקום האור X
        private HScrollBar hScrollBar12; // מיקום האור Y
        private HScrollBar hScrollBar13; // מיקום האור Z

        public Form1()
        {
            InitializeComponent();

            this.Text = "Parking Lot Project";
            this.ClientSize = new Size(950, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            panel1 = new Panel();
            panel1.Location = new Point(20, 20);
            panel1.Size = new Size(740, 740);
            panel1.BackColor = Color.Black;
            panel1.Paint += panel1_Paint;
            panel1.Resize += panel1_Resize;
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseMove += panel1_MouseMove;
            panel1.MouseWheel += panel1_MouseWheel;

            this.Controls.Add(panel1);

            GroupBox controls = new GroupBox();
            controls.Text = "Controls";
            controls.Location = new Point(770, 20);
            controls.Size = new Size(150, 500);
            this.Controls.Add(controls);

            // כפתורי תנועה
            Button upBtn = new Button() { Text = "↑", Location = new Point(50, 20), Size = new Size(40, 30) };
            Button downBtn = new Button() { Text = "↓", Location = new Point(50, 100), Size = new Size(40, 30) };
            Button leftBtn = new Button() { Text = "←", Location = new Point(10, 60), Size = new Size(40, 30) };
            Button rightBtn = new Button() { Text = "→", Location = new Point(90, 60), Size = new Size(40, 30) };

            upBtn.Click += (s, e) => { cGL.redCarY += 1; panel1.Invalidate(); };
            downBtn.Click += (s, e) => { cGL.redCarY -= 1; panel1.Invalidate(); };
            leftBtn.Click += (s, e) => { cGL.redCarX -= 1; panel1.Invalidate(); };
            rightBtn.Click += (s, e) => { cGL.redCarX += 1; panel1.Invalidate(); };

            // כפתור שער
            Button gateBtn = new Button()
            {
                Text = "פתח/סגור שער",
                Location = new Point(10, 140),
                Size = new Size(120, 30),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            gateBtn.Click += (s, e) =>
            {
                cGL.ToggleGate();
                panel1.Invalidate();
            };

            // כפתורי תאורה
            lightButton1 = new Button()
            {
                Text = "תאורה ראשית: דלוקה",
                Location = new Point(10, 180),
                Size = new Size(120, 30),
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 8, FontStyle.Bold)
            };

            lightButton2 = new Button()
            {
                Text = "תאורה משנית: דלוקה",
                Location = new Point(10, 220),
                Size = new Size(120, 30),
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 8, FontStyle.Bold)
            };

            lightButton1.Click += (s, e) =>
            {
                cGL.light1On = !cGL.light1On;
                UpdateLightButton1();
                panel1.Invalidate();
            };

            lightButton2.Click += (s, e) =>
            {
                cGL.light2On = !cGL.light2On;
                UpdateLightButton2();
                panel1.Invalidate();
            };

            // כפתורי רכב
            Button carLightsBtn = new Button()
            {
                Text = "אורות רכב",
                Location = new Point(10, 260),
                Size = new Size(120, 30)
            };
            carLightsBtn.Click += (s, e) =>
            {
                cGL.carLightsOn = !cGL.carLightsOn;
                cGL.carBlinking = false;
                panel1.Invalidate();
            };

            Button carBlinkBtn = new Button()
            {
                Text = "הבהוב",
                Location = new Point(10, 300),
                Size = new Size(120, 30)
            };
            carBlinkBtn.Click += (s, e) =>
            {
                cGL.carBlinking = !cGL.carBlinking;
                if (cGL.carBlinking)
                    cGL.carLightsOn = true;
                panel1.Invalidate();
            };

            // כפתורי רמזור
            btnRedLight = new Button()
            {
                Text = "אדום",
                Location = new Point(10, 340),
                Size = new Size(60, 25),
                Font = new Font("Arial", 8, FontStyle.Bold)
            };

            btnYellowLight = new Button()
            {
                Text = "צהוב",
                Location = new Point(75, 340),
                Size = new Size(60, 25),
                Font = new Font("Arial", 8, FontStyle.Bold)
            };

            btnGreenLight = new Button()
            {
                Text = "ירוק",
                Location = new Point(10, 370),
                Size = new Size(60, 25),
                Font = new Font("Arial", 8, FontStyle.Bold)
            };

            btnAutoLight = new Button()
            {
                Text = "אוטו",
                Location = new Point(75, 370),
                Size = new Size(60, 25),
                Font = new Font("Arial", 8, FontStyle.Bold),
                BackColor = Color.LightBlue
            };

            lblTrafficStatus = new Label()
            {
                Text = "רמזור: אוטומטי",
                Location = new Point(10, 400),
                Size = new Size(120, 15),
                Font = new Font("Arial", 8, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter
            };

            btnRedLight.Click += (s, e) =>
            {
                cGL.SetTrafficLightState(0);
                lblTrafficStatus.Text = "רמזור: אדום";
                UpdateTrafficButtons(0);
                panel1.Invalidate();
            };

            btnYellowLight.Click += (s, e) =>
            {
                cGL.SetTrafficLightState(1);
                lblTrafficStatus.Text = "רמזור: צהוב";
                UpdateTrafficButtons(1);
                panel1.Invalidate();
            };

            btnGreenLight.Click += (s, e) =>
            {
                cGL.SetTrafficLightState(2);
                lblTrafficStatus.Text = "רמזור: ירוק";
                UpdateTrafficButtons(2);
                panel1.Invalidate();
            };

            btnAutoLight.Click += (s, e) =>
            {
                cGL.EnableAutoTrafficLight();
                lblTrafficStatus.Text = "רמזור: אוטומטי";
                UpdateTrafficButtons(-1);
                panel1.Invalidate();
            };

            colorBtn = new Button()
            {
                Text = "צבעים רנדומליים",
                Location = new Point(10, 430),
                Size = new Size(120, 25),
                Font = new Font("Arial", 8, FontStyle.Bold)
            };

            colorBtn.Click += (s, e) =>
            {
                cGL.isColored = !cGL.isColored;
                colorBtn.BackColor = cGL.isColored ? Color.LightGreen : SystemColors.Control;
                panel1.Invalidate();
            };

            // הוספת כל הכפתורים לקבוצה
            controls.Controls.AddRange(new Control[] {
                upBtn, downBtn, leftBtn, rightBtn,
                gateBtn, lightButton1, lightButton2,
                carLightsBtn, carBlinkBtn,
                btnRedLight, btnYellowLight, btnGreenLight, btnAutoLight, lblTrafficStatus,
                colorBtn
            });

            //   לשליטה בצל
            hScrollBar11 = new HScrollBar()
            {
                Location = new Point(770, 540),
                Size = new Size(150, 20),
                Minimum = 0,
                Maximum = 200,
                Value = 150 
            };

            hScrollBar12 = new HScrollBar()
            {
                Location = new Point(770, 580),
                Size = new Size(150, 20),
                Minimum = 0,
                Maximum = 200,
                Value = 50   
            };

            hScrollBar13 = new HScrollBar()
            {
                Location = new Point(770, 620),
                Size = new Size(150, 20),
                Minimum = 100,
                Maximum = 300,
                Value = 200  
            };
            

            //  הסבר
            Label lblLightX = new Label()
            {
                Text = "מיקום האור X",
                Location = new Point(770, 520),
                Size = new Size(150, 15),
                Font = new Font("Arial", 8)
            };

            Label lblLightY = new Label()
            {
                Text = "מיקום האור Y",
                Location = new Point(770, 560),
                Size = new Size(150, 15),
                Font = new Font("Arial", 8)
            };

            Label lblLightZ = new Label()
            {
                Text = "גובה האור Z",
                Location = new Point(770, 600),
                Size = new Size(150, 15),
                Font = new Font("Arial", 8)
            };

           
            hScrollBar11.Scroll += hScrollBar11_Scroll;
            hScrollBar12.Scroll += hScrollBar12_Scroll;
            hScrollBar13.Scroll += hScrollBar13_Scroll;

            this.Controls.AddRange(new Control[] {
                hScrollBar11, hScrollBar12, hScrollBar13, 
                lblLightX, lblLightY, lblLightZ, 
            });

            cGL = new cOGL(panel1);
            // סנכרון ערכי התחלה  
            hScrollBar11_Scroll(hScrollBar11, null);
            hScrollBar12_Scroll(hScrollBar12, null);
            hScrollBar13_Scroll(hScrollBar13, null);


            timer1 = new Timer();
            timer1.Interval = 50;
            timer1.Tick += timer1_Tick;
            timer1.Start();
        }

        // פונקציות עזר לעדכון כפתורי התאורה
        private void UpdateLightButton1()
        {
            if (cGL.light1On)
            {
                lightButton1.Text = "תאורה ראשית: דלוקה";
                lightButton1.BackColor = Color.LightGreen;
            }
            else
            {
                lightButton1.Text = "תאורה ראשית: כבויה";
                lightButton1.BackColor = Color.LightCoral;
            }
        }

        private void UpdateLightButton2()
        {
            if (cGL.light2On)
            {
                lightButton2.Text = "תאורה משנית: דלוקה";
                lightButton2.BackColor = Color.LightGreen;
            }
            else
            {
                lightButton2.Text = "תאורה משנית: כבויה";
                lightButton2.BackColor = Color.LightCoral;
            }
        }

        private void UpdateTrafficButtons(int activeState)
        {
            btnRedLight.BackColor = SystemColors.Control;
            btnYellowLight.BackColor = SystemColors.Control;
            btnGreenLight.BackColor = SystemColors.Control;
            btnAutoLight.BackColor = SystemColors.Control;

            switch (activeState)
            {
                case 0: btnRedLight.BackColor = Color.LightCoral; break;
                case 1: btnYellowLight.BackColor = Color.LightYellow; break;
                case 2: btnGreenLight.BackColor = Color.LightGreen; break;
                default: btnAutoLight.BackColor = Color.LightBlue; break;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e) => cGL.Draw();
        private void panel1_Resize(object sender, EventArgs e) => cGL.OnResize();

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (cGL.autoTrafficLight)
            {
                string[] states = { "אדום", "צהוב", "ירוק" };
                lblTrafficStatus.Text = "רמזור: אוטומטי - " + states[cGL.trafficLightState];
                UpdateTrafficButtons(-1);
            }

            cGL.Draw();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                cGL.mouseDownPoint = new Point(e.X, e.Y);
                cGL.lastMouseX = e.X;
                cGL.lastMouseY = e.Y;
            }
        }

        

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int dx = e.X - cGL.lastMouseX;
                int dy = e.Y - cGL.lastMouseY;

                if (Control.ModifierKeys == Keys.Control)
                {
                    if (!cGL.isPanning)
                    {
                        cGL.isPanning = true;
                        cGL.isDragging = false;
                    }
                    cGL.panX += dx * 0.01f;
                    cGL.panY -= dy * 0.01f;
                }
                else
                {
                    if (!cGL.isDragging)
                    {
                        cGL.isDragging = true;
                        cGL.isPanning = false;
                    }
                    cGL.zAngle += dx * 0.5f;
                    cGL.xAngle += dy * 0.5f;

                    cGL.xAngle = Math.Max(-65, Math.Min(15, cGL.xAngle));
                }

                cGL.lastMouseX = e.X;
                cGL.lastMouseY = e.Y;
                panel1.Invalidate();
            }
        }

        private void panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                cGL.zoomDistance -= 0.5f;
            }
            else
            {
                cGL.zoomDistance += 0.5f;
            }

            cGL.zoomDistance = Math.Max(6.0f, Math.Min(25.0f, cGL.zoomDistance));
            panel1.Invalidate();
        }

        //    לשליטה בצל
        private void hScrollBar11_Scroll(object sender, ScrollEventArgs e)
        {
            HScrollBar hb = (HScrollBar)sender;
            cGL.pos[0] = (hb.Value - 100) / 10.0f; // מיקום X של האור
            cGL.Draw();
        }

        private void hScrollBar12_Scroll(object sender, ScrollEventArgs e)
        {
            HScrollBar hb = (HScrollBar)sender;
            cGL.pos[1] = (hb.Value - 100) / 10.0f; // מיקום Y של האור
            cGL.Draw();
        }

        private void hScrollBar13_Scroll(object sender, ScrollEventArgs e)
        {
            HScrollBar hb = (HScrollBar)sender;
            cGL.pos[2] = (hb.Value - 100) / 10.0f; // גובה Z של האור
            cGL.Draw();
        }

        private void hScrollBar14_Scroll(object sender, ScrollEventArgs e)
        {
            HScrollBar hb = (HScrollBar)sender;
            float groundHeight = (hb.Value - 100) / 10.0f;

            // עדכון גובה הקרקע לצל
            cGL.ground[0, 2] = groundHeight;
            cGL.ground[1, 2] = groundHeight;
            cGL.ground[2, 2] = groundHeight;

            cGL.Draw();
        }
    }
}