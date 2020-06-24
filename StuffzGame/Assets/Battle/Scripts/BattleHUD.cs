using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BattleHUD : MonoBehaviour
{

	public Text nameText;
	public Text levelText;
	public Slider hpSlider;
	public Pokemon pkmn;

	public void SetHUD(Pokemon pkmn)
	{
		this.pkmn=pkmn;
		nameText.text = pkmn.BasePokemon.Name;
		levelText.text = "Lvl " + pkmn.CurrentLevel;
		hpSlider.maxValue = pkmn.BasePokemon.Stats.Where(it => it.BaseStat.Name == StatName.HP).SingleOrDefault().CalculatedValue;
		hpSlider.value = pkmn.BasePokemon.Stats.Where(it => it.BaseStat.Name == StatName.HP).SingleOrDefault().CurrentValue;
	}

	public void SetHP()
	{
		hpSlider.value = pkmn.BasePokemon.Stats.Where(it => it.BaseStat.Name == StatName.HP).SingleOrDefault().CurrentValue;
	}

}
