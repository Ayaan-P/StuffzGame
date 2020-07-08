using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PartyStatistics : MonoBehaviour
{
    public GameObject typeWedge;
    public GameObject pieChartContainer;
    public GameObject weakTypeSlots;
    public GameObject typeImage;
    private UIManager uiManager;
    private Dictionary<PokemonType, int> typeCoverage;

    private void OnEnable()
    {
        uiManager = UIManager.Instance;
        typeCoverage = new Dictionary<PokemonType, int>();
        GeneratePieChart();
        PopulatePartyTypeWeaknesses();
    }

    private void OnDisable()
    {
        foreach(Transform child in pieChartContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in weakTypeSlots.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void GeneratePieChart()
    {

        foreach (PokemonSlotSpriteData slotData in uiManager.PartySlotDataList)
        {
            Pokemon pokemon = slotData.CurrentObject;
            AddDamagingMoveTypesToCoverageDict(pokemon);
        }

        float total = typeCoverage.Sum(it => it.Value);
        float radialRotation = 0f;
        float radialDegrees = 360f;

        foreach (var entry in typeCoverage)
        {
            GameObject typePieWedge = Instantiate(typeWedge, pieChartContainer.transform);
            Image wedgeImage = typePieWedge.GetComponentInChildren<Image>();
            Text wedgeText = typePieWedge.GetComponentInChildren<Text>();


            wedgeImage.color = UIUtils.GetColorForType(entry.Key);
            wedgeImage.fillAmount = entry.Value / total;

            Color bgColor = UIUtils.GetColorForType(entry.Key);
            wedgeImage.color = bgColor;

            wedgeImage.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, radialRotation));
            radialRotation += wedgeImage.fillAmount * radialDegrees;

            wedgeText.text = UIUtils.FormatText(entry.Key.ToString(), false);
            wedgeText.color = ColorPalette.GetTextContrastedColorFor(bgColor);

            RectTransform rect = (RectTransform)typePieWedge.transform;
            float radius = (wedgeImage.transform.right.x - wedgeImage.transform.position.x) / 4f;
            Debug.Log("radius: " + radius);

            Vector2 textPosition = GetTextPosition(radius, wedgeImage);

            Debug.Log("TextPos: " + wedgeText.transform.position);
            Debug.Log("new TextPos: " + textPosition);

            wedgeText.transform.position = wedgeImage.transform.position + new Vector3(textPosition.x, textPosition.y);
        }
    }

    private Vector2 GetTextPosition(float radius, Image wedgeImage)
    {
        // Topmost point of circle
        var point1 = Vector2.up * radius;

        // Point on cirlce, by angle from image fill, 0-1 is clockwise from empty to full
        var point2 = new Vector2(Mathf.Sin(wedgeImage.fillAmount * Mathf.PI * 2f) * radius, Mathf.Cos(wedgeImage.fillAmount * Mathf.PI * 2f)) ;

        // Centerpoint on circle, between poin1 and point2
        var centerPoint = (point1 + ((point2 - point1) * 0.5f)).normalized * radius;

        // more than half pie, swap point to inverse side
        if (wedgeImage.fillAmount >= 0.5f)
        {
            centerPoint *= -1f;
        }

        return centerPoint;
    }

    private void AddDamagingMoveTypesToCoverageDict(Pokemon pokemon)
    {
        foreach(PokemonMove move in pokemon.LearnedMoves)
        {
            if(move.BaseMove.MoveDamageClass != MoveDamageClass.STATUS)
            {
                if (typeCoverage.ContainsKey(move.BaseMove.Type))
                {
                    typeCoverage[move.BaseMove.Type]++;
                }
                else
                {
                    typeCoverage[move.BaseMove.Type] = 1;
                }
                
            }
        }
    }

    private void PopulatePartyTypeWeaknesses()
    {
        HashSet<PokemonType> partyTypeSet = new HashSet<PokemonType>();
        foreach (PokemonSlotSpriteData slotData in uiManager.PartySlotDataList)
        {
            Pokemon pokemon = slotData.CurrentObject;
            partyTypeSet.UnionWith(pokemon.BasePokemon.Types);
        }
        HashSet<PokemonType> partyWeaknessSet = new HashSet<PokemonType>();
        Modifiers mod = new Modifiers();

        foreach(PokemonType type in partyTypeSet)
        {
           partyWeaknessSet.UnionWith(mod.GetTypesStrongAgainst(type));
        }

        foreach(PokemonType type in typeCoverage.Keys)
        {
            partyWeaknessSet.ExceptWith(mod.GetTypesWeakTo(type));
        }

        SpriteLoader loader = new SpriteLoader();
        foreach (PokemonType type in partyWeaknessSet)
        {
            GameObject typeSlot = Instantiate(typeImage, weakTypeSlots.transform);
            Image typeSprite = typeSlot.GetComponent<Image>();
            typeSprite.sprite = loader.LoadTypeSprite(type);


        }

    }

 

}
