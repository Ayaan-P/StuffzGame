using System.Collections.Generic;
using UnityEngine;

public class PokemonSlotSpriteData : SpriteSlotData<Pokemon>
{
    public override Pokemon CurrentObject { get; }

    private readonly SpriteLoader loader;
    private Sprite pokemonSprite;
    private Sprite itemSprite;
    private List<Sprite> typeSpriteList;
    private Sprite genderSprite;
    private Sprite faintedSprite;
    private Sprite ailmentSprite;
    private List<Sprite> moveDamageClassSpriteList;
    private List<Sprite> moveDamageClassLongSpriteList;
    private List<Sprite> moveTypesSpriteList;
    public Sprite PokemonSprite { get => pokemonSprite; set => pokemonSprite = value; }
    public Sprite ItemSprite { get => itemSprite; }
    public List<Sprite> TypeSpriteList { get => typeSpriteList; }
    public Sprite GenderSprite { get => genderSprite; }
    public Sprite FaintedSprite { get => faintedSprite; }
    public Sprite AilmentSprite { get => ailmentSprite; }
    public List<Sprite> MoveDamageClassSpriteList { get => moveDamageClassSpriteList; }
    public List<Sprite> MoveDamageClassLongSpriteList { get => moveDamageClassLongSpriteList; }
    public List<Sprite> MoveTypesSpriteList { get => moveTypesSpriteList; }

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
        PreLoadAilmentSprite();
        PreLoadMoveDamagClassSprites();
        PreLoadMoveTypeSprites();
    }

    private void PreLoadPokemonSprite()
    {
        this.pokemonSprite = loader.LoadPokemonSprite<Sprite>(CurrentObject.BasePokemon.Id, CurrentObject.IsShiny, CurrentObject.Gender, SpriteType.BOX);
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

    private void PreLoadAilmentSprite()
    {
        this.ailmentSprite = loader.LoadAilmentSprite(CurrentObject.Ailment);
    }

    private void PreLoadGenderSprite()
    {
        this.genderSprite = loader.LoadGenderSprite(CurrentObject.Gender);
    }

    private void PreLoadMoveDamagClassSprites()
    {
        List<Sprite> damageClassSprites = new List<Sprite>();
        List<Sprite> damageClassLongSprites = new List<Sprite>();

        foreach (var move in CurrentObject.LearnedMoves)
        {
            damageClassSprites.Add(loader.LoadMoveDamageClassSprite(move.BaseMove.MoveDamageClass, false));
            damageClassLongSprites.Add(loader.LoadMoveDamageClassSprite(move.BaseMove.MoveDamageClass, true));
        }
        this.moveDamageClassSpriteList = damageClassSprites;
        this.moveDamageClassLongSpriteList = damageClassLongSprites;
    }

    private void PreLoadMoveTypeSprites()
    {
        List<Sprite> moveTypeSprites = new List<Sprite>();
        foreach (var move in CurrentObject.LearnedMoves)
        {
            moveTypeSprites.Add(loader.LoadTypeSprite(move.BaseMove.Type));
        }
        this.moveTypesSpriteList = moveTypeSprites;
    }

    public override bool AreSpritesReady()
    {
        if (pokemonSprite == null ||
           (CurrentObject.HeldItem != null && itemSprite == null) ||
           genderSprite == null ||
           (CurrentObject.IsFainted && faintedSprite == null) ||
           typeSpriteList == null ||
           typeSpriteList.Contains(null) ||
           moveDamageClassSpriteList == null ||
           moveDamageClassSpriteList.Contains(null) ||
           moveDamageClassLongSpriteList == null ||
           moveDamageClassLongSpriteList.Contains(null) ||
           moveTypesSpriteList == null ||
           moveTypesSpriteList.Contains(null) ||
           loader.DoesAilmentHaveSprite(CurrentObject.Ailment) && ailmentSprite == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}