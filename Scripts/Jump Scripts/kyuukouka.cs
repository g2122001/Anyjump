using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kyuukouka : MonoBehaviour
{
    public float jumpForce = 5f;         // �W�����v��
    public float diveForce = 10f;        // �}�~����

    private Rigidbody rb;
    private bool isJumping = false;
    private bool isDiving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // �W�����v
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isDiving)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }

        // �}�~���J�n
        if (Input.GetKeyDown(KeyCode.Z) && isJumping && !isDiving)
        {
            isDiving = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // ���������̑��x��ێ�
            rb.AddForce(Vector3.down * diveForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �n�ʂɐڐG������W�����v��ԂƋ}�~����Ԃ����Z�b�g
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            isDiving = false;
        }
    }
}