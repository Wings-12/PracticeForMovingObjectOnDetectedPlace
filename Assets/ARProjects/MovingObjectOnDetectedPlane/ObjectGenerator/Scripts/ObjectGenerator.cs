using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] GameObject gameObjectToInstantiate;

    GameObject spawndMovingObject = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalData.hitPose != default)
        {
            //Debug.Log("GlobalData.hitPose.positionの値は" + GlobalData.hitPose.position);

            if (spawndMovingObject == null)
            {
                spawndMovingObject = Instantiate(gameObjectToInstantiate, 
                                                 GlobalData.hitPose.position + new Vector3(0.0f, 3.0f, 0.0f), 
                                                 GlobalData.hitPose.rotation);

                // ここまでのif文は通っていた
                // 恐らく原因はGlobalData.hitPoseのデータがちゃんと取れていない
                Debug.Log("spawndMovingObjectは" + spawndMovingObject);
            }
        }
    }
}
