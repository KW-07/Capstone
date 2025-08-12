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
            Debug.Log("CSV �޾ƿ�");
            Debug.Log("CSV �̸�����: " + csv.Substring(0, Mathf.Min(500, csv.Length)));

            GenerateScripts(csv);
        }
        else
        {
            Debug.LogError("�ٿ�ε� ����: " + www.error);
            Debug.LogError("���� ����: " + www.downloadHandler.text);
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
        for (int i = testfirstcount - 1; i < testlastCount; i++) // 1��° ���� ����ĭ
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
                Debug.Log($"[��ũ��Ʈ ����] {skillNameEng}");
            }
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("�˸�", "��ũ��Ʈ ���� �Ϸ�. Unity�� �ڵ� �����ϵǸ� �ٽ� �����Ͽ� ScriptableObject�� �����ϼ���.", "Ȯ��");
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
                if (!float.TryParse(cells[0], out float skillID)) continue; //��ȣ
                string skillNameKor = cells[1]; // �̸�(�ѱ���)
                string skillNameEng = SanitizeClassName(cells[2]); // �̸�(����)
                string command = cells[3]; // Ŀ�ǵ�
                string type = cells[4]; // Ÿ��(���Ÿ�, �ٰŸ� etc)
                string element = cells[5]; // �Ӽ�
                float.TryParse(cells[6], out float skillRatio); // �μ����
                float.TryParse(cells[7], out float cooldown); // ��Ÿ��
                float.TryParse(cells[8], out float damage); // ������

                float.TryParse(cells[10], out float spawnDelay); // ���� ������
                float.TryParse(cells[11], out float destroyTime); // �Ҹ� �ð�
                
                string description = cells[14]; // ����
                string effectDescription = cells[15]; // ����Ʈ ����
                string animDescription = cells[16]; // �ִϸ��̼� ����

                string className = skillNameEng;
                string assetPath = Path.Combine(saveAssetFolder, $"{className}.asset");

                Type skillType = GetTypeByName(className);
                if (skillType == null || !typeof(CommandData).IsAssignableFrom(skillType))
                {
                    Debug.LogWarning($"{className} Ÿ���� ã�� �� ����. ������ �Ϸ� �� �ٽ� �õ��ϼ���.");
                    continue;
                }

                CommandData asset = AssetDatabase.LoadAssetAtPath(assetPath, skillType) as CommandData;
                if (asset == null)
                {
                    asset = ScriptableObject.CreateInstance(skillType) as CommandData;
                    AssetDatabase.CreateAsset(asset, assetPath);
                    Debug.Log($"[���� ����] {className}");
                }

                asset.commandID = skillID;
                asset.commandNameKor = skillNameKor;
                asset.commandNameEng = skillNameEng;
                asset.stringCommand = command;

                for (int j = 0; j < command.Length; j++)
                {
                    switch (command.Substring(j, 1))
                    {
                        case "��": asset.command[j] = 1; break;
                        case "��": asset.command[j] = 2; break;
                        case "��": asset.command[j] = 3; break;
                        case "��": asset.command[j] = 4; break;
                        case "A": asset.command[j] = 5; break;
                        case "S": asset.command[j] = 6; break;
                        case "D": asset.command[j] = 7; break;
                        case "W": asset.command[j] = 8; break;
                        default: break;
                    }
                }

                //switch(type)
                //{
                //    case "�ٰŸ�":
                //        asset.commandType = CommandType.Melee;
                //        break;
                //    case "���Ÿ�":
                //        asset.commandType = CommandType.Range;
                //        break;
                //    case "����":
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
                Debug.LogError($"{i + 1}��° �� ����: {ex.Message}");
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
