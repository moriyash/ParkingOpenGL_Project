using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace OpenGL
{
    [Serializable]
    public class SceneData
    {
        public bool Light1On { get; set; }
        public bool Light2On { get; set; }
        public float[] Light1Color { get; set; } = new float[3];
        public float[] Light2Color { get; set; } = new float[3];
        public float Light1Intensity { get; set; }
        public float Light2Intensity { get; set; }
        public float Light1Radius { get; set; }
        public float Light2Radius { get; set; }
        public float[] LightPosition { get; set; } = new float[3]; 

        public bool SunEnabled { get; set; }
        public float GlobalAmbient { get; set; }

        public bool ShadowsEnabled { get; set; }
        public float ShadowIntensity { get; set; }
        public bool ReflectionsEnabled { get; set; }
        public float ReflectionStrength { get; set; }

        public float[] CarCurrentX { get; set; } = new float[4];
        public float[] CarCurrentY { get; set; } = new float[4];
        public float[] CarCurrentAngle { get; set; } = new float[4];
        public float[] CarRotationAngle { get; set; } = new float[4];
        public bool[] CarIsMoving { get; set; } = new bool[4];
        public float CarSpeed { get; set; }

        public bool GateOpen { get; set; }
        public float GateAnimation { get; set; }
        public float GateScale { get; set; }
        public float GateRotation { get; set; }
        public float GateOffsetX { get; set; }
        public float GateOffsetY { get; set; }

        public int TrafficLightState { get; set; }
        public float TrafficLightScale { get; set; }
        public float TrafficLightRotation { get; set; }

        public float StreetLampScale { get; set; }
        public float StreetLampRotation { get; set; }

        public float[] ObjectPositionsX { get; set; } = new float[10];
        public float[] ObjectPositionsY { get; set; } = new float[10];
        public float[] ObjectPositionsZ { get; set; } = new float[10];

        public bool CarLightsOn { get; set; }
        public bool CarBlinking { get; set; }

        public float ZoomDistance { get; set; }
        public float XAngle { get; set; }
        public float YAngle { get; set; }
        public float ZAngle { get; set; }
        public float PanX { get; set; }
        public float PanY { get; set; }

        public bool TexturesEnabled { get; set; }
        public bool IsOrthogonal { get; set; }
        public bool IsColored { get; set; }
        public bool EnvironmentMapping { get; set; }
        public float TextureRotation { get; set; }
        public string CurrentMaterial { get; set; } = "default";

        public string SceneName { get; set; }
        public DateTime SaveTime { get; set; }
        public string Description { get; set; }
        public string Version { get; set; } = "1.0";
    }

    public static class SceneManager
    {
        public static bool SaveScene(cOGL cgl, string filePath, string sceneName = "", string description = "")
        {
            try
            {
                var data = new SceneData
                {
                    Light1On = cgl.light1On,
                    Light2On = cgl.light2On,
                    Light1Color = (float[])cgl.light1Color.Clone(),
                    Light2Color = (float[])cgl.light2Color.Clone(),
                    Light1Intensity = cgl.light1Intensity,
                    Light2Intensity = cgl.light2Intensity,
                    Light1Radius = cgl.light1Radius,
                    Light2Radius = cgl.light2Radius,
                    LightPosition = (float[])cgl.pos.Clone(),

                    SunEnabled = cgl.sunEnabled,
                    GlobalAmbient = HasProperty(cgl, "globalAmbient") ? GetFloatProperty(cgl, "globalAmbient") : 0.2f,

                    ShadowsEnabled = HasBoolProperty(cgl, "shadowsEnabled") ? GetBoolProperty(cgl, "shadowsEnabled") : false,
                    ShadowIntensity = HasProperty(cgl, "shadowIntensity") ? GetFloatProperty(cgl, "shadowIntensity") : 0.5f,
                    ReflectionsEnabled = HasBoolProperty(cgl, "reflectionsEnabled") ? GetBoolProperty(cgl, "reflectionsEnabled") : false,
                    ReflectionStrength = HasProperty(cgl, "reflectionStrength") ? GetFloatProperty(cgl, "reflectionStrength") : 0.3f,

                    CarCurrentX = (float[])cgl.carCurrentX.Clone(),
                    CarCurrentY = (float[])cgl.carCurrentY.Clone(),
                    CarCurrentAngle = (float[])cgl.carCurrentAngle.Clone(),
                    CarRotationAngle = (float[])cgl.carRotationAngle.Clone(),
                    CarIsMoving = (bool[])cgl.carIsMoving.Clone(),
                    CarSpeed = cgl.carSpeed,

                    GateOpen = cgl.gateOpen,
                    GateAnimation = cgl.gateAnimation,
                    GateScale = cgl.gateScale,
                    GateRotation = cgl.gateRotation,
                    GateOffsetX = cgl.gateOffsetX,
                    GateOffsetY = cgl.gateOffsetY,

                    TrafficLightState = cgl.trafficLightState,
                    TrafficLightScale = cgl.trafficLightScale,
                    TrafficLightRotation = cgl.trafficLightRotation,

                    StreetLampScale = cgl.streetLampScale,
                    StreetLampRotation = cgl.streetLampRotation,

                    ObjectPositionsX = ExtractPositionsX(cgl.objectPositions),
                    ObjectPositionsY = ExtractPositionsY(cgl.objectPositions),
                    ObjectPositionsZ = ExtractPositionsZ(cgl.objectPositions),

                    CarLightsOn = cgl.carLightsOn,
                    CarBlinking = cgl.carBlinking,

                    ZoomDistance = cgl.zoomDistance,
                    XAngle = cgl.xAngle,
                    YAngle = cgl.yAngle,
                    ZAngle = cgl.zAngle,
                    PanX = cgl.panX,
                    PanY = cgl.panY,

                    TexturesEnabled = cgl.texturesEnabled,
                    IsOrthogonal = cgl.isOrthogonal,
                    IsColored = cgl.isColored,
                    EnvironmentMapping = cgl.environmentMapping,
                    TextureRotation = cgl.textureRotation,
                    CurrentMaterial = HasStringProperty(cgl, "currentMaterial") ? GetStringProperty(cgl, "currentMaterial") : "default",

                    SceneName = string.IsNullOrEmpty(sceneName) ? "Parking Scene" : sceneName,
                    Description = description,
                    SaveTime = DateTime.Now
                };

                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving scene: " + ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool LoadScene(cOGL cgl, string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("File not found!", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                string json = File.ReadAllText(filePath);
                SceneData data = JsonConvert.DeserializeObject<SceneData>(json);

                cgl.light1On = data.Light1On;
                cgl.light2On = data.Light2On;
                cgl.light1Color = (float[])data.Light1Color.Clone();
                cgl.light2Color = (float[])data.Light2Color.Clone();
                cgl.light1Intensity = data.Light1Intensity;
                cgl.light2Intensity = data.Light2Intensity;
                cgl.light1Radius = data.Light1Radius;
                cgl.light2Radius = data.Light2Radius;
                cgl.pos = (float[])data.LightPosition.Clone();

                cgl.sunEnabled = data.SunEnabled;
                if (HasProperty(cgl, "globalAmbient"))
                    SetFloatProperty(cgl, "globalAmbient", data.GlobalAmbient);

                if (HasBoolProperty(cgl, "shadowsEnabled"))
                    SetBoolProperty(cgl, "shadowsEnabled", data.ShadowsEnabled);
                if (HasProperty(cgl, "shadowIntensity"))
                    SetFloatProperty(cgl, "shadowIntensity", data.ShadowIntensity);
                if (HasBoolProperty(cgl, "reflectionsEnabled"))
                    SetBoolProperty(cgl, "reflectionsEnabled", data.ReflectionsEnabled);
                if (HasProperty(cgl, "reflectionStrength"))
                    SetFloatProperty(cgl, "reflectionStrength", data.ReflectionStrength);

                cgl.carCurrentX = (float[])data.CarCurrentX.Clone();
                cgl.carCurrentY = (float[])data.CarCurrentY.Clone();
                cgl.carCurrentAngle = (float[])data.CarCurrentAngle.Clone();
                cgl.carRotationAngle = (float[])data.CarRotationAngle.Clone();
                cgl.carIsMoving = (bool[])data.CarIsMoving.Clone();
                cgl.carSpeed = data.CarSpeed;

                cgl.gateOpen = data.GateOpen;
                cgl.gateAnimation = data.GateAnimation;
                cgl.gateScale = data.GateScale;
                cgl.gateRotation = data.GateRotation;
                cgl.gateOffsetX = data.GateOffsetX;
                cgl.gateOffsetY = data.GateOffsetY;

                cgl.trafficLightState = data.TrafficLightState;
                cgl.trafficLightScale = data.TrafficLightScale;
                cgl.trafficLightRotation = data.TrafficLightRotation;

                cgl.streetLampScale = data.StreetLampScale;
                cgl.streetLampRotation = data.StreetLampRotation;

                RestoreObjectPositions(cgl, data.ObjectPositionsX, data.ObjectPositionsY, data.ObjectPositionsZ);

                cgl.carLightsOn = data.CarLightsOn;
                cgl.carBlinking = data.CarBlinking;

                cgl.zoomDistance = data.ZoomDistance;
                cgl.xAngle = data.XAngle;
                cgl.yAngle = data.YAngle;
                cgl.zAngle = data.ZAngle;
                cgl.panX = data.PanX;
                cgl.panY = data.PanY;

                cgl.texturesEnabled = data.TexturesEnabled;
                cgl.isOrthogonal = data.IsOrthogonal;
                cgl.isColored = data.IsColored;
                cgl.environmentMapping = data.EnvironmentMapping;
                cgl.textureRotation = data.TextureRotation;

                if (!string.IsNullOrEmpty(data.CurrentMaterial))
                {
                    cgl.SetCurrentMaterial(data.CurrentMaterial);
                }

                cgl.UpdateLightingColors();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading scene: " + ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static void ApplyDayPreset(cOGL cgl)
        {
            cgl.light1On = true;
            cgl.light2On = true;
            cgl.SetLight1Color(1.0f, 1.0f, 1.0f);
            cgl.SetLight2Color(1.0f, 1.0f, 1.0f);
            cgl.light1Intensity = 5.0f;
            cgl.light2Intensity = 5.0f;
            cgl.texturesEnabled = true;
            cgl.carLightsOn = false;
            cgl.trafficLightState = 2; // Green
            cgl.sunEnabled = true;
            cgl.UpdateLightingColors();
        }

        public static void ApplyNightPreset(cOGL cgl)
        {
            cgl.light1On = true;
            cgl.light2On = true;
            cgl.SetLight1Color(1.0f, 0.8f, 0.3f);  // Warm light
            cgl.SetLight2Color(1.0f, 0.8f, 0.3f);
            cgl.light1Intensity = 3.0f;
            cgl.light2Intensity = 3.0f;
            cgl.texturesEnabled = true;
            cgl.carLightsOn = true;
            cgl.trafficLightState = 0; // Red
            cgl.sunEnabled = false;
            cgl.UpdateLightingColors();
        }

        public static void ApplyPartyPreset(cOGL cgl)
        {
            cgl.light1On = true;
            cgl.light2On = true;
            cgl.SetLight1Color(1.0f, 0.2f, 1.0f);  // Magenta
            cgl.SetLight2Color(0.2f, 1.0f, 0.2f);  // Green
            cgl.light1Intensity = 4.0f;
            cgl.light2Intensity = 4.0f;
            cgl.texturesEnabled = true;
            cgl.carLightsOn = true;
            cgl.carBlinking = true;
            cgl.trafficLightState = 1; // Yellow
            cgl.sunEnabled = false;
            cgl.UpdateLightingColors();
        }

        private static float[] ExtractPositionsX(dynamic objectPositions)
        {
            float[] result = new float[10];
            try
            {
                for (int i = 0; i < Math.Min(objectPositions.Length, 10); i++)
                {
                    result[i] = objectPositions[i].X;
                }
            }
            catch { }
            return result;
        }

        private static float[] ExtractPositionsY(dynamic objectPositions)
        {
            float[] result = new float[10];
            try
            {
                for (int i = 0; i < Math.Min(objectPositions.Length, 10); i++)
                {
                    result[i] = objectPositions[i].Y;
                }
            }
            catch { }
            return result;
        }

        private static float[] ExtractPositionsZ(dynamic objectPositions)
        {
            float[] result = new float[10];
            try
            {
                for (int i = 0; i < Math.Min(objectPositions.Length, 10); i++)
                {
                    result[i] = objectPositions[i].Z;
                }
            }
            catch { }
            return result;
        }

        private static void RestoreObjectPositions(cOGL cgl, float[] x, float[] y, float[] z)
        {
            try
            {
                for (int i = 0; i < Math.Min(cgl.objectPositions.Length, 10); i++)
                {
                    var pos = cgl.objectPositions[i];
                    pos.X = x[i];
                    pos.Y = y[i];
                    pos.Z = z[i];
                    cgl.objectPositions[i] = pos;
                }
            }
            catch { }
        }

        private static bool HasProperty(object obj, string propertyName)
        {
            try
            {
                return obj.GetType().GetProperty(propertyName) != null ||
                       obj.GetType().GetField(propertyName) != null;
            }
            catch
            {
                return false;
            }
        }

        private static bool HasBoolProperty(object obj, string propertyName)
        {
            try
            {
                var prop = obj.GetType().GetProperty(propertyName);
                var field = obj.GetType().GetField(propertyName);
                return (prop != null && prop.PropertyType == typeof(bool)) ||
                       (field != null && field.FieldType == typeof(bool));
            }
            catch
            {
                return false;
            }
        }

        private static bool HasStringProperty(object obj, string propertyName)
        {
            try
            {
                var prop = obj.GetType().GetProperty(propertyName);
                var field = obj.GetType().GetField(propertyName);
                return (prop != null && prop.PropertyType == typeof(string)) ||
                       (field != null && field.FieldType == typeof(string));
            }
            catch
            {
                return false;
            }
        }

        private static bool HasMethod(object obj, string methodName)
        {
            try
            {
                return obj.GetType().GetMethod(methodName) != null;
            }
            catch
            {
                return false;
            }
        }

        private static float GetFloatProperty(object obj, string propertyName)
        {
            try
            {
                var prop = obj.GetType().GetProperty(propertyName);
                if (prop != null) return (float)prop.GetValue(obj);

                var field = obj.GetType().GetField(propertyName);
                if (field != null) return (float)field.GetValue(obj);

                return 0.0f;
            }
            catch
            {
                return 0.0f;
            }
        }

        private static bool GetBoolProperty(object obj, string propertyName)
        {
            try
            {
                var prop = obj.GetType().GetProperty(propertyName);
                if (prop != null) return (bool)prop.GetValue(obj);

                var field = obj.GetType().GetField(propertyName);
                if (field != null) return (bool)field.GetValue(obj);

                return false;
            }
            catch
            {
                return false;
            }
        }

        private static string GetStringProperty(object obj, string propertyName)
        {
            try
            {
                var prop = obj.GetType().GetProperty(propertyName);
                if (prop != null) return (string)prop.GetValue(obj);

                var field = obj.GetType().GetField(propertyName);
                if (field != null) return (string)field.GetValue(obj);

                return "";
            }
            catch
            {
                return "";
            }
        }

        private static void SetFloatProperty(object obj, string propertyName, float value)
        {
            try
            {
                var prop = obj.GetType().GetProperty(propertyName);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(obj, value);
                    return;
                }

                var field = obj.GetType().GetField(propertyName);
                if (field != null)
                {
                    field.SetValue(obj, value);
                }
            }
            catch { }
        }

        private static void SetBoolProperty(object obj, string propertyName, bool value)
        {
            try
            {
                var prop = obj.GetType().GetProperty(propertyName);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(obj, value);
                    return;
                }

                var field = obj.GetType().GetField(propertyName);
                if (field != null)
                {
                    field.SetValue(obj, value);
                }
            }
            catch { }
        }

        public static bool ValidateScene(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return false;

                string json = File.ReadAllText(filePath);
                SceneData data = JsonConvert.DeserializeObject<SceneData>(json);

                return data != null && !string.IsNullOrEmpty(data.SceneName);
            }
            catch
            {
                return false;
            }
        }

        public static SceneData GetSceneInfo(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return null;

                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<SceneData>(json);
            }
            catch
            {
                return null;
            }
        }
    }
}