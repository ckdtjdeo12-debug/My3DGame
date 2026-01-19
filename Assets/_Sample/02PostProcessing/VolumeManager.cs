using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace My3DGame
{
    public class VolumeManager : MonoBehaviour
    {
        #region Variables
        public Volume volume;

        private ColorAdjustments colorAdjustments;

        [ColorUsage(false, true)]
        private Color originHDRColor;
        [ColorUsage (false, true)]
        public Color redHDRColor;

        private Vignette vignette;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
            originHDRColor = colorAdjustments.colorFilter.value;

            //volume.profile.TryGet<Vignette>(out vignette);
            //vignette.intensity.value = 0f;
            //vignette.intensity.value = 0.5f;
        }
        #endregion

        #region Custom Method
        public void ChangeColor()
        {
            colorAdjustments.colorFilter.value = redHDRColor;
        }

        public void OriginColor()
        {
            colorAdjustments.colorFilter.value = originHDRColor;
        }
        #endregion
    }
}