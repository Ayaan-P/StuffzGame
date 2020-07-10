using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PokemonSlotSpriteData : SpriteSlotData<Pokemon>
{
    public override Pokemon CurrentObject { get; }
  
    private readonly SpriteLoader loader;
    private Sprite pokemonSprite;
    private Sprite itemSprite;
    private List<Sprite> typeSpriteList;
    private Sprite genderSprite;
    private Sprite faintedSprite;
    private List<Sprite> moveSpriteList;

    public Sprite PokemonSprite { get => pokemonSprite; set => pokemonSprite=value; }
    public Sprite ItemSprite { get => itemSprite; }
    public List<Sprite> TypeSpriteList { get => typeSpriteList; }
    public Sprite GenderSprite { get => genderSprite; }
    public Sprite FaintedSprite { get => faintedSprite; }
    public List<Sprite> MoveSpriteList { get => moveSpriteList; }

    public PokemonSlotSpriteData(Pokemon pokemon)
    {
        this.CurrentObject = pokemon;
        this.loader = new SpriteLoader();
    }

    public override void PreLoadSprites()
    {
        PreLoadPokemonSprite();
        PreLoadHeldItemSprite();
        PreLoadTypeSprites();
        PreLoadGenderSprite();
        PreLoadFaintedSprite();
        PreLoadMoveSprites();
    }

    private void PreLoadPokemonSprite()
    {
        this.pokemonSprite = loader.LoadPokemonSprite(CurrentObject.BasePokemon.Id, CurrentObject.IsShiny, CurrentObject.Gender, SpriteType.OVERWORLD);
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
        if (pokemonSprite == null ||
           (CurrentObject.HeldItem != null && itemSprite == null) ||
           genderSprite == null ||
           (CurrentObject.IsFainted && faintedSprite == null) ||
           typeSpriteList ==null ||
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