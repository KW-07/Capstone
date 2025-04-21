using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;

    public Player player { get; set; }
    public SceneData sceneData { get; set; }
    public SceneLoader sceneLoader { get; set; }

    private bool _isSaving;
    private bool _isLoading;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else instance = this;
    }

    private void Update()
    {
        // 세이브 및 로드
        // 현시점 사용안되어 주석처리
        //if(!_isSaving)
        //{
        //    SaveAsync();
        //}
        //if(!_isLoading)
        //{
        //    LoadAsync();
        //}
    }

    private async void SaveAsync()
    {
        _isSaving = true;
        await SaveSystem.SaveAsynchronously();
        _isSaving = false;
    }

    private async void LoadAsync()
    {
        _isLoading = true;
        await SaveSystem.LoadAsync();
        _isLoading = false;
    }
}
