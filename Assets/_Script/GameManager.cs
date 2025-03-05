using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    GameObject playerContainer;
    [SerializeField] GameObject playerPreviewPoint;
    public GameObject[] characterPrefabs;
    public int index;

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
        playerContainer = GameObject.FindGameObjectWithTag("Player");
    }

    public GameObject ModelCurrent()
    {
        return characterPrefabs[index].gameObject;
    }

    public void ChangeIndexModel(int value)
    {
        index += value;
        if (index >= characterPrefabs.Length)
        {
            index = 0;
        }
        else if (index < 0)
        {
            index = characterPrefabs.Length - 1;
        }
    }

    public void ResetPlayer()
    {
        UIManager.Instance.InGameUI(true);
        VirtualCameraSetting.Instance.FollowTarget(playerContainer.transform);
        playerContainer.transform.position = new Vector3(0,1,0);
        playerContainer.GetComponent<PlayerController>().Revive();
        playerContainer.GetComponent<PlayerController>().SetMove(true);
    }

    public void PreviewPlayer()
    {
        UIManager.Instance.InGameUI(false);
        playerContainer.transform.position = playerPreviewPoint.transform.position;
        playerContainer.transform.rotation = playerPreviewPoint.transform.rotation;
        playerContainer.GetComponent<PlayerController>().Revive();
        playerContainer.GetComponent<PlayerController>().SetMove(false);
    }
}
