using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

namespace StarAward
{
    public class FixIndexFile : MonoBehaviour
    {
        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target == BuildTarget.WebGL)
            {
                string text = File.ReadAllText(Path.Combine(pathToBuiltProject, "index.html"));
                string original = "<title>Unity WebGL Player | StarAward</title>";
                string replace = "<title>星肝宝贝</title>";
                text = text.Replace(original, replace);
                original = "    <div id=\"unityContainer\" style=\"width: 1080px; height: 1920px; margin: auto\"></div>";
                replace = "    <div id=\"unityContainer\" style=\"width: 100%; height: 100%; position: absolute\"></div>\r\n" +
                          "    <script>\r\n" +
                          "      var browser = {\r\n" +
                          "        versions: function () {\r\n" +
                          "          return {\r\n" +
                          "            android: navigator.userAgent.indexOf('Android') > -1\r\n" +
                          "          };\r\n" +
                          "        } ()\r\n" +
                          "      }\r\n" +
                          "      if (browser.versions.android) {\r\n" +
                          "        var style = document.getElementById(\"unityContainer\").style;\r\n" +
                          "        style.height = \"85%\";\r\n" +
                          "      }\r\n" +
                          "    </script>;";
                text = text.Replace(original, replace);
                File.WriteAllText(Path.Combine(pathToBuiltProject, "index.html"), text);
                //
                text = File.ReadAllText(Path.Combine(pathToBuiltProject, "Build/UnityLoader.js"));
                original = "UnityLoader.SystemInfo.hasWebGL?UnityLoader.SystemInfo.mobile?e.popup(\"Please note that Unity WebGL is not currently supported on mobiles. Press OK if you wish to continue anyway.\",[{text:\"OK\",callback:t}]):[\"Edge\",\"Firefox\",\"Chrome\",\"Safari\"].indexOf(UnityLoader.SystemInfo.browser)==-1?e.popup(\"Please note that your browser is not currently supported for this Unity WebGL content. Press OK if you wish to continue anyway.\",[{text:\"OK\",callback:t}]):t():e.popup(\"Your browser does not support WebGL\",[{text:\"OK\",callback:r}])";
                replace = "t();";
                text = text.Replace(original, replace);
                File.WriteAllText(Path.Combine(pathToBuiltProject, "Build/UnityLoader.js"), text);
            }
        }
    }
}
