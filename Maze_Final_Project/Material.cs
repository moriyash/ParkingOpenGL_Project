using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
namespace OpenGL
{
    public partial class cOGL
    {
        public struct MaterialProperties
        {
            public float[] ambient;
            public float[] diffuse;
            public float[] specular;
            public float shininess;

            public MaterialProperties(float[] amb, float[] diff, float[] spec, float shine)
            {
                ambient = amb;
                diffuse = diff;
                specular = spec;
                shininess = shine;
            }
        }
        private string currentMaterialType = "gold";
        private Dictionary<string, MaterialProperties> materials = new Dictionary<string, MaterialProperties>()
        {
            ["gold"] = new MaterialProperties(
          new float[] { 0.24725f, 0.2245f, 0.0645f, 1.0f },     
          new float[] { 0.34615f, 0.3143f, 0.0903f, 1.0f },     
          new float[] { 0.797357f, 0.723991f, 0.208006f, 1.0f }, 
          83.2f                                                  
      ),

            ["emerald"] = new MaterialProperties(
          new float[] { 0.0215f, 0.1745f, 0.0215f, 0.55f },    
          new float[] { 0.07568f, 0.61424f, 0.07568f, 0.55f },   
          new float[] { 0.633f, 0.727811f, 0.633f, 0.55f },     
          76.8f                                                   
      ),

            ["chrome"] = new MaterialProperties(
          new float[] { 0.25f, 0.25f, 0.25f, 1.0f },            
          new float[] { 0.4f, 0.4f, 0.4f, 1.0f },              
          new float[] { 0.774597f, 0.774597f, 0.774597f, 1.0f }, 
          76.8f                                                   
      ),
            ["white"] = new MaterialProperties(
        new float[] { 0.2f, 0.2f, 0.2f, 1.0f },
        new float[] { 1.0f, 1.0f, 1.0f, 1.0f },
        new float[] { 0.3f, 0.3f, 0.3f, 1.0f },
        16.0f
    )
        };
        private void SetMaterial(MaterialProperties material)
        {
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT, material.ambient);
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_DIFFUSE, material.diffuse);
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_SPECULAR, material.specular);
            GL.glMaterialf(GL.GL_FRONT_AND_BACK, GL.GL_SHININESS, material.shininess);
        }
        public void SetCurrentMaterial(string materialType)
        {
            if (materials.ContainsKey(materialType))
            {
                currentMaterialType = materialType;
                SetMaterial(materials[materialType]);
            }
        }
        private void ApplyMaterialForObject(string objectType)
        {
            switch (objectType)
            {
                case "plant":
                case "flower":
                case "flowerpot":
                    SetMaterial(materials[currentMaterialType]); 
                    break;
                default:
                    break; 


            }
        }
    }
    }

