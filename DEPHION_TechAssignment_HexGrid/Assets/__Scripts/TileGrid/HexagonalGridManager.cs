using System.Collections.Generic;
using UnityEngine;
using HexagonalTileGrid.Utilities;
using System.Collections;

namespace HexagonalTileGrid{
    public class HexagonalGridManager : MonoBehaviour
    {
        //Exposed Fields ---------------------------------------------------------------------------------------------------
        [Header("References")]
        [SerializeField]
        private MouseHexSelector _mouseHexSelector;
        [Header("Settings")]
        [SerializeField]
        private string _urlJSONData;
        [Header("Prefabs")]
        [SerializeField]
        private TileController _prfb_HexTile;



        //Internal Fields --------------------------------------------------------------------------------------------------
        List<Vector3Int> cubeDirectionVectors = new List<Vector3Int> {
            new Vector3Int(0, -1, +1), new Vector3Int(-1, 0, +1), new Vector3Int(-1, +1, 0),
            new Vector3Int(0, +1, -1), new Vector3Int(+1, 0, -1), new Vector3Int(+1, -1, 0)
        };

        const float _hexWidth = 0.433f;
        const float _hexHeigth = 0.5f;

        private Color _baseTileColor = default;
        private float _tileScale = 1f;
        private float _tilePadding = 0f;

        private TileController _currentlySelectedTile = default;
        
        #region InitializationMethods =========================================================================================================================================
        async void Start()
        {
            TileGridData tileGridData = await JSONDataReader.GetTileGridDataFromURL(_urlJSONData);
            _tilePadding = tileGridData.TilePadding;
            _tileScale = tileGridData.TileSize;
            ColorUtility.TryParseHtmlString($"#{tileGridData.DefaultTileColor}", out _baseTileColor);
            CreateSpiralGridFromData(tileGridData);
        }
        #endregion
        #region PublicMethods =================================================================================================================================================
        public void CreateSpiralGridFromData(TileGridData tileGridData)
        {
            List<Vector3Int> spiralCoordinates = GenerateSpiralHexGridCoordinatesFromData(tileGridData);
            StartCoroutine(CreateHexesInSequence(spiralCoordinates, tileGridData, 0.25f));

        }

        public void OnHexSelected(TileController hex)
        {
            if(_currentlySelectedTile != null)
            {
                _currentlySelectedTile.OnHexDeselected();
            }
            _currentlySelectedTile = hex;
        }

        #endregion

        #region InternalLogic =========================================================================================================================================
        private void CreateHex(Vector3Int position, TileData data)
        {
            TileController hex = Instantiate(_prfb_HexTile, GetHexWorldCoordinates(position), Quaternion.identity, this.transform);
            Color selectColor;
            ColorUtility.TryParseHtmlString($"#{data.ClickedColor}", out selectColor);
            hex.ExternalInitization(_baseTileColor, selectColor, OnHexSelected, _tileScale);
        }

        private List<Vector3Int> GenerateSpiralHexGridCoordinatesFromData(TileGridData tileGridData)
        {
            int radius = CalculateRadiusOfCubeRings(tileGridData.Tiles.Count);
            List<Vector3Int> spiralCoordinates = GetCubeSpiral(new Vector3Int(0, 0, 0), radius);
            return spiralCoordinates;
        }

        private Vector3 GetHexWorldCoordinates(Vector3Int hex)
        {
            float xCoordinate = (_hexWidth * _tileScale) * ((0.5f * hex.x) + (-0.5f * hex.z)) * ( 1f + _tilePadding);
            float zCoordinate = hex.y * 0.75f * (_hexHeigth * _tileScale) * ( 1f + _tilePadding);
            return new Vector3(xCoordinate, 0f, zCoordinate);
        }

        private int CalculateRadiusOfCubeRings(int numberOfHexes)
        {
            int layerCount = 0;
            while (numberOfHexes>0)
            {
                if(layerCount == 0)
                {
                    numberOfHexes--;
                    layerCount++;
                    continue;
                }
                layerCount++;
                numberOfHexes -= layerCount * 6;
            }
            return layerCount;
        }

        private List<Vector3Int> GetCubeSpiral(Vector3Int centerHex, int radius)
        {
            List<Vector3Int> results = new List<Vector3Int>();
            results.Add(centerHex);
            for (int k = 1; k < radius; k++)
            {
                results.AddRange(GetCubeRing(centerHex, k));
            }
            return results;
        }

        private List<Vector3Int> GetCubeRing(Vector3Int centerHex, int radius)
        {
            List<Vector3Int> results = new List<Vector3Int>();
            Vector3Int hex = AddCubes(centerHex, SetCubeScale(cubeDirectionVectors[4], radius));
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    hex = GetCubeNeighbor(hex, i);
                    results.Add(hex);
                }
            }
            return results;
        }

        private Vector3Int AddCubes(Vector3Int hex, Vector3Int vector)
        {
            return hex + vector;
        }

        private Vector3Int GetCubeNeighbor(Vector3Int cube, int direction)
        {
            return AddCubes(cube, cubeDirectionVectors[direction]);
        }

        private Vector3Int SetCubeScale(Vector3Int hex, int factor)
        {
            return new Vector3Int(Mathf.RoundToInt(hex.x * factor), Mathf.RoundToInt(hex.y * factor), Mathf.RoundToInt(hex.z * factor));
        }
        #endregion
        #region Coroutines ==========================================================================================================
        IEnumerator CreateHexesInSequence(List<Vector3Int> spiralCoordinates, TileGridData data, float timeBetweenSpawning)
        {
            for (int i = 0; i < spiralCoordinates.Count; i++)
            {
                CreateHex(spiralCoordinates[i], data.Tiles[i]) ;
                yield return new WaitForSeconds(timeBetweenSpawning);
            }
            _mouseHexSelector.ActivateSelector();
        }
        #endregion
    }

}
