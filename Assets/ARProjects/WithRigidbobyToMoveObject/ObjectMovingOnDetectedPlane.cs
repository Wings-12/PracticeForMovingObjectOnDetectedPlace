using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectMovingOnDetectedPlane : MonoBehaviour
{
    Vector2 _touchPosition = Vector2.zero;

    ARRaycastManager _aRRaycastManager;

    List<ARRaycastHit> _aRRaycastHits = new List<ARRaycastHit>();

    [SerializeField] GameObject _objectToInstantiate;

    GameObject _instantiatedObject;

    Rigidbody _rigidbody;

    Pose _hitPose;

    float elapsedTimeSinceTouchBegan = 0.0f;
    bool timerStartFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        _aRRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);
            _touchPosition = _touch.position;

            Vector3 _objectMoveDestination = Vector3.zero;

            if (_aRRaycastManager.Raycast(_touchPosition, _aRRaycastHits, TrackableType.PlaneWithinPolygon))
            {
                _hitPose = _aRRaycastHits[0].pose;

                _objectMoveDestination = _hitPose.position + new Vector3(0.0f, 1.0f, 0.0f);

                if (_instantiatedObject == null)
                {
                    _instantiatedObject = Instantiate(_objectToInstantiate, _objectMoveDestination, _hitPose.rotation);
                    _rigidbody = _instantiatedObject.GetComponent<Rigidbody>();
                }
                //else
                //{
                //    //_instantiatedObject.transform.position = _objectMoveDestination;

                //    float moveBoostPower = 10.0f;

                //    Vector3 force = new Vector3(_objectMoveDestination.x * moveBoostPower + this.transform.position.x,
                //        0.0f, 
                //        -_objectMoveDestination.z * moveBoostPower + this.transform.position.z);

                //    // バグ：タップした方向ではなく、逆の方向で、しかも左右に力が働きにくい？感じになっている
                //    _rigidbody.AddForce(force);
                //}
            }

            if (Tap())
            {
                float moveBoostPower = 10.0f;

                Vector3 force = new Vector3(0.0f,
                    _objectMoveDestination.y * moveBoostPower,
                    0.0f);

                _rigidbody.AddForce(force);
            }

            // バグ：AR検知した平面よりもオブジェクトが低い座標にいたら、オブジェクトの座標を
            // レイを投げた座標から+10.0fの座標に移動するようにしたかったが、平面から落ちてしまう
            if (_instantiatedObject.transform.position.y < _hitPose.position.y - 10.0f)
            {
                Debug.Log("通過");

                _instantiatedObject.transform.position =
                    new Vector3(_hitPose.position.x, _hitPose.position.y + 10.0f, _hitPose.position.y);
            }
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 force = new Vector3(0.0f, 20.0f, 0.0f);

        //    _rigidbody.AddForce(force);
        //}
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

            // タップ判定のための時間計測がタップした時間計測内の時間である場合
            if (elapsedTimeSinceTouchBegan < 0.1f)
            {
                Debug.Log("タップ判定");

                elapsedTimeSinceTouchBegan = 0.0f;

                // タップと判定
                return true;
            }
            else
            {
                Debug.Log("タップではないと判定");
                elapsedTimeSinceTouchBegan = 0.0f;

                return false;
            }
        }

        // タップではないと判定
        return false;
    }
}
