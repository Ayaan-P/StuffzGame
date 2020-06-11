using UnityEngine;

public class SpriteSwapBattle : MonoBehaviour
{
    public string POKEMON_NAME;

    private void LateUpdate()
    {
        var sprite = Resources.Load<Sprite>("BattleFront/" + POKEMON_NAME);
        var renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = sprite;
    }
}