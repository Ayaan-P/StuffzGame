using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartySlotSpriteData
{
    public Pokemon CurrentPokemon { get; }
  
    private readonly SpriteLoader loader;
    private Sprite pokemonSprite;
    private Sprite itemSprite;
    private List<Sprite> typeSpriteList;
    private Sprite genderSprite;
    private Sprite faintedSprite;
    private List<Sprite> moveSpriteList;

    public Sprite PokemonSprite { get => pokemonSprite; }
    public Sprite ItemSprite { get => itemSprite; }
    public List<Sprite> TypeSpriteList { get => typeSpriteList; }
    public Sprite GenderSprite { get => genderSprite; }
    public Sprite FaintedSprite { get => faintedSprite; }
    public List<Sprite> MoveSpriteList { get => moveSpriteList; }

    public PartySlotSpriteData(Pokemon pokemon)
    {
        this.CurrentPokemon = pokemon;
        this.loader = new SpriteLoader();
    }

    public void PreLoadSprites()
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
        this.pokemonSprite = loader.LoadPokemonSprite(CurrentPokemon.BasePokemon.Id, CurrentPokemon.IsShiny, SpriteType.OVERWORLD_POKEMON);
    }

    private void PreLoadHeldItemSprite()
    {
        if (CurrentPokemon.HeldItem != null)
        {
            this.itemSprite = loader.LoadItemSprite(CurrentPokemon.HeldItem.Name);
        }
        else
        {
            this.itemSprite = null;
        }
    }

    private void PreLoadTypeSprites()
    {
        List<Sprite> typeSprites = new List<Sprite>();
        foreach (var type in CurrentPokemon.BasePokemon.Types)
        {
            typeSprites.Add(loader.LoadTypeSprite(type));
        }
        this.typeSpriteList = typeSprites;
    }

    private void PreLoadFaintedSprite()
    {
        if (CurrentPokemon.IsFainted)
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
        this.genderSprite = loader.LoadGenderSprite(CurrentPokemon.Gender);
    }

    private void PreLoadMoveSprites()
    {
        List<Sprite> moveSprites = new List<Sprite>();
        foreach (var move in CurrentPokemon.LearnedMoves)
        {
            moveSprites.Add(loader.LoadMoveDamageClassSprite(move.BaseMove.MoveDamageClass, false));
        }
        this.moveSpriteList = moveSprites;
    }

    public bool AreSpritesReady()
    {
        if (pokemonSprite == null ||
           (CurrentPokemon.HeldItem != null && itemSprite == null) ||
           genderSprite == null ||
           (CurrentPokemon.IsFainted && faintedSprite == null) ||
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