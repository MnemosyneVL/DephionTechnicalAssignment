using UnityEngine;
using UnityEngine.Events;

namespace HexagonalTileGrid{
    public class TileController : MonoBehaviour
    {
        //Exposed Fields ---------------------------------------------------------------------------------------------------
        [SerializeField]
        private MeshRenderer _meshRenderer;
        [SerializeField]
        private float _selectionTransitionTime = 0.5f;
        [SerializeField]
        private float _selectionHeight = 0.5f;

        //Internal Fields --------------------------------------------------------------------------------------------------
        UnityAction<TileController> _onClickAction;
        Color _onClickColor;
        Color _basicColor;
        float _defaultYPos;
        bool _hexSelected = false;
        #region InitializationMethods =========================================================================================================================================
        public void ExternalInitization(Color basicColor, Color onClickColor, UnityAction<TileController> onClickAction, float scaleFactor)
        {
            transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            _onClickAction = onClickAction;
            _onClickColor = onClickColor;
            _defaultYPos = transform.position.y;
            StartCoroutine(TileUtilities.LerpHexVertically(this.transform, _defaultYPos - (2f* _selectionHeight), _defaultYPos, _selectionTransitionTime));
            _basicColor = basicColor;
            ChangeTileColor(basicColor);
            _hexSelected = false;
        }
        #endregion

        #region Public Methods ==================================================================================================
        public void OnHexSelected()
        {
            if (_hexSelected) return;
            _hexSelected = true;
            StartCoroutine(TileUtilities.LerpHexColor(_meshRenderer, _meshRenderer.materials[1].color, _onClickColor, _selectionTransitionTime));
            StartCoroutine(TileUtilities.LerpHexVertically(this.transform, this.transform.position.y, _defaultYPos + _selectionHeight, _selectionTransitionTime));
            _onClickAction?.Invoke(this);

        }
        public void OnHexDeselected()
        {
            if (!_hexSelected) return;
            _hexSelected = false;
            StartCoroutine(TileUtilities.LerpHexColor(_meshRenderer, _meshRenderer.materials[1].color, _basicColor, _selectionTransitionTime));
            StartCoroutine(TileUtilities.LerpHexVertically(this.transform, this.transform.position.y, _defaultYPos, _selectionTransitionTime));
        }
        #endregion

        #region Internal Methods ================================================================================================
        private void ChangeTileColor(Color color)
        {
            _meshRenderer.materials[1].color = color;
        }
        #endregion

    }
}
