using UnityEngine;
using UnityEditor;
using System.IO;

public class AnimationExtractor : EditorWindow
{
    private UnityEngine.Object fbxAsset;
    private string saveFolder = "Assets/Capstone/MOD_KW/Animation";

    [MenuItem("Tools/Animation Extractor")]
    public static void ShowWindow()
    {
        GetWindow<AnimationExtractor>("Animation Extractor");
    }

    private void OnGUI()
    {
        GUILayout.Label("FBX �ִϸ��̼� Ŭ�� �и���", EditorStyles.boldLabel);
        fbxAsset = EditorGUILayout.ObjectField("FBX ���� ����", fbxAsset, typeof(UnityEngine.Object), false);

        saveFolder = EditorGUILayout.TextField("���� ����", saveFolder);

        if (GUILayout.Button("����"))
        {
            ExtractAnimations();
        }
    }

    private void ExtractAnimations()
    {
        if (fbxAsset == null)
        {
            Debug.LogError("FBX ������ �����ϼ���");
            return;
        }

        string path = AssetDatabase.GetAssetPath(fbxAsset);
        var clips = AssetDatabase.LoadAllAssetsAtPath(path);
        int count = 0;

        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        foreach (var clip in clips)
        {
            if (clip is AnimationClip animationClip && !animationClip.name.Contains("__preview"))
            {
                // Blender���� �⺻���� ������ ������ �̸� �� '|'�� �ν����� ���ؼ� '_'�� ��ü �� �и�
                string safeName = animationClip.name.Replace("|", "_").Replace("/", "_").Replace("\\", "_");
                string clipPath = $"{saveFolder}/{safeName}.anim";

                AnimationClip newClip = new AnimationClip();
                EditorUtility.CopySerialized(animationClip, newClip);
                AssetDatabase.CreateAsset(newClip, clipPath);
                count++;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"{count}���� �ִϸ��̼� Ŭ�� {saveFolder}�� �����߽��ϴ�.");
    }
}
