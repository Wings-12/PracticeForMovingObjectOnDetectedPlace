using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpByTapping : MonoBehaviour
{
    float elapsedTimeSinceTouchBegan = 0.0f;
    bool timerStartFlag = false;

    Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Tap())
        {
            Vector3 force = new Vector3(0.0f, 90.0f, 0.0f);
            _rigidbody.AddForce(force);

        }    
    }

    bool Tap()
    {
        if (Input.touchCount > 0)
        {
            // タッチ情報の取得
            Touch _touch = Input.GetTouch(index: 0);

            // タッチ開始した場合にタップ判定のための時間計測開始
            if (_touch.phase == TouchPhase.Began)
            {
                //Debug.Log("タッチ開始した"); // OK
                timerStartFlag = true;
            }

            if (timerStartFlag == true)
            {
                elapsedTimeSinceTouchBegan += Time.deltaTime;
                Debug.Log("計測中： " + (elapsedTimeSinceTouchBegan).ToString()); // OK
            }

            // 相手エリアをタッチ終了した場合
            if (_touch.phase == TouchPhase.Ended)
            {
                //Debug.Log("タッチ終了"); // OK

                // タップ判定のための時間計測終了
                timerStartFlag = false;

                // タップ判定のための時間計測がタップした時間計測内の時間である場合
                // バグあり：条件文に入らない
                // なぜ？：
                /* ・elapsedTimeSinceTouchBeganが0.1fより小さくならないから。
                 * → しかし、なぜ0.1fより小さい場合があるのに入らないのか？：
                 * → 0.1fより小さい場合にタッチフェーズがEndedにならないから？
                 * 結論：Endedになった時のelapsedTimeSinceTouchBeganが0.1fより大きかったから
                 */
                if (elapsedTimeSinceTouchBegan < 0.2f)
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
        }

        // タップではないと判定
        return false;
    }
}
