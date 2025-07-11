using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace OpenGL
{
    class cOGL
    {
        Control p;
        int Width, Height;
        GLUquadric obj;
        Random rand = new Random();

        public bool isPanning = false; //עכבר תזוזה
        public float panX = 0f, panY = 0f;
        public Point mouseDownPoint;

        public bool light1On = true;  // תאורה ראשית
        public bool light2On = true;  // תאורה משנית
        //מערך טקסטורות
        public uint[] Textures = new uint[20];
        public uint[] SkyboxTextures = new uint[6]; // ייעודי ל־skybox

        public bool carLightsOn = false;
        public bool carBlinking = false;
        private float blinkTimer = 0.0f;

        public float zoomDistance = 9.0f; // מרחק זום במקום zShift
        public float yShift = 0.0f;
        public float xShift = -23.0f;

        public float zAngle = 10.0f;
        public float yAngle = 10.0f;
        public float xAngle = -15.0f; // זווית התחלתית טובה יותר

        public bool isDragging = false;
        public int lastMouseX, lastMouseY;

        float skyboxOffsetX = 0.0f;
        float skyboxOffsetY = 0.0f;

        // משתנים לשליטה ברמזור
        public int trafficLightState = 0; 
        public float trafficLightTimer = 0.0f;
        public bool autoTrafficLight = true; // אוטומטי או ידני

        public bool isColored = false;
        public bool isSmooth = false;
        Random rd = new Random();



        public int redCarX = 0, redCarY = 0; // משתנים עבור הרכב האדום

        // שליטה בעוצמת אורות שונים בחניון
        public float light1Intensity = 3.0f;
        public float light2Intensity = 3.0f;

        public bool gateOpen = false;        // מצב השער פתוח או סגור
        public float gateAnimation = 0.0f;   // אנימציה של השער סגור פתוח
        public bool gateMoving = false;   // האם השער בתנועה

        public bool doorOpen = false;
        public float[,] ground = new float[3, 3]
                          {
                                 { 1, 1, 0 },
                                 { 0, 1, 0 },
                                 { 1, 0, 0 }
                             };
        float[] cubeXform = new float[16];
        public float[] pos = new float[4];
        
        public float[] ScrollValue = new float[14]; 

        public int intOptionB = 1;
        public int intOptionC = 0;
        double[] AccumulatedRotationsTraslations = new double[16];

        public cOGL(Control pb)
        {
            p = pb;
            Width = p.Width;
            Height = p.Height;
            InitializeGL();
            obj = GLU.gluNewQuadric();
            pos[0] = 10f; 
            pos[1] = 10f;    
            pos[2] = 15f;  
            pos[3] = 1f;

            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);


        }
        void DrawParkingLot()
        {
            GL.glPushMatrix(); 

            // מיקום החניון במרחב    
            GL.glTranslatef(-4.0f, -3.5f, 0); 

            //המשטח כולו
            float lotWidth = 8.5f;     // רוחב המשטח האפור
            float lotHeight = 6.0f;    // אורך המשטח האפור
            float spaceWidth = 1.08f;  // רוחב של כל חנייה
            float roadGap = 2.0f;      // רווח בין השורות
                    
            //רצפת האבנים 
            float marginLeft = 0.6f;            
            float marginRight = 5.0f;          
            float marginBottom = 0.5f;
            float marginTop = 1.5f;

            float left = -marginLeft;
            float right = lotWidth + marginRight;
            float bottom = -marginBottom;
            float top = lotHeight + marginTop;

            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glColor3f(1f, 1f, 1f);
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[2]); // אבן

            // ציור הרקע האבן 
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(left, bottom, -0.01f);
            GL.glTexCoord2f(0, 3); GL.glVertex3f(left, top, -0.01f);
            GL.glTexCoord2f(4, 3); GL.glVertex3f(right, top, -0.01f);
            GL.glTexCoord2f(4, 0); GL.glVertex3f(right, bottom, -0.01f);
            GL.glEnd();

            GL.glDisable(GL.GL_TEXTURE_2D);

            // גבולות כלליים של החניות
            float parkingLeft = 0f;
            float parkingRight = 7.6f;
            float upperOffsetY = 1.0f; 


            float upperParkingTop = lotHeight - 0.2f + upperOffsetY;
            float upperParkingBottom = lotHeight / 2 + roadGap / 2 + upperOffsetY;

            float lowerParkingTop = lotHeight / 2 - roadGap / 2;
            float lowerParkingBottom = 0.2f;

            // משטח החניות העליונות עם טקסטורה
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);
            GL.glColor3f(1f, 1f, 1f);

            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(parkingLeft, upperParkingBottom, 0);
            GL.glTexCoord2f(0, 1); GL.glVertex3f(parkingLeft, upperParkingTop, 0);
            GL.glTexCoord2f(1, 1); GL.glVertex3f(parkingRight, upperParkingTop, 0);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(parkingRight, upperParkingBottom, 0);
            GL.glEnd();
            GL.glDisable(GL.GL_TEXTURE_2D);


            // משטח החניות התחתונות עם טקסטורה
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);
            GL.glColor3f(1f, 1f, 1f);

            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(parkingLeft, lowerParkingBottom, 0);
            GL.glTexCoord2f(0, 1); GL.glVertex3f(parkingLeft, lowerParkingTop, 0);
            GL.glTexCoord2f(1, 1); GL.glVertex3f(parkingRight, lowerParkingTop, 0);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(parkingRight, lowerParkingBottom, 0);
            GL.glEnd();

            GL.glDisable(GL.GL_TEXTURE_2D);

            //הכביש באמצע עם טקסטורת אספלט
            // גבולות הכביש Y
            float roadTop = lotHeight / 2 + roadGap / 2 + 1.0f; 
            float roadBottom = lotHeight / 2 - roadGap / 2;

            // גבולות הכביש X
            float roadLeft = 0.5f;
            float roadRight = 14.5f;

            // הכביש בחוץ עם טקסטורת אספלט
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[7]); 
            GL.glColor3f(1f, 1f, 1f);
            float shiftX = -1.0f; 
            float shiftY = -6.5f;
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(roadLeft + shiftX, roadBottom + shiftY, 0);
            GL.glTexCoord2f(0, 1); GL.glVertex3f(roadLeft + shiftX, roadTop + shiftY, 0);
            GL.glTexCoord2f(1, 1); GL.glVertex3f(roadRight + shiftX, roadTop + shiftY, 0);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(roadRight + shiftX, roadBottom + shiftY, 0);
            GL.glEnd();

            GL.glDisable(GL.GL_TEXTURE_2D);

            //כביש כניסה
            float offsetYWaiting1 = 0.55f; 
            float extraY = 0.3f; 

            float waitingAreaLeft1 = lotWidth + 2.0f;
            float waitingAreaRight1 = lotWidth + 5.0f;

            // הרחבה בציר Y
            float waitingAreaBottom1 = lotHeight / 2 - roadGap / 2 - extraY + offsetYWaiting1;
            float waitingAreaTop1 = lotHeight / 2 + roadGap / 2 + extraY + offsetYWaiting1;


            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[7]);
            GL.glColor3f(1f, 1f, 1f); //  כדי שהטקסטורה לא תוחשך

            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(waitingAreaLeft1, waitingAreaBottom1, 0); // תחתון שמאל
            GL.glTexCoord2f(0, 1); GL.glVertex3f(waitingAreaLeft1, waitingAreaTop1, 0); // עליון שמאל
            GL.glTexCoord2f(1, 1); GL.glVertex3f(waitingAreaRight1, waitingAreaTop1, 0); // עליון ימין
            GL.glTexCoord2f(1, 0); GL.glVertex3f(waitingAreaRight1, waitingAreaBottom1, 0); // תחתון ימין
            GL.glEnd();
            GL.glDisable(GL.GL_TEXTURE_2D);

            // קו האטה מפוספס בכניסה לאזור ההמתנה
            int stripeCount = 10; // מספר הפסים
            float stripeHeight = roadGap / stripeCount;  // פסים לאורך ציר Y
            float startY = lotHeight / 2 - roadGap / 2 + 0.5f;
            float xLeft = lotWidth;
            float xRight = lotWidth + 0.2f; 

            for (int i = 0; i < stripeCount; i++)
            {
                if (i % 2 == 0)
                    GL.glColor3f(1.0f, 0.5f, 0.0f); // כתום
                else
                    GL.glColor3f(0.0f, 0.0f, 0.0f); // שחור

                float y1c = startY + i * stripeHeight;
                float y2c = y1c + stripeHeight;

                GL.glBegin(GL.GL_QUADS);
                GL.glVertex3f(xLeft, y1c, 0.01f);
                GL.glVertex3f(xRight, y1c, 0.01f);
                GL.glVertex3f(xRight, y2c, 0.01f);
                GL.glVertex3f(xLeft, y2c, 0.01f);
                GL.glEnd();
            }

            // קו האטה מפוספס לצד החניות 
            int stripeCount1 = 15; //פס דק יותר
            float stripeWidth = 7.6f / stripeCount1; // אורך כולל  

            float startX = lotWidth - 8.5f;         
            float yBottom = lotHeight / 2 - roadGap / 2 + 4.9f; 
            float yTop = yBottom + 0.25f;     

            for (int i = 0; i < stripeCount1; i++)
            {
                if (i % 2 == 0)
                    GL.glColor3f(1.0f, 1.0f, 0.0f); // צהוב 
                else
                    GL.glColor3f(0.0f, 0.0f, 0.0f); // שחור

                float x1c = startX + i * stripeWidth;
                float x2c = x1c + stripeWidth;

                GL.glBegin(GL.GL_QUADS);
                GL.glVertex3f(x1c, yBottom, 0.01f);
                GL.glVertex3f(x2c, yBottom, 0.01f);
                GL.glVertex3f(x2c, yTop, 0.01f);
                GL.glVertex3f(x1c, yTop, 0.01f);
                GL.glEnd();
            }

            //קו האטה מפוספס נוסף בצד התחתון
            int stripeCount2 = 15;
            float stripeWidth2 = 7.6f / stripeCount2;

            float startX2 = lotWidth - 8.5f; 
            float yBottom2 = lotHeight / 2 - roadGap / 2 - 2.2f; 
            float yTop2 = yBottom2 + 0.25f;

            for (int i = 0; i < stripeCount2; i++)
            {
                if (i % 2 == 0)
                    GL.glColor3f(1.0f, 1.0f, 0.0f); // צהוב 
                else
                    GL.glColor3f(0.0f, 0.0f, 0.0f); // שחור

                float x1c = startX2 + i * stripeWidth2;
                float x2c = x1c + stripeWidth2;

                GL.glBegin(GL.GL_QUADS);
                GL.glVertex3f(x1c, yBottom2, 0.01f);
                GL.glVertex3f(x2c, yBottom2, 0.01f);
                GL.glVertex3f(x2c, yTop2, 0.01f);
                GL.glVertex3f(x1c, yTop2, 0.01f);
                GL.glEnd();
            }

            DrawSecurityBooth(lotWidth + 3.3f, lotHeight / 2 + 0.5f);

            // ציור השער המתקדם
            DrawGate(lotWidth + 1.8f, lotHeight / 2);
            DrawParkingWall();
  
            // קו מפריד מקווקו באמצע החניון
            float offsetLines = 0.5f; 
            float lineHeight = 0.15f;    

            GL.glColor3f(1.0f, 1.0f, 0.0f);
            for (float x = 0; x < 9; x += 1.3f)
            {
                GL.glBegin(GL.GL_QUADS);
                GL.glVertex3f(x, lotHeight / 2 - lineHeight + offsetLines, 0.01f);
                GL.glVertex3f(x, lotHeight / 2 + lineHeight + offsetLines, 0.01f);
                GL.glVertex3f(x + 0.2f, lotHeight / 2 + lineHeight + offsetLines, 0.01f);
                GL.glVertex3f(x + 0.2f, lotHeight / 2 - lineHeight + offsetLines, 0.01f);
                GL.glEnd();
            }

            float spacingBetweenSpaces = 0.1f; // רווח בין חניות

            float offsetY = 1.0f; 

            for (int i = 0; i < 7; i++)
            {
                float x = i * (spaceWidth + spacingBetweenSpaces); // רווח בין חניות


                GL.glColor3f(1.0f, 1.0f, 0.0f);
                GL.glBegin(GL.GL_LINE_LOOP);
                GL.glVertex3f(x, lotHeight / 2 + roadGap / 2 + offsetY, 0.01f);
                GL.glVertex3f(x, lotHeight - 0.2f + offsetY, 0.01f);
                GL.glVertex3f(x + spaceWidth, lotHeight - 0.2f + offsetY, 0.01f);
                GL.glVertex3f(x + spaceWidth, lotHeight / 2 + roadGap / 2 + offsetY, 0.01f);
                GL.glEnd();

               
                float offsetYGreen = 1.0f;
                GL.glBegin(GL.GL_QUADS);
                GL.glVertex3f(x + 0.4f, lotHeight - 0.5f + offsetYGreen, 0.02f);
                GL.glVertex3f(x + 0.4f, lotHeight - 0.3f + offsetYGreen, 0.02f);
                GL.glVertex3f(x + 0.7f, lotHeight - 0.3f + offsetYGreen, 0.02f);
                GL.glVertex3f(x + 0.7f, lotHeight - 0.5f + offsetYGreen, 0.02f);
                GL.glEnd();
            }

            // ציור השורה התחתונה של החניות  
            for (int i = 0; i < 7; i++)
            {
                float x = i * (spaceWidth + spacingBetweenSpaces); // רווח בין חניות

                GL.glColor3f(1.0f, 1.0f, 0.0f);
                GL.glBegin(GL.GL_LINE_LOOP);
                GL.glVertex3f(x, 0.2f, 0.01f);
                GL.glVertex3f(x, lotHeight / 2 - roadGap / 2, 0.01f);
                GL.glVertex3f(x + spaceWidth, lotHeight / 2 - roadGap / 2, 0.01f);
                GL.glVertex3f(x + spaceWidth, 0.2f, 0.01f);
                GL.glEnd();
                GL.glBegin(GL.GL_QUADS);
                GL.glVertex3f(x + 0.4f, 0.5f, 0.02f);
                GL.glVertex3f(x + 0.4f, 0.3f, 0.02f);
                GL.glVertex3f(x + 0.7f, 0.3f, 0.02f);
                GL.glVertex3f(x + 0.7f, 0.5f, 0.02f);
                GL.glEnd();
            }

            // תאורה למטה שמאל 
            DrawStreetLamp(-0.5f, 0.0f, 0, 1);
            if (light1On)
                DrawLightCircleOnGround(1.0f, 1.6f, 2.0f);

            // תאורה למעלה שמאל 
            DrawStreetLamp(-0.5f, 7.0f, 0, 1);
            if (light1On)
                DrawLightCircleOnGround(1.0f, 5.4f, 2.0f);

            // תאורה למעלה ימין
            DrawStreetLamp(8.3f, 5.2f, 270, 2);
            if (light2On)
                DrawLightCircleOnGround(8.7f, 5.2f, 2.0f);

            // תאורה למטה ימין 
            DrawStreetLamp(9.0f, 1.5f, 90, 2);
            if (light2On)
                DrawLightCircleOnGround(9.5f, 1.5f, 2.0f);

            // רמזור ליד הכניסה
            DrawTrafficLight(13.0f, 4.5f, 0);
            DrawTrafficLight(13.0f, -2.5f, 0);

            // רכב 1
            int redCarX1 = 0, redCarY1 = 0;
            float carCurrentX1 = 0, carCurrentY1 = 0;

           

            // רכב 3
            int redCarX3 = 2, redCarY3 = 0;
            float carCurrentX3 = -0.1f, carCurrentY3 = 0.3f;
            // רכב 4
            int redCarX4 = 2, redCarY4 = 0;
            float carCurrentX4 = -0.1f, carCurrentY4 = 0.3f;


            // רכב 1
            float x1 = lotWidth + 3.8f + (redCarX1 * 0.3f) + carCurrentX1;
            float y1 = lotHeight / 2 + (redCarY1 * 1.3f) + carCurrentY1;
            DrawCarWithShadow(x1, y1, 0);

            // רכב 2
            float x2 = lotWidth + 1.8f + (redCarX3 * 0.3f) - 2.5f + carCurrentX3;
            float y2 = lotHeight / 2 + (redCarY3 * 1.3f) - 5.8f + carCurrentY3;
            DrawCarWithShadow(x2, y2, 0);

            // רכב 3
            float x3 = lotWidth + 1.8f + (redCarX3 * 0.3f) + carCurrentX3;
            float y3 = lotHeight / 2 + (redCarY3 * 1.3f) - 5.8f + carCurrentY3;
            DrawCarWithShadow(x3, y3, 0);

            // רכב 4 (עם סיבוב 270 מעלות)
            float x4 = lotWidth - 7.3f + (redCarX4 * 0.3f) + carCurrentX4;
            float y4 = lotHeight / 2 + (redCarY4 * 5.3f) + 2.5f + carCurrentY4;
            DrawCarWithShadow(x4, y4, 0, 270);

            GL.glPopMatrix(); // החזרת המטריצה הראשית של החניון
        }

        void DrawFloor()
        {
            GL.glEnable(GL.GL_LIGHTING);
            GL.glBegin(GL.GL_QUADS);
            // רצפה כחולה שקופה קטנה - לצד החניון
            GL.glColor4d(0, 0, 1, 0.5);
            GL.glVertex3d(10, -3, 0);    // שמאל תחתון
            GL.glVertex3d(10, 3, 0);     // שמאל עליון  
            GL.glVertex3d(16, 3, 0);     // ימין עליון
            GL.glVertex3d(16, -3, 0);    // ימין תחתון
            GL.glEnd();
        }

        void DrawFigures()
        {
            GL.glPushMatrix();

            // הזזה לצד החניון
            GL.glTranslatef(13.0f, 0.0f, 0.0f); // הזזה ימינה לצד החניון

            // מקור האור חייב להיות בסצנה כדי שישתקף גם
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, pos);

            // ציור מקור האור
            GL.glDisable(GL.GL_LIGHTING);
            GL.glTranslatef(pos[0] / 5, pos[1] / 5, pos[2] / 5); // הקטנה של מיקום האור
                                                                 // מקור אור צהוב קטן
            GL.glColor3f(1, 1, 0);
            GLUT.glutSolidSphere(0.02, 8, 8); // כדור קטן יותר
            GL.glTranslatef(-pos[0] / 5, -pos[1] / 5, -pos[2] / 5);

            // קו הקרנה ממקור האור למישור
            GL.glBegin(GL.GL_LINES);
            GL.glColor3d(0.5, 0.5, 0);
            GL.glVertex3d(pos[0] / 5, pos[1] / 5, 0);
            GL.glVertex3d(pos[0] / 5, pos[1] / 5, pos[2] / 5);
            GL.glEnd();

            // הפעלת תאורה לאובייקטים
            GL.glEnable(GL.GL_LIGHTING);

            GL.glRotated(intOptionB, 0, 0, 1); // סיבוב שניהם

            // הקטנת האובייקטים
            GL.glScalef(0.3f, 0.3f, 0.3f); // הקטנה של כל האובייקטים

            // קוביה אדומה מסתובבת
            GL.glColor3f(1, 0, 0);
            GL.glTranslated(0, -1.5, 1);
            GL.glRotated(intOptionC, 1, 1, 1);
            GLUT.glutSolidCube(1);
            GL.glRotated(-intOptionC, 1, 1, 1);
            GL.glTranslated(0, 1.5, -1);

            // טיפות תה ציאן מסתובבת
            GL.glTranslated(2, 1.5, 1.5);
            GL.glRotated(90, 1, 0, 0);
            GL.glColor3d(0, 1, 1);
            GL.glRotated(intOptionB, 1, 0, 0);
            GLUT.glutSolidTeapot(1);
            GL.glRotated(-intOptionB, 1, 0, 0);
            GL.glRotated(-90, 1, 0, 0);
            GL.glTranslated(-2, -1.5, -1.5);

            GL.glPopMatrix();
        }

        void LoadSkyboxTextures()
        {
            string[] skyboxNames = {
        "skybox_back.jpg", "skybox_bottom.jpg","skybox_front.jpg"
         , "skybox_left.jpg","skybox_right.jpg",
        "skybox_top.jpg",
    };

            GL.glGenTextures(6, SkyboxTextures);

            for (int i = 0; i < 6; i++)
            {
                Bitmap image = new Bitmap(@"Textures/Skybox/" + skyboxNames[i]);
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                BitmapData bitmapdata = image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                GL.glBindTexture(GL.GL_TEXTURE_2D, SkyboxTextures[i]);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_S, (int)GL.GL_CLAMP);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_T, (int)GL.GL_CLAMP);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);

                GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB8, image.Width, image.Height,
                                0, GL.GL_BGR_EXT, GL.GL_UNSIGNED_byte, bitmapdata.Scan0);

                image.UnlockBits(bitmapdata);
                image.Dispose();
            }
        }

        void DrawSkybox(float size, float uOffset, float vOffset)
        {
            GL.glDisable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, SkyboxTextures[0]); // טקסטורת שמיים

            GL.glColor3f(1, 1, 1);
            float half = size / 2f;

            // רצפה (bottom)
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0 + uOffset, 0 + vOffset); GL.glVertex3f(-half, -half, -half);
            GL.glTexCoord2f(1 + uOffset, 0 + vOffset); GL.glVertex3f(half, -half, -half);
            GL.glTexCoord2f(1 + uOffset, 1 + vOffset); GL.glVertex3f(half, half, -half);
            GL.glTexCoord2f(0 + uOffset, 1 + vOffset); GL.glVertex3f(-half, half, -half);
            GL.glEnd();

            // תקרה (top)
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0 + uOffset, 0 + vOffset); GL.glVertex3f(-half, -half, half);
            GL.glTexCoord2f(1 + uOffset, 0 + vOffset); GL.glVertex3f(half, -half, half);
            GL.glTexCoord2f(1 + uOffset, 1 + vOffset); GL.glVertex3f(half, half, half);
            GL.glTexCoord2f(0 + uOffset, 1 + vOffset); GL.glVertex3f(-half, half, half);
            GL.glEnd();

            // קיר אחורי (back)
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0 + uOffset, 0 + vOffset); GL.glVertex3f(-half, -half, -half);
            GL.glTexCoord2f(1 + uOffset, 0 + vOffset); GL.glVertex3f(-half, -half, half);
            GL.glTexCoord2f(1 + uOffset, 1 + vOffset); GL.glVertex3f(half, -half, half);
            GL.glTexCoord2f(0 + uOffset, 1 + vOffset); GL.glVertex3f(half, -half, -half);
            GL.glEnd();

            // קיר קדמי (front)
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0 + uOffset, 0 + vOffset); GL.glVertex3f(-half, half, -half);
            GL.glTexCoord2f(1 + uOffset, 0 + vOffset); GL.glVertex3f(-half, half, half);
            GL.glTexCoord2f(1 + uOffset, 1 + vOffset); GL.glVertex3f(half, half, half);
            GL.glTexCoord2f(0 + uOffset, 1 + vOffset); GL.glVertex3f(half, half, -half);
            GL.glEnd();

            // קיר שמאלי (left)
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0 + uOffset, 0 + vOffset); GL.glVertex3f(-half, -half, -half);
            GL.glTexCoord2f(1 + uOffset, 0 + vOffset); GL.glVertex3f(-half, half, -half);
            GL.glTexCoord2f(1 + uOffset, 1 + vOffset); GL.glVertex3f(-half, half, half);
            GL.glTexCoord2f(0 + uOffset, 1 + vOffset); GL.glVertex3f(-half, -half, half);
            GL.glEnd();

            // קיר ימני (right)
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0 + uOffset, 0 + vOffset); GL.glVertex3f(half, -half, -half);
            GL.glTexCoord2f(1 + uOffset, 0 + vOffset); GL.glVertex3f(half, half, -half);
            GL.glTexCoord2f(1 + uOffset, 1 + vOffset); GL.glVertex3f(half, half, half);
            GL.glTexCoord2f(0 + uOffset, 1 + vOffset); GL.glVertex3f(half, -half, half);
            GL.glEnd();

            GL.glDisable(GL.GL_TEXTURE_2D);
            GL.glEnable(GL.GL_LIGHTING);
        }

        void GenerateTextures()
        {
            GL.glGenTextures(8, Textures);
            string[] imagesName = { "floor.jpg", "parking_floor.jpg", "road.jpg", "Road_Center.jpg", "Security.jpg", "Waiting_Corner.jpg", "Wall_Parking.jpg", "Road_out2.jpg" };

            string basePath = Application.StartupPath + @"\Textures\";

            for (int i = 0; i < 8; i++) 
            {
                string fullPath = Path.Combine(basePath, imagesName[i]);

                if (!File.Exists(fullPath))
                {
                    MessageBox.Show("קובץ לא נמצא: " + fullPath);
                    continue;
                }

                Bitmap image = new Bitmap(fullPath);
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                BitmapData bitmapdata = image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[i]);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_S, (int)GL.GL_REPEAT);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_T, (int)GL.GL_REPEAT);
                GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB8, image.Width, image.Height, 0, GL.GL_BGR_EXT, GL.GL_UNSIGNED_byte, bitmapdata.Scan0);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);

                image.UnlockBits(bitmapdata);
                image.Dispose();
            }
        }
        public void Draw()
        {
            if (p == null) return;

            UpdateGateAnimation();

            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT | GL.GL_STENCIL_BUFFER_BIT);
            GL.glLoadIdentity();

            GL.glPushMatrix();
            GL.glLoadIdentity();

            skyboxOffsetX = (skyboxOffsetX + 0.0002f) % 1f;
            skyboxOffsetY = (skyboxOffsetY + 0.0001f) % 1f;

            DrawSkybox(10000f, skyboxOffsetX, skyboxOffsetY);
            GL.glPopMatrix();

            float cameraX = zoomDistance * 0.2f;
            float cameraY = -zoomDistance * 1.5f;
            float cameraZ = zoomDistance * 1.9f;
            GLU.gluLookAt(cameraX, cameraY, cameraZ, 0, 0, 0, 0, 0, 1);

            GL.glRotatef(xAngle, 1, 0, 0);
            GL.glRotatef(zAngle, 0, 0, 1);
            GL.glTranslatef(xShift * 0.1f, yShift * 0.1f, 0);
            GL.glTranslatef(panX, panY, 0);
            UpdateTrafficLight();

            // עדכון סיבובים
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

            // ציור הסצנה המשתקפת
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

            DrawFigures();
            DrawParkingLot();

            GL.glFlush();
            WGL.wglSwapBuffers(WGL.GetDC((uint)p.Handle));
        }

        void DrawCarWithShadow(float x, float y, float z, float angle = 0)
        {
            GL.glPushMatrix();
            GL.glTranslatef(x, y, z);

            // ציור הצל
            GL.glDisable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_POLYGON_OFFSET_FILL);
            GL.glPolygonOffset(-2.0f, -4.0f); 

            GL.glPushMatrix();
            MakeShadowMatrix(ground);
            GL.glMultMatrixf(cubeXform);
            GL.glRotatef(angle, 0, 0, 1);
            GL.glColor3f(0.4f, 0.4f, 0.4f); 
            DrawUpgradedWaitingCar(true);
            GL.glPopMatrix();

            GL.glDisable(GL.GL_POLYGON_OFFSET_FILL);
            GL.glEnable(GL.GL_LIGHTING);

            // ציור הרכב
            GL.glRotatef(angle, 0, 0, 1);
            DrawUpgradedWaitingCar(false);

            GL.glPopMatrix();
        }
        void MakeShadowMatrix(float[,] ground)
        {
            float[] planeCoeff = new float[4];
            float dot;

            float[] v1 = new float[3];
            float[] v2 = new float[3];

            v1[0] = ground[0, 0] - ground[1, 0];
            v1[1] = ground[0, 1] - ground[1, 1];
            v1[2] = ground[0, 2] - ground[1, 2];

            v2[0] = ground[1, 0] - ground[2, 0];
            v2[1] = ground[1, 1] - ground[2, 1];
            v2[2] = ground[1, 2] - ground[2, 2];

            planeCoeff[0] = v1[1] * v2[2] - v1[2] * v2[1];
            planeCoeff[1] = v1[2] * v2[0] - v1[0] * v2[2];
            planeCoeff[2] = v1[0] * v2[1] - v1[1] * v2[0];

            float length = (float)Math.Sqrt(planeCoeff[0] * planeCoeff[0] +
                                           planeCoeff[1] * planeCoeff[1] +
                                           planeCoeff[2] * planeCoeff[2]);
            if (length != 0)
            {
                planeCoeff[0] /= length;
                planeCoeff[1] /= length;
                planeCoeff[2] /= length;
            }

            planeCoeff[3] = -(planeCoeff[0] * ground[2, 0] +
                              planeCoeff[1] * ground[2, 1] +
                              planeCoeff[2] * ground[2, 2]);

            float[] lightPos = { pos[0], pos[1], pos[2], pos[3] }; // ← השינוי היחיד!

            // מכפלה סקלרית
            dot = planeCoeff[0] * lightPos[0] +
                  planeCoeff[1] * lightPos[1] +
                  planeCoeff[2] * lightPos[2] +
                  planeCoeff[3] * lightPos[3];

            cubeXform[0] = dot - lightPos[0] * planeCoeff[0];
            cubeXform[4] = 0.0f - lightPos[0] * planeCoeff[1];
            cubeXform[8] = 0.0f - lightPos[0] * planeCoeff[2];
            cubeXform[12] = 0.0f - lightPos[0] * planeCoeff[3];

            cubeXform[1] = 0.0f - lightPos[1] * planeCoeff[0];
            cubeXform[5] = dot - lightPos[1] * planeCoeff[1];
            cubeXform[9] = 0.0f - lightPos[1] * planeCoeff[2];
            cubeXform[13] = 0.0f - lightPos[1] * planeCoeff[3];

            cubeXform[2] = 0.0f - lightPos[2] * planeCoeff[0];
            cubeXform[6] = 0.0f - lightPos[2] * planeCoeff[1];
            cubeXform[10] = dot - lightPos[2] * planeCoeff[2];
            cubeXform[14] = 0.0f - lightPos[2] * planeCoeff[3];

            cubeXform[3] = 0.0f - lightPos[3] * planeCoeff[0];
            cubeXform[7] = 0.0f - lightPos[3] * planeCoeff[1];
            cubeXform[11] = 0.0f - lightPos[3] * planeCoeff[2];
            cubeXform[15] = dot - lightPos[3] * planeCoeff[3];
        }
        void DrawCube()
        {
            float size = 1.0f;
            float half = size / 2.0f;

            GL.glBegin(GL.GL_QUADS);

            // קדמית
            GL.glVertex3f(-half, -half, half);
            GL.glVertex3f(half, -half, half);
            GL.glVertex3f(half, half, half);
            GL.glVertex3f(-half, half, half);

            // אחורית
            GL.glVertex3f(-half, -half, -half);
            GL.glVertex3f(-half, half, -half);
            GL.glVertex3f(half, half, -half);
            GL.glVertex3f(half, -half, -half);

            // עליונה
            GL.glVertex3f(-half, half, -half);
            GL.glVertex3f(-half, half, half);
            GL.glVertex3f(half, half, half);
            GL.glVertex3f(half, half, -half);

            // תחתונה
            GL.glVertex3f(-half, -half, -half);
            GL.glVertex3f(half, -half, -half);
            GL.glVertex3f(half, -half, half);
            GL.glVertex3f(-half, -half, half);

            // צד ימין
            GL.glVertex3f(half, -half, -half);
            GL.glVertex3f(half, half, -half);
            GL.glVertex3f(half, half, half);
            GL.glVertex3f(half, -half, half);

            // צד שמאל
            GL.glVertex3f(-half, -half, -half);
            GL.glVertex3f(-half, -half, half);
            GL.glVertex3f(-half, half, half);
            GL.glVertex3f(-half, half, -half);

            GL.glEnd();
        }
        // עמוד תאורה 
        void DrawStreetLamp(float x, float y, float rotationAngleDeg = 0, int lampType = 1)

        {
            GL.glPushMatrix();
            GL.glTranslatef(x + 0.4f, y, 0);
            GL.glRotatef(rotationAngleDeg, 0, 0, 1); // סיבוב סביב ציר Z

            GL.glScalef(0.6f, 0.6f, 0.6f);

            // בסיס יציב - בטון אפור כהה
            GL.glColor3f(0.3f, 0.3f, 0.35f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 0.05f);
            GL.glScalef(0.6f, 0.6f, 0.1f);
            DrawCube();
            GL.glPopMatrix();

            // ברגים בבסיס  
            GL.glColor3f(0.15f, 0.15f, 0.15f);
            for (int i = 0; i < 4; i++)
            {
                float angle = i * 90.0f;
                GL.glPushMatrix();
                GL.glRotatef(angle, 0, 0, 1);
                GL.glTranslatef(0.2f, 0, 0.12f);
                GL.glScalef(0.03f, 0.03f, 0.05f);
                DrawCube();
                GL.glPopMatrix();
            }

            // חלק תחתון של העמוד 
            GL.glColor3f(0.12f, 0.12f, 0.12f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 0.4f);
            GL.glScalef(0.12f, 0.12f, 0.8f);
            DrawCube();
            GL.glPopMatrix();

            // העמוד הראשי 
            GL.glColor3f(0.18f, 0.18f, 0.18f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 1.8f);
            GL.glScalef(0.09f, 0.09f, 2.8f);
            DrawCube();
            GL.glPopMatrix();

            // פסים דקורטיביים על העמוד
            GL.glColor3f(0.25f, 0.25f, 0.25f);
            for (int i = 1; i <= 3; i++)
            {
                GL.glPushMatrix();
                GL.glTranslatef(0, 0, 1.0f + i * 0.7f);
                GL.glScalef(0.11f, 0.11f, 0.05f);
                DrawCube();
                GL.glPopMatrix();
            }

            // זרוע הפנס 
            GL.glColor3f(0.15f, 0.15f, 0.15f);
            GL.glPushMatrix();
            GL.glTranslatef(0.25f, 0, 3.2f);
            GL.glScalef(0.5f, 0.08f, 0.08f);
            DrawCube();
            GL.glPopMatrix();

            // חיבור הזרוע לעמוד 
            GL.glColor3f(0.1f, 0.1f, 0.1f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 3.2f);
            GL.glScalef(0.15f, 0.15f, 0.15f);
            DrawCube();
            GL.glPopMatrix();

            // מסגרת הפנס 
            GL.glColor3f(0.08f, 0.08f, 0.08f);
            GL.glPushMatrix();
            GL.glTranslatef(0.45f, 0, 3.0f);
            GL.glScalef(0.3f, 0.3f, 0.4f);
            DrawCube();
            GL.glPopMatrix();

            // רפלקטור פנימי   
            GL.glColor3f(0.6f, 0.6f, 0.6f);
            GL.glPushMatrix();
            GL.glTranslatef(0.45f, 0, 3.0f);
            GL.glScalef(0.25f, 0.25f, 0.35f);
            DrawCube();
            GL.glPopMatrix();

            // בדיקת מצב המנורה
            bool lampIsOn = (lampType == 1) ? light1On : light2On;

            if (lampIsOn)
            {
                // מנורה דלוקה   
                GL.glColor3f(1.0f, 0.95f, 0.8f);
                GL.glPushMatrix();
                GL.glTranslatef(0.45f, 0, 3.0f);
                GL.glScalef(0.15f, 0.15f, 0.2f);
                DrawCube();
                GL.glPopMatrix();

                // הילה של אור סביב הפנס
                GL.glDisable(GL.GL_LIGHTING);
                GL.glEnable(GL.GL_BLEND);
                GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE);
                GL.glColor4f(1.0f, 0.9f, 0.7f, 0.2f);
                GL.glPushMatrix();
                GL.glTranslatef(0.45f, 0, 3.0f);
                GL.glScalef(0.5f, 0.5f, 0.3f);
                DrawCube();
                GL.glPopMatrix();
                GL.glDisable(GL.GL_BLEND);
                GL.glEnable(GL.GL_LIGHTING);

                
            }
            else
            {
                // מנורה כבויה   
                GL.glColor3f(0.2f, 0.2f, 0.2f);
                GL.glPushMatrix();
                GL.glTranslatef(0.45f, 0, 3.0f);
                GL.glScalef(0.18f, 0.18f, 0.25f);
                DrawCube();
                GL.glPopMatrix();
            }

            // כיפת הגנה עליונה
            GL.glColor3f(0.05f, 0.05f, 0.05f);
            GL.glPushMatrix();
            GL.glTranslatef(0.45f, 0, 3.25f);
            GL.glScalef(0.35f, 0.35f, 0.1f);
            DrawCube();
            GL.glPopMatrix();

            // סנסור תנועה קטן
            GL.glColor3f(0.2f, 0.2f, 0.3f);
            GL.glPushMatrix();
            GL.glTranslatef(0.45f, 0.12f, 2.9f);
            GL.glScalef(0.04f, 0.04f, 0.04f);
            DrawCube();
            GL.glPopMatrix();

            // כבל חשמל דק יורד לאורך העמוד
            GL.glColor3f(0.05f, 0.05f, 0.05f);
            GL.glPushMatrix();
            GL.glTranslatef(0.08f, 0, 1.6f);
            GL.glScalef(0.01f, 0.01f, 3.2f);
            DrawCube();
            GL.glPopMatrix();
            GL.glPopMatrix();
        }
        void DrawLightCircleOnGround(float centerX, float centerY, float radius)
        {
            // אפקט אור על הקרקע עיגול מואר
            GL.glDisable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);

            GL.glBegin(GL.GL_TRIANGLE_FAN);
            // מרכז העיגול הכי בהיר
            GL.glColor4f(7.0f, 7.95f, 7.8f, 7.3f);
            GL.glVertex3f(centerX, centerY, 0.002f);   

            // קצוות העיגול דוהים
            GL.glColor4f(1.0f, 0.9f, 0.7f, 0.0f);
            for (int i = 0; i <= 32; i++)
            {
                float angle = (float)(i * 2.0 * Math.PI / 32.0);
                float x = centerX + (float)Math.Cos(angle) * radius;
                float y = centerY + (float)Math.Sin(angle) * radius;
                GL.glVertex3f(x, y, 0.002f);
            }
            GL.glEnd();
            GL.glDisable(GL.GL_BLEND);
            GL.glEnable(GL.GL_LIGHTING);
        }

        // ביתן השמירה    
        void DrawSecurityBooth(float x, float y)
        {
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);  

            GL.glPushMatrix();
            GL.glTranslatef(x - 2.4f, y + 1.5f, 0);
            GL.glScalef(0.5f, 0.6f, 0.8f);

            // בסיס הביתן
            GL.glColor3f(1.0f, 1.0f, 1.0f);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(0, 0, 0);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(0, 2.0f, 0);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(2.5f, 2.0f, 0);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(2.5f, 0, 0);
            GL.glEnd();

            // קירות הביתן עם טקסטורה
            GL.glColor3f(1.0f, 1.0f, 1.0f);

            GL.glBegin(GL.GL_QUADS);

            // פס תחתון מתחת לחלון
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(0, 0, 0);
            GL.glTexCoord2f(0.0f, 0.1f); GL.glVertex3f(0, 0, 0.2f);
            GL.glTexCoord2f(1.0f, 0.1f); GL.glVertex3f(2.5f, 0, 0.2f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(2.5f, 0, 0);

            // פס עליון מעל החלון
            GL.glTexCoord2f(0.0f, 0.9f); GL.glVertex3f(0, 0, 1.8f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(0, 0, 2.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(2.5f, 0, 2.0f);
            GL.glTexCoord2f(1.0f, 0.9f); GL.glVertex3f(2.5f, 0, 1.8f);

            // פס שמאל ליד החלון
            GL.glTexCoord2f(0.0f, 0.1f); GL.glVertex3f(0, 0, 0.2f);
            GL.glTexCoord2f(0.0f, 0.9f); GL.glVertex3f(0, 0, 1.8f);
            GL.glTexCoord2f(0.08f, 0.9f); GL.glVertex3f(0.2f, 0, 1.8f);
            GL.glTexCoord2f(0.08f, 0.1f); GL.glVertex3f(0.2f, 0, 0.2f);

            // פס ימין ליד החלון
            GL.glTexCoord2f(0.92f, 0.1f); GL.glVertex3f(2.3f, 0, 0.2f);
            GL.glTexCoord2f(0.92f, 0.9f); GL.glVertex3f(2.3f, 0, 1.8f);
            GL.glTexCoord2f(1.0f, 0.9f); GL.glVertex3f(2.5f, 0, 1.8f);
            GL.glTexCoord2f(1.0f, 0.1f); GL.glVertex3f(2.5f, 0, 0.2f);

            GL.glEnd();

            // קיר ימני עם דלת
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(2.5f, 0, 0);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(2.5f, 0, 2.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(2.5f, 2.0f, 2.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(2.5f, 2.0f, 0);
            GL.glEnd();

            // קיר אחורי
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(2.5f, 2.0f, 0);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(2.5f, 2.0f, 2.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(0, 2.0f, 2.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(0, 2.0f, 0);
            GL.glEnd();

            // קיר שמאל
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(0, 2.0f, 0);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(0, 2.0f, 2.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(0, 0, 2.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(0, 0, 0);
            GL.glEnd();

            // גג
            GL.glColor3f(0.85f, 0.85f, 0.85f);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-0.1f, -0.1f, 2.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-0.1f, 2.1f, 2.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(2.6f, 2.1f, 2.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(2.6f, -0.1f, 2.0f);
            GL.glEnd();

            // כיבוי טקסטורה לחלון
            GL.glBindTexture(GL.GL_TEXTURE_2D, 0);

            //  שקיפות    
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);

            GL.glColor4f(0.5f, 0.7f, 1.0f, 0.6f); // כחול שקוף כמו הרצפה
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(0.2f, -0.01f, 0.2f);  // שמאל תחתון
            GL.glVertex3f(0.2f, -0.01f, 1.8f);  // שמאל עליון
            GL.glVertex3f(2.3f, -0.01f, 1.8f);  // ימין עליון
            GL.glVertex3f(2.3f, -0.01f, 0.2f);  // ימין תחתון
            GL.glEnd();

            // שכבת השתקפות 
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.3f); // לבן שקוף להשתקפות
            GL.glBegin(GL.GL_QUADS);

            // השתקפות אלכסונית כמו השתקפות אור
            GL.glVertex3f(0.4f, -0.005f, 1.2f);
            GL.glVertex3f(0.4f, -0.005f, 1.6f);
            GL.glVertex3f(1.2f, -0.005f, 1.6f);
            GL.glVertex3f(1.2f, -0.005f, 1.2f);
            GL.glEnd();

            // השתקפות נוספת קטנה יותר
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.2f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(1.5f, -0.005f, 0.5f);
            GL.glVertex3f(1.5f, -0.005f, 0.8f);
            GL.glVertex3f(2.0f, -0.005f, 0.8f);
            GL.glVertex3f(2.0f, -0.005f, 0.5f);
            GL.glEnd();

            // כיבוי שקיפות
            GL.glDisable(GL.GL_BLEND);

            // מסגרת החלון - שחורה
            GL.glColor3f(0.2f, 0.2f, 0.2f);
            GL.glLineWidth(3.0f);
            GL.glBegin(GL.GL_LINE_LOOP);
            GL.glVertex3f(0.2f, -0.02f, 0.2f);
            GL.glVertex3f(0.2f, -0.02f, 1.8f);
            GL.glVertex3f(2.3f, -0.02f, 1.8f);
            GL.glVertex3f(2.3f, -0.02f, 0.2f);
            GL.glEnd();

            // דלת בצד
            GL.glColor3f(0.8f, 0.8f, 0.8f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(2.51f, 0.3f, 0.2f);
            GL.glVertex3f(2.51f, 0.3f, 1.8f);
            GL.glVertex3f(2.51f, 1.7f, 1.8f);
            GL.glVertex3f(2.51f, 1.7f, 0.2f);
            GL.glEnd();

            // ידית דלת
            GL.glColor3f(0.3f, 0.3f, 0.3f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(2.52f, 1.5f, 0.9f);
            GL.glVertex3f(2.52f, 1.5f, 1.0f);
            GL.glVertex3f(2.52f, 1.6f, 1.0f);
            GL.glVertex3f(2.52f, 1.6f, 0.9f);
            GL.glEnd();

            GL.glPopMatrix();
            GL.glBindTexture(GL.GL_TEXTURE_2D, 0);
            GL.glDisable(GL.GL_TEXTURE_2D);
        }

        //   השער     
        void DrawGate(float x, float y)
        {
            GL.glPushMatrix();
            GL.glTranslatef(x + 0.0f, y + 1.8f, 0); 


            GL.glRotatef(-270, 0, 0, 1); 


            // קופסת הבקרה הצהובה  
            GL.glColor3f(1.0f, 0.8f, 0.0f); // צהוב
            GL.glBegin(GL.GL_QUADS);
            // חזית
            GL.glVertex3f(-0.2f, -0.25f, 0.0f);
            GL.glVertex3f(-0.2f, -0.25f, 0.5f);
            GL.glVertex3f(0.2f, -0.25f, 0.5f);
            GL.glVertex3f(0.2f, -0.25f, 0.0f);
            // צדדים וגג
            GL.glVertex3f(0.2f, -0.25f, 0.0f);
            GL.glVertex3f(0.2f, -0.25f, 0.5f);
            GL.glVertex3f(0.2f, 0.25f, 0.5f);
            GL.glVertex3f(0.2f, 0.25f, 0.0f);
            GL.glVertex3f(0.2f, 0.25f, 0.0f);
            GL.glVertex3f(0.2f, 0.25f, 0.5f);
            GL.glVertex3f(-0.2f, 0.25f, 0.5f);
            GL.glVertex3f(-0.2f, 0.25f, 0.0f);
            GL.glVertex3f(-0.2f, 0.25f, 0.0f);
            GL.glVertex3f(-0.2f, 0.25f, 0.5f);
            GL.glVertex3f(-0.2f, -0.25f, 0.5f);
            GL.glVertex3f(-0.2f, -0.25f, 0.0f);
            GL.glVertex3f(-0.2f, -0.25f, 0.5f);
            GL.glVertex3f(-0.2f, 0.25f, 0.5f);
            GL.glVertex3f(0.2f, 0.25f, 0.5f);
            GL.glVertex3f(0.2f, -0.25f, 0.5f);
            GL.glEnd();

            // לוח קדמי שחור
            GL.glColor3f(0.1f, 0.1f, 0.1f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(-0.15f, -0.2f, 0.51f);
            GL.glVertex3f(-0.15f, 0.2f, 0.51f);
            GL.glVertex3f(0.15f, 0.2f, 0.51f);
            GL.glVertex3f(0.15f, -0.2f, 0.51f);
            GL.glEnd();

            // נורת סטטוס ירוקה אם פתוח אדומה אם סגור
            if (gateOpen || gateMoving)
                GL.glColor3f(0.0f, 1.0f, 0.0f); // ירוק  
            else
                GL.glColor3f(1.0f, 0.0f, 0.0f); // אדום  

            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(-0.06f, 0.06f, 0.52f);
            GL.glVertex3f(-0.06f, 0.15f, 0.52f);
            GL.glVertex3f(0.06f, 0.15f, 0.52f);
            GL.glVertex3f(0.06f, 0.06f, 0.52f);
            GL.glEnd();

            // כפתור/לוגו על הקופסה
            GL.glColor3f(0.0f, 0.0f, 0.0f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(-0.1f, -0.15f, 0.52f);
            GL.glVertex3f(-0.1f, -0.06f, 0.52f);
            GL.glVertex3f(0.1f, -0.06f, 0.52f);
            GL.glVertex3f(0.1f, -0.15f, 0.52f);
            GL.glEnd();

            // בסיס הקופסה
            GL.glColor3f(0.3f, 0.3f, 0.3f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(-0.3f, -0.35f, -0.05f);
            GL.glVertex3f(-0.3f, 0.35f, -0.05f);
            GL.glVertex3f(0.3f, 0.35f, -0.05f);
            GL.glVertex3f(0.3f, -0.35f, -0.05f);
            GL.glEnd();

            // מחסום השער 
            float gateAngle = gateAnimation * 90.0f; // זווית השער 0-90 מעלות
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.0f, 0.4f); // ציר הסיבוב
            GL.glRotatef(gateAngle, 0, 1, 0); 

            // מוט המחסום הראשי 
            float barrierLength = 2.5f; 

            GL.glColor3f(0.95f, 0.95f, 0.95f);
            GL.glBegin(GL.GL_QUADS);

            // צד תחתון
            GL.glVertex3f(0, -0.03f, -0.03f);
            GL.glVertex3f(0, -0.03f, 0.03f);
            GL.glVertex3f(-barrierLength, -0.03f, 0.03f);
            GL.glVertex3f(-barrierLength, -0.03f, -0.03f);

            // צד עליון
            GL.glVertex3f(0, 0.03f, -0.03f);
            GL.glVertex3f(-barrierLength, 0.03f, -0.03f);
            GL.glVertex3f(-barrierLength, 0.03f, 0.03f);
            GL.glVertex3f(0, 0.03f, 0.03f);

            // צד ימין
            GL.glVertex3f(0, -0.03f, 0.03f);
            GL.glVertex3f(0, 0.03f, 0.03f);
            GL.glVertex3f(-barrierLength, 0.03f, 0.03f);
            GL.glVertex3f(-barrierLength, -0.03f, 0.03f);

            // צד שמאל
            GL.glVertex3f(-barrierLength, -0.03f, -0.03f);
            GL.glVertex3f(-barrierLength, 0.03f, -0.03f);
            GL.glVertex3f(0, 0.03f, -0.03f);
            GL.glVertex3f(0, -0.03f, -0.03f);

            GL.glEnd();

            // פסים אדומים על המחסום 
            GL.glColor3f(1.0f, 0.0f, 0.0f);
            for (float i = -0.2f; i > -2.5f; i -= 0.25f) 
            {
                GL.glBegin(GL.GL_QUADS);
                GL.glVertex3f(i, -0.04f, -0.04f);
                GL.glVertex3f(i, -0.04f, 0.04f);
                GL.glVertex3f(i - 0.12f, -0.04f, 0.04f);
                GL.glVertex3f(i - 0.12f, -0.04f, -0.04f);

                GL.glVertex3f(i, 0.04f, -0.04f);
                GL.glVertex3f(i - 0.12f, 0.04f, -0.04f);
                GL.glVertex3f(i - 0.12f, 0.04f, 0.04f);
                GL.glVertex3f(i, 0.04f, 0.04f);
                GL.glEnd();
            }

            GL.glPopMatrix(); // סוף מחסום השער
            GL.glPopMatrix();
        }
        //חומות
        void DrawParkingWall()
        {
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[6]); 
            GL.glColor3f(0.9f, 0.9f, 0.9f); 

            float wallHeight = 0.5f; // גובה החומה
            float wallThickness = 0.3f; // עובי החומה
            float postWidth = 0.3f; // רוחב העמודים
            float postHeight = 1.0f; 

            float lotWidth = 6.5f;
            float lotHeight = 7.0f;
            float wallLeft = -0.6f;
            float wallRight = lotWidth + 4.1f;
            float wallBottom = -0.6f;
            float wallTop = lotHeight + 0.6f;

            // קיר תחתון דרום
            DrawWallSection(wallLeft, wallBottom, wallRight, wallBottom + wallThickness, wallHeight);

            // קיר עליון צפון
            DrawWallSection(wallLeft, wallTop - wallThickness, wallRight, wallTop, wallHeight);

            // קיר שמאל מערב
            DrawWallSection(wallLeft, wallBottom, wallLeft + wallThickness, wallTop, wallHeight);

            // קיר ימני עם פתח לכניסה
            float gateStart = lotHeight / 2 - 1.5f;
            float gateEnd = lotHeight / 2 + 2.7f;
            // חלק תחתון של הקיר הימני
            DrawWallSection(wallRight - wallThickness, wallBottom, wallRight, gateStart, wallHeight);
            // חלק עליון של הקיר הימני
            DrawWallSection(wallRight - wallThickness, gateEnd, wallRight, wallTop, wallHeight);

            // עמודים בפינות
            GL.glColor3f(0.8f, 0.8f, 0.8f); 

            // עמוד פינה דרום מערב
            DrawPost(wallLeft, wallBottom, postWidth, postHeight);
            // עמוד פינה דרום מזרח
            DrawPost(wallRight - postWidth, wallBottom, postWidth, postHeight);
            // עמוד פינה צפון מערב
            DrawPost(wallLeft, wallTop - postWidth, postWidth, postHeight);
            // עמוד פינה צפון מזרח
            DrawPost(wallRight - postWidth, wallTop - postWidth, postWidth, postHeight);

            // עמודים ליד הפתח 
            DrawPost(wallRight - postWidth, gateStart - postWidth, postWidth, postHeight);
            DrawPost(wallRight - postWidth, gateEnd, postWidth, postHeight);

            // עמודים באמצע הקירות הארוכים
            float midX = (wallLeft + wallRight) / 2;
            DrawPost(midX - postWidth / 2, wallBottom, postWidth, postHeight); // אמצע תחתון
            DrawPost(midX - postWidth / 2, wallTop - postWidth, postWidth, postHeight); // אמצע עליון

            GL.glBindTexture(GL.GL_TEXTURE_2D, 0);
            GL.glDisable(GL.GL_TEXTURE_2D);

            // קטע זכוכית ארוך בין שני העמודים בצד הצפוני
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glDisable(GL.GL_TEXTURE_2D);
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.4f);  // צבע זכוכית

            float glassY = wallTop - 0.05f;  
            float glassZTop = postHeight * 0.9f;    

            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(wallLeft + postWidth, glassY, 0.0f);        // שמאל למטה
            GL.glVertex3f(wallLeft + postWidth, glassY, glassZTop);   // שמאל למעלה
            GL.glVertex3f(wallRight - postWidth, glassY, glassZTop);  // ימין למעלה
            GL.glVertex3f(wallRight - postWidth, glassY, 0.0f);       // ימין למטה
            GL.glEnd();

            GL.glDisable(GL.GL_BLEND);

            // קטע זכוכית ארוך בין שני העמודים בצד הדרומי
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glDisable(GL.GL_TEXTURE_2D);
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.4f); // צבע זכוכית

            float glassYSouth = wallBottom + 0.05f;  
            float glassZTopD = postHeight * 0.9f; // גובה הגדר השקופה

            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(wallLeft + postWidth, glassYSouth, 0.0f);        // שמאל למטה
            GL.glVertex3f(wallLeft + postWidth, glassYSouth, glassZTopD);   // שמאל למעלה
            GL.glVertex3f(wallRight - postWidth, glassYSouth, glassZTopD);  // ימין למעלה
            GL.glVertex3f(wallRight - postWidth, glassYSouth, 0.0f);       // ימין למטה
            GL.glEnd();

            GL.glDisable(GL.GL_BLEND);

            // קטע זכוכית בין שני עמודים בצד הימני  
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glDisable(GL.GL_TEXTURE_2D);
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.4f); // צבע זכוכית

            float yOffset = -4.2f; 

            float glassXR = wallRight - 0.05f; 
            float gateStartR = lotHeight / 2 - 0.1f;
            float gateEndR = lotHeight / 2 + 2.7f;
            float glassZTopR = postHeight * 0.9f;

            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(glassXR, gateStartR + postWidth + yOffset, 0.0f);       
            GL.glVertex3f(glassXR, gateStartR + postWidth + yOffset, glassZTopR);   
            GL.glVertex3f(glassXR, gateEndR - postWidth + yOffset, glassZTopR);     
            GL.glVertex3f(glassXR, gateEndR - postWidth + yOffset, 0.0f);         
            GL.glEnd();

            GL.glDisable(GL.GL_BLEND);

            // קטע זכוכית בין שני עמודים בצד הימני קיר מזרחי למעלה הקטן
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glDisable(GL.GL_TEXTURE_2D);
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.4f); // צבע זכוכית

            float yOffsetR2 = 4.2f; 

            float glassXR2 = wallRight - 0.05f; 
            float gateStartR2 = lotHeight / 2 - 1.9f;
            float gateEndR2 = lotHeight / 2 + 1.7f;
            float glassZTopR2 = postHeight * 0.9f;
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(glassXR2, gateStartR2 + postWidth + yOffsetR2 + 0.4f, 0.0f);        
            GL.glVertex3f(glassXR2, gateStartR2 + postWidth + yOffsetR2 + 0.4f, glassZTopR2); 
            GL.glVertex3f(glassXR2, gateEndR2 - postWidth + yOffsetR2 - 1.5f, glassZTopR2);   
            GL.glVertex3f(glassXR2, gateEndR2 - postWidth + yOffsetR2 - 1.5f, 0.0f);          
            GL.glEnd();


            GL.glDisable(GL.GL_BLEND);
            // קטע זכוכית בין שני העמודים בצד השמאלי מערב
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glDisable(GL.GL_TEXTURE_2D);
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.4f); // צבע זכוכית 

            float glassXLeft = wallLeft + 0.05f;         
            float glassZTopLeft = postHeight * 0.9f;      

            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(glassXLeft, wallBottom + postWidth, 0.0f);         
            GL.glVertex3f(glassXLeft, wallBottom + postWidth, glassZTopLeft); 
            GL.glVertex3f(glassXLeft, wallTop - postWidth, glassZTopLeft);   
            GL.glVertex3f(glassXLeft, wallTop - postWidth, 0.0f);            
            GL.glEnd();

            GL.glDisable(GL.GL_BLEND);
        }

        void DrawWallSection(float x1, float y1, float x2, float y2, float height)
        {
            float texRepeatX = (x2 - x1) / 1.5f;
            float texRepeatY = (y2 - y1) / 1.5f;
            float texRepeatZ = height / 0.5f;

            GL.glBegin(GL.GL_QUADS);

            // קיר חיצוני תחתון
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x1, y1, 0);
            GL.glTexCoord2f(0, texRepeatZ); GL.glVertex3f(x1, y1, height);
            GL.glTexCoord2f(texRepeatX, texRepeatZ); GL.glVertex3f(x2, y1, height);
            GL.glTexCoord2f(texRepeatX, 0); GL.glVertex3f(x2, y1, 0);

            // קיר פנימי עליון
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x2, y2, 0);
            GL.glTexCoord2f(0, texRepeatZ); GL.glVertex3f(x2, y2, height);
            GL.glTexCoord2f(texRepeatX, texRepeatZ); GL.glVertex3f(x1, y2, height);
            GL.glTexCoord2f(texRepeatX, 0); GL.glVertex3f(x1, y2, 0);

            // צד שמאל
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x1, y1, 0);
            GL.glTexCoord2f(0, texRepeatZ); GL.glVertex3f(x1, y1, height);
            GL.glTexCoord2f(texRepeatY, texRepeatZ); GL.glVertex3f(x1, y2, height);
            GL.glTexCoord2f(texRepeatY, 0); GL.glVertex3f(x1, y2, 0);

            // צד ימין
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x2, y2, 0);
            GL.glTexCoord2f(0, texRepeatZ); GL.glVertex3f(x2, y2, height);
            GL.glTexCoord2f(texRepeatY, texRepeatZ); GL.glVertex3f(x2, y1, height);
            GL.glTexCoord2f(texRepeatY, 0); GL.glVertex3f(x2, y1, 0);

            // חלק עליון
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x1, y1, height);
            GL.glTexCoord2f(0, texRepeatY); GL.glVertex3f(x1, y2, height);
            GL.glTexCoord2f(texRepeatX, texRepeatY); GL.glVertex3f(x2, y2, height);
            GL.glTexCoord2f(texRepeatX, 0); GL.glVertex3f(x2, y1, height);

            GL.glEnd();
        }


        void DrawPost(float x, float y, float width, float height)
        {
            float texRepeat = height / 0.3f; 

            GL.glBegin(GL.GL_QUADS);

            // צד קדמי
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x, y, 0);
            GL.glTexCoord2f(0, texRepeat); GL.glVertex3f(x, y, height);
            GL.glTexCoord2f(1, texRepeat); GL.glVertex3f(x + width, y, height);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(x + width, y, 0);

            // צד אחורי
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x + width, y + width, 0);
            GL.glTexCoord2f(0, texRepeat); GL.glVertex3f(x + width, y + width, height);
            GL.glTexCoord2f(1, texRepeat); GL.glVertex3f(x, y + width, height);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(x, y + width, 0);

            // צד שמאל
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x, y + width, 0);
            GL.glTexCoord2f(0, texRepeat); GL.glVertex3f(x, y + width, height);
            GL.glTexCoord2f(1, texRepeat); GL.glVertex3f(x, y, height);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(x, y, 0);

            // צד ימין
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x + width, y, 0);
            GL.glTexCoord2f(0, texRepeat); GL.glVertex3f(x + width, y, height);
            GL.glTexCoord2f(1, texRepeat); GL.glVertex3f(x + width, y + width, height);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(x + width, y + width, 0);

            // חלק עליון
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x, y, height);
            GL.glTexCoord2f(0, 1); GL.glVertex3f(x, y + width, height);
            GL.glTexCoord2f(1, 1); GL.glVertex3f(x + width, y + width, height);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(x + width, y, height);

            GL.glEnd();
        }

        // פונקציה לציור רמזור
        void DrawTrafficLight(float x, float y, float z)
        {
            GL.glPushMatrix();
            GL.glTranslatef(x, y, z);
            GL.glScalef(0.7f, 0.7f, 0.7f); 

            // עמוד הרמזור 
            GL.glColor3f(0.3f, 0.3f, 0.3f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 1.0f);
            GL.glScalef(0.08f, 0.08f, 2.0f);
            DrawCube();
            GL.glPopMatrix();

            // בסיס העמוד
            GL.glColor3f(0.2f, 0.2f, 0.2f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 0.1f);
            GL.glScalef(0.15f, 0.15f, 0.2f);
            DrawCube();
            GL.glPopMatrix();

            // קופסת הרמזור 
            GL.glColor3f(0.1f, 0.1f, 0.1f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 2.2f);
            GL.glScalef(0.3f, 0.2f, 0.8f);
            DrawCube();
            GL.glPopMatrix();

            // מסגרת חיצונית של הקופסה 
            GL.glColor3f(0.0f, 0.0f, 0.0f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 2.2f);
            GL.glScalef(0.32f, 0.22f, 0.82f);
            DrawCube();
            GL.glPopMatrix();

            // הקופסה השחורה שוב מעל המסגרת
            GL.glColor3f(0.1f, 0.1f, 0.1f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 2.2f);
            GL.glScalef(0.28f, 0.18f, 0.78f);
            DrawCube();
            GL.glPopMatrix();

            // אור אדום עליון
            if (trafficLightState == 0) // אדום 
                GL.glColor3f(1.0f, 0.0f, 0.0f);
            else // אדום כבוי
                GL.glColor3f(0.3f, 0.1f, 0.1f);

            GL.glPushMatrix();
            GL.glTranslatef(0, -0.11f, 2.5f);
            GL.glScalef(0.12f, 0.05f, 0.12f);
            DrawCube();
            GL.glPopMatrix();

            // אור צהוב אמצעי
            if (trafficLightState == 1) // צהוב 
                GL.glColor3f(1.0f, 1.0f, 0.0f);
            else // צהוב כבוי
                GL.glColor3f(0.3f, 0.3f, 0.1f);

            GL.glPushMatrix();
            GL.glTranslatef(0, -0.11f, 2.2f);
            GL.glScalef(0.12f, 0.05f, 0.12f);
            DrawCube();
            GL.glPopMatrix();

            // אור ירוק תחתון
            if (trafficLightState == 2) // ירוק דלוק
                GL.glColor3f(0.0f, 1.0f, 0.0f);
            else // ירוק כבוי
                GL.glColor3f(0.1f, 0.3f, 0.1f);

            GL.glPushMatrix();
            GL.glTranslatef(0, -0.11f, 1.9f);
            GL.glScalef(0.12f, 0.05f, 0.12f);
            DrawCube();
            GL.glPopMatrix();

            // כיפת הגנה עליונה
            GL.glColor3f(0.2f, 0.2f, 0.2f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 2.65f);
            GL.glScalef(0.35f, 0.25f, 0.1f);
            DrawCube();
            GL.glPopMatrix();

            GL.glPopMatrix();
        }

        // עדכון מצב הרמזור האוטומטי
        void UpdateTrafficLight()
        {
            if (!autoTrafficLight) return;

            trafficLightTimer += 0.02f; // מהירות השינוי

            if (trafficLightTimer < 3.0f)
            {
                trafficLightState = 0; // אדום
            }
            else if (trafficLightTimer < 4.0f)
            {
                trafficLightState = 1; // צהוב
            }
            else if (trafficLightTimer < 7.0f)
            {
                trafficLightState = 2; // ירוק
            }
            else
            {
                trafficLightTimer = 0.0f; // חזרה להתחלה
            }
        }
        //  לשליטה ידנית ברמזור
        public void SetTrafficLightState(int state)
        {
            autoTrafficLight = false;
            trafficLightState = state; 
        }

        // פונקציה להפעלת מצב אוטומטי
        public void EnableAutoTrafficLight()
        {
            autoTrafficLight = true;
            trafficLightTimer = 0.0f;
        }
        private void setColor()
        {
            if (isColored)
            {
                float r, g, b;
                r = (float)rd.NextDouble();
                g = (float)rd.NextDouble();
                b = (float)rd.NextDouble();
                GL.glColor3f(r, g, b);
            }
            else
                GL.glColor3f(1, 1, 0); // צהוב ברירת מחדל
        }

        void DrawUpgradedWaitingCar(bool isShadow = false)
        {
            GL.glTranslatef(0, 0, 0.25f);
            GL.glRotatef(-180, 0, 0, 1);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT, GL.GL_AMBIENT_AND_DIFFUSE);

            blinkTimer += 0.1f;
            if (blinkTimer > 1.0f) blinkTimer = 0.0f;

            bool showLights = carLightsOn;
            if (carBlinking)
                showLights = blinkTimer < 0.5f;

            void SafeColor(float r, float g, float b)
            {
                if (isShadow)
                    GL.glColor3f(0.2f, 0.2f, 0.2f);
                else
                    GL.glColor3f(r, g, b);
            }

            // בסיס הרכב
            if (!isShadow) setColor(); else SafeColor(0, 0, 0);
            GL.glPushMatrix();
            GL.glScalef(1.5f, 0.6f, 0.3f);
            DrawCube();
            GL.glPopMatrix();

            if (!isShadow) setColor(); else SafeColor(0, 0, 0);
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.0f, 0.25f);
            GL.glScalef(1.2f, 0.5f, 0.4f);
            DrawCube();
            GL.glPopMatrix();

            if (!isShadow) setColor(); else SafeColor(0, 0, 0);
            GL.glPushMatrix();
            GL.glTranslatef(-0.1f, 0.0f, 0.5f);
            GL.glScalef(0.8f, 0.4f, 0.3f);
            DrawCube();
            GL.glPopMatrix();

            if (!isShadow) setColor(); else SafeColor(0, 0, 0);
            GL.glPushMatrix();
            GL.glTranslatef(0.5f, 0.0f, 0.32f);
            GL.glScalef(0.5f, 0.5f, 0.15f);
            DrawCube();
            GL.glPopMatrix();

            SafeColor(0.3f, 0.5f, 0.8f); // שמשות
            GL.glPushMatrix(); GL.glTranslatef(0.3f, 0.0f, 0.55f); GL.glScalef(0.02f, 0.35f, 0.25f); DrawCube(); GL.glPopMatrix();
            GL.glPushMatrix(); GL.glTranslatef(-0.3f, 0.0f, 0.55f); GL.glScalef(0.02f, 0.35f, 0.2f); DrawCube(); GL.glPopMatrix();
            GL.glPushMatrix(); GL.glTranslatef(0.0f, 0.26f, 0.55f); GL.glScalef(0.3f, 0.02f, 0.2f); DrawCube(); GL.glPopMatrix();
            GL.glPushMatrix(); GL.glTranslatef(0.0f, -0.26f, 0.55f); GL.glScalef(0.3f, 0.02f, 0.2f); DrawCube(); GL.glPopMatrix();

            float wheelOffsetX = 0.5f, wheelOffsetY = 0.35f, wheelOffsetZ = -0.1f;
            for (int i = 0; i < 4; i++)
            {
                float x = (i < 2) ? wheelOffsetX : -wheelOffsetX;
                float y = (i % 2 == 0) ? wheelOffsetY : -wheelOffsetY;

                SafeColor(0.1f, 0.1f, 0.1f); // גלגל חיצוני
                GL.glPushMatrix(); GL.glTranslatef(x, y, wheelOffsetZ); GL.glScalef(0.25f, 0.25f, 0.15f); DrawCube(); GL.glPopMatrix();

                SafeColor(0.7f, 0.7f, 0.8f); // חישוק
                GL.glPushMatrix(); GL.glTranslatef(x, y, wheelOffsetZ); GL.glScalef(0.15f, 0.15f, 0.08f); DrawCube(); GL.glPopMatrix();
            }

            // פנסים קדמיים
            if (isShadow)
                SafeColor(0, 0, 0);
            else if (showLights)
                GL.glColor3f(1.0f, 1.0f, 0.9f);
            else
                GL.glColor3f(0.6f, 0.6f, 0.6f);
            GL.glPushMatrix(); GL.glTranslatef(0.76f, 0.2f, 0.25f); GL.glScalef(0.12f, 0.12f, 0.08f); DrawCube(); GL.glPopMatrix();
            GL.glPushMatrix(); GL.glTranslatef(0.76f, -0.2f, 0.25f); GL.glScalef(0.12f, 0.12f, 0.08f); DrawCube(); GL.glPopMatrix();
            GL.glPushMatrix(); GL.glTranslatef(0.76f, 0.1f, 0.4f); GL.glScalef(0.06f, 0.06f, 0.04f); DrawCube(); GL.glPopMatrix();
            GL.glPushMatrix(); GL.glTranslatef(0.76f, -0.1f, 0.4f); GL.glScalef(0.06f, 0.06f, 0.04f); DrawCube(); GL.glPopMatrix();

            SafeColor(0.9f, 0.1f, 0.1f); // אורות אחוריים
            GL.glPushMatrix(); GL.glTranslatef(-0.76f, 0.2f, 0.25f); GL.glScalef(0.08f, 0.1f, 0.06f); DrawCube(); GL.glPopMatrix();
            GL.glPushMatrix(); GL.glTranslatef(-0.76f, -0.2f, 0.25f); GL.glScalef(0.08f, 0.1f, 0.06f); DrawCube(); GL.glPopMatrix();
            GL.glPushMatrix(); GL.glTranslatef(-0.76f, 0.0f, 0.4f); GL.glScalef(0.06f, 0.3f, 0.04f); DrawCube(); GL.glPopMatrix();

            if (!isShadow) setColor(); else SafeColor(0, 0, 0); // פגושים
            GL.glPushMatrix(); GL.glTranslatef(0.8f, 0.0f, 0.1f); GL.glScalef(0.08f, 0.8f, 0.12f); DrawCube(); GL.glPopMatrix();
            GL.glPushMatrix(); GL.glTranslatef(-0.8f, 0.0f, 0.1f); GL.glScalef(0.08f, 0.8f, 0.12f); DrawCube(); GL.glPopMatrix();

            SafeColor(0.2f, 0.2f, 0.2f); // רשת
            GL.glPushMatrix(); GL.glTranslatef(0.75f, 0.0f, 0.35f); GL.glScalef(0.02f, 0.4f, 0.1f); DrawCube(); GL.glPopMatrix();

            SafeColor(0.3f, 0.3f, 0.3f); // אנטנה
            GL.glPushMatrix(); GL.glTranslatef(-0.4f, 0.0f, 0.8f); GL.glScalef(0.01f, 0.01f, 0.2f); DrawCube(); GL.glPopMatrix();

            SafeColor(0.9f, 0.9f, 0.9f); // לוחיות
            GL.glPushMatrix(); GL.glTranslatef(0.78f, 0.0f, 0.0f); GL.glScalef(0.02f, 0.3f, 0.08f); DrawCube(); GL.glPopMatrix();
            GL.glPushMatrix(); GL.glTranslatef(-0.78f, 0.0f, 0.0f); GL.glScalef(0.02f, 0.3f, 0.08f); DrawCube(); GL.glPopMatrix();

            if (!isShadow) setColor(); else SafeColor(0, 0, 0); // מראות
            GL.glPushMatrix(); GL.glTranslatef(0.2f, 0.32f, 0.6f); GL.glScalef(0.05f, 0.08f, 0.05f); DrawCube(); GL.glPopMatrix();
            GL.glPushMatrix(); GL.glTranslatef(0.2f, -0.32f, 0.6f); GL.glScalef(0.05f, 0.08f, 0.05f); DrawCube(); GL.glPopMatrix();
        }

        public void OnResize()
        {
            Width = p.Width;
            Height = p.Height;
            GL.glViewport(0, 0, Width, Height);
            Draw();
        }

        protected virtual void InitializeGL()
        {
            uint hwnd = (uint)p.Handle;
            uint dc = WGL.GetDC(hwnd);
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
            WGL.SetPixelFormat(dc, pixelFormatIndex, ref pfd);
            uint rc = WGL.wglCreateContext(dc);
            WGL.wglMakeCurrent(dc, rc);

            // הפעלת בדיקת עומק
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glDepthFunc(GL.GL_LEQUAL);

            // הפעלת תאורה כללית
            GL.glEnable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_LIGHT0);

            // הגדרת מקור האור מיקום אלכסוני עם עוצמה מעט גבוהה יותר
            float[] light_position = { 12.0f, -12.0f, 10.0f, 1.0f };
            float[] light_ambient = { 0.35f, 0.35f, 0.35f, 1.0f };
            float[] light_diffuse = { 0.75f, 0.75f, 0.75f, 1.0f };
            float[] light_specular = { 0.4f, 0.4f, 0.4f, 1.0f };

            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, light_position);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, light_ambient);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, light_diffuse);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, light_specular);

            // הפעלת חומר מבוסס צבע
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT, GL.GL_AMBIENT_AND_DIFFUSE);

            // צבע רקע שחור שקוף
            GL.glClearColor(0f, 0f, 0f, 0f);

            // הגדרת פרספקטיבה ומטריצות
            GL.glViewport(0, 0, Width, Height);
            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();
            GLU.gluPerspective(45.0f, (float)Width / Height, 1.0f, 5000.0f);
            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();

            // טעינת הטקסטורות
            GenerateTextures();
            LoadSkyboxTextures();

            GL.glEnable(GL.GL_STENCIL_TEST);
            GL.glClearStencil(0);
        }
        public void ToggleGate()
        {
            if (!gateMoving)
            {
                gateMoving = true;
                gateOpen = !gateOpen;
            }
        }
        //פתיחת השער
        public void UpdateGateAnimation()
        {
            if (gateMoving)
            {
                if (gateOpen)
                {
                    gateAnimation += 0.02f; // מהירות פתיחה
                    if (gateAnimation >= 1.0f)
                    {
                        gateAnimation = 1.0f;
                        gateMoving = false;
                    }
                }
                else
                {
                    gateAnimation -= 0.02f; // מהירות סגירה
                    if (gateAnimation <= 0.0f)
                    {
                        gateAnimation = 0.0f;
                        gateMoving = false;
                    }
                }
            }
        }
    }
}