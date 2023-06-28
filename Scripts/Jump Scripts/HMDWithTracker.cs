//using UnityEngine;
//using Valve.VR;

//public class HMDWithTracker : MonoBehaviour
//{
//    public SteamVR_TrackedObject footTracker;
//    public SteamVR_Action_Pose hmdPose;
//    public float jumpThreshold; // ジャンプ判定の距離のしきい値
//    public float jumpForce; // ジャンプ力

//    private void Update()
//    {
//        // 膝のトラッカーの位置を取得
//        Vector3 footPosition = footTracker.transform.position;

//        // HMDの位置を取得
//        Vector3 hmdPosition = hmdPose.GetLocalPosition(SteamVR_Input_Sources.Head);

//        // 距離を計算
//        float distance = Vector3.Distance(footPosition, hmdPosition);

//        // しきい値を超えたらジャンプ
//        if (distance < jumpThreshold)
//        {
//            Debug.Log("A");
//            Jump();
//        }
//    }

//    private void Jump()
//    {
//        // ジャンプ処理を実装する
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
    public float jumpThreshold; // ジャンプ判定の距離のしきい値
    public float jumpForce; // ジャンプ力
    enum State { Neutral, Save, Jump };
    State state = State.Neutral;

    //private void Awake()
    //{
    //    velocityEstimator = GetComponent<VelocityEstimator>();
    //}

    private void Update()
    {
        // 膝のトラッカーの位置を取得
        //Vector3 footPosition = footTracker.transform.position;
        Tracker1Posision = tracker1.GetLocalPosition(SteamVR_Input_Sources.LeftFoot);

        // HMDの位置を取得
        //Vector3 hmdPosition = hmdPose.GetLocalPosition(SteamVR_Input_Sources.Head);
        HMDPosition = tracker1.GetLocalPosition(SteamVR_Input_Sources.Head);

        // 距離を計算
        float distance = Vector3.Distance(Tracker1Posision, HMDPosition);

        //Vector3 HMDS= velocityEstimator.GetVelocityEstimate();

            switch (state)
            {
                case State.Neutral:
                    if (distance < jumpThreshold)
                    {
                        state = State.Save;
                        Debug.Log("Save遷移完了");
                        break;
                    }
                    break;
                case State.Save:
                    if (distance >= 0.5)
                    {
                        state = State.Jump;
                        Debug.Log("Jump遷移完了");
                        break;
                    }
                    break;
                //else if (HMDS.magnitude < 1)
                //{
                //    state = State.Neutral;
                //    Debug.Log("Neutral遷移完了");
                //    break;
                //}
                //break;
                case State.Jump:
                    Jump();
                    state = State.Neutral;
                    Debug.Log("Neutral遷移完了");
                    break;
        }
    }



        //// しきい値を超えたらジャンプ
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
        // ジャンプ方向を計算（斜め前方向）
        Vector3 jumpDirection = (transform.forward + transform.up).normalized;
        // ジャンプ処理を実装する
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
        }
    }
}
