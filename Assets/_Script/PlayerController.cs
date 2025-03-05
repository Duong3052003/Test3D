using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private Rigidbody rb;
    private PlayerModel playerModel;

    [SerializeField] Sensor_Collider groundSensor;
    [SerializeField] LayerMask deadLayer;
    [SerializeField] FixedJoystick fixedJoystick;

    private Vector3 move;
    private bool isDead =false;
    private bool canMove =false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerModel = GetComponent<PlayerModel>();
    }

    void Update()
    {
        if (canMove)
        {
            Move();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        HandldeAnimation();
    }

    void Move()
    {
        float joystickX = fixedJoystick != null ? fixedJoystick.Vertical : 0f;
        float joystickZ = fixedJoystick != null ? fixedJoystick.Horizontal : 0f;

        float keyboardX = Input.GetAxis("Vertical");
        float keyboardZ = Input.GetAxis("Horizontal");

        float moveX = Mathf.Abs(joystickX) > 0.1f ? joystickX : keyboardX;
        float moveZ = Mathf.Abs(joystickZ) > 0.1f ? joystickZ : keyboardZ;

        move = new Vector3(moveX, 0, -moveZ).normalized * moveSpeed;
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);

        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            if (Quaternion.Angle(transform.rotation, targetRotation) > 2f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20);
            }
        }
    }


    public void Jump()
    {
        if (groundSensor.State())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    void HandldeAnimation()
    {
        if(isDead)
            playerModel.ChangeStateAnimation(3);
        else if (!groundSensor.State())
            playerModel.ChangeStateAnimation(2);
        else if (move.magnitude > 0.1f)
            playerModel.ChangeStateAnimation(1);
        else playerModel.ChangeStateAnimation(0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & deadLayer) != 0)
        {
            Die();
        }

        if (other.gameObject.CompareTag("WinPoint"))
        {
            Win();
        }
    }


    void Win()
    {
        LevelManager.Instance.CompleteLevel();
        LevelManager.Instance.NextLevel();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        SetMove(false);
        UIManager.Instance.GameOverScreen(true);
    }

    public void Revive()
    {
        isDead = false;
    }

    public void SetMove(bool boolen)
    {
        move = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.useGravity = boolen;
        canMove = boolen;
    }
}
