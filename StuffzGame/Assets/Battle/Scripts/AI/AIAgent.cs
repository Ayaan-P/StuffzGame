using System.Collections.Generic;

public interface AIAgent
{
     KeyValuePair<AIAction,int> GetNextAction(Pokemon player, Pokemon ai);
}

public enum AIAction
{
    USE_MOVE,
    USE_ITEM,
    SWITCH_OUT
}