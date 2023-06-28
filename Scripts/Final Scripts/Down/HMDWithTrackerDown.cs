using System;
using UnityEngine;
using Valve.VR;
using System.Collections;
using Valve.VR.InteractionSystem;
using System.Threading;
using UnityEngine.Events;
using UnityEngine.XR;
public class HMDWithTrackerDown : MonoBehaviour
{
    //public SteamVR_TrackedObject footTracker;
    //public SteamVR_TrackedObject trackedObject;
    //private VelocityEstimator velocityEstimator;
    private Vector3 Tracker1Posision;
    private SteamVR_Action_Pose tracker1 = SteamVR_Actions.default_Pose;

    //public SteamVR_Action_Pose hmdPose;
    private Vector3 HMDPosition;
    public float groundDistanceThreshold; // �n�ʂ���̋�����臒l
    public float jumpThreshold; // �W�����v����̋����̂������l
    public float diveForce;        // �}�~����
    private Rigidbody rb;
    public string groundTag = "Ground"; // �n�ʂ̃^�O
    enum State { Neutral, Down, Back };
    State state = State.Neutral;
    private bool isJumping = false;//Jump�����𔻒f����
    private Vector3 initialPosition; // �����ʒu

    private void Start()
    {
        // �����ʒu��ۑ�
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // �G�̃g���b�J�[�̈ʒu���擾
        //Vector3 footPosition = footTracker.transform.position;
        Tracker1Posision = tracker1.GetLocalPosition(SteamVR_Input_Sources.LeftFoot);

        // HMD�̈ʒu���擾
        //Vector3 hmdPosition = hmdPose.GetLocalPosition(SteamVR_Input_Sources.Head);
        HMDPosition = InputTracking.GetLocalPosition(XRNode.Head);

        // �������v�Z
        float distance = Vector3.Distance(Tracker1Posision, HMDPosition);//���̋����ɉ�����jumpThreshold�Ȃǂ�ς��Ă���

        //�����ŋ�����Print���Ă݂�
        Debug.Log("Distance =" +distance);

        //Ground�̈ʒu���擾
        Vector3 GroundTranseform = GameObject.Find("Plane").transform.position;

        //CameraRig�̎擾
        Vector3 CameraRig = GameObject.Find("Camera").transform.position;

        float groundDistance = Vector3.Distance(CameraRig, GroundTranseform);

        //groundDistance��print���Ă݂�
        //Debug.Log("groundDistance=" +groundDistance);


        // �n�ʂ���̋�����臒l�𒴂����ꍇ�̏���
        if (groundDistance > groundDistanceThreshold)
        {
            switch (state)
            {
                case State.Neutral:
                    if (distance < jumpThreshold)
                    {
                        state = State.Down;
                        Debug.Log("Down�J�ڊ���");
                        break;
                    }
                    break;

                case State.Down:
                    Down();
                    state = State.Back;
                    Debug.Log("Back�J�ڊ���");
                    break;

                case State.Back:
                    if (distance >= 0.9)//���l�v����
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

    private void Down()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            isJumping = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // ���������̑��x��ێ�
            rb.AddForce(Vector3.down * diveForce, ForceMode.Impulse);
        }

        private void ResetPosition()
        {
            // �ʒu�������ʒu�Ƀ��Z�b�g����
            transform.position = initialPosition;
            isJumping = false;
        }
    }
