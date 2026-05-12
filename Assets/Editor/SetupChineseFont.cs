using UnityEngine;
using UnityEditor;
using TMPro;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class SetupChineseFont
{
    // Step 1: Generate the Chinese TMP Font Asset
    [MenuItem("Tools/Kailius/1. Generate Chinese Font Asset")]
    static void GenerateFontAsset()
    {
        string ttfPath = "Assets/Font/ark-pixel-12px-zh_cn.ttf";
        string sdfPath = "Assets/Font/ark-pixel-12px-zh_cn SDF.asset";

        Font sourceFont = AssetDatabase.LoadAssetAtPath<Font>(ttfPath);
        if (sourceFont == null)
        {
            Debug.LogError("Font not found! Make sure Assets/Font/ark-pixel-12px-zh_cn.ttf exists.");
            return;
        }

        // Create Font Asset with dynamic atlas population
        TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(sourceFont);

        if (fontAsset == null)
        {
            // Fallback: try with explicit params
            fontAsset = TMP_FontAsset.CreateFontAsset(
                sourceFont,
                90,     // point size for sampling
                9,      // padding
                UnityEngine.TextCore.LowLevel.GlyphRenderMode.SDFAA,
                4096,   // atlas width
                4096,   // atlas height,
                AtlasPopulationMode.Dynamic
            );
        }

        if (fontAsset == null)
        {
            Debug.LogError("Could not create TMP Font Asset via script. Please create it manually:\n" +
                "1. Right-click Assets/Font/ark-pixel-12px-zh_cn.ttf in Project view\n" +
                "2. Select Create > TextMeshPro > Font Asset\n" +
                "3. Then run 'Tools/Kailius/2. Replace Font References'");
            return;
        }

        fontAsset.atlasPopulationMode = AtlasPopulationMode.Dynamic;
        fontAsset.name = "ark-pixel-12px-zh_cn SDF";

        AssetDatabase.CreateAsset(fontAsset, sdfPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        string guid = AssetDatabase.AssetPathToGUID(sdfPath);
        Debug.Log("Font asset created: " + sdfPath);
        Debug.Log("GUID: " + guid);
        EditorUtility.DisplayDialog("Font Asset Created",
            "Chinese TMP Font Asset created!\n\nGUID: " + guid + "\n\nNow run 'Tools/Kailius/2. Replace Font References'",
            "OK");
    }

    // Step 2: Replace font GUID in all prefabs and scenes
    [MenuItem("Tools/Kailius/2. Replace Font References")]
    static void ReplaceFontReferences()
    {
        string sdfPath = "Assets/Font/ark-pixel-12px-zh_cn SDF.asset";
        string newGuid = AssetDatabase.AssetPathToGUID(sdfPath);

        if (string.IsNullOrEmpty(newGuid))
        {
            Debug.LogError("Chinese SDF font not found! Run 'Tools/Kailius/1. Generate Chinese Font Asset' first.\n" +
                "Or if you created the font manually, make sure it's saved at: " + sdfPath);
            EditorUtility.DisplayDialog("Font Not Found",
                "Chinese SDF font not found at:\n" + sdfPath + "\n\nRun step 1 first.",
                "OK");
            return;
        }

        // Old font GUID: upheavtt SDF.asset
        string oldGuid = "d22c986e05eac53b8bc884c1372fa019";

        string assetsPath = Application.dataPath;
        string[] allFiles = Directory.GetFiles(assetsPath, "*.prefab", SearchOption.AllDirectories);
        string[] sceneFiles = Directory.GetFiles(assetsPath, "*.unity", SearchOption.AllDirectories);

        List<string> allTargetFiles = new List<string>();
        allTargetFiles.AddRange(allFiles);
        allTargetFiles.AddRange(sceneFiles);

        int updatedCount = 0;
        foreach (string filePath in allTargetFiles)
        {
            string content = File.ReadAllText(filePath, Encoding.UTF8);
            if (content.Contains(oldGuid))
            {
                string updated = content.Replace(oldGuid, newGuid);
                File.WriteAllText(filePath, updated, new UTF8Encoding(false));
                updatedCount++;
                Debug.Log("Updated: " + Path.GetFileName(filePath));
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Replaced font GUID in " + updatedCount + " files.");
        EditorUtility.DisplayDialog("Done!",
            string.Format("Font GUID replaced in {0} files.\n\n{1} -> {2}\n\nYou can now rebuild the APK.",
            updatedCount, oldGuid, newGuid),
            "OK");
    }

    // Step 3: List all text content for verification
    [MenuItem("Tools/Kailius/3. Verify Chinese Text")]
    static void VerifyChineseText()
    {
        string assetsPath = Application.dataPath;
        string[] allFiles = Directory.GetFiles(assetsPath, "*.prefab", SearchOption.AllDirectories);
        string[] sceneFiles = Directory.GetFiles(assetsPath, "*.unity", SearchOption.AllDirectories);

        List<string> allTargetFiles = new List<string>();
        allTargetFiles.AddRange(allFiles);
        allTargetFiles.AddRange(sceneFiles);

        List<string> textStrings = new List<string>();

        foreach (string filePath in allTargetFiles)
        {
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            foreach (string line in lines)
            {
                if (line.Contains("m_text:") || line.Contains("value:"))
                {
                    textStrings.Add(Path.GetFileName(filePath) + ": " + line.Trim());
                }
            }
        }

        string output = string.Join("\n", textStrings);
        Debug.Log("=== All text strings in project ===\n" + output);
        EditorUtility.DisplayDialog("Text Strings Found",
            "Found " + textStrings.Count + " text entries. Check the Console window for details.",
            "OK");
    }
}
