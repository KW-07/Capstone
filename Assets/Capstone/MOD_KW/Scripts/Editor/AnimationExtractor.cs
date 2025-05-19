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
        GUILayout.Label("FBX 애니메이션 클립 분리기", EditorStyles.boldLabel);
        fbxAsset = EditorGUILayout.ObjectField("FBX 파일 선택", fbxAsset, typeof(UnityEngine.Object), false);

        saveFolder = EditorGUILayout.TextField("저장 폴더", saveFolder);

        if (GUILayout.Button("추출"))
        {
            ExtractAnimations();
        }
    }

    private void ExtractAnimations()
    {
        if (fbxAsset == null)
        {
            Debug.LogError("FBX 파일을 선택하세요");
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
                // Blender에서 기본으로 나오는 파일의 이름 중 '|'을 인식하지 못해서 '_'로 교체 후 분리
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

        Debug.Log($"{count}개의 애니메이션 클립 {saveFolder}에 저장했습니다.");
    }
}
