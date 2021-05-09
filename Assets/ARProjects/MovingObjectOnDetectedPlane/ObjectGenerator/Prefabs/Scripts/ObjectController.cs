using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    #region タップ処理用
    float elapsedTimeSinceTouchBegan;
    bool timerStartFlag;
    #endregion

    Rigidbody _spawndObject_rigidbody;

    bool isTapped;

    // Start is called before the first frame update
    void Awake()
    {
        #region タップ処理用
        elapsedTimeSinceTouchBegan = 0.0f;
        timerStartFlag = false;
        #endregion

        _spawndObject_rigidbody = GetComponent<Rigidbody>();

        isTapped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Tap() == true)
        {
            isTapped = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTapped == true)
        {
            Vector3 force = new Vector3(0.0f, 9.0f, 0.0f);
            _spawndObject_rigidbody.AddForce(force);

            isTapped = false;
        }
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
                elapsedTimeSinceTouchBegan = 0.0f;

                // タップと判定
                return true;
            }
            else
            {
                elapsedTimeSinceTouchBegan = 0.0f;

                return false;
            }
        }

        // タップではないと判定
        return false;
    }
}
