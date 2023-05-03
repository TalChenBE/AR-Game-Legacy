using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RayCastPlace : MonoBehaviour
{
    public static RayCastPlace Instance;
    public GameObject spwanPrefab;
    bool isSpawned;
    ARRaycastManager aRRaycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public Camera camera;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(aRRaycastManager.Raycast(spwanPrefab.transform.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            spwanPrefab.transform.position = hitPose.position;
            spwanPrefab.transform.rotation = hitPose.rotation;
        }
    }
}
