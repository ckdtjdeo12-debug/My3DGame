using UnityEngine;
using Unity.Cinemachine;

namespace My3DGame
{
    /// <summary>
    /// 카메라 셋팅 설정
    /// </summary>
    public class CameraSettings : MonoBehaviour
    {
        #region Variables
        public CinemachineCamera freeLookCamera;

        public Transform lookAt;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            UpdateCameraSettings();
        }
        #endregion

        #region Custom Method
        //카메라 속성 값 설정
        private void UpdateCameraSettings()
        {
            //
            freeLookCamera.LookAt = lookAt;


        }
        #endregion
    }
}