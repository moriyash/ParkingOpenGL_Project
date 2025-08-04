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
        private Button projectionBtn;
        private HScrollBar hScrollBar11;
        private HScrollBar hScrollBar12;
        private HScrollBar hScrollBar13;
        private HScrollBar lightIntensityBar, lightRadiusBar;
        private CheckBox texturesCheckBox;

        public Form1()
        {
            InitializeComponent();

            this.Text = "Parking Project";
            this.ClientSize = new Size(950, 885); 
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

            CreateControls();

            cGL = new cOGL(panel1);
            cGL.InitializeCarRoutes();

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            timer1 = new Timer();
            timer1.Interval = 50;
            timer1.Tick += timer1_Tick;
            timer1.Start();
        }
        private void CreateControls()
        {
            GroupBox mainControls = new GroupBox();
            mainControls.Text = "Controls";
            mainControls.Location = new Point(785, 10);
            mainControls.Size = new Size(140, 780);
            this.Controls.Add(mainControls);

            // lighting box
            GroupBox lightingBox = new GroupBox()
            {
                Text = "Light",
                Location = new Point(5, 15),
                Size = new Size(130, 90)
            };

            lightButton1 = new Button()
            {
                Text = "Light",
                Location = new Point(5, 15),
                Size = new Size(90, 20),
            };
            lightButton1.Click += (s, e) =>
            {
                cGL.light1On = !cGL.light1On;
                cGL.light2On = cGL.light1On;
                if (!cGL.light1On)
                    cGL.pos[2] = -100.0f;//off
                else
                    cGL.pos[2] = 15.0f; //height
                UpdateLightButton1();
                cGL.Draw();
            };

            Label lblLightPosX = new Label() { Text = "X:", Location = new Point(5, 40), Size = new Size(12, 12) };
            HScrollBar lightPosX = new HScrollBar()
            {
                Location = new Point(20, 40),
                Size = new Size(70, 12),
                Minimum = -12,
                Maximum = 150,
                Value = 0,
                SmallChange = 1,
                LargeChange = 5
            };
            Label lblLightPosXVal = new Label() { Text = "0.0", Location = new Point(95, 40), Size = new Size(30, 12) };

            Label lblLightPosY = new Label() { Text = "Y:", Location = new Point(5, 55), Size = new Size(12, 12) };
            HScrollBar lightPosY = new HScrollBar()
            {
                Location = new Point(20, 55),
                Size = new Size(70, 12),
                Minimum = -28,
                Maximum = 150,
                Value = 0,
                SmallChange = 1,
                LargeChange = 5
            };
            Label lblLightPosYVal = new Label() { Text = "0.0", Location = new Point(95, 55), Size = new Size(30, 12) };

            float maxXY = 100.0f;

            lightPosX.Scroll += (s, e) =>
            {
                float value = lightPosX.Value / 10.0f;
                value = Math.Max(-maxXY, Math.Min(maxXY, value));
                cGL.pos[0] = value;
                lblLightPosXVal.Text = value.ToString("F1");
            };

            lightPosY.Scroll += (s, e) =>
            {
                float value = lightPosY.Value / 10.0f;
                value = Math.Max(-maxXY, Math.Min(maxXY, value));
                cGL.pos[1] = value;
                lblLightPosYVal.Text = value.ToString("F1");
            };

            lightingBox.Controls.AddRange(new Control[] {
        lightButton1, lblLightPosX, lightPosX, lblLightPosXVal,
        lblLightPosY, lightPosY, lblLightPosYVal
    });

            // objects
            GroupBox objectsBox = new GroupBox()
            {
                Text = "Objects",
                Location = new Point(5, 110),
                Size = new Size(130, 115)
            };

            ComboBox objSelect = new ComboBox()
            {
                Location = new Point(5, 15),
                Size = new Size(120, 18),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            objSelect.Items.AddRange(new string[] { "Car 1", "Gate", "Traffic Light" });
            objSelect.SelectedIndex = 0;

            Label lblScale = new Label() { Text = "Size", Location = new Point(5, 38), Size = new Size(25, 12) };
            HScrollBar scaleBar = new HScrollBar()
            {
                Location = new Point(30, 38),
                Size = new Size(60, 12),
                Minimum = 50,
                Maximum = 200,
                Value = 100
            };
            Label lblScaleVal = new Label()
            {
                Text = "1.0",
                Location = new Point(95, 38),
                Size = new Size(30, 12)
            };

            Label lblRot = new Label() { Text = "Rotate", Location = new Point(5, 53), Size = new Size(40, 12) };
            HScrollBar rotBar = new HScrollBar()
            {
                Location = new Point(50, 53),
                Size = new Size(70, 12),
                Minimum = 0,
                Maximum = 360,
                Value = 0
            };

            Label lblPosX = new Label() { Text = "X:", Location = new Point(5, 68), Size = new Size(12, 12) };
            HScrollBar posBarX = new HScrollBar()
            {
                Location = new Point(20, 68),
                Size = new Size(60, 12),
                Minimum = -100,
                Maximum = 100,
                Value = 0
            };
            Label lblPosXVal = new Label() { Text = "0.0", Location = new Point(85, 68), Size = new Size(30, 12) };

            Label lblPosY = new Label() { Text = "Y:", Location = new Point(5, 83), Size = new Size(12, 12) };
            HScrollBar posBarY = new HScrollBar()
            {
                Location = new Point(20, 83),
                Size = new Size(60, 12),
                Minimum = -100,
                Maximum = 100,
                Value = 0
            };
            Label lblPosYVal = new Label() { Text = "0.0", Location = new Point(85, 83), Size = new Size(30, 12) };

            Label lblPosZ = new Label() { Text = "Z:", Location = new Point(5, 98), Size = new Size(12, 12) };
            HScrollBar posBarZ = new HScrollBar()
            {
                Location = new Point(20, 98),
                Size = new Size(60, 12),
                Minimum = -100,
                Maximum = 100,
                Value = 0
            };
            Label lblPosZVal = new Label() { Text = "0.0", Location = new Point(85, 98), Size = new Size(30, 12) };

            scaleBar.Scroll += (s, e) =>
            {
                float scale = scaleBar.Value / 100.0f;
                lblScaleVal.Text = scale.ToString("0.0");
                ApplyObjectScale(objSelect.SelectedIndex, scale);
            };
            rotBar.Scroll += (s, e) => ApplyObjectRotation(objSelect.SelectedIndex, rotBar.Value);

            posBarX.Scroll += (s, e) =>
            {
                float posX = posBarX.Value / 10.0f;
                lblPosXVal.Text = posX.ToString("F1");
                int index = objSelect.SelectedIndex;
                if (index >= 0 && index < cGL.objectPositions.Length)
                {
                    var pos = cGL.objectPositions[index];
                    pos.X = posX;
                    cGL.objectPositions[index] = pos;
                }
                cGL.Draw();
            };

            posBarY.Scroll += (s, e) =>
            {
                float posY = posBarY.Value / 10.0f;
                lblPosYVal.Text = posY.ToString("F1");
                int index = objSelect.SelectedIndex;
                if (index >= 0 && index < cGL.objectPositions.Length)
                {
                    var pos = cGL.objectPositions[index];
                    pos.Y = posY;
                    cGL.objectPositions[index] = pos;
                }
                cGL.Draw();
            };

            posBarZ.Scroll += (s, e) =>
            {
                float posZ = posBarZ.Value / 10.0f;
                lblPosZVal.Text = posZ.ToString("F1");
                int index = objSelect.SelectedIndex;
                if (index >= 0 && index < cGL.objectPositions.Length)
                {
                    var pos = cGL.objectPositions[index];
                    pos.Z = posZ;
                    cGL.objectPositions[index] = pos;
                }
                cGL.Draw();
            };

            objectsBox.Controls.AddRange(new Control[] {
        objSelect, lblScale, scaleBar, lblScaleVal, lblRot, rotBar,
        lblPosX, posBarX, lblPosXVal, lblPosY, posBarY, lblPosYVal,
        lblPosZ, posBarZ, lblPosZVal
    });

            // textures
            GroupBox texturesBox = new GroupBox()
            {
                Text = "Textures",
                Location = new Point(5, 230),
                Size = new Size(130, 65)
            };

            texturesCheckBox = new CheckBox()
            {
                Text = "Enable Textures",
                Location = new Point(5, 15),
                Size = new Size(120, 15),
                Checked = true
            };
            texturesCheckBox.CheckedChanged += (s, e) =>
            {
                cGL.texturesEnabled = texturesCheckBox.Checked;
                cGL.Draw();
            };

            ComboBox materialBox = new ComboBox()
            {
                Location = new Point(5, 35),
                Size = new Size(120, 18),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            materialBox.Items.AddRange(new string[] { "Gold", "Emerald", "Chrome" });
            materialBox.SelectedIndex = 0;
            materialBox.SelectedIndexChanged += (s, e) => ApplyMaterial(materialBox.SelectedIndex);

            texturesBox.Controls.AddRange(new Control[] { texturesCheckBox, materialBox });

            // animation
            GroupBox animationBox = new GroupBox()
            {
                Text = "Animation",
                Location = new Point(5, 300),
                Size = new Size(130, 90)
            };

            Button car1Btn = new Button()
            {
                Text = "Start Car 1",
                Location = new Point(5, 15),
                Size = new Size(58, 25),
            };
            car1Btn.Click += (s, e) => cGL.StartCar(0);

            Button car2Btn = new Button()
            {
                Text = "Start Car 2",
                Location = new Point(67, 15),
                Size = new Size(58, 25),
            };
            car2Btn.Click += (s, e) => cGL.StartCar(1);

            Label lblSpeed = new Label()
            {
                Text = "Speed",
                Location = new Point(5, 45),
                Size = new Size(30, 15)
            };

            HScrollBar speedBar = new HScrollBar()
            {
                Location = new Point(40, 45),
                Size = new Size(80, 15),
                Minimum = 5,
                Maximum = 50,
                Value = 10
            };
            speedBar.Scroll += (s, e) => { cGL.carSpeed = speedBar.Value / 10.0f; };

            Button gateBtn = new Button()
            {
                Text = "Gate Open/Close",
                Location = new Point(5, 65),
                Size = new Size(115, 20),
            };
            gateBtn.Click += (s, e) => cGL.ToggleGate();

            animationBox.Controls.AddRange(new Control[] { car1Btn, car2Btn, lblSpeed, speedBar, gateBtn });

            // view control
            GroupBox viewBox = new GroupBox()
            {
                Text = "View Control",
                Location = new Point(5, 395),
                Size = new Size(130, 70)
            };

            projectionBtn = new Button()
            {
                Text = "Perspective",
                Location = new Point(5, 15),
                Size = new Size(120, 25),
            };
            projectionBtn.Click += (s, e) =>
            {
                cGL.ToggleProjectionMode();
                projectionBtn.Text = cGL.isOrthogonal ? "Orthogonal" : "Perspective";
                projectionBtn.BackColor = cGL.isOrthogonal ? Color.LightBlue : SystemColors.Control;
            };

            Label lblZoom = new Label() { Text = "Zoom", Location = new Point(5, 45), Size = new Size(30, 15) };
            HScrollBar zoomBar = new HScrollBar()
            {
                Location = new Point(40, 45),
                Size = new Size(80, 15),
                Minimum = 60,
                Maximum = 250,
                Value = 90
            };
            zoomBar.Scroll += (s, e) => { cGL.zoomDistance = zoomBar.Value / 10.0f; cGL.Draw(); };

            viewBox.Controls.AddRange(new Control[] { projectionBtn, lblZoom, zoomBar });

            // global lights
            GroupBox globalLightBox = new GroupBox()
            {
                Text = "Global Light",
                Location = new Point(5, 480),
                Size = new Size(130, 85)
            };

            Label lblAmbient = new Label()
            {
                Text = "Ambient",
                Location = new Point(5, 20),
                Size = new Size(45, 15)
            };

            HScrollBar ambientBar = new HScrollBar()
            {
                Location = new Point(55, 20),
                Size = new Size(65, 15),
                Minimum = 0,
                Maximum = 100,
                Value = 20
            };
            ambientBar.Scroll += (s, e) =>
            {
                float intensity = ambientBar.Value / 100.0f;
                cGL.SetGlobalAmbient(intensity);
            };

            RadioButton radioSunOn = new RadioButton()
            {
                Text = "Off",
                Location = new Point(5, 45),
                Size = new Size(55, 18),
                Checked = true
            };

            RadioButton radioSunOff = new RadioButton()
            {
                Text = "On",
                Location = new Point(65, 45),
                Size = new Size(55, 18)
            };

            radioSunOn.CheckedChanged += (s, e) =>
            {
                if (radioSunOn.Checked)
                {
                    cGL.sunEnabled = true;
                    cGL.ToggleSun();
                    cGL.Draw();
                }
            };

            radioSunOff.CheckedChanged += (s, e) =>
            {
                if (radioSunOff.Checked)
                {
                    cGL.sunEnabled = false;
                    cGL.ToggleSun();
                    cGL.Draw();
                }
            };

            globalLightBox.Controls.AddRange(new Control[]
            {
    lblAmbient, ambientBar, radioSunOn, radioSunOff
            });

            // scene action
            GroupBox sceneBox = new GroupBox()
            {
                Text = "Scene Actions",
                Location = new Point(5, 575),
                Size = new Size(130, 145)
            };

            Button resetBtn = new Button()
            {
                Text = "Reset",
                Location = new Point(5, 15),
                Size = new Size(50, 20),
            };

            Label lblRotation = new Label()
            {
                Text = "Rotation",
                Location = new Point(5, 40),
                Size = new Size(50, 12),
            };

            Label lblRotX = new Label() { Text = "X", Location = new Point(5, 55), Size = new Size(12, 15) };
            NumericUpDown rotXControl = new NumericUpDown()
            {
                Location = new Point(18, 55),
                Size = new Size(35, 15),
                Minimum = -180,
                Maximum = 180,
                Value = -15,
                DecimalPlaces = 1
            };

            Label lblRotY = new Label() { Text = "Y", Location = new Point(68, 55), Size = new Size(12, 15) };
            NumericUpDown rotYControl = new NumericUpDown()
            {
                Location = new Point(81, 55),
                Size = new Size(35, 15),
                Minimum = -180,
                Maximum = 180,
                Value = 0,
                DecimalPlaces = 1
            };

            Label lblRotZ = new Label() { Text = "Z", Location = new Point(5, 75), Size = new Size(12, 15) };
            NumericUpDown rotZControl = new NumericUpDown()
            {
                Location = new Point(18, 75),
                Size = new Size(35, 15),
                Minimum = -180,
                Maximum = 180,
                Value = 10,
                DecimalPlaces = 1
            };

            Label lblTranslation = new Label()
            {
                Text = "Translation",
                Location = new Point(68, 40),
                Size = new Size(55, 12),
            };

            Label lblTransX = new Label() { Text = "X", Location = new Point(68, 75), Size = new Size(12, 15) };
            NumericUpDown transXControl = new NumericUpDown()
            {
                Location = new Point(81, 75),
                Size = new Size(35, 15),
                Minimum = -20,
                Maximum = 20,
                Value = 0,
                DecimalPlaces = 1,
                Increment = 0.1m
            };

            Label lblTransY = new Label() { Text = "Y", Location = new Point(5, 95), Size = new Size(12, 15) };
            NumericUpDown transYControl = new NumericUpDown()
            {
                Location = new Point(18, 95),
                Size = new Size(35, 15),
                Minimum = -20,
                Maximum = 20,
                Value = 0,
                DecimalPlaces = 1,
                Increment = 0.1m
            };

            Label lblTransZ = new Label() { Text = "Z", Location = new Point(68, 95), Size = new Size(12, 15) };
            NumericUpDown transZControl = new NumericUpDown()
            {
                Location = new Point(81, 95),
                Size = new Size(35, 15),
                Minimum = 5,
                Maximum = 25,
                Value = 9,
                DecimalPlaces = 1,
                Increment = 0.1m
            };

            resetBtn.Click += (s, e) =>
            {
                cGL.xAngle = -15.0f; cGL.zAngle = 10.0f; cGL.panX = 0f; cGL.panY = 0f; cGL.zoomDistance = 9.0f;
                rotXControl.Value = (decimal)cGL.xAngle; rotZControl.Value = (decimal)cGL.zAngle;
                transXControl.Value = (decimal)cGL.panX; transYControl.Value = (decimal)cGL.panY; transZControl.Value = (decimal)cGL.zoomDistance;
                cGL.Draw();
            };

            rotXControl.ValueChanged += (s, e) => { cGL.xAngle = (float)rotXControl.Value; cGL.Draw(); };

            rotZControl.ValueChanged += (s, e) => { cGL.zAngle = (float)rotZControl.Value; cGL.Draw(); };
            transXControl.ValueChanged += (s, e) => { cGL.panX = (float)transXControl.Value; cGL.Draw(); };
            transYControl.ValueChanged += (s, e) => { cGL.panY = (float)transYControl.Value; cGL.Draw(); };
            transZControl.ValueChanged += (s, e) => { cGL.zoomDistance = (float)transZControl.Value; cGL.Draw(); };

            sceneBox.Controls.AddRange(new Control[] {
    resetBtn, lblRotation, lblRotX, rotXControl, lblRotY, rotYControl, lblRotZ, rotZControl,
    lblTranslation, lblTransX, transXControl, lblTransY, transYControl, lblTransZ, transZControl
});

            // file
            GroupBox fileBox = new GroupBox()
            {
                Text = "File",
                Location = new Point(5, 730),
                Size = new Size(130, 45)
            };

            Button saveBtn = new Button()
            {
                Text = "Save Scene",
                Location = new Point(5, 15),
                Size = new Size(58, 20),
            };
            Button loadBtn = new Button()
            {
                Text = "Load Scene",
                Location = new Point(67, 15),
                Size = new Size(58, 20),
            };

            saveBtn.Click += (s, e) => SaveScene();
            loadBtn.Click += (s, e) => LoadScene();

            fileBox.Controls.AddRange(new Control[] { saveBtn, loadBtn });

            
            mainControls.Controls.AddRange(new Control[] {
    lightingBox, objectsBox, texturesBox, animationBox,
    viewBox, globalLightBox, sceneBox, fileBox
});
        }
        private void ApplyObjectScale(int objIndex, float scale)
        {
            switch (objIndex)
            {
                case 0: 
                    break; 
                case 1: // Gate
                    cGL.gateScale = scale;
                    break;
                case 2: // Traffic Light
                    cGL.trafficLightScale = scale;
                    break;
            }
            cGL.Draw();
        }
        private void ApplyObjectRotation(int objIndex, float rotation)
        {
            switch (objIndex)
            {
                case 0: // Car 1
                    cGL.carRotationAngle[0] = rotation;
                    break;
                case 1: // Gate
                    cGL.gateRotation = rotation;
                    break;
                case 2: // Traffic Light
                    cGL.trafficLightRotation = rotation;
                    break;
            }
            cGL.Draw();
        }   
        private void ApplyMaterial(int materialIndex)
        {
            string[] materialTypes = { "gold", "emerald", "chrome" };

            if (materialIndex >= 0 && materialIndex < materialTypes.Length)
            {
                cGL.SetCurrentMaterial(materialTypes[materialIndex]);
                cGL.Draw();
            }
        }
        private void SaveScene()
        {
            SaveFileDialog dlg = new SaveFileDialog()
            {
                Filter = "Parking Files (*.parking)|*.parking",
                DefaultExt = "parking"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (SceneManager.SaveScene(cGL, dlg.FileName, "3D Scene"))
                        MessageBox.Show("Scene saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving scene: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void LoadScene()
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "Parking Files (*.parking)|*.parking"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (SceneManager.LoadScene(cGL, dlg.FileName))
                        MessageBox.Show("Scene loaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading scene: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void UpdateLightButton1()
        {
            lightButton1.Text = cGL.light1On ? "On" : "Off";
          
        }
        private void panel1_Paint(object sender, PaintEventArgs e) => cGL.Draw();
        private void panel1_Resize(object sender, EventArgs e) => cGL.OnResize();
        private void timer1_Tick(object sender, EventArgs e)
        {
            cGL.UpdateCars();
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
            }
        }
        private void panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                cGL.zoomDistance -= 0.5f;
            else
                cGL.zoomDistance += 0.5f;

            cGL.zoomDistance = Math.Max(6.0f, Math.Min(25.0f, cGL.zoomDistance));
            cGL.Draw();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            float moveSpeed = 0.3f;

            switch (e.KeyCode)
            {
                case Keys.Left:
                    cGL.panX -= moveSpeed;
                    break;
                case Keys.Right:
                    cGL.panX += moveSpeed;
                    break;
                case Keys.Up:
                    cGL.panY += moveSpeed;
                    break;
                case Keys.Down:
                    cGL.panY -= moveSpeed;
                    break;
                case Keys.P:
                    cGL.ToggleProjectionMode();
                    projectionBtn.Text = cGL.isOrthogonal ? "Orthogonal" : "Perspective";
                    projectionBtn.BackColor = cGL.isOrthogonal ? Color.LightBlue : SystemColors.Control;
                    break;
                case Keys.T:
                    cGL.texturesEnabled = !cGL.texturesEnabled;
                    texturesCheckBox.Checked = cGL.texturesEnabled;
                    break;
            }

            cGL.Draw();
            e.Handled = true;
        }
    }
    
}