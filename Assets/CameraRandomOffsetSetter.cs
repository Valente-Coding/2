using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraRandomOffsetSetter : MonoBehaviour
{
   [SerializeField] CinemachineVirtualCamera _myCam;
   [SerializeField] List<Vector3> _possibleCamPositions;

    private void OnEnable()
    {
        var transposer = _myCam.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_FollowOffset = _possibleCamPositions[Random.Range(0,_possibleCamPositions.Count)];
    }
}
