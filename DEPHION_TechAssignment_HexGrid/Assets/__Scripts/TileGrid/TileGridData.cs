using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonalTileGrid{

    [Serializable]
    public class TileGridData
    {
        [SerializeField]
        public float TileSize;
        [SerializeField]
        public float TilePadding;
        [SerializeField]
        public string DefaultTileColor;
        [SerializeField]
        public List<TileData> Tiles = new List<TileData>();
    }

    [Serializable]
    public class TileData 
    {
        [SerializeField]
        public int Index = default;
        [SerializeField]
        public string ClickedColor = default;
    }

}
