using System;
using UnityEngine;
using Valve.VR;
using System.Collections;
using Valve.VR.InteractionSystem;
using System.Threading;
using UnityEngine.Events;
using UnityEngine.XR;

public class AngleBuck : MonoBehaviour
{
    private Vector3 HMDPosition;
    public float groundDistanceThreshold; // �n�ʂ���̋�����臒l
    public string groundTag = "Ground"; // �n�ʂ̃^�O
    private Transform hmdTransform; // HMD�̃g�����X�t�H�[��
    enum State { Neutral, Buck };
    State state = State.Neutral;
    private bool isJumping = false;//Jump�����𔻒f����
    private Vector3 initialPosition; // �����ʒu
    private Rigidbody rb;
    public float forceMagnitude = 5f; // �i�s�����Ƌt�̗͂̑傫��
    public float thresholdAngle; // �i�s�����Ƌt�ɐi�ޏ����ƂȂ�p�x��臒l

    public SteamVR_Action_Pose poseAction = SteamVR_Input.GetAction<SteamVR_Action_Pose>("Pose");

    private void Start()
    {
        // HMD�̃g�����X�t�H�[�����擾
        rb = GetComponent<Rigidbody>();
        // �����ʒu��ۑ�
        initialPosition = transform.position;


    }

    private void Update()
    {

        // �w�b�h�}�E���g�f�B�X�v���C�̌������擾
        Transform targetTransform = GameObject.Find("Camera").transform;

        Quaternion rotation = targetTransform.rotation;


        //Debug.Log("Rotation"+rotation);

        // ������x�N�g�����擾
        Vector3 upDirection = Vector3.up;

        // HMD�̏�����x�N�g�����擾
        Vector3 hmdUpDirection =rotation * upDirection;

        // ������p�x�̌v�Z
        float angle = Vector3.Angle(hmdUpDirection, upDirection);


        //�p�x������Ă݂�
        Debug.Log("angle=" + angle);

        //Ground�̈ʒu���擾
        Vector3 GroundTranseform = GameObject.Find("Plane").transform.position;

        //CameraRig�̎擾
        Vector3 CameraRig = GameObject.Find("Camera").transform.position;

        float groundDistance = Vector3.Distance(CameraRig, GroundTranseform);


        // �n�ʂ���̋�����臒l�𒴂��Ă�����
        if (groundDistance > groundDistanceThreshold)
        {
            switch (state)
            {
                case State.Neutral:
                    //angle��臒l�𒴂��Ă�����
                    if (angle > thresholdAngle)
                    {
                        state = State.Buck;
                        Debug.Log("Buck�J�ڊ���");
                        break;
                    }
                    break;

                case State.Buck:
                    Buck();
                    state = State.Neutral;
                    Debug.Log("Neutral�J�ڊ���");
                    break;

                    //case State.Slow:
                    //   if (distance >= 0.5)//���l�v����
                    //   {
                    //     state = State.Neutral;
                    //       Debug.Log("Neutral�J�ڊ���");
                    //       break;
                    //   }
                    //   break;
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

    private void ResetPosition()
        {
            // �ʒu�������ʒu�Ƀ��Z�b�g����
            transform.position = initialPosition;
            isJumping = false;
        }

        private void Buck()
        {
            isJumping = true;
            // �i�s�����Ƌt�̕����ɗ͂�������
            rb.AddForce(-transform.forward * forceMagnitude, ForceMode.Impulse);
        }
    }