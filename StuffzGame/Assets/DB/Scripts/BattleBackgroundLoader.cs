using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BattleBackgroundLoader : MonoBehaviour
{
    private bool isBackgroundSet;
    private Image bg;
    private Sprite backgroundSprite;

    private void Start()
    {
        backgroundSprite = null;
        isBackgroundSet = false;
        bg = this.GetComponent<Image>();
        EncounterData encounterData = EncounterData.Instance;
        if (encounterData != null)
        {
            KeyValuePair<BattleTerrain, TimeOfDay> terrainAndTime = encounterData.TerrainAndTime;
            LoadRandomBackground();
        }
        else
        {
            Debug.LogError($"{typeof(EncounterData)} is null, cant load battle background terrain or time");
        }
    }

    private void Update()
    {
        if (!isBackgroundSet && backgroundSprite != null)
        {
            bg.sprite = backgroundSprite;
            isBackgroundSet = true;
        }
    }

    private void LoadBackgroundAddressable(string address)
    {
        string loadAddress = $"Assets/Battle/Sprites/Backgrounds/{address}.png";
        Debug.Log($"Loading Battle background: {loadAddress}");
        AsyncOperationHandle<Sprite> spriteHandle = Addressables.LoadAssetAsync<Sprite>(loadAddress);
        spriteHandle.Completed += LoadSpritesWhenReady;
    }

    private void LoadSpritesWhenReady(AsyncOperationHandle<Sprite> spriteHandle)
    {
        if (spriteHandle.Status == AsyncOperationStatus.Succeeded)
        {
            backgroundSprite = spriteHandle.Result;
        }
        else
        {
            Debug.LogError("Battle background sprite not found!");
        }
    }

    public void LoadBackground(BattleTerrain terrain, TimeOfDay timeOfDay)
    {
        string address;
        switch (terrain)
        {
            case BattleTerrain.BEACH:
                address = $"beach{GetBackgroundAddressForTime(timeOfDay)}";
                LoadBackgroundAddressable(address);
                break;

            case BattleTerrain.CAVE:
                address = $"cave";
                LoadBackgroundAddressable(address);
                break;

            case BattleTerrain.CAVE_DARK:
                address = $"cave_dark";
                LoadBackgroundAddressable(address);
                break;

            case BattleTerrain.CAVE_LAVA:
                address = $"cave_lava";
                LoadBackgroundAddressable(address);
                break;

            case BattleTerrain.CAVE_SNOW:
                address = $"cave_snow";
                LoadBackgroundAddressable(address);
                break;

            case BattleTerrain.HILL:
                address = $"hill{GetBackgroundAddressForTime(timeOfDay)}";
                LoadBackgroundAddressable(address);
                break;

            case BattleTerrain.LAKE:
                address = $"lake{GetBackgroundAddressForTime(timeOfDay)}";
                LoadBackgroundAddressable(address);
                break;

            case BattleTerrain.MEADOWS:
                address = $"meadows{GetBackgroundAddressForTime(timeOfDay)}";
                LoadBackgroundAddressable(address);
                break;

            case BattleTerrain.MOUNTAIN:
                address = $"mountain{GetBackgroundAddressForTime(timeOfDay)}";
                LoadBackgroundAddressable(address);
                break;

            case BattleTerrain.MOUNTAIN_ROCKY:
                address = $"mountain_rocky{GetBackgroundAddressForTime(timeOfDay)}";
                LoadBackgroundAddressable(address);
                break;

            case BattleTerrain.MOUNTAIN_SNOW:
                address = $"mountain_snow{GetBackgroundAddressForTime(timeOfDay)}";
                LoadBackgroundAddressable(address);
                break;

            case BattleTerrain.MOUNTAIN_VOLCANO:
                address = $"mountain_volcano{GetBackgroundAddressForTime(timeOfDay)}";
                LoadBackgroundAddressable(address);
                break;

            case BattleTerrain.SEA:
                address = $"sea{GetBackgroundAddressForTime(timeOfDay)}";
                LoadBackgroundAddressable(address);
                break;

            default:
                Debug.LogError($"Terrain {terrain} is invalid for backgrounds");
                break;
        }
    }

    public void LoadRandomBackground()
    {
        Array terrainValues = System.Enum.GetValues(typeof(BattleTerrain));
        Array timeValues = System.Enum.GetValues(typeof(TimeOfDay));

        int randomTerrainIndex = new System.Random().Next(0, terrainValues.Length);
        int randomTimeIndex = new System.Random().Next(0, timeValues.Length);

        LoadBackground((BattleTerrain)terrainValues.GetValue(randomTerrainIndex), (TimeOfDay)timeValues.GetValue(randomTimeIndex));
    }

    private string GetBackgroundAddressForTime(TimeOfDay time)
    {
        switch (time)
        {
            case TimeOfDay.NULL:
            case TimeOfDay.MORNING:
            case TimeOfDay.DAY:
            case TimeOfDay.NOON:
                return "";

            case TimeOfDay.EVENING:
                return "_evening";

            case TimeOfDay.NIGHT:
            case TimeOfDay.MIDNIGHT:
                return "_night";

            default:
                Debug.LogError($"Time of day {time} is invalid for backgrounds");
                return "";
        }
    }
}

public enum BattleTerrain
{
    BEACH,
    CAVE,
    CAVE_DARK,
    CAVE_LAVA,
    CAVE_SNOW,
    HILL,
    LAKE,
    MEADOWS,
    MOUNTAIN,
    MOUNTAIN_ROCKY,
    MOUNTAIN_SNOW,
    MOUNTAIN_VOLCANO,
    SEA
}