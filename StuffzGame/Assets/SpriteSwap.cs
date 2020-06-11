using UnityEngine;

public class SpriteSwap : MonoBehaviour
{
    public string POKEMON_NAME;

    private void LateUpdate()
    {
        var sprites = Resources.LoadAll<Sprite>("OverworldPokemon/" + POKEMON_NAME);
        Sprite newSprite;
        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            string spriteName = renderer.sprite.name;
            newSprite = renderer.sprite;
            foreach (var sp in sprites)
            {
                if (sp.name == spriteName)
                {
                    newSprite = sp;
                    break;
                }
            }

            renderer.sprite = newSprite;
        }
    }
}