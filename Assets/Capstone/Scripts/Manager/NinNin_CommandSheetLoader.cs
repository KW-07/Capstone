using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class NinNin_CommandSheetLoader : MonoBehaviour
{
    private string sheetUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRJTa_wrtr7eqFO1VRujYZmlv9TnjSYrbwEcOnWO71LXpywROK7dXjmcZz92LGQbhorpBeZFeCu-oLT/pub?gid=652829718&single=true&output=csv";

    public int testfirstcount;
    public int testlastCount;
    public bool allGenerate;

    private string localFile => Path.Combine(Application.persistentDataPath, "SkillData.csv");
    private string saveScriptFolder = "Assets/Capstone/Scripts/CommandDataScripts";
    private string saveAssetFolder = "Assets/Capstone/Scripts/CommandData";
    
#if UNITY_EDITOR
    [ContextMenu("Download CSV from Drive")]
    public void UpdateSkillData()
    {
        StartCoroutine(DownloadAndGenerate());
    }

    IEnumerator DownloadAndGenerate()
    {
        UnityWebRequest www = UnityWebRequest.Get(sheetUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string csv = www.downloadHandler.text;
            File.WriteAllText(localFile, csv);
            Debug.Log("CSV 받아옴");
            Debug.Log("CSV 미리보기: " + csv.Substring(0, Mathf.Min(500, csv.Length)));

            GenerateScripts(csv);
        }
        else
        {
            Debug.LogError("다운로드 실패: " + www.error);
            Debug.LogError("응답 내용: " + www.downloadHandler.text);
        }
    }

    void GenerateScripts(string csv)
    {
        if (!Directory.Exists(saveScriptFolder)) Directory.CreateDirectory(saveScriptFolder);
        if (!Directory.Exists(saveAssetFolder)) Directory.CreateDirectory(saveAssetFolder);

        string[] lines = csv.Split('\n');
        string nl = Environment.NewLine;

        if (allGenerate)
        {
            testfirstcount = 2;
            testlastCount = lines.Length;
        }
        for (int i = testfirstcount - 1; i < testlastCount; i++) // 1번째 행은 설명칸
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] cells = ParseCsvLine(lines[i]);
            if (cells.Length < 12) continue;

            string skillNameEng = SanitizeClassName(cells[2]);
            string scriptPath = Path.Combine(saveScriptFolder, skillNameEng + ".cs");

            if (!File.Exists(scriptPath))
            {
                string scriptContent =
                    $"using UnityEngine;" + nl + nl +
                    $"[CreateAssetMenu(fileName = \"{skillNameEng}\", menuName = \"CommandData/{skillNameEng}\")]" + nl +
                    $"public class {skillNameEng} : CommandData" + nl +
                    "{" + nl +
                    "    public override void ActivateSkill(GameObject castPoint, GameObject target)" + nl +
                    "    {" + nl +
                    "        // if (effectPrefab != null)" + nl +
                    "        // {" + nl +
                    "        //     GameObject effect = Instantiate(effectPrefab, castPoint.transform.position, Quaternion.identity);" + nl +
                    "        //     Destroy(effect, destroyTime);" + nl +
                    "        // }" + nl +
                    "    }" + nl +
                    "}";

                File.WriteAllText(scriptPath, scriptContent);
                Debug.Log($"[스크립트 생성] {skillNameEng}");
            }
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("알림", "스크립트 생성 완료. Unity가 자동 컴파일되면 다시 실행하여 ScriptableObject를 생성하세요.", "확인");
    }

    [ContextMenu("Create ScriptableObjects from Generated Scripts")]
    public void CreateScriptableObjects()
    {
        string[] lines = File.ReadAllLines(localFile);

        if (allGenerate)
        {
            testfirstcount = 2;
            testlastCount = lines.Length;
        }
        for (int i = testfirstcount - 1; i < testlastCount; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] cells = ParseCsvLine(lines[i]);
            if (cells.Length < 12) continue;

            try
            {
                if (!float.TryParse(cells[0], out float skillID)) continue; //번호
                string skillNameKor = cells[1]; // 이름(한국어)
                string skillNameEng = SanitizeClassName(cells[2]); // 이름(영어)
                string command = cells[3]; // 커맨드
                string type = cells[4]; // 타입(원거리, 근거리 etc)
                string element = cells[5]; // 속성
                float.TryParse(cells[6], out float skillRatio); // 인술계수
                float.TryParse(cells[7], out float cooldown); // 쿨타임
                float.TryParse(cells[8], out float damage); // 데미지

                float.TryParse(cells[10], out float spawnDelay); // 스폰 딜레이
                float.TryParse(cells[11], out float destroyTime); // 소멸 시간
                
                string description = cells[14]; // 설명
                string effectDescription = cells[15]; // 이펙트 설명
                string animDescription = cells[16]; // 애니메이션 설명

                string className = skillNameEng;
                string assetPath = Path.Combine(saveAssetFolder, $"{className}.asset");

                Type skillType = GetTypeByName(className);
                if (skillType == null || !typeof(CommandData).IsAssignableFrom(skillType))
                {
                    Debug.LogWarning($"{className} 타입을 찾을 수 없음. 컴파일 완료 후 다시 시도하세요.");
                    continue;
                }

                CommandData asset = AssetDatabase.LoadAssetAtPath(assetPath, skillType) as CommandData;
                if (asset == null)
                {
                    asset = ScriptableObject.CreateInstance(skillType) as CommandData;
                    AssetDatabase.CreateAsset(asset, assetPath);
                    Debug.Log($"[에셋 생성] {className}");
                }

                asset.commandID = skillID;
                asset.commandNameKor = skillNameKor;
                asset.commandNameEng = skillNameEng;
                asset.stringCommand = command;

                for (int j = 0; j < command.Length; j++)
                {
                    switch (command.Substring(j, 1))
                    {
                        case "←": asset.command[j] = 1; break;
                        case "↓": asset.command[j] = 2; break;
                        case "→": asset.command[j] = 3; break;
                        case "↑": asset.command[j] = 4; break;
                        case "A": asset.command[j] = 5; break;
                        case "S": asset.command[j] = 6; break;
                        case "D": asset.command[j] = 7; break;
                        case "W": asset.command[j] = 8; break;
                        default: break;
                    }
                }

                //switch(type)
                //{
                //    case "근거리":
                //        asset.commandType = CommandType.Melee;
                //        break;
                //    case "원거리":
                //        asset.commandType = CommandType.Range;
                //        break;
                //    case "버프":
                //        asset.commandType = CommandType.Buff;
                //        break;
                //    default:
                //        break;
                //}

                asset.element = element;
                asset.skillRatio = skillRatio;
                asset.cooldown = cooldown;
                asset.damage = damage;
                asset.spawnDelay = spawnDelay;
                asset.destroyTime = destroyTime;

                asset.description = description;
                asset.effectDescription = effectDescription;
                asset.animDescription = animDescription;

                EditorUtility.SetDirty(asset);
            }
            catch (Exception ex)
            {
                Debug.LogError($"{i + 1}번째 줄 에러: {ex.Message}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    string[] ParseCsvLine(string line)
    {
        List<string> result = new List<string>();
        bool inQuotes = false;
        string current = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"') { inQuotes = !inQuotes; continue; }

            if (c == ',' && !inQuotes)
            {
                result.Add(current);
                current = "";
            }
            else
            {
                current += c;
            }
        }

        result.Add(current);
        return result.ToArray();
    }

    string SanitizeClassName(string raw)
    {
        return Regex.Replace(raw, "[^a-zA-Z0-9_]", "");
    }

    Type GetTypeByName(string className)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var type = assembly.GetTypes().FirstOrDefault(t => t.Name == className);
            if (type != null) return type;
        }
        return null;
    }
#endif
}
