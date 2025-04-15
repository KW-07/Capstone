using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;

public class SaveSystem
{
    private static SaveData _saveData = new SaveData();

    [System.Serializable]
    public struct SaveData
    {
        public PlayerMoveSavaData playerMoveData;
        // 추가
        public SceneSaveData sceneSaveData;
    }

    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "/save" + ".save";
        return saveFile;
    }

    #region Save Async

    public static async Task SaveAsynchronously()
    {
        await SaveAsync();
    }

    private static async Task SaveAsync()
    {
        HandleSaveData();

        await File.WriteAllTextAsync(SaveFileName(), JsonUtility.ToJson(_saveData, true));
    }

    #endregion

    private static void HandleSaveData()
    {
        GameManager.instance.player.Save(ref _saveData.playerMoveData);

        GameManager.instance.sceneData.Save(ref _saveData.sceneSaveData);
    }

    #region Load Async

    public static async Task LoadAsync()
    {
        string saveContent = File.ReadAllText(SaveFileName());

        _saveData = JsonUtility.FromJson<SaveData>(saveContent);

        await HandleLoadDataAsync();
    }

    private static async Task HandleLoadDataAsync()
    {
        await GameManager.instance.sceneData.LoadAsync(_saveData.sceneSaveData);

        await GameManager.instance.sceneData.WaitForSceneToBeFullyLoaded();

        // 추가되는 모든 것
        GameManager.instance.player.Load(_saveData.playerMoveData);
    }
    #endregion
}
