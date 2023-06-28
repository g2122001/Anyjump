using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System;
using System.Collections;
using UnityEngine.XR;

public class ControllerJump : MonoBehaviour
{
    private SteamVR_Action_Pose tracker1 = SteamVR_Actions.default_Pose;
    private Vector3 HMDPosition;
    private Vector3 Controller;
    public float jumpThreshold; // �W�����v�̑��x臒l
    public float groundDistanceThreshold; // �n�ʂ���̋�����臒l
    public string groundTag = "Ground"; // �n�ʂ̃^�O
    enum State { Neutral, Jump, Slow };
    State state = State.Neutral;
    private bool isJumping = false;//Jump�����𔻒f����
    private Vector3 initialPosition; // �����ʒu
    public float jumpForce;
    Vector3 Ground = new Vector3(0f, 0f, 0f);
    private Vector3 GroundTranceform;//�n�ʂ̈ʒu���



    private void Start()
    {
        // �����ʒu��ۑ�
        initialPosition = transform.position;

    }

    private void Update()
    {


        HMDPosition = InputTracking.GetLocalPosition(XRNode.Head);

        Controller = tracker1.GetLocalPosition(SteamVR_Input_Sources.RightHand);


        float distance = Vector3.Distance(Controller, HMDPosition);
        Debug.Log("Distance =" + distance);

        //Ground�̈ʒu���擾
        Vector3 GroundTranseform = GameObject.Find("Plane").transform.position;

        //CameraRig�̎擾
        Vector3 CameraRig = GameObject.Find("Camera").transform.position;

        float groundDistance = Vector3.Distance(CameraRig, GroundTranseform);
        //Debug.Log("groundDistance=" +groundDistance);


        // �n�ʂ���̋�����臒l�𒴂����ꍇ�̏���
        if (groundDistance > groundDistanceThreshold)
        {
            //Debug.Log("�����N���A�I");
            switch (state)
            {
                case State.Neutral:
                    // �R���g���[���[�̑��x�̑傫����臒l�𒴂����ꍇ�ɃW�����v
                    if (distance > jumpThreshold)
                    {
                        Debug.Log("ControllerJump_Jump");
                        state = State.Jump;
                        break;
                    }
                    break;

                case State.Jump:
                    Jump();
                    state = State.Slow;
                    Debug.Log("ControllerJumo_Slow");
                    break;

                case State.Slow:
                    //�R���g���[���[�̑��x�������Ⴍ�Ȃ�����Neutral�ɑJ��
                    if (distance < 0.3)
                    {
                        state = State.Neutral;
                        Debug.Log("ControllerJump_Neutral");
                        break;
                    }
                    break;
            }
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