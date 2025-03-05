using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    private int indexSceneCurrent;
    private List<int> completedLevels = new List<int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCompletedLevels();
            SceneManager.sceneLoaded += OnLevelLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (completedLevels.Count == 0)
        {
            completedLevels.Add(0);
            SaveCompletedLevels();
        }
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        indexSceneCurrent = scene.buildIndex;
    }

    public void LoadLevelAdditive(int sceneIndex)
    {
        if (!SceneManager.GetSceneByBuildIndex(sceneIndex).isLoaded)
        {
            indexSceneCurrent = sceneIndex;
            SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        }
    }

    public void UnloadLevelCurrent()
    {
        if (SceneManager.GetSceneByBuildIndex(indexSceneCurrent).isLoaded)
        {
            SceneManager.UnloadSceneAsync(indexSceneCurrent);
        }
    }

    public void ReloadCurrentLevel()
    {
        if (indexSceneCurrent != 0)
        {
            UnloadLevelCurrent();
            LoadLevelAdditive(indexSceneCurrent);
        }
    }

    public void NextLevel()
    {
        int nextLevel = indexSceneCurrent + 1;
        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            UnloadLevelCurrent();
            LoadLevelAdditive(nextLevel);
        }
        else
        {
            UIManager.Instance.MenuGame();
        }
    }

    public void CompleteLevel()
    {
        if (!completedLevels.Contains(indexSceneCurrent))
        {
            completedLevels.Add(indexSceneCurrent);
            SaveCompletedLevels();
        }
    }

    private void SaveCompletedLevels()
    {
        string saveData = string.Join(",", completedLevels);
        PlayerPrefs.SetString("CompletedLevels", saveData);
        PlayerPrefs.Save();
    }

    private void LoadCompletedLevels()
    {
        string saveData = PlayerPrefs.GetString("CompletedLevels", "");
        if (!string.IsNullOrEmpty(saveData))
        {
            completedLevels = new List<int>(Array.ConvertAll(saveData.Split(','), int.Parse));
        }
    }

    public bool IsLevelCompleted(int levelIndex)
    {
        return completedLevels.Contains(levelIndex);
    }

    public void ResetLevels()
    {
        PlayerPrefs.DeleteKey("CompletedLevels");
        completedLevels.Clear();
        completedLevels.Add(0);
        SaveCompletedLevels();
    }
}