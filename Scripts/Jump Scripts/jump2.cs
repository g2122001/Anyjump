using UnityEngine;

public class jump2 : MonoBehaviour
{
    public GameObject avatar;
    public Transform leftController;
    public Transform rightController;
    public float jumpForce = 5f;
    public float jumpThreshold = 0.2f;

    private bool isJumping = false;
    private bool isStretching = false;

    void Update()
    {
        // ���E�̃R���g���[���[�̈ʒu�����擾
        Vector3 leftControllerPos = leftController.position;
        Vector3 rightControllerPos = rightController.position;

        // �R���g���[���[�����̈ʒu��艺�ɂ���ꍇ�ɃW�����v
        if ((leftControllerPos.y < jumpThreshold) && (rightControllerPos.y < jumpThreshold))
        {
            if (!isStretching)
            {
                isJumping = true;
                isStretching = true;
            }
        }
        else
        {
            isStretching = false;
        }
    }

    void FixedUpdate()
    {
        // �W�����v����
        if (isJumping)
        {
            // �W�����v�������v�Z�i�΂ߑO�����j
            Vector3 jumpDirection = (transform.forward + transform.up).normalized;

            // �L�����N�^�[�ɃW�����v�͂�^����
            Rigidbody avatarRigidbody = avatar.GetComponent<Rigidbody>();
            avatarRigidbody.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);

            isJumping = false;
        }
    }
}