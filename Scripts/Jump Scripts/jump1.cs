using UnityEngine;

public class jump1 : MonoBehaviour
{
    public Transform leftController;
    public Transform rightController;
    public float trackingScale = 1f;

    void Update()
    {
        // VIVE�g���b�J�[����v���C���[�̈ʒu�����擾
        Vector3 playerPosition = (leftController.position + rightController.position) * 0.5f * trackingScale;

        // �L�����N�^�[�̈ʒu�ɔ��f
        transform.position = playerPosition;
    }
}