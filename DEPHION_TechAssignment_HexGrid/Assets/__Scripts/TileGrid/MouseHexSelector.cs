using UnityEngine;

namespace HexagonalTileGrid{
    public class MouseHexSelector : MonoBehaviour
    {
        //Exposed Fields ---------------------------------------------------------------------------------------------------

        [Header("References")]
        [SerializeField]
        private Camera _camera;
        //Internal Fields -------------------------------------------------------------------------------------------------
        private bool _selectorActive = false;
        #region UpdateMethods =========================================================================================================================================
        private void Update()
        {
            
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 100f;
            mousePos = _camera.ScreenToWorldPoint(mousePos);
            Debug.DrawRay(transform.position, mousePos - transform.position, Color.red);

            if (!_selectorActive) return;

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100f))
                {
                    TileController tileController = hit.transform.GetComponentInParent<TileController>();
                    if (tileController != null)
                    {
                        tileController.OnHexSelected();
                    }
                }
            }
        }
        #endregion

        #region Public Methods ==================================================================================================
        public void ActivateSelector()
        {
            _selectorActive = true;
        }
        #endregion

    }
}
