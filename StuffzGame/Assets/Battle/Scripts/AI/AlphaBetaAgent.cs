using System.Collections.Generic;

public class AlphaBetaAgent : AIAgent
{
    private const int WIN_REWARD = 10;
    private const int LOSS_PENALTY = -10;
    private const int LOOK_AHEAD_DEPTH = 4;

    private float EvaluateState(Pokemon player, Pokemon enemy)
    {
        int playerHP = player.GetStat(StatName.HP).CurrentValue;
        int enemyHP = enemy.GetStat(StatName.HP).CurrentValue;

        if (playerHP <= 0)
        {
            return float.PositiveInfinity;
        }else if(enemyHP <= 0)
        {
            return float.NegativeInfinity;
        }
        else
        {
            float reward = (playerHP > enemyHP ? LOSS_PENALTY : WIN_REWARD);
            return reward;
        }
    }

    private List<int> GetSuccessorStates(int state)
    {
        return new List<int>();
    }

    public KeyValuePair<AIAction, int> GetNextAction(Pokemon player, Pokemon ai)

    {
        throw new System.NotImplementedException();
    }

   /* private KeyValuePair<AIAction,int> Minimax(
                    int depth,
                    int nodeIndex,
                    bool isMaximizer,
                    Dictionary<AIAction, List<int>> gameStates, 
                    int alpha,
                    int beta)
    {

        if (depth == LOOK_AHEAD_DEPTH)
            return values[nodeIndex];

        if (isMaximizer)
        {
            int best = int.MinValue;

            // Recur for left and
            // right children
            for (int i = 0; i < 2; i++)
            {
                int val = Minimax(depth + 1, nodeIndex * 2 + i,
                                false, values, alpha, beta);
                best = Math.Max(best, val);
                alpha = Math.Max(alpha, best);

                // Alpha Beta Pruning
                if (beta <= alpha)
                    break;
            }
            return best;
        }
        else
        {
            int best = MAX;

            // Recur for left and
            // right children
            for (int i = 0; i < 2; i++)
            {
                int val = Minimax(depth + 1, nodeIndex * 2 + i,
                                true, values, alpha, beta);
                best = Math.Min(best, val);
                beta = Math.Min(beta, best);

                // Alpha Beta Pruning
                if (beta <= alpha)
                    break;
            }
            return best;
        }
    }*/
}