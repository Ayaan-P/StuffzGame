using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

public class BattleDialogueLoader
{
    private string directory = "Assets/DB/Dialogues/";
    private string fileName = $"battle_dialogue";
    private string fileExtension = ".json";

    private JObject JsonObject { get; }
    public string Variable { get; private set; }
    private List<JObject> generalDialogues;
    private List<JObject> effectDialogues;

    public BattleDialogueLoader()
    {
        this.JsonObject = new JsonLoader().LoadJSON($"{directory}{fileName}{fileExtension}");
        this.Variable = JsonObject["variable"].Value<string>();
        this.generalDialogues = (JsonObject["general_dialogues"] as JArray).Select(obj => obj as JObject).ToList();
        this.effectDialogues = (JsonObject["effects"] as JArray).Select(obj => obj as JObject).ToList();
    }

    public string GetGeneralDialogue(GeneralDialogue dialogueType, Actor actor)
    {
        JObject actorObject = generalDialogues.Where<JObject>(obj => obj["type"].Value<string>().Equals(actor.ToString(), System.StringComparison.CurrentCultureIgnoreCase)).SingleOrDefault();
        List<JObject> actorDialogues = (actorObject["dialogues"] as JArray).Select(obj => obj as JObject).ToList();
        JObject dialogueObject = actorDialogues.Where(it => it["id"].Value<int>() == (int)dialogueType).SingleOrDefault<JObject>();
        return dialogueObject["text"].Value<string>();
    }

    public string GetEffectDialogue(EffectDialogue dialogueType)
    {
        JObject dialogueObject = effectDialogues.Where(it => it["id"].Value<int>() == (int)dialogueType).SingleOrDefault<JObject>();
        return dialogueObject["text"].Value<string>();
    }

    public enum GeneralDialogue
    {
        ENCOUNTER = 1,
        USE_MOVE = 2,
        USE_ITEM = 3,
        SWITCH_OUT = 4,
        SWITCH_IN = 5,
        FAINT = 6,
        IDLE = 7
    }

    public enum EffectDialogue
    {
        SUPER = 1,
        NOT_VERY = 2,
        NO_EFFECT = 3,
        STAT_LOWER = 4,
        STAT_RAISE = 5,
        STAT_LOWER_SHARPLY = 6,
        STAT_RAISE_SHARPLY = 7,
        BURN = 8,
        SLEEP = 9,
        FREEZE = 10,
        PARALYSIS = 11,
        POISON = 12,
        BADLY_POISON = 13,
        EXP = 14,
        LEVEL_UP = 15
    }

    public enum Actor
    {
        PLAYER,
        WILD,
        TRAINER
    }
}