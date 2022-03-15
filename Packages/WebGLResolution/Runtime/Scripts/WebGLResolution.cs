using System.Runtime.InteropServices;
using UnityEngine;

namespace WebGLResolution
{
    public class ScreenWindow
    {
        public static string ms_DebugMessage;

        [DllImport("__Internal")]
        private static extern int GetWindowWidth();
        [DllImport("__Internal")]
        private static extern int GetWindowHeight();
        [DllImport("__Internal")]
        private static extern void ResetCanvasSize(int width, int height);

        public static Vector2 GetWindowSize()
        {
            int w = GetWindowWidth();
            int h = GetWindowHeight();
            return new Vector2(w, h);
        }
        public static void SetCustonCanvasSize(int width, int height)
        {
            ResetCanvasSize(width, height);
            Screen.SetResolution(width, height, false);
        }
        public static void SetCanvasMaxSize()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            //Vector2 vector2 = GetWindowSize();
            //int width = Mathf.RoundToInt(vector2.x);
            //int height = Mathf.RoundToInt(vector2.y);
            //ms_DebugMessage = string.Format("W: {0}, H: {1}", width, height);
            //ResetCanvasSize(width, height);
            //Screen.SetResolution(width, height, false);
#endif
        }
    }
}
