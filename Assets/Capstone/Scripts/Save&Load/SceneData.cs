using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneData : MonoBehaviour
{
    public SceneDataSO Data;

    private void Awake()
    {
        SaveLoadManager.instance.sceneData = this;
    }

    #region Save and Load

    public void Save(ref SceneSaveData data)
    {
        data.sceneID = Data.uniqueName;
    }

    public void Load(SceneSaveData data)
    {
        SaveLoadManager.instance.sceneLoader.LoadSceneByIndex(data.sceneID);
    }

    public async Task LoadAsync(SceneSaveData data)
    {
        await SaveLoadManager.instance.sceneLoader.LoadSceneByIndexAsync(data.sceneID);
    }

    public Task WaitForSceneToBeFullyLoaded()
    {
        TaskCompletionSource<bool> taskCompletion = new TaskCompletionSource<bool>();

        UnityEngine.Events.UnityAction<Scene, LoadSceneMode> sceneLoaderHandler = null;

        sceneLoaderHandler = (scene, mode) =>
        {
            taskCompletion.SetResult(true);
            SceneManager.sceneLoaded -= sceneLoaderHandler;
        };

        SceneManager.sceneLoaded += sceneLoaderHandler;

        return taskCompletion.Task;
    }

    #endregion
}

[System.Serializable]
public struct SceneSaveData
{
    public string sceneID;
}