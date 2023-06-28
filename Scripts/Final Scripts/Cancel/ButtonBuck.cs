using UnityEngine;
using Valve.VR;
using UnityEngine.UI;
using System;

public class ButtonBuck : MonoBehaviour
{
    public GameObject avatar;
    public bool isJumping = false;
    public SteamVR_Input_Sources GrabPinch;
    private Vector3 initialPosition; // �����ʒu
    public float forceMagnitude = 5f; // �i�s�����Ƌt�̗͂̑傫��
    private Rigidbody rb;

    private void Start()
    {
        // �����ʒu��ۑ�
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // �g���K�[�{�^����������Ă���Ԍ�ނ���
        if (SteamVR_Input.GetState("A", GrabPinch))
        {
            Buck();
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

    private void Buck()
    {
        isJumping = true;
        // �i�s�����Ƌt�̕����ɗ͂�������
        rb.AddForce(-transform.forward * forceMagnitude, ForceMode.Impulse);
    }

    private void ResetPosition()
    {
        // �ʒu�������ʒu�Ƀ��Z�b�g����
        transform.position = initialPosition;
        isJumping = false;
    }
}
