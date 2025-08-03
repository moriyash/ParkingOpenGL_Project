using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Milkshape;
using System.Collections.Generic;
namespace OpenGL
{
    public partial class cOGL
    {
        Control p;
        int Width, Height;
        GLUquadric obj;
        Milkshape.Character carModel;
        public bool isOrthogonal = false;

        public bool texturesEnabled = true;

        public bool isPanning = false;
        public float panX = 0f, panY = 0f;
        public Point mouseDownPoint;

        public bool light1On = true;
        public bool light2On = true;
        public uint[] Textures = new uint[20];

        public uint[] SkyboxTextures = new uint[6];

        public bool carLightsOn = false;
        public bool carBlinking = false;

        public float zoomDistance = 9.0f;
        public float yShift = 0.0f;
        public float xShift = -28.0f;

        public float zAngle = 10.0f;
        public float yAngle = 10.0f;
        public float xAngle = -15.0f;

        public bool isDragging = false;
        public int lastMouseX, lastMouseY;

        public float skyboxOffsetX = 0.0f;
        public float skyboxOffsetY = 0.0f;

        public int trafficLightState = 0;
        public float trafficLightTimer = 0.0f;
        public bool autoTrafficLight = true;

        public bool isColored = false;
        public bool isSmooth = false;


        public float light1Intensity = 3.0f;
        public float light2Intensity = 3.0f;

        public bool gateOpen = false;
        public float gateAnimation = 0.0f;
        public bool gateMoving = false;

        public bool doorOpen = false;
        public float[,] ground = new float[3, 3]
                          {
                                 { 1, 1, 0 },
                                 { 0, 1, 0 },
                                 { 1, 0, 0 }
                             };

   

        float[] cubeXform = new float[16];
        public float[] pos = new float[4] { 0.0f, 0.0f, 300.0f, 1.0f }; // X, Y, Z, W

        public float[] ScrollValue = new float[14];

        public int intOptionB = 1;
        public int intOptionC = 0;
        double[] AccumulatedRotationsTraslations = new double[16];


        public float[] carCurrentX = new float[4];
        public float[] carCurrentY = new float[4];
        public float[] carCurrentAngle = new float[4] { 90f, 90f, 90f, 90f };
        public float[] carRotationAngle = new float[4] { 180f, 180f, 90f, 90f };
        public bool[] carIsMoving = new bool[4];


        public float[,] carRouteX = new float[4, 10];
        public float[,] carRouteY = new float[4, 10];
        public float[,] carRouteAngle = new float[4, 10];
        public int[] carRouteLength = new int[4];

        public int[] carCurrentWaypoint = new int[4];
        public float[] carProgress = new float[4];

        public float carSpeed = 1.0f;

        public float[] light1Color = new float[3] { 1.0f, 1.0f, 1.0f };
        public float[] light2Color = new float[3] { 1.0f, 1.0f, 1.0f };
        public float light1Radius = 2.0f;
        public float light2Radius = 2.0f;

        public bool shadowsEnabled = true;
        public bool reflectionsEnabled = false;
        public float reflectionStrength = 0.5f;

        public bool enableCulling = true;

        public float textureRotation = 0.0f;
        public bool environmentMapping = false;
        public int textureWrapMode = 0;
       
        public struct Vector3
        {
            public float X, Y, Z;
            public Vector3(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }

        public Vector3[] objectPositions = new Vector3[3]; // עבור Car1, Car2, Gate

        public bool sunEnabled = true;
        public float[] sunColor = new float[3] { 1.0f, 0.95f, 0.8f };
        public float sunIntensity = 0.8f;
        public float sunAngleX = -45.0f;
        public float sunAngleY = 30.0f;

        public float globalAmbientIntensity = 0.2f;
        public float[] globalAmbientColor = new float[4] { 0.3f, 0.3f, 0.4f, 1.0f };

        public float shadowOpacity = 0.5f;  

        public float gateScale = 1.0f;
        public float gateRotation = 0.0f;
        public float gateOffsetX = 0.0f;
        public float gateOffsetY = 0.0f;

        public float trafficLightScale = 1.0f;
        public float trafficLightRotation = 0.0f;

        public float streetLampScale = 1.0f;
        public float streetLampRotation = 0.0f;

        public float fieldOfView = 60.0f;

        public cOGL(Control pb)
        {
            p = pb;
            Width = p.Width;
            Height = p.Height;
            InitializeGL();
            obj = GLU.gluNewQuadric();
            carModel = new Milkshape.Character("f360.ms3d");
            pos[0] = 10f;
            pos[1] = 10f;
            pos[2] = 15f;
            pos[3] = 1f;

            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);

        }
        public void Draw()
        {
            if (p == null) return;
            if (texturesEnabled)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
            }
            else
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, 0);
            }

            skyboxOffsetX = (skyboxOffsetX + 0.0002f) % 1f;
            skyboxOffsetY = (skyboxOffsetY + 0.0001f) % 1f;
            textureRotation = textureRotation += 1;



            UpdateGateAnimation();

            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT | GL.GL_STENCIL_BUFFER_BIT);
            GL.glLoadIdentity();

            GL.glPushMatrix();
            GL.glLoadIdentity(); 

            GL.glDepthMask((byte)GL.GL_FALSE);  
            GL.glDisable(GL.GL_DEPTH_TEST);     

            DrawSkybox(1000f, skyboxOffsetX, skyboxOffsetY); 

            GL.glDepthMask((byte)GL.GL_TRUE);
            GL.glEnable(GL.GL_DEPTH_TEST);

            GL.glPopMatrix();

            float cameraX = zoomDistance * 0.2f;
            float cameraY = -zoomDistance * 1.5f;
            float cameraZ = zoomDistance * 1.9f;
            GLU.gluLookAt(cameraX, cameraY, cameraZ, 0, 0, 0, 0, 0, 1);

            SetupParkingLighting();

            GL.glEnable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE);
            GL.glEnable(GL.GL_NORMALIZE);


            GL.glRotatef(xAngle, 1, 0, 0);
            GL.glRotatef(zAngle, 0, 0, 1);
            GL.glTranslatef(xShift * 0.1f, yShift * 0.1f, 0);
            GL.glTranslatef(panX, panY, 0);

            UpdateTrafficLight();

            intOptionB += 2;
            intOptionC += 1;

            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);

            GL.glEnable(GL.GL_STENCIL_TEST);
            GL.glStencilOp(GL.GL_REPLACE, GL.GL_REPLACE, GL.GL_REPLACE);
            GL.glStencilFunc(GL.GL_ALWAYS, 1, 0xFFFFFFFF);

            GL.glColorMask((byte)GL.GL_FALSE, (byte)GL.GL_FALSE, (byte)GL.GL_FALSE, (byte)GL.GL_FALSE);
            GL.glDisable(GL.GL_DEPTH_TEST);

            DrawFloor(); 

            GL.glColorMask((byte)GL.GL_TRUE, (byte)GL.GL_TRUE, (byte)GL.GL_TRUE, (byte)GL.GL_TRUE);
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glStencilFunc(GL.GL_EQUAL, 1, 0xFFFFFFFF);
            GL.glStencilOp(GL.GL_KEEP, GL.GL_KEEP, GL.GL_KEEP);

            GL.glPushMatrix();
            GL.glScalef(1, 1, -1); 
            GL.glEnable(GL.GL_CULL_FACE);
            GL.glCullFace(GL.GL_BACK);
            DrawFigures(); 
            GL.glCullFace(GL.GL_FRONT);
            DrawFigures(); 
            GL.glDisable(GL.GL_CULL_FACE);
            GL.glPopMatrix();

            GL.glDepthMask((byte)GL.GL_FALSE);
            DrawFloor(); 
            GL.glDepthMask((byte)GL.GL_TRUE);

            GL.glDisable(GL.GL_STENCIL_TEST);

            GL.glEnable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_LIGHT0);
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glDisable(GL.GL_BLEND);

            DrawCars();
            DrawFigures();
            DrawParkingLot();
            DrawParkingObjects();

            GL.glFlush();
            WGL.wglSwapBuffers(WGL.GetDC((uint)p.Handle));
        }
        public void OnResize()
        {
            Width = p.Width;
            Height = p.Height;
            GL.glViewport(0, 0, Width, Height);

            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();

            float aspect = (float)Width / (float)Height;

            if (isOrthogonal)
            {
                float size = zoomDistance;
                GL.glOrtho(-size * aspect, size * aspect, -size, size, 1.0, 100.0);
            }
            else
            {
                GLU.gluPerspective(fieldOfView, aspect, 1.0, 100.0); 
            }

            GL.glMatrixMode(GL.GL_MODELVIEW);
            InitializeParkingRenderingSettings();
            Draw();
        }
        protected virtual void InitializeGL()
        {
            uint hwnd = (uint)p.Handle;
            uint dc = WGL.GetDC(hwnd);

            // Not doing the following WGL.wglSwapBuffers() on the DC will
            // result in a failure to subsequently create the RC.
            WGL.wglSwapBuffers(dc);

            WGL.PIXELFORMATDESCRIPTOR pfd = new WGL.PIXELFORMATDESCRIPTOR();
            WGL.ZeroPixelDescriptor(ref pfd);
            pfd.nVersion = 1;
            pfd.dwFlags = (WGL.PFD_DRAW_TO_WINDOW | WGL.PFD_SUPPORT_OPENGL | WGL.PFD_DOUBLEBUFFER);
            pfd.iPixelType = (byte)(WGL.PFD_TYPE_RGBA);
            pfd.cColorBits = 32;
            pfd.cDepthBits = 32;
            pfd.cStencilBits = 32;
            pfd.iLayerType = (byte)(WGL.PFD_MAIN_PLANE);

            int pixelFormatIndex = WGL.ChoosePixelFormat(dc, ref pfd);
            if (pixelFormatIndex == 0)
            {
                MessageBox.Show("Unable to retrieve pixel format");
                return;
            }

            if (WGL.SetPixelFormat(dc, pixelFormatIndex, ref pfd) == 0)
            {
                MessageBox.Show("Unable to set pixel format");
                return;
            }

            uint rc = WGL.wglCreateContext(dc);
            if (rc == 0)
            {
                MessageBox.Show("Unable to get rendering context");
                return;
            }

            if (WGL.wglMakeCurrent(dc, rc) == 0)
            {
                MessageBox.Show("Unable to make rendering context current");
                return;
            }

            InitializeParkingRenderingSettings();
        }
        protected virtual void InitializeParkingRenderingSettings()
        {
            try
            {
                GL.glEnable(GL.GL_DEPTH_TEST);
                GL.glDepthFunc(GL.GL_LEQUAL);

                GL.glEnable(GL.GL_LIGHTING);
                GL.glEnable(GL.GL_LIGHT0); 

                SetupParkingLighting();

                GL.glEnable(GL.GL_COLOR_MATERIAL);
                GL.glColorMaterial(GL.GL_FRONT, GL.GL_AMBIENT_AND_DIFFUSE);

                GL.glClearColor(0f, 0f, 0f, 0f);

                SetupParkingViewportAndMatrices();

                LoadParkingTextures();

                GL.glEnable(GL.GL_STENCIL_TEST);
                GL.glClearStencil(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"error: {ex.Message}");
            }
        }
        public void ToggleCulling()
        {
            enableCulling = !enableCulling;
            if (enableCulling)
                GL.glEnable(GL.GL_CULL_FACE);
            else
                GL.glDisable(GL.GL_CULL_FACE);
        }
        public void SetFieldOfView(float fov)
        {
            fieldOfView = fov;
            OnResize(); 
        }

    }
}