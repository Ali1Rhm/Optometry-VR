using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesManager : MonoBehaviour
{
    [SerializeField] private Camera leftEyeCamera;
    [SerializeField] private Camera rightEyeCamera;
    [SerializeField] private LayerMask mainCullingMask;
    [SerializeField] private LayerMask noneCullingMask;

    public void ToggleLeftEye()
    {
        if (leftEyeCamera.cullingMask == mainCullingMask)
            leftEyeCamera.cullingMask = noneCullingMask;
        else
            leftEyeCamera.cullingMask = mainCullingMask;
    }

    public void ToggleRightEye()
    {
        if (rightEyeCamera.cullingMask == mainCullingMask)
            rightEyeCamera.cullingMask = noneCullingMask;
        else
            rightEyeCamera.cullingMask = mainCullingMask;
    }
}
