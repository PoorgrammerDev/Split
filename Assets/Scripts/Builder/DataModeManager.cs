using System.Collections.Generic;
using UnityEngine;
using Split.Tiles;

namespace Split.Builder {
    public class DataModeManager : MonoBehaviour {
        [Header("Options")]
        [SerializeField] private TileType[] dataTypesArr;
        [SerializeField] private Color mainTileColor;

        [Header("References")]
        [SerializeField] private CameraController controller;
        [SerializeField] private BuilderManager builderManager;
        [SerializeField] private BuilderLevelLoader loader;

        [Header("UI References")]
        [SerializeField] private GameObject dataMode;
        [SerializeField] private GameObject normalMenu;

        private Vector2Int selectedDataTile;
        private GameObject dataTileHighlighter;
        private HashSet<TileType> dataTypes;

        private void Awake() {
            dataTypes = new HashSet<TileType>(dataTypesArr);
        }

        public void EnterDataMode() {
            //Ensures that there is a selected tile
            Vector2Int? pos = controller.GetState().GetPosition();
            if (!pos.HasValue) return;

            //Ensures that selected tile holds data
            if (!dataTypes.Contains(loader.LevelData.gridData[pos.Value.x][pos.Value.y])) return;
            
            normalMenu.SetActive(false);
            dataMode.SetActive(true);
            selectedDataTile = pos.Value;

            //Highlight the selected tile
            dataTileHighlighter = builderManager.GetTileHighlighter(mainTileColor);
            dataTileHighlighter.transform.position = loader.GridToWorldPos(selectedDataTile.x, selectedDataTile.y);
        }

        public void ExitDataMode() {
            normalMenu.SetActive(true);
            dataMode.SetActive(false);

            builderManager.RemoveTileHighlighter(ref dataTileHighlighter);
        }
    }
}