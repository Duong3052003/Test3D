using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrapFakeGround : MonoBehaviour
{
    [SerializeField] GameObject fakeGround;
    public Vector3 targetPosition;
    private bool isMoving = false;
    private float moveSpeed = 2f;

    private void Start()
    {
        targetPosition = new Vector3(fakeGround.transform.localPosition.x, fakeGround.transform.localPosition.y - 10, fakeGround.transform.localPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isMoving)
        {
            StartMoving();
        }
    }

    void Update()
    {
        if (isMoving)
        {
            fakeGround.transform.position = Vector3.Lerp(fakeGround.transform.position, targetPosition, Time.deltaTime * moveSpeed);

            if (Vector3.Distance(fakeGround.transform.position, targetPosition) < 0.01f)
            {
                fakeGround.transform.position = targetPosition;
            }
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }
}
