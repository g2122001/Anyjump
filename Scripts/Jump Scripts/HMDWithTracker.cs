//using UnityEngine;
//using Valve.VR;

//public class HMDWithTracker : MonoBehaviour
//{
//    public SteamVR_TrackedObject footTracker;
//    public SteamVR_Action_Pose hmdPose;
//    public float jumpThreshold; // �W�����v����̋����̂������l
//    public float jumpForce; // �W�����v��

//    private void Update()
//    {
//        // �G�̃g���b�J�[�̈ʒu���擾
//        Vector3 footPosition = footTracker.transform.position;

//        // HMD�̈ʒu���擾
//        Vector3 hmdPosition = hmdPose.GetLocalPosition(SteamVR_Input_Sources.Head);

//        // �������v�Z
//        float distance = Vector3.Distance(footPosition, hmdPosition);

//        // �������l�𒴂�����W�����v
//        if (distance < jumpThreshold)
//        {
//            Debug.Log("A");
//            Jump();
//        }
//    }

//    private void Jump()
//    {
//        // �W�����v��������������
//        Rigidbody rb = GetComponent<Rigidbody>();
//        if (rb != null)
//        {
//            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//        }
//    }
//}


using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HMDWithTracker : MonoBehaviour
{
    //public SteamVR_TrackedObject footTracker;
    //public SteamVR_TrackedObject trackedObject;
    //private VelocityEstimator velocityEstimator;
    private Vector3 Tracker1Posision;
    private SteamVR_Action_Pose tracker1 = SteamVR_Actions.default_Pose;
    
    //public SteamVR_Action_Pose hmdPose;
    private Vector3 HMDPosition;
    public float jumpThreshold; // �W�����v����̋����̂������l
    public float jumpForce; // �W�����v��
    enum State { Neutral, Save, Jump };
    State state = State.Neutral;

    //private void Awake()
    //{
    //    velocityEstimator = GetComponent<VelocityEstimator>();
    //}

    private void Update()
    {
        // �G�̃g���b�J�[�̈ʒu���擾
        //Vector3 footPosition = footTracker.transform.position;
        Tracker1Posision = tracker1.GetLocalPosition(SteamVR_Input_Sources.LeftFoot);

        // HMD�̈ʒu���擾
        //Vector3 hmdPosition = hmdPose.GetLocalPosition(SteamVR_Input_Sources.Head);
        HMDPosition = tracker1.GetLocalPosition(SteamVR_Input_Sources.Head);

        // �������v�Z
        float distance = Vector3.Distance(Tracker1Posision, HMDPosition);

        //Vector3 HMDS= velocityEstimator.GetVelocityEstimate();

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
                    if (distance >= 0.5)
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



        //// �������l�𒴂�����W�����v
        //timeleft -= Time.deltaTime;
        //if (timeleft <= 0.0)
        //{
        //    timeleft = 1.0f;
        //    if (distance < jumpThreshold)
        //    {
        //        Debug.Log("A");
        //        Jump();
        //    }

    private void Jump()
    {
        // �W�����v�������v�Z�i�΂ߑO�����j
        Vector3 jumpDirection = (transform.forward + transform.up).normalized;
        // �W�����v��������������
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
        }
    }
}
