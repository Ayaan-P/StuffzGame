using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StorageSlotSpriteData : SpriteSlotData<Pokemon>
{
    public override Pokemon CurrentObject { get; }

    private readonly SpriteLoader loader;
    private Sprite summarySprite;
    private Sprite pokemonSprite;
    private Sprite itemSprite;
    private List<Sprite> typeSpriteList;
    private Sprite genderSprite;
    private Sprite faintedSprite;
    private List<Sprite> moveSpriteList;
    public Sprite SummarySprite { get => summarySprite; set => summarySprite = value; }
    public Sprite PokemonSprite { get => pokemonSprite; set => pokemonSprite = value; }
    public Sprite ItemSprite { get => itemSprite; }
    public List<Sprite> TypeSpriteList { get => typeSpriteList; }
    public Sprite GenderSprite { get => genderSprite; }
    public Sprite FaintedSprite { get => faintedSprite; }
    public List<Sprite> MoveSpriteList { get => moveSpriteList; }

    public StorageSlotSpriteData(Pokemon pokemon)
    {
        this.CurrentObject = pokemon;
        this.loader = new SpriteLoader();
    }

    public override void PreLoadSprites()
    {
        PreLoadSummarySprite();
        PreLoadPokemonSprite();
        PreLoadHeldItemSprite();
        PreLoadTypeSprites();
        PreLoadGenderSprite();
        PreLoadFaintedSprite();
        PreLoadMoveSprites();
    }

    private void PreLoadSummarySprite()
    {
        this.summarySprite = loader.LoadPokemonSprite(CurrentObject.BasePokemon.Id, CurrentObject.IsShiny, SpriteType.BATTLE_FRONT);
    }
    private void PreLoadPokemonSprite()
    {
        this.pokemonSprite = loader.LoadPokemonSprite(CurrentObject.BasePokemon.Id, CurrentObject.IsShiny, SpriteType.OVERWORLD_POKEMON);
    }

    private void PreLoadHeldItemSprite()
    {
        if (CurrentObject.HeldItem != null)
        {
            if (CurrentObject.HeldItem.IsMachine)
            {
                this.itemSprite = loader.LoadTMSprite(CurrentObject.HeldItem.Name, (CurrentObject.HeldItem as Machine).TMType);
            }
            else
            {
                this.itemSprite = loader.LoadItemSprite(CurrentObject.HeldItem.Name);
            }
        }
        else
        {
            this.itemSprite = null;
        }
    }

    private void PreLoadTypeSprites()
    {
        List<Sprite> typeSprites = new List<Sprite>();
        foreach (var type in CurrentObject.BasePokemon.Types)
        {
            typeSprites.Add(loader.LoadTypeSprite(type));
        }
        this.typeSpriteList = typeSprites;
    }

    private void PreLoadFaintedSprite()
    {
        if (CurrentObject.IsFainted)
        {
            this.faintedSprite = loader.LoadFaintedSprite();
        }
        else
        {
            this.faintedSprite = null;
        }
    }

    private void PreLoadGenderSprite()
    {
        this.genderSprite = loader.LoadGenderSprite(CurrentObject.Gender);
    }

    private void PreLoadMoveSprites()
    {
        List<Sprite> moveSprites = new List<Sprite>();
        foreach (var move in CurrentObject.LearnedMoves)
        {
            moveSprites.Add(loader.LoadMoveDamageClassSprite(move.BaseMove.MoveDamageClass, false));
        }
        this.moveSpriteList = moveSprites;
    }

    public override bool AreSpritesReady()
    {
        if (summarySprite == null ||
            pokemonSprite == null ||
           (CurrentObject.HeldItem != null && itemSprite == null) ||
           genderSprite == null ||
           (CurrentObject.IsFainted && faintedSprite == null) ||
           typeSpriteList == null ||
           typeSpriteList.Contains(null) ||
           moveSpriteList == null ||
           moveSpriteList.Contains(null))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}