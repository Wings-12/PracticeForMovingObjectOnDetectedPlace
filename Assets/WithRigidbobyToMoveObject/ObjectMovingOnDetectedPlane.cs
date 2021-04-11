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

            if (_aRRaycastManager.Raycast(_touchPosition, _aRRaycastHits, TrackableType.PlaneWithinPolygon))
            {
                var _hitPose = _aRRaycastHits[0].pose;

                Vector3 _objectMoveDestination = _hitPose.position + new Vector3(0.0f, 10.0f, 0.0f);

                if (_instantiatedObject == null)
                {
                    _instantiatedObject = Instantiate(_objectToInstantiate, _objectMoveDestination, _hitPose.rotation);
                    _rigidbody = _instantiatedObject.GetComponent<Rigidbody>();
                }
                else
                {
                    //_instantiatedObject.transform.position = _objectMoveDestination;
                    //Vector3 force = new Vector3(_objectMoveDestination.x, 0.0f, _objectMoveDestination.z);
                    //Vector3 force = new Vector3(0.0f, 20.0f, 0.0f);

                    //_rigidbody.AddForce(force);

                    
                }

                if (_instantiatedObject.transform.position.y < _hitPose.position.y)
                {
                    //_instantiatedObject.transform.position = _hitPose.position;
                    Vector3 force = new Vector3(0.0f, 20.0f, 0.0f);

                    _rigidbody.AddForce(force);
                }
            }
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 force = new Vector3(0.0f, 20.0f, 0.0f);

        //    _rigidbody.AddForce(force);
        //}
    }
}
