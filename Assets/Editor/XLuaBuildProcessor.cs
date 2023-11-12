using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using XLua;

public class XLuaBuildProcessor : IPostBuildPlayerScriptDLLs 
{
    public int callbackOrder => 0;

    public void OnPostBuildPlayerScriptDLLs(BuildReport report) {
        string dir = string.Empty;
        foreach (var item in report.files) {
            if (item.path.Contains("Assembly-CSharp.dll")) {
                dir = item.path.Replace("Assembly-CSharp.dll", "");
            }
        }
        Hotfix.HotfixInject(dir);
    }
}
