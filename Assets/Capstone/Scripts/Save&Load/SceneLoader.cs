using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneDataSO[] _sceneDataSOArray;
    private Dictionary<string, int> _sceneIDToIndexMap = new Dictionary<string, int>();

    private void Awake()
    {
        GameManager.instance.sceneLoader = this;

        PopulateSceneMappings();
    }

    private void PopulateSceneMappings()
    {
        foreach(var sceneDataSO in _sceneDataSOArray)
        {
            _sceneIDToIndexMap[sceneDataSO.uniqueName] = sceneDataSO.sceneIndex;
        }
    }

    public void LoadSceneByIndex(string saveSceneID)
    {
        if(_sceneIDToIndexMap.TryGetValue(saveSceneID, out int sceneIndex))
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError($"No Scene fount for ID: {saveSceneID}");
        }
    }

    public async Task LoadSceneByIndexAsync(string saveSceneID)
    {
        if (_sceneIDToIndexMap.TryGetValue(saveSceneID, out int sceneIndex))
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                if(asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                    break;
                }
                await Task.Yield();
            }
        }
        else
        {
            Debug.LogError($"No Scene fount for ID: {saveSceneID}");
        }
    }
}
