using System;
using UnityEngine;
using Valve.VR;
using System.Collections;
using Valve.VR.InteractionSystem;
using System.Threading;
using UnityEngine.Events;
using UnityEngine.XR;


public class BaseJump : MonoBehaviour
{
    //public SteamVR_TrackedObject footTracker;
    //public SteamVR_TrackedObject trackedObject;
    //private VelocityEstimator velocityEstimator;
    private Vector3 Tracker1Posision;
    private SteamVR_Action_Pose tracker1 = SteamVR_Actions.default_Pose;

    //public SteamVR_Action_Pose hmdPose;
    private Vector3 HMDPosition;
    public float groundDistanceThreshold; // �n�ʂ���̋�����臒l
    public string groundTag = "Ground"; // �n�ʂ̃^�O
    private Transform hmdTransform; // HMD�̃g�����X�t�H�[��
    public float jumpThreshold; // �W�����v����̋����̂������l
    public float jumpForce; // �W�����v��
    enum State { Neutral, Save, Jump };
    State state = State.Neutral;
    private bool isJumping = false;//Jump�����𔻒f����
    private Vector3 initialPosition; // �����ʒu
    Vector3 Ground = new Vector3(0f, 0f, 0f);
    private Vector3 GroundTranceform;//�n�ʂ̈ʒu���


    private void Start()
    {
        // �����ʒu��ۑ�
        initialPosition = transform.position;
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
        //Debug.Log("Distance =" +distance);

        //Vector3 HMDS= velocityEstimator.GetVelocityEstimate();

        //Ground�̈ʒu���擾
        Vector3 GroundTranseform = GameObject.Find("Plane").transform.position;

        //CameraRig�̎擾
        Vector3 CameraRig = GameObject.Find("Camera").transform.position;

        float groundDistance = Vector3.Distance(CameraRig, GroundTranseform);
        //Debug.Log("groudndistanse="+groundDistance);


        // �W�����v����O�Ȃ̂�Distanse��臒l���Ⴉ������
        if (groundDistance < groundDistanceThreshold)
        {
            switch (state)
            {
                case State.Neutral:
                    if (distance < jumpThreshold)
                    {
                        state = State.Save;
                        Debug.Log("Save�J�ڊ���");
                        break;
                    }
                    break;
                case State.Save:
                    if (distance >= 0.9)//���l�v����
                    {
                        state = State.Jump;
                        Debug.Log("Jump�J�ڊ���");
                        break;
                    }
                    break;
                //else if (HMDS.magnitude < 1)
                //{
                //    state = State.Neutral;
                //    Debug.Log("Neutral�J�ڊ���");
                //    break;
                //}
                //break;
                case State.Jump:
                    Jump();
                    state = State.Neutral;
                    Debug.Log("Neutral�J�ڊ���");
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

    //private float GetGroundHeight()
    //{
    //    // Ground�Ƃ����^�O�̃I�u�W�F�N�g��T���A���̈ʒu��Ԃ�
    //    GameObject groundObject = GameObject.FindGameObjectWithTag(groundTag);
    //    if (groundObject != null)
    //    {
    //        return groundObject.transform.position.y;
    //    }
    //    else
    //    {
    //        return 0f; // �n�ʂ�������Ȃ��ꍇ��0��Ԃ��i�f�t�H���g�̍����j
    //    }
    //}

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