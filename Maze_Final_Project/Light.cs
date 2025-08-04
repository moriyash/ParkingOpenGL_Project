using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
namespace OpenGL
{
    public partial class cOGL
    {

        private void SetupParkingLighting()
        {
            GL.glEnable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_LIGHT0);
            GL.glEnable(GL.GL_NORMALIZE);

            //float[] light_position = { 1.0f, 1.6f, 3.0f, 1.0f };
            //GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, light_position);

            float[] lightPos = { pos[0], pos[1], pos[2], 1.0f };
            float intensity = light1Intensity * 0.3f;  
            float[] bright = { intensity, intensity, intensity, 1.0f };
            float[] dim = { intensity * 0.1f, intensity * 0.1f, intensity * 0.1f, 1.0f };

            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, lightPos);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, bright);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, bright);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, dim);
            //look dark when the light is moving
            GL.glLightf(GL.GL_LIGHT0, GL.GL_CONSTANT_ATTENUATION, 0.1f);
            GL.glLightf(GL.GL_LIGHT0, GL.GL_LINEAR_ATTENUATION, 0.05f);

            SetupSunLighting();
            UpdateLightingColors();
        }
        void DrawLightCircleOnGround(float centerX, float centerY, float radius, int lightType = 1)
        {
            GL.glDisable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);

            float[] color = (lightType == 1) ? light1Color : light2Color;
            float intensity = (lightType == 1) ? light1Intensity : light2Intensity;
            float actualRadius = (lightType == 1) ? light1Radius : light2Radius;

            GL.glBegin(GL.GL_TRIANGLE_FAN);
            GL.glColor4f(
                color[0] * intensity / 3.0f,
                color[1] * intensity / 3.0f,
                color[2] * intensity / 3.0f,
                0.6f
            );
            GL.glVertex3f(centerX, centerY, 0.002f);

            GL.glColor4f(
                color[0] * intensity / 10.0f,
                color[1] * intensity / 10.0f,
                color[2] * intensity / 10.0f,
                0.0f
            );
            for (int i = 0; i <= 32; i++)
            {
                float angle = (float)(i * 2.0 * Math.PI / 32.0);
                float x = centerX + (float)Math.Cos(angle) * actualRadius;
                float y = centerY + (float)Math.Sin(angle) * actualRadius;
                GL.glVertex3f(x, y, 0.002f);
            }
            GL.glEnd();
            GL.glDisable(GL.GL_BLEND);
            GL.glEnable(GL.GL_LIGHTING);
        }
        public void SetLight1Color(float r, float g, float b)
        {
            light1Color[0] = r;
            light1Color[1] = g;
            light1Color[2] = b;
            UpdateLightingColors();
        }
        public void SetLight2Color(float r, float g, float b)
        {
            light2Color[0] = r;
            light2Color[1] = g;
            light2Color[2] = b;
            UpdateLightingColors();
        }
        public void UpdateLightingColors()
        {
            return;
        }
        void DrawFigures()
        {

            GL.glPushMatrix();
            GL.glTranslatef(13.0f, 0.0f, 0.0f); 

            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, pos);

            //GL.glDisable(GL.GL_LIGHTING);
            //GL.glTranslatef(pos[0] / 5 - 5.0f, pos[1] / 5, pos[2] / 5 + 5.0f); 
            //GL.glColor3f(1.0f, 1.0f, 0.0f); 
            //GLU.gluSphere(obj, 0.5f, 16, 16);
            //GL.glTranslatef(-(pos[0] / 5 - 5.0f), -pos[1] / 5, -(pos[2] / 5 + 5.0f)); 
            //GL.glEnable(GL.GL_LIGHTING);

            GL.glTranslatef(-13.0f, 0.0f, 0.0f); 
            DrawRotatingFlowerPots();           
            GL.glTranslatef(13.0f, 0.0f, 0.0f);  

            GL.glPopMatrix();
        }
        public void SetTrafficLightState(int state)
        {
            autoTrafficLight = false;
            trafficLightState = state;
        }
        public void EnableAutoTrafficLight()
        {
            autoTrafficLight = true;
            trafficLightTimer = 0.0f;
        }


        public void SetSunAngle(float angleX, float angleY)
        {
            sunAngleX = angleX;
            sunAngleY = angleY;
            SetupSunLighting();
        }
        public void ToggleSun()
        {
            sunEnabled = !sunEnabled;
            SetupSunLighting();
        }
        private void SetupGlobalAmbient()
        {
            float[] globalAmbient = {
        globalAmbientColor[0] * globalAmbientIntensity,
        globalAmbientColor[1] * globalAmbientIntensity,
        globalAmbientColor[2] * globalAmbientIntensity,
        1.0f
    };

            GL.glLightModelfv(GL.GL_LIGHT_MODEL_AMBIENT, globalAmbient);
            GL.glLightModeli(GL.GL_LIGHT_MODEL_TWO_SIDE, (int)GL.GL_TRUE);
        }
        private void SetupSunLighting()
        {
            if (!sunEnabled)
            {
                GL.glDisable(GL.GL_LIGHT2);
                return;
            }

            GL.glEnable(GL.GL_LIGHT2);

            float radX = (float)(sunAngleX * Math.PI / 180.0);
            float radY = (float)(sunAngleY * Math.PI / 180.0);

            float[] sunDirection = {
        (float)(Math.Cos(radY) * Math.Sin(radX)),
        (float)(Math.Cos(radY) * Math.Cos(radX)),
        (float)(-Math.Sin(radY)),
        0.0f
    };

            GL.glLightfv(GL.GL_LIGHT2, GL.GL_POSITION, sunDirection);

            float[] sunDiffuse = {
        sunColor[0] * sunIntensity,
        sunColor[1] * sunIntensity,
        sunColor[2] * sunIntensity,
        1.0f
    }; //power

            float[] sunAmbient = {
        sunColor[0] * 0.3f,
        sunColor[1] * 0.3f,
        sunColor[2] * 0.3f,
        1.0f
    };

            GL.glLightfv(GL.GL_LIGHT2, GL.GL_DIFFUSE, sunDiffuse);
            GL.glLightfv(GL.GL_LIGHT2, GL.GL_AMBIENT, sunAmbient);
            GL.glLightfv(GL.GL_LIGHT2, GL.GL_SPECULAR, sunDiffuse);
        }
        public void SetGlobalAmbient(float intensity)
        {
            globalAmbientIntensity = intensity;
            SetupGlobalAmbient();
            GL.glLightModeli(GL.GL_LIGHT_MODEL_LOCAL_VIEWER, (int)GL.GL_TRUE);
        }

    }
}
