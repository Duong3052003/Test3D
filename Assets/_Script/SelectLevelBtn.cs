using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevelBtn : BaseButton
{
    private int index;

    private void OnEnable()
    {
        Initiation(index);
    }

    public void Initiation(int _index)
    {
        index = _index;
        bool unlocked = LevelManager.Instance.IsLevelCompleted(index-1);
        button.interactable = unlocked;
    }

    protected override void OnClick()
    {
        LevelManager.Instance.LoadLevelAdditive(index);
        UIManager.Instance.MenuActive(false);
        VirtualCameraSetting.Instance.MoveTo(new Vector3(-7, 6, 0));
        VirtualCameraSetting.Instance.Rotate(new Vector3(30, 87, 0));
        GameManager.Instance.ResetPlayer();
    }
}
