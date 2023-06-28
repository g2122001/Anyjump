using UnityEngine;
using Valve.VR;
using UnityEngine.UI;
using System;

public class ButtonJump : MonoBehaviour
{
    public GameObject avatar;
    public float jumpForce;

    public bool isJumping = false;
    public SteamVR_Input_Sources GrabPinch;
    private Vector3 initialPosition; // �����ʒu

    private void Start()
    {
        // �����ʒu��ۑ�
        initialPosition = transform.position;
    }

    void Update()
    {
        // �g���K�[�{�^���������ꂽ��W�����v
        if (SteamVR_Input.GetStateDown("A", GrabPinch))
        {
            Jump();
            //Debug.Log("A");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // �n�ʂɐڐG������W�����v�ƕ��V�����Z�b�g
        if ((other.gameObject.tag == "Ground") && (isJumping = true))
        {
            ResetPosition();
            ResetForce();
        }
    }
    private void ResetForce()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        // �͂��[���ɂ���
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void Jump()
    {
        isJumping = true;
        // �W�����v�������v�Z�i�΂ߑO�����j
        Vector3 jumpDirection = (transform.forward + transform.up).normalized;
        // �W�����v��������������
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
        }
    }
    private void ResetPosition()
    {
        // �ʒu�������ʒu�Ƀ��Z�b�g����
        transform.position = initialPosition;
        isJumping = false;
    }
}