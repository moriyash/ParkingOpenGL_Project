using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
namespace OpenGL
{
    public partial class cOGL
    {
        void DrawFloor()
        {
            GL.glDisable(GL.GL_LIGHTING); 
            ApplyMaterialForObject("shiny");

            GL.glNormal3f(0.0f, 0.0f, 1.0f);
            GL.glColor4f(0.4f, 0.6f, 0.9f, 0.7f);
            GL.glTranslatef(7.0f, 5.0f, 0.002f); 
            GLU.gluDisk(obj, 0.0, 0.5, 16, 1);
            GL.glTranslatef(-7.0f, -5.0f, -0.002f);

            GL.glColor4f(0.6f, 0.4f, 0.9f, 0.7f);
            GL.glTranslatef(8.5f, 5.0f, 0.002f); 
            GLU.gluDisk(obj, 0.0, 0.5, 16, 1);
            GL.glTranslatef(-8.5f, -5.0f, -0.002f);

            GL.glColor4f(0.9f, 0.6f, 0.4f, 0.7f);
            GL.glTranslatef(10.0f, 5.0f, 0.002f); 
            GLU.gluDisk(obj, 0.0, 0.5, 16, 1);
            GL.glTranslatef(-10.0f, -5.0f, -0.002f);

            GL.glColor4f(0.4f, 0.9f, 0.6f, 0.7f);
            GL.glTranslatef(7.0f, -5.0f, 0.002f); 
            GLU.gluDisk(obj, 0.0, 0.5, 16, 1);
            GL.glTranslatef(-7.0f, 5.0f, -0.002f);

            GL.glColor4f(0.9f, 0.4f, 0.6f, 0.7f);
            GL.glTranslatef(8.5f, -5.0f, 0.002f); 
            GLU.gluDisk(obj, 0.0, 0.5, 16, 1);
            GL.glTranslatef(-8.5f, 5.0f, -0.002f);

            GL.glColor4f(0.6f, 0.9f, 0.4f, 0.7f);
            GL.glTranslatef(10.0f, -5.0f, 0.002f); 
            GLU.gluDisk(obj, 0.0, 0.5, 16, 1);
            GL.glTranslatef(-10.0f, 5.0f, -0.002f);

            GL.glEnable(GL.GL_LIGHTING); 
        }
        void DrawRotatingFlowerPots()
        {
            //1
            GL.glPushMatrix();
            GL.glTranslatef(7.0f, 5.0f, 0.1f);
            GL.glRotatef(intOptionB, 0, 0, 1);
            DrawFlowerPot(0.0f, 0.0f);
            GL.glPopMatrix();
            //2
            GL.glPushMatrix();
            GL.glTranslatef(8.5f, 5.0f, 0.1f);
            GL.glRotatef(-intOptionB * 0.7f, 0, 0, 1);
            DrawFlowerPot(0.0f, 0.0f);
            GL.glPopMatrix();
            //3
            GL.glPushMatrix();
            GL.glTranslatef(10.0f, 5.0f, 0.1f);
            GL.glRotatef(intOptionB * 1.2f, 0, 0, 1);
            DrawFlowerPot(0.0f, 0.0f);
            GL.glPopMatrix();
            //4
            GL.glPushMatrix();
            GL.glTranslatef(7.0f, -5.0f, 0.1f);
            GL.glRotatef(-intOptionB * 0.5f, 0, 0, 1);
            DrawFlowerPot(0.0f, 0.0f);
            GL.glPopMatrix();
            //5
            GL.glPushMatrix();
            GL.glTranslatef(8.5f, -5.0f, 0.1f);
            GL.glRotatef(intOptionB * 0.8f, 0, 0, 1);
            DrawFlowerPot(0.0f, 0.0f);
            GL.glPopMatrix();
            //6
            GL.glPushMatrix();
            GL.glTranslatef(10.0f, -5.0f, 0.2f);
            GL.glRotatef(-intOptionB * 1.5f, 0, 0, 1);
            DrawFlowerPot(0.0f, 0.0f);
            GL.glPopMatrix();
        }
        void DrawFlowerPot(float x, float y)
        {
            GL.glEnable(GL.GL_LIGHTING);

            ApplyMaterialForObject("flowerpot");

            GL.glPushMatrix();
            GL.glTranslatef(x, y, 0.0f);

            GL.glColor3f(1.0f, 1.0f, 1.0f);
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.0f, 0.2f);
            GL.glScalef(0.3f, 0.3f, 0.4f);
            DrawCube();
            GL.glPopMatrix();

            GL.glColor3f(1.0f, 1.0f, 1.0f); 

            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.0f, 0.5f);
            GL.glScalef(0.15f, 0.15f, 0.25f);
            DrawCube();
            GL.glPopMatrix();

            GL.glPushMatrix();
            GL.glTranslatef(0.15f, 0.0f, 0.6f);
            GL.glScalef(0.12f, 0.08f, 0.15f);
            DrawCube();
            GL.glPopMatrix();

            GL.glPushMatrix();
            GL.glTranslatef(-0.15f, 0.0f, 0.6f);
            GL.glScalef(0.12f, 0.08f, 0.15f);
            DrawCube();
            GL.glPopMatrix();

            GL.glColor3f(0.9f, 0.2f, 0.3f);
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.0f, 0.75f);
            GL.glScalef(0.08f, 0.08f, 0.08f);
            DrawCube();
            GL.glPopMatrix();

            GL.glPopMatrix();
        }
        void DrawParkingObjects()
        {

            DrawRotatingFlowerPots();
        }
    }
}
