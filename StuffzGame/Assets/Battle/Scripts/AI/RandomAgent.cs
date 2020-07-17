using System;
using System.Collections.Generic;

public class RandomAgent : AIAgent
{
    private readonly Random random;

    public RandomAgent()
    {
        this.random = new Random();
    }

    public KeyValuePair<AIAction, int> GetNextAction(Pokemon player,Pokemon ai)
    {
        int numAIMoves = ai.LearnedMoves.Count;
        int randDamagingMoveIndex = random.Next(0, numAIMoves);
        return new KeyValuePair<AIAction, int>(AIAction.USE_MOVE, randDamagingMoveIndex);
    }

}
