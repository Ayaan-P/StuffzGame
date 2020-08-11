using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public GameObject playerPokemonContainer;
    public GameObject enemyPokemonContainer;
    public GameObject battleSprite;

    public GameObject playerBattleHUD;
    public GameObject enemyBattleHUD;

    public GameObject playerBattleHUDContainer;
    public GameObject enemyBattleHUDContainer;

    public PartyStatusUI partyStatus;
    public GameObject battleMenu;
    public MoveSelection moveSelection;

    public BattleSystem battleSystem;
    private Player player;
    private EncounterData encounterData;
    // Start is called before the first frame update
    private void Start()
    {
        player = Player.Instance;
        encounterData = EncounterData.Instance;
        if (player == null || encounterData == null)
        {
            Debug.LogError($"Cannot start battle: {typeof(Player)} is null? {player == null} or {typeof(EncounterData)} is null? {encounterData == null}");
        }
        else
        {
            int currentPokemonIndex = 0;
            Pokemon playerPokemon = player.Party.GetPokemonAtIndex(currentPokemonIndex);
            List<Pokemon> enemyParty = encounterData.GetCurrentEncounterData();
            Pokemon enemyPokemon = enemyParty[currentPokemonIndex];
            SetPokemonBattleSprites(playerPokemon, enemyPokemon);
            SetBattleHUDs(playerPokemon, enemyPokemon);
            SetPartyStatus();
            PopulateMoves(currentPokemonIndex);
        }
    }

    private void OnEnable()
    {
        //Subscribe Listeners

    }

    private void OnDisable()
    {
        //Unsubscribe Listeners
    }

    private void SetPartyStatus()
    {
        partyStatus.PopulatePartyStatusIcons(player.Party.PartySize());
    }

    private void PopulateMoves(int partyIndex)
    {
        moveSelection.PopulateMoves(partyIndex);
    }

    private void SetPokemonBattleSprites(Pokemon player, Pokemon enemy)
    {
        GameObject playerPokemonSprite = Instantiate(battleSprite, playerPokemonContainer.transform.position, Quaternion.identity, playerPokemonContainer.transform);
        GameObject enemyPokemonSprite = Instantiate(battleSprite, enemyPokemonContainer.transform.position, Quaternion.identity, enemyPokemonContainer.transform);

        AbstractSpriteSwap playerSpriteSwap = playerPokemonSprite.GetComponent<AbstractSpriteSwap>();
        playerSpriteSwap.Pokemon = player;
        playerSpriteSwap.Sprite = SpriteType.BATTLE_BACK;

        AbstractSpriteSwap enemySpriteSwap = enemyPokemonSprite.GetComponent<AbstractSpriteSwap>();
        enemySpriteSwap.Pokemon = enemy;
        enemySpriteSwap.Sprite = SpriteType.BATTLE_FRONT;
    }

    private void SetBattleHUDs(Pokemon player, Pokemon enemy)
    {
        GameObject playerPokemonHUD = Instantiate(playerBattleHUD, playerBattleHUDContainer.transform, false);
        GameObject enemyPokemonHUD = Instantiate(enemyBattleHUD, enemyBattleHUDContainer.transform, false);

        BattlePokemonHUD playerHUD = playerPokemonHUD.GetComponent<BattlePokemonHUD>();
        BattlePokemonHUD enemyHUD = enemyPokemonHUD.GetComponent<BattlePokemonHUD>();

        playerHUD.Pokemon = player;
        playerHUD.IsEnemyHUD = false;

        enemyHUD.Pokemon = enemy;
        enemyHUD.IsEnemyHUD = true;

        playerHUD.UpdateHUD();
        enemyHUD.UpdateHUD();
    }
}