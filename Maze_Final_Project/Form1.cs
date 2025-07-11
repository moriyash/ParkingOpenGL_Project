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




        //מאתחל את הטופס, הכפתורים, הפאנל, הפונקציות והטיימר.
        public Form1()
        {
            InitializeComponent();

            this.Text = "Parking Project";
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
            controls.Size = new Size(150, 700);
            this.Controls.Add(controls);


            // כפתור שער
            Button gateBtn = new Button()
            {
                Text = "close/open gate",
                Location = new Point(10, 140),
                Size = new Size(120, 30),
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            gateBtn.Click += (s, e) =>
            {
                cGL.ToggleGate();
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
            };

            lightButton2.Click += (s, e) =>
            {
                cGL.light2On = !cGL.light2On;
                UpdateLightButton2();
            };

            // כפתורי רכב
            Button carLightsBtn = new Button()
            {
                Text = "light car",
                Location = new Point(10, 260),
                Size = new Size(120, 30)
            };
            carLightsBtn.Click += (s, e) =>
            {
                cGL.carLightsOn = !cGL.carLightsOn;
                cGL.carBlinking = false;
            };

            Button carBlinkBtn = new Button()
            {
                Text = "blink car",
                Location = new Point(10, 300),
                Size = new Size(120, 30)
            };
            carBlinkBtn.Click += (s, e) =>
            {
                cGL.carBlinking = !cGL.carBlinking;
                if (cGL.carBlinking)
                    cGL.carLightsOn = true;
            };

            // כפתורי רמזור
            btnRedLight = new Button()
            {
                Text = "red",
                Location = new Point(10, 340),
                Size = new Size(60, 25),
                Font = new Font("Arial", 8, FontStyle.Bold)
            };

            btnYellowLight = new Button()
            {
                Text = "yellow",
                Location = new Point(75, 340),
                Size = new Size(60, 25),
                Font = new Font("Arial", 8, FontStyle.Bold)
            };

            btnGreenLight = new Button()
            {
                Text = "green",
                Location = new Point(10, 370),
                Size = new Size(60, 25),
                Font = new Font("Arial", 8, FontStyle.Bold)
            };

            btnAutoLight = new Button()
            {
                Text = "car",
                Location = new Point(75, 370),
                Size = new Size(60, 25),
                Font = new Font("Arial", 8, FontStyle.Bold),
                BackColor = Color.LightBlue
            };

            lblTrafficStatus = new Label()
            {
                Text = "auto",
                Location = new Point(10, 400),
                Size = new Size(120, 15),
                Font = new Font("Arial", 8, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter
            };

            btnRedLight.Click += (s, e) =>
            {
                cGL.SetTrafficLightState(0);
                lblTrafficStatus.Text = "red traffic light";
                UpdateTrafficButtons(0);
            };

            btnYellowLight.Click += (s, e) =>
            {
                cGL.SetTrafficLightState(1);
                lblTrafficStatus.Text = "yellow traffic light";
                UpdateTrafficButtons(1);
            };

            btnGreenLight.Click += (s, e) =>
            {
                cGL.SetTrafficLightState(2);
                lblTrafficStatus.Text = "green traffic light";
                UpdateTrafficButtons(2);
            };

            btnAutoLight.Click += (s, e) =>
            {
                cGL.EnableAutoTrafficLight();
                lblTrafficStatus.Text = "auto traffic light";
                UpdateTrafficButtons(-1);
            };

            colorBtn = new Button()
            {
                Text = "random colors",
                Location = new Point(10, 430),
                Size = new Size(120, 25),
                Font = new Font("Arial", 8, FontStyle.Bold)
            };

            colorBtn.Click += (s, e) =>
            {
                cGL.isColored = !cGL.isColored;
                colorBtn.BackColor = cGL.isColored ? Color.LightGreen : SystemColors.Control;
            };

            // Labels לשליטה בצל
            Label lblLightZ = new Label()
            {
                Text = "גובה האור Z",
                Location = new Point(10, 460),
                Size = new Size(120, 15),
                Font = new Font("Arial", 8)
            };

            Label lblLightX = new Label()
            {
                Text = "מיקום האור X",
                Location = new Point(10, 500),
                Size = new Size(120, 15),
                Font = new Font("Arial", 8)
            };

            Label lblLightY = new Label()
            {
                Text = "מיקום האור Y",
                Location = new Point(10, 540),
                Size = new Size(120, 15),
                Font = new Font("Arial", 8)
            };

            // HScrollBars לשליטה בצל
            hScrollBar13 = new HScrollBar()
            {
                Location = new Point(10, 480),
                Size = new Size(120, 15),
                Minimum = 100,
                Maximum = 300,
                Value = 200
            };

            hScrollBar11 = new HScrollBar()
            {
                Location = new Point(10, 520),
                Size = new Size(120, 15),
                Minimum = 0,
                Maximum = 200,
                Value = 150
            };

            hScrollBar12 = new HScrollBar()
            {
                Location = new Point(10, 560),
                Size = new Size(120, 15),
                Minimum = 0,
                Maximum = 200,
                Value = 50
            };

            // חיבור אירועי הגלילה
            hScrollBar11.Scroll += hScrollBar11_Scroll;
            hScrollBar12.Scroll += hScrollBar12_Scroll;
            hScrollBar13.Scroll += hScrollBar13_Scroll;

            // הוספת כל הפקדים לקבוצה
            controls.Controls.AddRange(new Control[] {
                gateBtn, lightButton1, lightButton2,
                carLightsBtn, carBlinkBtn,
                btnRedLight, btnYellowLight, btnGreenLight, btnAutoLight, lblTrafficStatus,
                colorBtn,
                lblLightZ, hScrollBar13, lblLightX, hScrollBar11, lblLightY, hScrollBar12
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

        //מעדכן את טקסט וצבע כפתור תאורה 1 לפי מצב הדלקה
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
        //מעדכן את טקסט וצבע כפתור תאורה 2 לפי מצב הדלקה
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


        //מעדכן את צבעי כפתורי הרמזור לפי מצב נבחר אדום/צהוב/ירוק/אוטומטי
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


        //קורא לפונקציית Draw() של cGL לצייר את הסצנה בזמן Paint
        private void panel1_Paint(object sender, PaintEventArgs e) => cGL.Draw();


       // קורא ל-OnResize() של cGL להתאים תצוגה כשגודל הפאנל משתנה
        private void panel1_Resize(object sender, EventArgs e) => cGL.OnResize();


        //רענון הרמזור
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (cGL.autoTrafficLight)
            {
                string[] states = { "red", "yellow", "green" };
                lblTrafficStatus.Text = "auto traffic light " + states[cGL.trafficLightState];
                UpdateTrafficButtons(-1);
            }

            cGL.Draw();
        }


        //שומר נקודת לחיצה של העכבר לתחילת גרירה או תזוזה
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                cGL.mouseDownPoint = new Point(e.X, e.Y);
                cGL.lastMouseX = e.X;
                cGL.lastMouseY = e.Y;
            }
        }
        //מבצע סיבוב מצלמה או הזזה (Pan) לפי תזוזת עכבר
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
            }
        }
        //משנה את הזום (מרחק המצלמה) בגלגלת העכבר
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
        }



        //סרגלי גלילה לשליטת אור וצל

        //משנה את מיקום האור בציר X
        private void hScrollBar11_Scroll(object sender, ScrollEventArgs e)
        {
            HScrollBar hb = (HScrollBar)sender;
            cGL.pos[0] = (hb.Value - 100) / 10.0f; // מיקום X של האור
        }
        //משנה את מיקום האור בציר Y
        private void hScrollBar12_Scroll(object sender, ScrollEventArgs e)
        {
            HScrollBar hb = (HScrollBar)sender;
            cGL.pos[1] = (hb.Value - 100) / 10.0f; // מיקום Y של האור
        }
        //משנה את גובה האור בציר Z
        private void hScrollBar13_Scroll(object sender, ScrollEventArgs e)
        {
            HScrollBar hb = (HScrollBar)sender;
            cGL.pos[2] = (hb.Value - 100) / 10.0f; // גובה Z של האור
        }
        //משנה את גובה הקרקע לצל
        private void hScrollBar14_Scroll(object sender, ScrollEventArgs e)
        {
            HScrollBar hb = (HScrollBar)sender;
            float groundHeight = (hb.Value - 100) / 10.0f;

            // עדכון גובה הקרקע לצל
            cGL.ground[0, 2] = groundHeight;
            cGL.ground[1, 2] = groundHeight;
            cGL.ground[2, 2] = groundHeight;

            //cGL.Draw();
        }
    }
}