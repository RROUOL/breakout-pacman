using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeReset : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera vCam;


    public void CameraReset()
    {
        StartCoroutine("CameraRestIE");
    }

    IEnumerator CameraRestIE()
    {

        yield return new WaitForSeconds(1f);
        vCam.enabled = false;
        vCam.transform.SetPositionAndRotation(new Vector3(40.99304f, 22.67594f, 5.166766f), Quaternion.Euler(36.892f, -90.44701f, 0.233f));
        vCam.enabled = true;

    }
}
