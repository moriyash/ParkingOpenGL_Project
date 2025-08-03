using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography.X509Certificates;
namespace OpenGL
{
    public partial class cOGL
    {
        void DrawParkingWall()
        {
           

            float wallHeight = 0.5f;
            float wallThickness = 0.3f;
            float postWidth = 0.3f;
            float postHeight = 1.0f;

            float lotWidth = 6.5f;
            float lotHeight = 7.0f;
            float wallLeft = -0.6f;
            float wallRight = lotWidth + 4.1f;
            float wallBottom = -0.6f;
            float wallTop = lotHeight + 0.6f;

            float gateStart = lotHeight / 2 - 1.5f;
            float gateEnd = lotHeight / 2 + 2.7f;
            float midX = (wallLeft + wallRight) / 2;
            //GL.glEnable(GL.GL_LIGHTING);
            //GL.glEnable(GL.GL_COLOR_MATERIAL);
            //GL.glColorMaterial(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE);

            //if (shadowsEnabled)
            //{
            //    GL.glDisable(GL.GL_LIGHTING);
            //    GL.glEnable(GL.GL_POLYGON_OFFSET_FILL);
            //    GL.glPolygonOffset(-2.0f, -4.0f);
            //    GL.glPushMatrix();
            //    MakeShadowMatrix(ground);
            //    GL.glMultMatrixf(cubeXform);
            //    GL.glColor3f(0.2f, 0.2f, 0.2f);

            //    DrawPost(wallLeft, wallBottom, postWidth, postHeight);
            //    DrawPost(wallRight - postWidth, wallBottom, postWidth, postHeight);
            //    DrawPost(wallLeft, wallTop - postWidth, postWidth, postHeight);
            //    DrawPost(wallRight - postWidth, wallTop - postWidth, postWidth, postHeight);

            //    DrawPost(wallRight - postWidth, gateStart - postWidth, postWidth, postHeight);
            //    DrawPost(wallRight - postWidth, gateEnd, postWidth, postHeight);

            //    DrawPost(midX - postWidth / 2, wallBottom, postWidth, postHeight);
            //    DrawPost(midX - postWidth / 2, wallTop - postWidth, postWidth, postHeight);

            //    GL.glPopMatrix();
            //    GL.glDisable(GL.GL_POLYGON_OFFSET_FILL);
            //    GL.glEnable(GL.GL_LIGHTING);
            //}
            if (texturesEnabled) { GL.glEnable(GL.GL_TEXTURE_2D); GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[6]); }
            else { GL.glDisable(GL.GL_TEXTURE_2D); }
            GL.glColor3f(0.9f, 0.9f, 0.8f);

            DrawWallSection(wallLeft, wallBottom, wallRight, wallBottom + wallThickness, wallHeight);

            DrawWallSection(wallLeft, wallTop - wallThickness, wallRight, wallTop, wallHeight);

            DrawWallSection(wallLeft, wallBottom, wallLeft + wallThickness, wallTop, wallHeight);

            DrawWallSection(wallRight - wallThickness, wallBottom, wallRight, gateStart, wallHeight);
            DrawWallSection(wallRight - wallThickness, gateEnd, wallRight, wallTop, wallHeight);

            GL.glColor3f(0.8f, 0.8f, 0.8f);

            DrawPost(wallLeft, wallBottom, postWidth, postHeight);
            DrawPost(wallRight - postWidth, wallBottom, postWidth, postHeight);
            DrawPost(wallLeft, wallTop - postWidth, postWidth, postHeight);
            DrawPost(wallRight - postWidth, wallTop - postWidth, postWidth, postHeight);

            DrawPost(wallRight - postWidth, gateStart - postWidth, postWidth, postHeight);
            DrawPost(wallRight - postWidth, gateEnd, postWidth, postHeight);

            DrawPost(midX - postWidth / 2, wallBottom, postWidth, postHeight);
            DrawPost(midX - postWidth / 2, wallTop - postWidth, postWidth, postHeight);

            GL.glBindTexture(GL.GL_TEXTURE_2D, 0);
            GL.glDisable(GL.GL_TEXTURE_2D);

            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glDisable(GL.GL_TEXTURE_2D);
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.4f);

            float glassY = wallTop - 0.05f;
            float glassZTop = postHeight * 0.9f;

            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(wallLeft + postWidth, glassY, 0.0f);
            GL.glVertex3f(wallLeft + postWidth, glassY, glassZTop);
            GL.glVertex3f(wallRight - postWidth, glassY, glassZTop);
            GL.glVertex3f(wallRight - postWidth, glassY, 0.0f);
            GL.glEnd();

            GL.glDisable(GL.GL_BLEND);

            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glDisable(GL.GL_TEXTURE_2D);
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.4f);

            float glassYSouth = wallBottom + 0.05f;
            float glassZTopD = postHeight * 0.9f;

            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(wallLeft + postWidth, glassYSouth, 0.0f);
            GL.glVertex3f(wallLeft + postWidth, glassYSouth, glassZTopD);
            GL.glVertex3f(wallRight - postWidth, glassYSouth, glassZTopD);
            GL.glVertex3f(wallRight - postWidth, glassYSouth, 0.0f);
            GL.glEnd();

            GL.glDisable(GL.GL_BLEND);

            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glDisable(GL.GL_TEXTURE_2D);
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.4f);

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

            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glDisable(GL.GL_TEXTURE_2D);
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.4f);

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

            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glDisable(GL.GL_TEXTURE_2D);
            GL.glColor4f(1.0f, 1.0f, 1.0f, 0.4f);

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

            GL.glEnable(GL.GL_LIGHTING); 
            GL.glNormal3f(0, 0, 1); 

            GL.glBegin(GL.GL_QUADS);

            GL.glNormal3f(0, -1, 0);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x1, y1, 0);
            GL.glTexCoord2f(0, texRepeatZ); GL.glVertex3f(x1, y1, height);
            GL.glTexCoord2f(texRepeatX, texRepeatZ); GL.glVertex3f(x2, y1, height);
            GL.glTexCoord2f(texRepeatX, 0); GL.glVertex3f(x2, y1, 0);

            GL.glNormal3f(0, 1, 0);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x2, y2, 0);
            GL.glTexCoord2f(0, texRepeatZ); GL.glVertex3f(x2, y2, height);
            GL.glTexCoord2f(texRepeatX, texRepeatZ); GL.glVertex3f(x1, y2, height);
            GL.glTexCoord2f(texRepeatX, 0); GL.glVertex3f(x1, y2, 0);

            GL.glNormal3f(-1, 0, 0);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x1, y1, 0);
            GL.glTexCoord2f(0, texRepeatZ); GL.glVertex3f(x1, y1, height);
            GL.glTexCoord2f(texRepeatY, texRepeatZ); GL.glVertex3f(x1, y2, height);
            GL.glTexCoord2f(texRepeatY, 0); GL.glVertex3f(x1, y2, 0);

            GL.glNormal3f(1, 0, 0);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x2, y2, 0);
            GL.glTexCoord2f(0, texRepeatZ); GL.glVertex3f(x2, y2, height);
            GL.glTexCoord2f(texRepeatY, texRepeatZ); GL.glVertex3f(x2, y1, height);
            GL.glTexCoord2f(texRepeatY, 0); GL.glVertex3f(x2, y1, 0);

            GL.glNormal3f(0, 0, 1);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x1, y1, height);
            GL.glTexCoord2f(0, texRepeatY); GL.glVertex3f(x1, y2, height);
            GL.glTexCoord2f(texRepeatX, texRepeatY); GL.glVertex3f(x2, y2, height);
            GL.glTexCoord2f(texRepeatX, 0); GL.glVertex3f(x2, y1, height);

            GL.glEnd();
        }
        void DrawPost(float x, float y, float width, float height)
        {
            float texRepeat = height / 0.3f;

            GL.glEnable(GL.GL_LIGHTING);
            GL.glBegin(GL.GL_QUADS);

            GL.glNormal3f(0, -1, 0);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x, y, 0);
            GL.glTexCoord2f(0, texRepeat); GL.glVertex3f(x, y, height);
            GL.glTexCoord2f(1, texRepeat); GL.glVertex3f(x + width, y, height);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(x + width, y, 0);

            GL.glNormal3f(0, 1, 0);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x + width, y + width, 0);
            GL.glTexCoord2f(0, texRepeat); GL.glVertex3f(x + width, y + width, height);
            GL.glTexCoord2f(1, texRepeat); GL.glVertex3f(x, y + width, height);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(x, y + width, 0);

            GL.glNormal3f(-1, 0, 0);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x, y + width, 0);
            GL.glTexCoord2f(0, texRepeat); GL.glVertex3f(x, y + width, height);
            GL.glTexCoord2f(1, texRepeat); GL.glVertex3f(x, y, height);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(x, y, 0);

            GL.glNormal3f(1, 0, 0);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x + width, y, 0);
            GL.glTexCoord2f(0, texRepeat); GL.glVertex3f(x + width, y, height);
            GL.glTexCoord2f(1, texRepeat); GL.glVertex3f(x + width, y + width, height);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(x + width, y + width, 0);

            GL.glNormal3f(0, 0, 1);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(x, y, height);
            GL.glTexCoord2f(0, 1); GL.glVertex3f(x, y + width, height);
            GL.glTexCoord2f(1, 1); GL.glVertex3f(x + width, y + width, height);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(x + width, y, height);

            GL.glEnd();
        }
    }
}
