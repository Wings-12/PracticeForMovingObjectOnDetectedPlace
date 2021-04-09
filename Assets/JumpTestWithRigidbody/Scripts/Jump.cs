using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    Rigidbody _rigidbody;

    float elapsedTimeSinceTouchBegan;
    bool timerStartFlag;

    bool isTapping;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        elapsedTimeSinceTouchBegan = 0.0f;
        timerStartFlag = false;
        isTapping = false;
    }

    // Update is called once per frame
    void Update()
    {
        isTapping = Tap();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (Tap() && collision.gameObject.tag == "Plane")
        if (isTapping == true)
        {
            Vector3 force = new Vector3(0.0f, 500.0f, 0.0f);
            _rigidbody.AddForce(force);

            isTapping = false;
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
