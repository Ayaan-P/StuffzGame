public class PokemonStat
{
    public BasePokemonStat BaseStat { get; set; }
    public int BaseValue { get; set; }
    public int CalculatedValue { get; set; }
    public int CurrentValue { get; set; }
    public int EVsGainedOnDefeat { get; set; }
    public int? EV { get; set; }
    public int? IV { get; set; }

    internal void CalculateStat(int increasedStatId, int decreasedStatId, int currentLevel)
    {
        // performing int division for most calculations. int truncation is quicker than converting to float and applying floor().
        float NATURE_PERCENT_STAT_CHANGE = 0.1f;
        int HP_BONUS = 10;
        int OTHER_STAT_BONUS = 5;
        int HUNDRED = 100;
        int STAT_MULTIPLIER = 2;
        int EV_DIVISOR = 4;

        int EVValue = EV ?? 0;

        if (BaseStat.Name == StatName.HP)
        {
            int newCalculatedValue = (((STAT_MULTIPLIER * BaseValue) + (int)IV + (EVValue / EV_DIVISOR)) * currentLevel / HUNDRED) + currentLevel + HP_BONUS;
            int difference = newCalculatedValue - CalculatedValue;
            CalculatedValue += difference;
            CurrentValue += difference;
            if (CurrentValue > CalculatedValue)
            {
                CurrentValue = CalculatedValue;
            }
        }
        else
        {
            float NATURE_MULTIPLIER;
            if (increasedStatId == BaseStat.Id)
            {
                NATURE_MULTIPLIER = 1f + NATURE_PERCENT_STAT_CHANGE;
            }
            else if (decreasedStatId == BaseStat.Id)
            {
                NATURE_MULTIPLIER = 1f - NATURE_PERCENT_STAT_CHANGE;
            }
            else
            {
                NATURE_MULTIPLIER = 1f;
            }

            int newCalculatedValue = (int)(((((STAT_MULTIPLIER * BaseValue) + (int)IV + (EVValue / EV_DIVISOR)) * currentLevel / HUNDRED) + OTHER_STAT_BONUS) * NATURE_MULTIPLIER);
            int difference = newCalculatedValue - CalculatedValue;
            CalculatedValue += difference;
            CurrentValue += difference;
            if (CurrentValue > CalculatedValue)
            {
                CurrentValue = CalculatedValue;
            }
        }
    }

   
}