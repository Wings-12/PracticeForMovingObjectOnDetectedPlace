using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ARでレイキャストしてAR平面上とぶつかったところの情報を保持するグローバル変数を扱うデータクラス
/// </summary>
public static class GlobalData
{
    public static Pose hitPose = default;
}
