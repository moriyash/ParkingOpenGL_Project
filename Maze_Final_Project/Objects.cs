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
            carRouteX[0, 1] = 1.0f; carRouteY[0, 1] = 0.0f; carRouteAngle[0, 1] = 90;    
            carRouteX[0, 2] = -1.0f; carRouteY[0, 2] = 0.5f; carRouteAngle[0, 2] = 45;   
            carRouteX[0, 3] = -2.2f; carRouteY[0, 3] = 2.0f; carRouteAngle[0, 3] = 0f;     
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
       
    }
}
