using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
namespace OpenGL
{
    public partial class cOGL
    {
        void DrawCube()
        {
            float size = 1.0f;
            float half = size / 2.0f;

            GL.glBegin(GL.GL_QUADS);

            // front
            GL.glNormal3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(-half, -half, half);
            GL.glVertex3f(half, -half, half);
            GL.glVertex3f(half, half, half);
            GL.glVertex3f(-half, half, half);

            // back
            GL.glNormal3f(0.0f, 0.0f, -1.0f);
            GL.glVertex3f(-half, -half, -half);
            GL.glVertex3f(-half, half, -half);
            GL.glVertex3f(half, half, -half);
            GL.glVertex3f(half, -half, -half);

            // top
            GL.glNormal3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(-half, half, -half);
            GL.glVertex3f(-half, half, half);
            GL.glVertex3f(half, half, half);
            GL.glVertex3f(half, half, -half);

            // button
            GL.glNormal3f(0.0f, -1.0f, 0.0f);
            GL.glVertex3f(-half, -half, -half);
            GL.glVertex3f(half, -half, -half);
            GL.glVertex3f(half, -half, half);
            GL.glVertex3f(-half, -half, half);

            // right
            GL.glNormal3f(1.0f, 0.0f, 0.0f);
            GL.glVertex3f(half, -half, -half);
            GL.glVertex3f(half, half, -half);
            GL.glVertex3f(half, half, half);
            GL.glVertex3f(half, -half, half);

            // left
            GL.glNormal3f(-1.0f, 0.0f, 0.0f);
            GL.glVertex3f(-half, -half, -half);
            GL.glVertex3f(-half, -half, half);
            GL.glVertex3f(-half, half, half);
            GL.glVertex3f(-half, half, -half);

            GL.glEnd();
        }
        void DrawSecurityBooth(float x, float y)
        {

            if (texturesEnabled)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);
            }
            else
            {
                GL.glColor3f(0.9f, 0.9f, 0.8f);
            }

            GL.glPushMatrix();
            GL.glTranslatef(x -1.5f, y + 1.5f, 0);
            GL.glScalef(0.5f, 0.6f, 0.8f);

            // base
            GL.glColor3f(1.0f, 1.0f, 1.0f);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(0, 0, 0);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(0, 2.0f, 0);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(2.5f, 2.0f, 0);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(2.5f, 0, 0);
            GL.glEnd();

            GL.glColor3f(1.0f, 1.0f, 1.0f);

            GL.glBegin(GL.GL_QUADS);

            // under window
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(0, 0, 0);
            GL.glTexCoord2f(0.0f, 0.1f); GL.glVertex3f(0, 0, 0.2f);
            GL.glTexCoord2f(1.0f, 0.1f); GL.glVertex3f(2.5f, 0, 0.2f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(2.5f, 0, 0);

            // top window
            GL.glTexCoord2f(0.0f, 0.9f); GL.glVertex3f(0, 0, 1.8f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(0, 0, 2.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(2.5f, 0, 2.0f);
            GL.glTexCoord2f(1.0f, 0.9f); GL.glVertex3f(2.5f, 0, 1.8f);

            // left window
            GL.glTexCoord2f(0.0f, 0.1f); GL.glVertex3f(0, 0, 0.2f);
            GL.glTexCoord2f(0.0f, 0.9f); GL.glVertex3f(0, 0, 1.8f);
            GL.glTexCoord2f(0.08f, 0.9f); GL.glVertex3f(0.2f, 0, 1.8f);
            GL.glTexCoord2f(0.08f, 0.1f); GL.glVertex3f(0.2f, 0, 0.2f);

            // right window
            GL.glTexCoord2f(0.92f, 0.1f); GL.glVertex3f(2.3f, 0, 0.2f);
            GL.glTexCoord2f(0.92f, 0.9f); GL.glVertex3f(2.3f, 0, 1.8f);
            GL.glTexCoord2f(1.0f, 0.9f); GL.glVertex3f(2.5f, 0, 1.8f);
            GL.glTexCoord2f(1.0f, 0.1f); GL.glVertex3f(2.5f, 0, 0.2f);

            GL.glEnd();

            // door wall
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(2.5f, 0, 0);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(2.5f, 0, 2.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(2.5f, 2.0f, 2.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(2.5f, 2.0f, 0);
            GL.glEnd();

            // button wall
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(2.5f, 2.0f, 0);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(2.5f, 2.0f, 2.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(0, 2.0f, 2.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(0, 2.0f, 0);
            GL.glEnd();

            // left wall
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(0, 2.0f, 0);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(0, 2.0f, 2.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(0, 0, 2.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(0, 0, 0);
            GL.glEnd();

            // top
            GL.glColor3f(0.85f, 0.85f, 0.85f);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-0.1f, -0.1f, 2.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-0.1f, 2.1f, 2.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(2.6f, 2.1f, 2.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(2.6f, -0.1f, 2.0f);
            GL.glEnd();

            GL.glBindTexture(GL.GL_TEXTURE_2D, 0);
            //object inside
            GL.glColor3f(0.4f, 0.25f, 0.1f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(1.0f, 1.0f, 0.0f); 
            GL.glVertex3f(1.0f, 1.0f, 0.5f); 
            GL.glVertex3f(1.5f, 1.0f, 0.5f); 
            GL.glVertex3f(1.5f, 1.0f, 0.0f); 
            GL.glEnd();

            //object inside
            GL.glColor3f(0.5f, 0.3f, 0.1f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(0.95f, 0.95f, 0.5f);
            GL.glVertex3f(0.95f, 1.55f, 0.5f);
            GL.glVertex3f(1.55f, 1.55f, 0.5f);
            GL.glVertex3f(1.55f, 0.95f, 0.5f);
            GL.glEnd();
            //  reflection    
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);

            GL.glColor4f(0.5f, 0.7f, 1.0f, 0.6f); 
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(0.2f, -0.01f, 0.2f);  
            GL.glVertex3f(0.2f, -0.01f, 1.8f);  
            GL.glVertex3f(2.3f, -0.01f, 1.8f);  
            GL.glVertex3f(2.3f, -0.01f, 0.2f);  
            GL.glEnd();

            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.3f); 
            GL.glBegin(GL.GL_QUADS);

            GL.glVertex3f(0.4f, -0.005f, 1.2f);
            GL.glVertex3f(0.4f, -0.005f, 1.6f);
            GL.glVertex3f(1.2f, -0.005f, 1.6f);
            GL.glVertex3f(1.2f, -0.005f, 1.2f);
            GL.glEnd();

            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.2f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(1.5f, -0.005f, 0.5f);
            GL.glVertex3f(1.5f, -0.005f, 0.8f);
            GL.glVertex3f(2.0f, -0.005f, 0.8f);
            GL.glVertex3f(2.0f, -0.005f, 0.5f);
            GL.glEnd();

            GL.glDisable(GL.GL_BLEND);
            // rounded window
            GL.glColor3f(0.2f, 0.2f, 0.2f);
            GL.glLineWidth(3.0f);
            GL.glBegin(GL.GL_LINE_LOOP);
            GL.glVertex3f(0.2f, -0.02f, 0.2f);
            GL.glVertex3f(0.2f, -0.02f, 1.8f);
            GL.glVertex3f(2.3f, -0.02f, 1.8f);
            GL.glVertex3f(2.3f, -0.02f, 0.2f);
            GL.glEnd();

            // door
            GL.glColor3f(0.3f, 0.3f, 0.3f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(2.51f, 0.3f, 0.2f);
            GL.glVertex3f(2.51f, 0.3f, 1.8f);
            GL.glVertex3f(2.51f, 1.7f, 1.8f);
            GL.glVertex3f(2.51f, 1.7f, 0.2f);
            GL.glEnd();

            // door
            GL.glColor3f(0.8f, 0.8f, 0.8f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(2.52f, 1.5f, 0.9f);
            GL.glVertex3f(2.52f, 1.5f, 1.0f);
            GL.glVertex3f(2.52f, 1.6f, 1.0f);
            GL.glVertex3f(2.52f, 1.6f, 0.9f);
            GL.glEnd();

            GL.glPopMatrix();
            //GL.glBindTexture(GL.GL_TEXTURE_2D, 0);
            //GL.glDisable(GL.GL_TEXTURE_2D);
        }
        void DrawGate(float x, float y)
        {
            float gateAngle = gateAnimation * 90.0f;

            if (light1On)
                GL.glDisable(GL.GL_LIGHTING);
            else
                GL.glEnable(GL.GL_LIGHTING);

            GL.glEnable(GL.GL_POLYGON_OFFSET_FILL);
            GL.glPolygonOffset(-2.0f, -4.0f);

            GL.glPushMatrix();

            Vector3 gatePos = objectPositions[1];
            GL.glTranslatef(gatePos.X, gatePos.Y, gatePos.Z);
            GL.glTranslatef(x + gateOffsetX, y + gateOffsetY, 0);
            GL.glRotatef(gateRotation, 0, 0, 1);
            GL.glScalef(gateScale, gateScale, gateScale);

            MakeShadowMatrix(ground);
            GL.glMultMatrixf(cubeXform);

            GL.glColor3f(0.3f, 0.3f, 0.3f);

            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 1.8f, 0);
            GL.glRotatef(-270, 0, 0, 1);

            GL.glPushMatrix();
            GL.glTranslatef(0, 0, 0.25f);
            GL.glScalef(0.4f, 0.5f, 0.5f);
            DrawCube();
            GL.glPopMatrix();

            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.0f, 0.4f);
            GL.glRotatef(gateAngle, 0, 1, 0);
            GL.glTranslatef(-1.25f, 0, 0);
            GL.glScalef(2.5f, 0.06f, 0.06f);
            DrawCube();
            GL.glPopMatrix();

            GL.glPopMatrix();
            GL.glPopMatrix();

            GL.glDisable(GL.GL_POLYGON_OFFSET_FILL);
            GL.glEnable(GL.GL_LIGHTING);

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
            GL.glEnable(GL.GL_LIGHTING);
        }
        public void ToggleGate()
        {
            if (!gateMoving)
            {
                gateMoving = true;
                gateOpen = !gateOpen;
            }
        }
        public void UpdateGateAnimation()
        {
            if (gateMoving)
            {
                if (gateOpen)
                {
                    gateAnimation += 0.02f; 
                    if (gateAnimation >= 1.0f)
                    {
                        gateAnimation = 1.0f;
                        gateMoving = false;
                    }
                }
                else
                {
                    gateAnimation -= 0.02f; 
                    if (gateAnimation <= 0.0f)
                    {
                        gateAnimation = 0.0f;
                        gateMoving = false;
                    }
                }
            }
        }
        void DrawTrafficLight(float x, float y, float z)
        {
            Vector3 trafficLightPos = objectPositions[2];
            float finalX = trafficLightPos.X + x + gateOffsetX;
            float finalY = trafficLightPos.Y + y + gateOffsetY;
            float finalZ = trafficLightPos.Z + z;

            GL.glDisable(GL.GL_LIGHTING);
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
        void UpdateTrafficLight()
        {
            if (!autoTrafficLight) return;

            trafficLightTimer += 0.02f; 

            if (trafficLightTimer < 3.0f)
            {
                trafficLightState = 0; // red
            }
            else if (trafficLightTimer < 4.0f)
            {
                trafficLightState = 1; // yellow
            }
            else if (trafficLightTimer < 7.0f)
            {
                trafficLightState = 2; // green
            }
            else
            {
                trafficLightTimer = 0.0f; 
            }
        }
        public void InitializeCarRoutes()
        {
            carCurrentX[0] = 9.0f; carCurrentY[0] = 0.0f; carCurrentAngle[0] = 90f;      
            carCurrentX[1] = 8.0f; carCurrentY[1] = -7.5f; carCurrentAngle[1] = 90f;    
            carCurrentX[2] = 0.15f; carCurrentY[2] = -2.2f; carCurrentAngle[2] = 90f;    
            carCurrentX[3] = 0.15f; carCurrentY[3] = 2.0f; carCurrentAngle[3] = 90f;

            //car1 
            carRouteX[0, 0] = 9.0f; carRouteY[0, 0] = 0.0f; carRouteAngle[0, 0] = 90f;  
            carRouteX[0, 1] = 1.0f; carRouteY[0, 1] = 0.0f; carRouteAngle[0, 1] = 90f;    
            carRouteX[0, 2] = 2.0f; carRouteY[0, 2] = 1.5f; carRouteAngle[0, 2] = 45f;   
            carRouteX[0, 3] = 1.5f; carRouteY[0, 3] = 2.0f; carRouteAngle[0, 3] = 0f;     
            carRouteLength[0] = 4;

            // car2 
            carRouteX[1, 0] = 8.0f; carRouteY[1, 0] = -7.5f; carRouteAngle[1, 0] = 90f;    
            carRouteX[1, 1] = -2.5f; carRouteY[1, 1] = -7.5f; carRouteAngle[1, 1] = 90f;    
            carRouteLength[1] = 2;

        }
        public void StartCar(int carId)
        {
            carIsMoving[carId] = true;
            carCurrentWaypoint[carId] = 0;
            carProgress[carId] = 0f;

            carCurrentX[carId] = carRouteX[carId, 0];
            carCurrentY[carId] = carRouteY[carId, 0];
            carCurrentAngle[carId] = carRouteAngle[carId, 0];
        }
        public void UpdateCars()
        {
            for (int carId = 0; carId < 4; carId++)
            {
                if (!carIsMoving[carId]) continue;

                if (carCurrentWaypoint[carId] >= carRouteLength[carId] - 1)
                {
                    carIsMoving[carId] = false;
                    continue;
                }

                int currentPoint = carCurrentWaypoint[carId];
                int nextPoint = currentPoint + 1;

                carProgress[carId] += carSpeed * 0.01f;

                if (carProgress[carId] >= 1.0f)
                {
                    carCurrentWaypoint[carId]++;
                    carProgress[carId] = 0f;

                    if (carCurrentWaypoint[carId] < carRouteLength[carId])
                    {
                        carCurrentX[carId] = carRouteX[carId, carCurrentWaypoint[carId]];
                        carCurrentY[carId] = carRouteY[carId, carCurrentWaypoint[carId]];
                        carCurrentAngle[carId] = carRouteAngle[carId, carCurrentWaypoint[carId]];
                    }
                }
                else
                {
                    float t = carProgress[carId];
                    carCurrentX[carId] = carRouteX[carId, currentPoint] +
                                       (carRouteX[carId, nextPoint] - carRouteX[carId, currentPoint]) * t;
                    carCurrentY[carId] = carRouteY[carId, currentPoint] +
                                       (carRouteY[carId, nextPoint] - carRouteY[carId, currentPoint]) * t;

                    float angleDiff = carRouteAngle[carId, nextPoint] - carRouteAngle[carId, currentPoint];
                    if (angleDiff > 180) angleDiff -= 360;
                    if (angleDiff < -180) angleDiff += 360;
                    carCurrentAngle[carId] = carRouteAngle[carId, currentPoint] + angleDiff * t;
                }
            }
        }
        private void DrawCars()
        {
            if (carModel == null) return;

            GL.glPushAttrib(GL.GL_ALL_ATTRIB_BITS);
            GL.glEnable(GL.GL_LIGHTING);

            ApplyMaterialForObject("car");

            if (texturesEnabled)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glTexEnvi(GL.GL_TEXTURE_ENV, GL.GL_TEXTURE_ENV_MODE, (int)GL.GL_MODULATE);
            }

            for (int i = 0; i < 4; i++)
            {
                GL.glPushMatrix();
                GL.glTranslatef(carCurrentX[i], carCurrentY[i], 0.3f);
                GL.glRotatef(carCurrentAngle[i], 0, 0, 1);
                GL.glRotatef(carRotationAngle[i], 0, 0, 1);
                GL.glRotatef(90, 1, 0, 0);
                GL.glScalef(0.02f, 0.02f, 0.02f);

                carModel.DrawModel();
                GL.glPopMatrix();
            }

            GL.glPopAttrib();
        }
        public void SetCarPosition(float x, float y, float angle)
        {
            carCurrentX[0] = x;
            carCurrentY[0] = y;
            carCurrentAngle[0] = angle;
        }
        void DrawStreetLamp(float x, float y, float rotationAngleDeg = 0, int lampType = 1)
        {

            GL.glDisable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_POLYGON_OFFSET_FILL);
            GL.glPolygonOffset(-2.0f, -4.0f);

            GL.glPushMatrix();
            MakeShadowMatrix(ground);
            GL.glMultMatrixf(cubeXform);


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

            // protecting top
            GL.glPushMatrix();
            GL.glTranslatef(0.45f, 0, 3.25f);
            GL.glScalef(0.35f, 0.35f, 0.1f);
            DrawCube();
            GL.glPopMatrix();

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

            // strips on the pole
            GL.glColor3f(0.25f, 0.25f, 0.25f);
            for (int i = 1; i <= 3; i++)
            {
                GL.glPushMatrix();
                GL.glTranslatef(0, 0, 1.0f + i * 0.7f);
                GL.glScalef(0.11f, 0.11f, 0.05f);
                DrawCube();
                GL.glPopMatrix();
            }

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

            bool lampIsOn = (lampType == 1) ? light1On : light2On;
            float[] color = (lampType == 1) ? light1Color : light2Color;
            float intensity = (lampType == 1) ? light1Intensity : light2Intensity;

            if (lampIsOn)
            {
                GL.glColor3f(
                    color[0] * intensity / 5.0f,
                    color[1] * intensity / 5.0f,
                    color[2] * intensity / 5.0f
                );
                GL.glPushMatrix();
                GL.glTranslatef(0.45f, 0, 3.0f);
                GL.glScalef(0.15f, 0.15f, 0.2f);
                DrawCube();
                GL.glPopMatrix();

                GL.glDisable(GL.GL_LIGHTING);
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
    }
}
