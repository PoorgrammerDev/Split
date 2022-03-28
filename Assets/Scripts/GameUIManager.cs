using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VectorGraphics;
using Split.LevelLoading;

public class GameUIManager : MonoBehaviour
{
    public const int MAX_MAX_PLAYERS = 9;

    [Header("UI References")]
    [SerializeField] private SVGImage[] playerIcons;
    [SerializeField] private CanvasGroup menu;

    [Header("References")]
    [SerializeField] private LevelGenerator generator;

    public SVGImage[] PlayerIcons => playerIcons;

    private void Start() {
        if (playerIcons.Length != MAX_MAX_PLAYERS) {
            Debug.LogError("GameUIManager Player Icons length not equal to MaxMaxPlayers");
            return;
        }

        //disable extra player icons
        for (int i = generator.LevelData.maxPlayers; i < playerIcons.Length; ++i) {
            playerIcons[i].gameObject.SetActive(false);
        }

    }

}
