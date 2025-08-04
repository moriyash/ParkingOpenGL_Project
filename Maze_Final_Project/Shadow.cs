using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
namespace OpenGL
{
    public partial class cOGL
    {
        void MakeShadowMatrix(float[,] ground)
        {
            float[] planeCoeff = new float[4];
            float dot;
            //vectors
            float[] v1 = new float[3];
            float[] v2 = new float[3];
            //point 1 to point 2
            v1[0] = ground[0, 0] - ground[1, 0];
            v1[1] = ground[0, 1] - ground[1, 1];
            v1[2] = ground[0, 2] - ground[1, 2];
            //point 2 to 3
            v2[0] = ground[1, 0] - ground[2, 0];
            v2[1] = ground[1, 1] - ground[2, 1];
            v2[2] = ground[1, 2] - ground[2, 2];
            //normal
            planeCoeff[0] = v1[1] * v2[2] - v1[2] * v2[1];
            planeCoeff[1] = v1[2] * v2[0] - v1[0] * v2[2];
            planeCoeff[2] = v1[0] * v2[1] - v1[1] * v2[0];
            //normal length
            float length = (float)Math.Sqrt(planeCoeff[0] * planeCoeff[0] +
                                           planeCoeff[1] * planeCoeff[1] +
                                           planeCoeff[2] * planeCoeff[2]);
            if (length != 0)
            {
                planeCoeff[0] /= length;
                planeCoeff[1] /= length;
                planeCoeff[2] /= length;
            }
            //d
            planeCoeff[3] = -(planeCoeff[0] * ground[2, 0] +
                              planeCoeff[1] * ground[2, 1] +
                              planeCoeff[2] * ground[2, 2]);
            //light location
            float[] lightPos = { pos[0], pos[1], pos[2], pos[3] };
            //place&light
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
        void DrawStreetLamp(float x, float y, float rotationAngleDeg = 0, int lampType = 1)
        {

            GL.glDisable(GL.GL_LIGHTING);
            //for z-fighting shadow and floor
            GL.glEnable(GL.GL_POLYGON_OFFSET_FILL);
            GL.glPolygonOffset(-2.0f, -4.0f);
            GL.glPushMatrix();
            //shadow
            MakeShadowMatrix(ground);
            GL.glMultMatrixf(cubeXform);

            //shadow
            GL.glColor3f(0.3f, 0.3f, 0.3f);
            GL.glPushMatrix();
            GL.glTranslatef(x + 0.4f, y, 0);
            GL.glRotatef(rotationAngleDeg, 0, 0, 1);
            GL.glScalef(0.6f, 0.6f, 0.6f);

            // pole shadow
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 1.8f);
            GL.glScalef(0.09f, 0.09f, 2.8f);
            DrawCube();
            GL.glPopMatrix();

            // flashlight arm
            GL.glPushMatrix();
            GL.glTranslatef(0.25f, 0, 3.2f);
            GL.glScalef(0.5f, 0.08f, 0.08f);
            DrawCube();
            GL.glPopMatrix();

            // connect arm to pole
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 3.2f);
            GL.glScalef(0.15f, 0.15f, 0.15f);
            DrawCube();
            GL.glPopMatrix();

            //  top
            GL.glPushMatrix();
            GL.glTranslatef(0.45f, 0, 3.25f);
            GL.glScalef(0.35f, 0.35f, 0.1f);
            DrawCube();
            GL.glPopMatrix();

            //return 
            GL.glPopMatrix();
            GL.glPopMatrix();
            GL.glDisable(GL.GL_POLYGON_OFFSET_FILL);
            GL.glEnable(GL.GL_LIGHTING);

            // original street lamp
            GL.glPushMatrix();
            GL.glTranslatef(x + 0.4f, y, 0);
            GL.glRotatef(rotationAngleDeg, 0, 0, 1);
            GL.glScalef(0.6f, 0.6f, 0.6f);

            GL.glColor3f(0.3f, 0.3f, 0.35f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 0.05f);
            GL.glScalef(0.6f, 0.6f, 0.1f);
            DrawCube();
            GL.glPopMatrix();

            GL.glColor3f(0.12f, 0.12f, 0.12f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 0.4f);
            GL.glScalef(0.12f, 0.12f, 0.8f);
            DrawCube();
            GL.glPopMatrix();

            // main pole 
            GL.glColor3f(0.18f, 0.18f, 0.18f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 1.8f);
            GL.glScalef(0.09f, 0.09f, 2.8f);
            DrawCube();
            GL.glPopMatrix();

            GL.glColor3f(0.15f, 0.15f, 0.15f);
            GL.glPushMatrix();
            GL.glTranslatef(0.25f, 0, 3.2f);
            GL.glScalef(0.5f, 0.08f, 0.08f);
            DrawCube();
            GL.glPopMatrix();

            GL.glColor3f(0.1f, 0.1f, 0.1f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 3.2f);
            GL.glScalef(0.15f, 0.15f, 0.15f);
            DrawCube();
            GL.glPopMatrix();
            //light mode
            bool lampIsOn = (lampType == 1) ? light1On : light2On;
            float[] color = (lampType == 1) ? light1Color : light2Color;
            float intensity = (lampType == 1) ? light1Intensity : light2Intensity;

            if (lampIsOn)
            {
                //light color
                GL.glColor3f(
                    color[0] * intensity / 5.0f,
                    color[1] * intensity / 5.0f,
                    color[2] * intensity / 5.0f
                );
                //light form
                GL.glPushMatrix();
                GL.glTranslatef(0.45f, 0, 3.0f);
                GL.glScalef(0.15f, 0.15f, 0.2f);
                DrawCube();
                GL.glPopMatrix();
                //not to be affected by light and normalization
                GL.glDisable(GL.GL_LIGHTING);
                //transparency
                GL.glEnable(GL.GL_BLEND);
                GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE);
                GL.glColor4f(
                    color[0] * intensity / 8.0f,
                    color[1] * intensity / 8.0f,
                    color[2] * intensity / 8.0f,
                    0.3f
                );
                GL.glPushMatrix();
                GL.glTranslatef(0.45f, 0, 3.0f);
                GL.glScalef(0.5f, 0.5f, 0.3f);
                DrawCube();
                GL.glPopMatrix();
                //end
                GL.glDisable(GL.GL_BLEND);
                GL.glEnable(GL.GL_LIGHTING);
            }
            else
            {
                //off
                GL.glColor3f(0.2f, 0.2f, 0.2f);
                GL.glPushMatrix();
                GL.glTranslatef(0.45f, 0, 3.0f);
                GL.glScalef(0.18f, 0.18f, 0.25f);
                DrawCube();
                GL.glPopMatrix();
            }

            GL.glColor3f(0.05f, 0.05f, 0.05f);
            GL.glPushMatrix();
            GL.glTranslatef(0.45f, 0, 3.25f);
            GL.glScalef(0.35f, 0.35f, 0.1f);
            DrawCube();
            GL.glPopMatrix();

            GL.glColor3f(0.2f, 0.2f, 0.3f);
            GL.glPushMatrix();
            GL.glTranslatef(0.45f, 0.12f, 2.9f);
            GL.glScalef(0.04f, 0.04f, 0.04f);
            DrawCube();
            GL.glPopMatrix();

            GL.glPopMatrix();
        }
        void DrawGate(float x, float y)
        {
            float gateAngle = gateAnimation * 90.0f;
            GL.glDisable(GL.GL_LIGHTING);

            if (light1On)
                GL.glDisable(GL.GL_LIGHTING);
            else
                GL.glEnable(GL.GL_LIGHTING);

            GL.glEnable(GL.GL_POLYGON_OFFSET_FILL);
            GL.glPolygonOffset(-2.0f, -4.0f);

            GL.glPushMatrix();
            //for scrollbar
            Vector3 gatePos = objectPositions[1];
            GL.glTranslatef(gatePos.X, gatePos.Y, gatePos.Z);
            GL.glTranslatef(x + gateOffsetX, y + gateOffsetY, 0);
            GL.glRotatef(gateRotation, 0, 0, 1);
            GL.glScalef(gateScale, gateScale, gateScale);
            //shadow
            MakeShadowMatrix(ground);
            GL.glMultMatrixf(cubeXform);

            GL.glColor3f(0.3f, 0.3f, 0.3f);
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 1.8f, 0);
            GL.glRotatef(-270, 0, 0, 1);
            //base
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 0.25f);
            GL.glScalef(0.4f, 0.5f, 0.5f);
            DrawCube();
            GL.glPopMatrix();
            
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.0f, 0.4f);
            //for rotation
            GL.glRotatef(gateAngle, 0, 1, 0);
            GL.glTranslatef(-1.25f, 0, 0);
            GL.glScalef(2.5f, 0.06f, 0.06f);
            DrawCube();
            GL.glPopMatrix();

            GL.glPopMatrix();
            GL.glPopMatrix();
            //end
            GL.glDisable(GL.GL_POLYGON_OFFSET_FILL);
            GL.glEnable(GL.GL_LIGHTING);
            //open/close
            if (light1On)
                GL.glDisable(GL.GL_LIGHTING);
            else
                GL.glEnable(GL.GL_LIGHTING);

            GL.glPushMatrix();
            GL.glTranslatef(gatePos.X, gatePos.Y, gatePos.Z);
            GL.glTranslatef(x + gateOffsetX, y + gateOffsetY, 0);
            GL.glRotatef(gateRotation, 0, 0, 1);
            GL.glScalef(gateScale, gateScale, gateScale);

            GL.glTranslatef(0.0f, 1.8f, 0);
            GL.glRotatef(-270, 0, 0, 1);

            GL.glColor3f(1.0f, 0.8f, 0.0f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(-0.2f, -0.25f, 0.0f);
            GL.glVertex3f(-0.2f, -0.25f, 0.5f);
            GL.glVertex3f(0.2f, -0.25f, 0.5f);
            GL.glVertex3f(0.2f, -0.25f, 0.0f);
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

            GL.glColor3f(0.1f, 0.1f, 0.1f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(-0.15f, -0.2f, 0.51f);
            GL.glVertex3f(-0.15f, 0.2f, 0.51f);
            GL.glVertex3f(0.15f, 0.2f, 0.51f);
            GL.glVertex3f(0.15f, -0.2f, 0.51f);
            GL.glEnd();

            if (gateOpen || gateMoving)
                GL.glColor3f(0.0f, 1.0f, 0.0f);
            else
                GL.glColor3f(1.0f, 0.0f, 0.0f);

            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(-0.06f, 0.06f, 0.52f);
            GL.glVertex3f(-0.06f, 0.15f, 0.52f);
            GL.glVertex3f(0.06f, 0.15f, 0.52f);
            GL.glVertex3f(0.06f, 0.06f, 0.52f);
            GL.glEnd();

            GL.glColor3f(0.0f, 0.0f, 0.0f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(-0.1f, -0.15f, 0.52f);
            GL.glVertex3f(-0.1f, -0.06f, 0.52f);
            GL.glVertex3f(0.1f, -0.06f, 0.52f);
            GL.glVertex3f(0.1f, -0.15f, 0.52f);
            GL.glEnd();

            GL.glColor3f(0.3f, 0.3f, 0.3f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(-0.3f, -0.35f, -0.05f);
            GL.glVertex3f(-0.3f, 0.35f, -0.05f);
            GL.glVertex3f(0.3f, 0.35f, -0.05f);
            GL.glVertex3f(0.3f, -0.35f, -0.05f);
            GL.glEnd();

            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.0f, 0.4f);
            GL.glRotatef(gateAngle, 0, 1, 0);

            float barrierLength = 2.5f;

            GL.glColor3f(0.95f, 0.95f, 0.95f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(0, -0.03f, -0.03f);
            GL.glVertex3f(0, -0.03f, 0.03f);
            GL.glVertex3f(-barrierLength, -0.03f, 0.03f);
            GL.glVertex3f(-barrierLength, -0.03f, -0.03f);

            GL.glVertex3f(0, 0.03f, -0.03f);
            GL.glVertex3f(-barrierLength, 0.03f, -0.03f);
            GL.glVertex3f(-barrierLength, 0.03f, 0.03f);
            GL.glVertex3f(0, 0.03f, 0.03f);

            GL.glVertex3f(0, -0.03f, 0.03f);
            GL.glVertex3f(0, 0.03f, 0.03f);
            GL.glVertex3f(-barrierLength, 0.03f, 0.03f);
            GL.glVertex3f(-barrierLength, -0.03f, 0.03f);

            GL.glVertex3f(-barrierLength, -0.03f, -0.03f);
            GL.glVertex3f(-barrierLength, 0.03f, -0.03f);
            GL.glVertex3f(0, 0.03f, -0.03f);
            GL.glVertex3f(0, -0.03f, -0.03f);
            GL.glEnd();

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

            GL.glPopMatrix();
            GL.glPopMatrix();
            //important
            GL.glEnable(GL.GL_LIGHTING);
        }
        private void DrawLighting(float lotWidth, float lotHeight)
        {
            DrawStreetLamp(8.5f, 2.0f, 90, 2);
            if (light1On)
                DrawLightCircleOnGround(9.0f, 2.0f, light1Radius, 4);

            DrawStreetLamp(12.5f, 5.0f, 270, 1);
            if (light1On)
                DrawLightCircleOnGround(13.0f, 5.0f, light1Radius, 4);

            DrawStreetLamp(8.3f, 4.8f, 270, 2);
            if (light2On)
                DrawLightCircleOnGround(8.7f, 5.2f, light2Radius, 4);
            DrawStreetLamp(11.5f, 2.0f, 90, 1);
            if (light2On)
                DrawLightCircleOnGround(12.0f, 2.0f, light2Radius, 4);
        }
        private void DrawTrafficLights()
        {
            //DrawTrafficLight(11.0f, 1.5f, 0);
            DrawTrafficLight(2.0f, -3.0f, 0);
        }
        void DrawTrafficLight(float x, float y, float z)
        {
            Vector3 trafficLightPos = objectPositions[2];
            float finalX = trafficLightPos.X + x + gateOffsetX;
            float finalY = trafficLightPos.Y + y + gateOffsetY;
            float finalZ = trafficLightPos.Z + z;

            GL.glDisable(GL.GL_LIGHTING);
            //for z-fighting shadow and floor 
            GL.glEnable(GL.GL_POLYGON_OFFSET_FILL);
            GL.glPolygonOffset(-2.0f, -4.0f);

            GL.glPushMatrix();
            GL.glTranslatef(finalX, finalY, finalZ);
            GL.glRotatef(trafficLightRotation, 0, 0, 1);
            GL.glScalef(trafficLightScale, trafficLightScale, trafficLightScale);

            MakeShadowMatrix(ground);
            GL.glMultMatrixf(cubeXform);
            GL.glColor3f(0.3f, 0.3f, 0.3f);

            GL.glPushMatrix();
            GL.glRotatef(textureRotation, 0, 0, 1);
            GL.glScalef(0.7f, 0.7f, 0.7f);

            // shadow pole
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 1.0f);
            GL.glScalef(0.08f, 0.08f, 2.0f);
            DrawCube();
            GL.glPopMatrix();

            // base shadow
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 0.1f);
            GL.glScalef(0.15f, 0.15f, 0.2f);
            DrawCube();
            GL.glPopMatrix();

            // box traffic
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 2.2f);
            GL.glScalef(0.28f, 0.18f, 0.78f);
            DrawCube();
            GL.glPopMatrix();

            // top
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 2.65f);
            GL.glScalef(0.35f, 0.25f, 0.1f);
            DrawCube();
            GL.glPopMatrix();

            GL.glPopMatrix();
            GL.glPopMatrix();

            GL.glDisable(GL.GL_POLYGON_OFFSET_FILL);
            GL.glEnable(GL.GL_LIGHTING);
            // traffic light body
            GL.glPushMatrix();
            GL.glTranslatef(finalX, finalY, finalZ);
            GL.glRotatef(trafficLightRotation, 0, 0, 1);
            GL.glScalef(trafficLightScale, trafficLightScale, trafficLightScale);

            GL.glRotatef(textureRotation, 0, 0, 1);
            GL.glScalef(0.7f, 0.7f, 0.7f);

            // pole
            GL.glColor3f(0.3f, 0.3f, 0.3f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 1.0f);
            GL.glScalef(0.08f, 0.08f, 2.0f);
            DrawCube();
            GL.glPopMatrix();

            // base
            GL.glColor3f(0.2f, 0.2f, 0.2f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 0.1f);
            GL.glScalef(0.15f, 0.15f, 0.2f);
            DrawCube();
            GL.glPopMatrix();

            // box
            GL.glColor3f(0.1f, 0.1f, 0.1f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 2.2f);
            GL.glScalef(0.3f, 0.2f, 0.8f);
            DrawCube();
            GL.glPopMatrix();

            // rounded
            GL.glColor3f(0.0f, 0.0f, 0.0f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 2.2f);
            GL.glScalef(0.32f, 0.22f, 0.82f);
            DrawCube();
            GL.glPopMatrix();

            // box
            GL.glColor3f(0.1f, 0.1f, 0.1f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 2.2f);
            GL.glScalef(0.28f, 0.18f, 0.78f);
            DrawCube();
            GL.glPopMatrix();


            // red
            GL.glColor3f(trafficLightState == 0 ? 1.0f : 0.3f, 0.0f, 0.0f);
            GL.glPushMatrix();
            GL.glTranslatef(0, -0.11f, 2.5f);
            GL.glScalef(0.12f, 0.05f, 0.12f);
            DrawCube();
            GL.glPopMatrix();

            // yellow
            GL.glColor3f(
                trafficLightState == 1 ? 1.0f : 0.3f,
                trafficLightState == 1 ? 1.0f : 0.3f,
                0.0f);
            GL.glPushMatrix();
            GL.glTranslatef(0, -0.11f, 2.2f);
            GL.glScalef(0.12f, 0.05f, 0.12f);
            DrawCube();
            GL.glPopMatrix();

            // green
            GL.glColor3f(0.0f, trafficLightState == 2 ? 1.0f : 0.3f, 0.0f);
            GL.glPushMatrix();
            GL.glTranslatef(0, -0.11f, 1.9f);
            GL.glScalef(0.12f, 0.05f, 0.12f);
            DrawCube();
            GL.glPopMatrix();

            // top
            GL.glColor3f(0.2f, 0.2f, 0.2f);
            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 2.65f);
            GL.glScalef(0.35f, 0.25f, 0.1f);
            DrawCube();
            GL.glPopMatrix();

            GL.glPopMatrix();
        }

    }
}
