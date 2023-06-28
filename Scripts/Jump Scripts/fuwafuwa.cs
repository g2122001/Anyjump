using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class fuwafuwa : MonoBehaviour
{
    public float jumpForce = 5f;         // �W�����v��
    public float floatForce = 2f;        // ���V��
    public float maxFloatDuration = 2f;  // ���V���Ԃ̍ő�l

    private Rigidbody rb;
    private bool isJumping = false;
    private bool isFloating = false;
    private float floatTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // �W�����v
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isFloating)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }

        // ���V
        if (isJumping && !isFloating)
        {
            floatTimer += Time.deltaTime;
            if (floatTimer >= maxFloatDuration)
            {
                isFloating = true;
            }
        }

        // ���V���̗���
        if (isFloating)
        {
            rb.AddForce(Vector3.down * floatForce, ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �n�ʂɐڐG������W�����v�ƕ��V�����Z�b�g
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            isFloating = false;
            floatTimer = 0f;
        }
    }
}