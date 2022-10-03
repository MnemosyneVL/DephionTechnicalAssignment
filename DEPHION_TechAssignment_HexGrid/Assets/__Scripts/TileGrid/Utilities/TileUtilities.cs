using System.Collections;
using UnityEngine;

namespace HexagonalTileGrid{
    public static class TileUtilities
    {
        #region Coroutines ======================================================================================================
        public static IEnumerator LerpHexVertically(Transform TargetTransform, float yStart, float yFinish, float transitionTime)
        {
            float startTime = Time.time;
            float workTime = 0f;
            float finalPosition = 0f;

            while (true)
            {
                workTime = Time.time - startTime;
                finalPosition = workTime / transitionTime;
                float currentValue = Mathf.Lerp(yStart, yFinish, finalPosition);

                TargetTransform.position = new Vector3(TargetTransform.position.x, currentValue, TargetTransform.position.z);


                if (finalPosition >= 1)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }

        }
        public static IEnumerator LerpHexColor(MeshRenderer _meshRenderer, Color start, Color finish, float transitionTime)
        {
            float startTime = Time.time;
            float workTime = 0f;
            float finalPosition = 0f;

            while (true)
            {
                workTime = Time.time - startTime;
                finalPosition = workTime / transitionTime;
                Color currentValue = Color.Lerp(start, finish, finalPosition);

                _meshRenderer.materials[1].color = currentValue;


                if (finalPosition >= 1)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
        }
        #endregion
    }
}
