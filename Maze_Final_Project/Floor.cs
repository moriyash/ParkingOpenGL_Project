using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
namespace OpenGL
{
    public partial class cOGL
    {
        private void DrawParkingBackground(float lotWidth, float lotHeight)
        {
            float marginLeft = 0.6f;
            float marginRight = 6.0f;
            float marginBottom = 0.6f;
            float marginTop = 1.6f;

            float left = -marginLeft;
            float right = lotWidth + marginRight;
            float bottom = -marginBottom;
            float top = lotHeight + marginTop;

            GL.glEnable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE);
            GL.glEnable(GL.GL_NORMALIZE); 

            GL.glColor3f(1.0f, 1.0f, 1.0f);

            if (texturesEnabled)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[2]);
            }
            else
            {
                GL.glDisable(GL.GL_TEXTURE_2D);
            }

            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3f(0, 0, 1); 

            GL.glTexCoord2f(0, 0); GL.glVertex3f(left, bottom, -0.01f);
            GL.glTexCoord2f(0, 3); GL.glVertex3f(left, top, -0.01f);
            GL.glTexCoord2f(4, 3); GL.glVertex3f(right, top, -0.01f);
            GL.glTexCoord2f(4, 0); GL.glVertex3f(right, bottom, -0.01f);

            GL.glEnd();

            GL.glDisable(GL.GL_TEXTURE_2D); 
        }
        private void DrawParkingSurfaces(float lotWidth, float lotHeight, float roadGap)
        {
            float parkingLeft = 0f;
            float parkingRight = 8.3f;
            float upperOffsetY = 0.5f;

            float upperParkingTop = lotHeight+0.1f  + upperOffsetY;
            float upperParkingBottom = lotHeight / 2 + roadGap / 2 + upperOffsetY;

            //GL.glEnable(GL.GL_LIGHTING);
            //GL.glEnable(GL.GL_COLOR_MATERIAL);
            //GL.glColorMaterial(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE);
            if (texturesEnabled) { GL.glEnable(GL.GL_TEXTURE_2D); GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]); } else { GL.glDisable(GL.GL_TEXTURE_2D); }
            GL.glColor3f(1.0f, 1.0f, 1.0f); ;


            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(parkingLeft, upperParkingBottom, 0);
            GL.glTexCoord2f(0, 1); GL.glVertex3f(parkingLeft, upperParkingTop, 0);
            GL.glTexCoord2f(1, 1); GL.glVertex3f(parkingRight, upperParkingTop, 0);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(parkingRight, upperParkingBottom, 0);
            GL.glEnd();

            float lowerParkingTop = (lotHeight+0.6f) / 2 - roadGap / 2;
            float lowerParkingBottom = 0.2f;

            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(parkingLeft, lowerParkingBottom, 0);
            GL.glTexCoord2f(0, 1); GL.glVertex3f(parkingLeft, lowerParkingTop, 0);
            GL.glTexCoord2f(1, 1); GL.glVertex3f(parkingRight, lowerParkingTop, 0);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(parkingRight, lowerParkingBottom, 0);
            GL.glEnd();

            ResetTextureMatrix();

            GL.glDisable(GL.GL_TEXTURE_2D);
        }
        private void DrawRoads(float lotWidth, float lotHeight, float roadGap)
        {
            DrawMainRoad(lotWidth, lotHeight, roadGap);
            DrawEntranceRoad(lotWidth, lotHeight, roadGap);
            DrawStopLines(lotWidth, lotHeight, roadGap);
        }
        private void DrawMainRoad(float lotWidth, float lotHeight, float roadGap)
        {
            float roadTop = lotHeight / 2 + roadGap / 2 + 1.0f;
            float roadBottom = lotHeight / 2 - roadGap / 2;
            float roadLeft = 0.5f;
            float roadRight = 15.5f;
            float shiftX = -1.0f;
            float shiftY = -7.5f;
            if (texturesEnabled) { GL.glEnable(GL.GL_TEXTURE_2D); GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[7]); } else { GL.glDisable(GL.GL_TEXTURE_2D); }
            GL.glColor3f(1.0f, 1.0f, 1.0f);

            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(roadLeft + shiftX, roadBottom + shiftY, 0);
            GL.glTexCoord2f(0, 1); GL.glVertex3f(roadLeft + shiftX, roadTop + shiftY, 0);
            GL.glTexCoord2f(1, 1); GL.glVertex3f(roadRight + shiftX, roadTop + shiftY, 0);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(roadRight + shiftX, roadBottom + shiftY, 0);
            GL.glEnd();

            GL.glDisable(GL.GL_TEXTURE_2D);
        }
        private void DrawEntranceRoad(float lotWidth, float lotHeight, float roadGap)
        {
            float offsetYWaiting1 = 0.5f;
            float extraY = 0.3f;
            float waitingAreaLeft1 = lotWidth + 2.0f;   
            float waitingAreaRight1 = lotWidth + 6.0f;  
            float waitingAreaBottom1 = lotHeight / 2 - roadGap / 2 - extraY + offsetYWaiting1;
            float waitingAreaTop1 = lotHeight / 2 + roadGap / 2 + extraY + offsetYWaiting1;

            if (texturesEnabled) { GL.glEnable(GL.GL_TEXTURE_2D); GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[7]); } else { GL.glDisable(GL.GL_TEXTURE_2D); }
            GL.glColor3f(1.0f, 1.0f, 1.0f);

            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0, 0); GL.glVertex3f(waitingAreaLeft1, waitingAreaBottom1, 0);
            GL.glTexCoord2f(0, 1); GL.glVertex3f(waitingAreaLeft1, waitingAreaTop1, 0);
            GL.glTexCoord2f(1, 1); GL.glVertex3f(waitingAreaRight1, waitingAreaTop1, 0);
            GL.glTexCoord2f(1, 0); GL.glVertex3f(waitingAreaRight1, waitingAreaBottom1, 0);
            GL.glEnd();

            GL.glDisable(GL.GL_TEXTURE_2D);
        }
        private void DrawStopLines(float lotWidth, float lotHeight, float roadGap)
        {
            DrawStripedLine(lotWidth, lotHeight / 2 - roadGap / 2 + 0.5f, 10, 0.2f, true);

            DrawStripedLine(lotWidth - 8.5f, lotHeight / 2 - roadGap / 2 + 4.9f, 15, 7.6f / 15, false);

            DrawStripedLine(lotWidth - 8.5f, lotHeight / 2 - roadGap / 2 - 2.2f, 15, 7.6f / 15, false);
        }
        private void DrawStripedLine(float startX, float startY, int stripeCount, float stripeSize, bool vertical)
        {
            for (int i = 0; i < stripeCount; i++)
            {
                if (i % 2 == 0)
                    GL.glColor3f(1.0f, 1.0f, 0.0f); 
                else
                    GL.glColor3f(0.0f, 0.0f, 0.0f); 

                float x1, y1, x2, y2;
                if (vertical)
                {
                    x1 = startX; x2 = startX + 0.2f;
                    y1 = startY + i * stripeSize;
                    y2 = y1 + stripeSize;
                }
                else
                {
                    y1 = startY; y2 = startY + 0.25f;
                    x1 = startX + i * stripeSize;
                    x2 = x1 + stripeSize;
                }

                GL.glBegin(GL.GL_QUADS);
                GL.glVertex3f(x1, y1, 0.01f);
                GL.glVertex3f(x2, y1, 0.01f);
                GL.glVertex3f(x2, y2, 0.01f);
                GL.glVertex3f(x1, y2, 0.01f);
                GL.glEnd();
            }
        }
        private void DrawParkingSpaces(float lotWidth, float lotHeight, float spaceWidth, float roadGap)
        {
            DrawCenterDividerLine(lotHeight);
            DrawParkingSpaceMarkings(lotWidth, lotHeight, spaceWidth, roadGap);
        }
        private void DrawCenterDividerLine(float lotHeight)
        {
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
        }
        private void DrawParkingSpaceMarkings(float lotWidth, float lotHeight, float spaceWidth, float roadGap)
        {
            float spacingBetweenSpaces = 0.1f;
            float offsetY = 0.5f;

            for (int i = 0; i < 7; i++)
            {
                float x = i * (spaceWidth + spacingBetweenSpaces);
                DrawSingleParkingSpace(x, lotHeight / 2 + roadGap / 2 + offsetY,
                                     lotHeight - 0.2f + offsetY, spaceWidth, 0, i);
            }

            for (int i = 0; i < 7; i++)
            {
                float x = i * (spaceWidth + spacingBetweenSpaces);
                DrawSingleParkingSpace(x, 0.2f, lotHeight / 2 - roadGap / 2, spaceWidth, 1, i);
            }
        }
        private void DrawSingleParkingSpace(float x, float y1, float y2, float width, int row, int col)
        {
            float scale = 1.15f; 

            float scaledWidth = width * scale;
            float scaledHeight = (y2 - y1) * scale;
            float scaledY2 = y1 + scaledHeight;

            GL.glColor3f(0.0f, 0.0f, 0.0f);
            GL.glBegin(GL.GL_LINE_LOOP);
            GL.glVertex3f(x, y1, 0.01f);
            GL.glVertex3f(x, scaledY2, 0.01f);
            GL.glVertex3f(x + scaledWidth, scaledY2, 0.01f);
            GL.glVertex3f(x + scaledWidth, y1, 0.01f);
            GL.glEnd();

            GL.glColor3f(1.0f, 1.0f, 1.0f);
            float WhiteY = (y1 + scaledY2) / 2;
            float lineWidth = 0.3f * scale;  
            float lineHeight = 0.2f * scale; 
            float lineStartX = x + (0.4f * scale); 

            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(lineStartX, WhiteY - lineHeight / 2, 0.02f);
            GL.glVertex3f(lineStartX, WhiteY + lineHeight / 2, 0.02f);
            GL.glVertex3f(lineStartX + lineWidth, WhiteY + lineHeight / 2, 0.02f);
            GL.glVertex3f(lineStartX + lineWidth, WhiteY - lineHeight / 2, 0.02f);
            GL.glEnd();
        }
    }
}

