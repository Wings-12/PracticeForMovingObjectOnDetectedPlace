using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class TouchARRaycastInput : MonoBehaviour
{
    Vector2 touchPosition;

    ARRaycastManager _arRaycastManager;

    static List<ARRaycastHit> _arRaycastHits = new List<ARRaycastHit>();

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    /// <summary>
    /// 画面タッチした座標を取得
    /// </summary>
    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(index: 0).position;

            return true;
        }

        touchPosition = default;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (TryGetTouchPosition(out this.touchPosition) == false)
        {
            return;
        }

        if (_arRaycastManager.Raycast(this.touchPosition, _arRaycastHits, TrackableType.PlaneWithinPolygon))
        {
            GlobalData.hitPose = _arRaycastHits[0].pose;
        }
    }
}
