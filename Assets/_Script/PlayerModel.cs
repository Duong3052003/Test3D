using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] private GameObject currentCharacter;

    private void Start()
    {
        if (currentCharacter != null) return;
        CreateModel();
    }

    public void ChangeCharacter(int value)
    {
        GameManager.Instance.ChangeIndexModel(value);

        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }

        CreateModel();
    }

    private void CreateModel()
    {
        currentCharacter = Instantiate(GameManager.Instance.ModelCurrent(), transform.position, transform.rotation);
        currentCharacter.transform.parent = transform;
        currentCharacter.transform.localScale = Vector3.one;
    }

    public void ChangeStateAnimation(int _index)
    {
        if (currentCharacter == null) return;
        currentCharacter.GetComponent<Animator>().SetInteger("State", _index);
    }
}
