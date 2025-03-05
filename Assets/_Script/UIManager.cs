using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject levelButtonPrefab;
    [SerializeField] Transform levelContainer;
    [SerializeField] GameObject[] backGrounds;
    [SerializeField] GameObject levelScreen;
    [SerializeField] GameObject menuScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject inGameUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GenerateLevelButtons();
    }

    void GenerateLevelButtons()
    {
        int totalLevels = SceneManager.sceneCountInBuildSettings;

        for (int i = 1; i < totalLevels; i++)
        {
            GameObject buttonObj = Instantiate(levelButtonPrefab, levelContainer);
            SelectLevelBtn button = buttonObj.GetComponent<SelectLevelBtn>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = (i).ToString();

            int levelIndex = i;
            button.Initiation(levelIndex);
        }
    }

    void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void PlayBtn()
    {
        VirtualCameraSetting.Instance.Rotate(new Vector3(-46, 0, 0));
        VirtualCameraSetting.Instance.MoveTo(new Vector3(0, 200, -10));
    }

    public void MenuActive(bool boolen)
    {
        levelScreen.SetActive(boolen);
        menuScreen.SetActive(boolen);
        backGrounds[0].SetActive(boolen);
        backGrounds[1].SetActive(!boolen);
    }

    public void GameOverScreen(bool boolen)
    {
        gameOverScreen.SetActive(boolen);
    }
    
    public void InGameUI(bool boolen)
    {
        inGameUI.SetActive(boolen);
    }

    public void RetryLevel()
    {
        GameOverScreen(false);
        LevelManager.Instance.ReloadCurrentLevel();
        GameManager.Instance.ResetPlayer();
    }

    public void NextLevel()
    {
        LevelManager.Instance.NextLevel();
        GameManager.Instance.ResetPlayer();
    }

    public void MenuGame()
    {
        gameOverScreen.SetActive(false);
        VirtualCameraSetting.Instance.StopFollowTarget();
        VirtualCameraSetting.Instance.Rotate(new Vector3(0, 0, 0));
        VirtualCameraSetting.Instance.MoveTo(new Vector3(0, 200, -10));
        GameManager.Instance.PreviewPlayer();
        MenuActive(true);
    }
}
