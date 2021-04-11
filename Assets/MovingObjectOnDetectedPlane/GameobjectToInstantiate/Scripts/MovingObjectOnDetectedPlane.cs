using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class MovingObjectOnDetectedPlane : MonoBehaviour
{
    public GameObject gameObjectToInstantiate;

    Vector2 touchPosition;

    GameObject spawndMovingObject;
    ARRaycastManager _arRaycastManager;

    static List<ARRaycastHit> _arRaycastHits = new List<ARRaycastHit>();

    Rigidbody _spawndObject_rigidbody;

    // タップ処理用
    float elapsedTimeSinceTouchBegan;
    bool timerStartFlag;

    private CharacterController characterController;//  CharacterControllerを使うための変数
    private Vector3 moveDirection;//  CharacterControllerを動かすための変数
    public float JumpPower;//  ジャンプ力

    bool isJumping = false;

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();

        //_spawndObject_rigidbody = _spawndObject_rigidbody.GetComponent<Rigidbody>();

        elapsedTimeSinceTouchBegan = 0.0f;
        timerStartFlag = false;

        

        moveDirection = Vector3.zero;
    }

    /// <summary>
    /// レイが検知した平面に当たったかどうかを判定する
    /// </summary>
    /// <remarks>true: 当たった / false: 当たってない</remarks>
    private bool TryGetTouchPosition(out Vector2 objectPosition)
    {
        if (Input.touchCount > 0)
        {
            objectPosition = Input.GetTouch(index:0).position;
            return true;
        }

        objectPosition = default;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!TryGetTouchPosition(out touchPosition))
        {
            return;
        }

        if (_arRaycastManager.Raycast(touchPosition, _arRaycastHits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = _arRaycastHits[0].pose;

            Vector3 spawnedMovingObjectMovingPosition = hitPose.position + new Vector3(0.0f, 0.1f, 0.0f);

            if (spawndMovingObject == null)
            {
                spawndMovingObject = Instantiate(gameObjectToInstantiate, spawnedMovingObjectMovingPosition, hitPose.rotation);
                // characterControllerにCharacterControllerの値を代入する
                characterController = spawndMovingObject.GetComponent<CharacterController>();
            }
            //else
            //{
            //    spawndMovingObject.transform.position = hitPose.position + new Vector3(0.0f, 0.1f, 0.0f);
            //}
        }

        // バグ：ジャンプできない
        // 推測される原因：
        // ・次のフレームですぐに移動処理に入ってしまうから
        if (characterController.isGrounded)//  もし地面についていたら、
        {
            isJumping = false;

            if (Input.GetMouseButtonDown(0))//  もし、タップされたら、
            {
                isJumping = true;
                moveDirection.y = JumpPower;//  y座標をジャンプ力の分だけ動かす
            }
            //else if (isJumping == false)
            //{
            //    spawndMovingObject.transform.position = playerMovingPosition;
            //}
        }

        moveDirection.y += Physics.gravity.y * Time.deltaTime; //常にy座標を重力の分だけ動かす(重力処理)
        characterController.Move(moveDirection * Time.deltaTime); //CharacterControlloerをmoveDirectionの方向に動かす
    }

    bool Tap()
    {
        // タッチ情報の取得
        Touch _touch = Input.GetTouch(index: 0);

        // タッチ開始した場合にタップ判定のための時間計測開始
        if (_touch.phase == TouchPhase.Began)
        {
            timerStartFlag = true;
        }

        if (timerStartFlag == true)
        {
            elapsedTimeSinceTouchBegan += Time.deltaTime;
            Debug.Log("計測中： " + (elapsedTimeSinceTouchBegan).ToString());
        }

        // 相手エリアをタッチ終了した場合
        if (_touch.phase == TouchPhase.Ended)
        {
            // タップ判定のための時間計測終了
            timerStartFlag = false;
            elapsedTimeSinceTouchBegan = 0.0f;

            // タップ判定のための時間計測がタップした時間計測内の時間である場合
            if (elapsedTimeSinceTouchBegan < 0.1f)
            {
                // タップと判定
                return true;
            }
        }

        // タップではないと判定
        return false;
    }
}
