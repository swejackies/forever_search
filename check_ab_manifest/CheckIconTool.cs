using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CheckIconTool
{
    private static List<string> loglist = new List<string>();
    #region 编辑器调试代码
    [MenuItem("Naruto/AssetBundle/测试/BuildSelectionAB")]
    static public void BuildSelectionAB()
    {
        var folder = Application.streamingAssetsPath;
        var bundles = new List<AssetBundleBuild>();
        var target = BuildTarget.StandaloneWindows64;



        var build = new AssetBundleBuild();
        build.assetBundleName = Selection.activeObject.name;
        var assetsNames = new List<string>();
        assetsNames.Add(AssetDatabase.GetAssetPath(Selection.activeObject));
        build.assetNames = assetsNames.ToArray();
        bundles.Add(build);

        
        var build2 = new AssetBundleBuild();
        build2.assetBundleName = "Skill";
        var assetsNames2 = new List<string>();
        assetsNames2.Add("Assets\\Naruto\\Resources\\Icon\\Skill");
        build2.assetNames = assetsNames2.ToArray();
        bundles.Add(build2);


        var option = BuildAssetBundleOptions.None;
        BuildPipeline.BuildAssetBundles(folder, bundles.ToArray(), option | BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.StrictMode, target);
    }

    [MenuItem("Naruto/AssetBundle/测试/ClearPrefabIconSprite")]
    static public void ClearPrefabIconSprite()
    {
        GameObject obj = Selection.activeObject as GameObject;
        var icons = obj.GetComponentsInChildren<Icon>(true);
        if (icons.Length > 0)
        {
            foreach (var icon in icons)
            {
                icon.GetComponent<Image>().sprite = null;
            }
            PrefabUtility.SetPropertyModifications(obj, new PropertyModification[0]);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Naruto/AssetBundle/测试/CheckSelectionNoIconScript")]
    static public void CheckSelectionNoIconScript()
    {
        string prefab = AssetDatabase.GetAssetPath(Selection.activeObject);
        CheckPrefab(prefab, true);
    }

    [MenuItem("Naruto/AssetBundle/测试/CheckAllNoIconScript")]
    static public void CheckAllNoIconScript()
    {
        loglist.Clear();       

        List<string> views = new List<string>(Directory.GetFiles("Assets/Naruto/Resources/Biz", "*.prefab", SearchOption.AllDirectories));
        views.AddRange(Directory.GetFiles("Assets/Naruto/Resources/Common", "*.prefab", SearchOption.AllDirectories));
        foreach (var prefab in views)
        {
            CheckPrefab(prefab.Replace("\\", "/"), false);
        }

        File.WriteAllLines(Application.dataPath + "/" + "log.txt", loglist.ToArray());
    }

    private static void CheckPrefab(string assetPath, bool containsRef)
    {
        var go = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        if (go == null)
        {
            return;
        }

        var temps = AssetDatabase.GetDependencies(assetPath);
        List<string> listIconFiles = new List<string>();
        foreach (var file in temps)
        {
            if (containsRef && file != assetPath && file.EndsWith(".prefab"))
            {
                CheckPrefab(file, true);
            }
            else if (file.EndsWith(".png") || file.EndsWith(".jpg"))
            {
                if (file.Contains("Assets/Naruto/Resources/Icon"))
                {
                    listIconFiles.Add(Path.GetFileNameWithoutExtension(file));
                    //Debug.LogWarningFormat("file = {0}", file);
                }
            }
        }

        if (listIconFiles.Count == 0)
            return;

        var imgs = go.GetComponentsInChildren<Image>(true);
        for (int index = 0; index < imgs.Length; ++index)
        {
            var img = imgs[index];
            if (img.sprite != null && listIconFiles.Contains(img.sprite.texture.name) && img.GetComponent<Icon>() == null)
            {
                string log = string.Format("prefab = {0}, node = {1}, texture = {2}", assetPath, FullPath(img.gameObject), img.sprite.texture.name);
                loglist.Add(log);
                Debug.LogWarning(log);
            }
        }
    }

    private static string FullPath(GameObject go)
    {
        return go.transform.parent == null
            ? go.name
                : FullPath(go.transform.parent.gameObject) + "/" + go.name;
    }
    #endregion
}