using UnityEngine;
using Valve.VR;

public class JumpWithHMD: MonoBehaviour
{
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;
    public SteamVR_Action_Vector2 thumbstickAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("default", "Thumbstick");

    public float jumpThreshold = 0.5f;  // ���L����̂������l
    public float jumpForce = 5f;        // �W�����v��

    private bool isJumping = false;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        Vector2 thumbstickValue = thumbstickAction.GetAxis(inputSource);

        if (!isJumping && Mathf.Abs(thumbstickValue.y) >= jumpThreshold)
        {
            isJumping = true;
            Jump();
        }
    }

    private void Jump()
    {
        // �W�����v��������������
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
