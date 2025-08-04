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
            //ApplyMaterialForObject("shiny");

            GL.glNormal3f(0.0f, 0.0f, 1.0f);
            //disk 1
            GL.glColor4f(0.4f, 0.6f, 0.9f, 0.7f);
            GL.glTranslatef(7.0f, 5.0f, 0.002f);
            GLU.gluDisk(obj, 0.0, 0.5, 16, 1);
            GL.glTranslatef(-7.0f, -5.0f, -0.002f);
            //2
            GL.glColor4f(0.6f, 0.4f, 0.9f, 0.7f);
            GL.glTranslatef(8.5f, 5.0f, 0.002f);
            GLU.gluDisk(obj, 0.0, 0.5, 16, 1);
            GL.glTranslatef(-8.5f, -5.0f, -0.002f);
            //3
            GL.glColor4f(0.9f, 0.6f, 0.4f, 0.7f);
            GL.glTranslatef(10.0f, 5.0f, 0.002f);
            GLU.gluDisk(obj, 0.0, 0.5, 16, 1);
            GL.glTranslatef(-10.0f, -5.0f, -0.002f);
            //4
            GL.glColor4f(0.4f, 0.9f, 0.6f, 0.7f);
            GL.glTranslatef(7.0f, -5.0f, 0.002f);
            GLU.gluDisk(obj, 0.0, 0.5, 16, 1);
            GL.glTranslatef(-7.0f, 5.0f, -0.002f);
            //5
            GL.glColor4f(0.9f, 0.4f, 0.6f, 0.7f);
            GL.glTranslatef(8.5f, -5.0f, 0.002f);
            GLU.gluDisk(obj, 0.0, 0.5, 16, 1);
            GL.glTranslatef(-8.5f, 5.0f, -0.002f);
            //6
            GL.glColor4f(0.6f, 0.9f, 0.4f, 0.7f);
            GL.glTranslatef(10.0f, -5.0f, 0.002f);
            GLU.gluDisk(obj, 0.0, 0.5, 16, 1);
            GL.glTranslatef(-10.0f, 5.0f, -0.002f);

            GL.glEnable(GL.GL_LIGHTING);
        }
        void DrawRotatingFlowerPots()
        {
            //location and roation for flowers
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
            //GL.glEnable(GL.GL_TEXTURE_2D);

            GL.glDisable(GL.GL_COLOR_MATERIAL);

            GL.glTexEnvi(GL.GL_TEXTURE_ENV, GL.GL_TEXTURE_ENV_MODE, (int)GL.GL_MODULATE);

            //GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);

            ApplyMaterialForObject("flowerpot");
            GL.glPushMatrix();
            GL.glTranslatef(x, y, 0.0f);

           

            // Pot base
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.0f, 0.2f);
            GL.glScalef(0.3f, 0.3f, 0.4f);
            DrawCube(); 
            GL.glPopMatrix();

            // Middle pot
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.0f, 0.5f);
            GL.glScalef(0.15f, 0.15f, 0.25f);
            DrawCube();
            GL.glPopMatrix();

            // Side blocks
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

            GL.glDisable(GL.GL_TEXTURE_2D); 
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE);
            //enable glcolor for the flower
            GL.glColor3f(0.9f, 0.2f, 0.3f);
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.0f, 0.75f);
            GL.glScalef(0.08f, 0.08f, 0.08f);
            DrawCube();
            GL.glPopMatrix();

            GL.glPopMatrix();

            GL.glDisable(GL.GL_TEXTURE_2D);
        }
        void DrawParkingObjects()
        {

            DrawRotatingFlowerPots();
        }
    }
}