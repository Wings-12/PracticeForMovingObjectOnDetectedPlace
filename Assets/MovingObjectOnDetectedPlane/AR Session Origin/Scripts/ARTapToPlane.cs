using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlane : MonoBehaviour
{
    ARRaycastManager _arRaycastManager;

    List<ARRaycastHit> _arRaycastHits = new List<ARRaycastHit>();

    Vector2 touchPosition;

    [SerializeField] GameObject prefabCube;
    GameObject instantiatedCube;

    // Start is called before the first frame update
    void Start()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

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
        if(!TryGetComponent(out this.touchPosition))
        {
            return;
        }

        if(!_arRaycastManager.Raycast(this.touchPosition, _arRaycastHits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = _arRaycastHits[0].pose;

            if (prefabCube == null)
            {
                instantiatedCube = Instantiate(prefabCube, hitPose.position, hitPose.rotation); 
            }
            else
            {
                instantiatedCube.transform.position = hitPose.position;
            }
        }

        if (Input.touchCount > 0)
        {

        }
    }
}
