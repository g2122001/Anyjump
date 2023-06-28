using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System;
using System.Collections;
using UnityEngine.XR;

public class ControllerMovebuck : MonoBehaviour
{
    private SteamVR_Action_Pose tracker1 = SteamVR_Actions.default_Pose;
    private Vector3 HMDPosition;
    private Vector3 Controller;
    public float jumpThreshold; // �R���g���[���\�̑��x臒l
    public float groundDistanceThreshold; // �n�ʂ���̋�����臒l
    enum State { Neutral, Buck, Slow };
    State state = State.Neutral;
    private bool isJumping = false;//Jump�����𔻒f����
    private Vector3 initialPosition; // �����ʒu
    public float forceMagnitude; // �i�s�����Ƌt�̗͂̑傫��
    private Rigidbody rb;

    private void Start()
    {
        // �����ʒu��ۑ�
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        // �R���g���[���[�̈ʒu���擾
        //Vector3 footPosition = footTracker.transform.position;
        Controller= tracker1.GetLocalPosition(SteamVR_Input_Sources.RightHand);

        // HMD�̈ʒu���擾
        //Vector3 hmdPosition = hmdPose.GetLocalPosition(SteamVR_Input_Sources.Head);
        HMDPosition = InputTracking.GetLocalPosition(XRNode.Head);

        float distance = Vector3.Distance(Controller, HMDPosition);
        //Debug.Log("Distance =" + distance);

        //Ground�̈ʒu���擾
        Vector3 GroundTranseform = GameObject.Find("Plane").transform.position;

        //CameraRig�̎擾
        Vector3 CameraRig = GameObject.Find("Camera").transform.position;

        float groundDistance = Vector3.Distance(CameraRig, GroundTranseform);

        // �n�ʂ���̋�����臒l�𒴂����ꍇ�̏���
        if (groundDistance > groundDistanceThreshold)
        {

            switch (state)
            {
                case State.Neutral:
                    // �R���g���[���[�̑��x�̑傫����臒l�𒴂����ꍇ�Ƀo�b�N
                    if (distance > jumpThreshold)
                    {
                        Debug.Log("Buck�J�ڊ���");
                        state = State.Buck;
                        break;
                    }
                    break;
                case State.Buck:
                    Buck();
                    state = State.Slow;
                    Debug.Log("Slow�J�ڊ���");
                    break;

                case State.Slow:
                          //�R���g���[���[�̑��x�������Ⴍ�Ȃ�����Neutral�ɑJ��
                          if (distance < jumpThreshold)
                    {
                        state = State.Neutral;
                              Debug.Log("Neutral�J�ڊ���");
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