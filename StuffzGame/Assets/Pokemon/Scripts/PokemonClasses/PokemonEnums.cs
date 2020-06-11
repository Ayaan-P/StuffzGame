public enum MoveDamageClass
{
    NULL,
    SPECIAL,
    PHYSICAL,
    STATUS
}

public enum PokemonType
{
    NORMAL = 1,
    FIGHTING = 2,
    FLYING = 3,
    POISON = 4,
    GROUND = 5,
    ROCK = 6,
    BUG = 7,
    GHOST = 8,
    STEEL = 9,
    FIRE = 10,
    WATER = 11,
    GRASS = 12,
    ELECTRIC = 13,
    PSYCHIC = 14,
    ICE = 15,
    DRAGON = 16,
    DARK = 17,
    FAIRY = 18,
    NULL = -1
}

public enum StatName
{
    HP = 1,
    ATTACK = 2,
    DEFENSE = 3,
    SPECIAL_ATTACK = 4,
    SPECIAL_DEFENSE = 5,
    SPEED = 6,
    ACCURACY = 7,
    EVASION = 8,
    NULL = -1
}
public enum MoveAilment
{
    UNKNOWN = -1,
    NONE = 0,
    PARALYSIS = 1,
    SLEEP = 2,
    FREEZE = 3,
    BURN = 4,
    POISON = 5,
    CONFUSION = 6,
    INFATUATION = 7,
    TRAP = 8,
    NIGHTMARE = 9,
    TORMENT = 12,
    DISABLE = 13,
    YAWN = 14,
    HEAL_BLOCK = 15,
    NO_TYPE_IMMUNITY = 17,
    LEECH_SEED = 18,
    EMBARGO = 19,
    PERISH_SONG = 20,
    INGRAIN = 21,
    SILENCE = 24
}

public enum MoveCategory
{
    DAMAGE = 0,
    AILMENT = 1,
    NET_GOOD_STATS = 2,
    HEAL = 3,
    DAMAGE_AND_AILMENT = 4,
    SWAGGER = 5,
    DAMAGE_AND_LOWER_STATS = 6,
    DAMAGE_AND_RAISE_STATS = 7,
    DAMAGE_AND_HEAL = 8,
    OHKO = 9,
    WHOLE_FIELD_EFFECT = 10,
    FIELD_EFFECT = 11,
    FORCE_SWITCH = 12,
    UNIQUE = 13,
    NULL = -1
}

public enum MovePriority
{
    POS_8 = 8,
    POS_7 = 7,
    POS_6 = 6,
    POS_5 = 5,
    POS_4 = 4,
    POS_3 = 3,
    POS_2 = 2,
    POS_1 = 1,
    ZERO = 0,
    NEG_1 = -1,
    NEG_2 = -2,
    NEG_3 = -3,
    NEG_4 = -4,
    NEG_5 = -5,
    NEG_6 = -6,
    NEG_7 = -7,
    NEG_8 = -8
}

public enum MoveLearnMethod
{
    LEVEL_UP = 1,
    EGG = 2,
    TUTOR = 3,
    MACHINE = 4,
    STADIUM_SURFING_PIKACHU = 5,
    LIGHT_BALL_EGG = 6,
    COLOSSEUM_PURIFICATION =7,
    XD_SHADOW = 8,
    XD_PURIFICATION = 9,
    FORM_CHANGE = 10,
    NULL = -1
}

public enum PokemonTarget
{
    SPECIFIC_MOVE,
    SELECTED_POKEMON_ME_FIRST,
    ALLY,
    USERS_FIELD,
    USER_OR_ALLY,
    OPPONENTS_FIELD,
    USER,
    RANDOM_OPPONENT,
    ALL_OTHER_POKEMON,
    SELECTED_POKEMON,
    ALL_OPPONENTS,
    ENTIRE_FIELD,
    USER_AND_ALLIES,
    ALL_POKEMON,
    NULL
}

public enum EggGroup
{
    MONSTER = 1,
    WATER_1 = 2,
    WATER_2 = 12,
    WATER_3 = 9,
    BUG = 3,
    FLYING = 4,
    GROUND = 5,
    FAIRY = 6,
    PLANT = 7,
    HUMANOID = 8,
    MINERAL = 10,
    INDETERMINATE = 11,
    DITTO = 13,
    DRAGON = 14,
    NO_EGGS = 15
}

public enum GenderRate
{
    /* Source: https://bulbapedia.bulbagarden.net/wiki/Gender#:~:text=From%20Generation%20III%20to%20Generation,referred%20to%20as%20pgender
     * 
     * From Generation VI onward, the gender threshold is compared to a random number between 1 and 252 (inclusive)
     * This causes Pokémon with a "1:1" gender ratio to actually be distributed according to the ideal ratio.
     * All other Pokémon are more likely to be the more common gender than they would be according to their nominal ratio.
     * 
     * If the gender threshold is not a special value (0, 8, or -1), the random number is compared to the gender threshold.
     * If the random number is greater than or equal to the gender threshold, the Pokémon is male, otherwise it is female.
     * Because the comparison to determine gender is greater than or equal, Pokémon are slightly more likely to be male than they would be according to the ideal ratios.
     */
    GENDERLESS = -1,
    FEMALE = 8,
    M1_F7 = 7,
    M1_F3 = 6,
    M1_F1 = 4,
    M3_F1 = 2,
    M7_F1 = 1,
    MALE = 0
}

public enum Gender
{
    GENDERLESS = 3,
    MALE = 2,
    FEMALE = 1,
    NULL = -1
}

public enum TimeOfDay
{
    MORNING,
    NOON,
    EVENING,
    MIDNIGHT,
    DAY,
    NIGHT,
    NULL
}

public enum EvolutionTrigger
{
    LEVEL_UP = 1,
    TRADE = 2,
    USE_ITEM = 3,
    SHED = 4,
    NULL = -1
}

public enum Nature
{
    HARDY,
    BOLD,
    MODEST,
    CALM,
    TIMID,
    LONELY,
    DOCILE,
    MILD,
    GENTLE,
    HASTY,
    ADAMANT,
    IMPISH,
    BASHFUL,
    CAREFUL,
    RASH,
    JOLLY,
    NAUGHTY,
    LAX,
    QUIRKY,
    NAIVE,
    BRAVE,
    RELAXED,
    QUIET,
    SASSY,
    SERIOUS,
    NULL
}

public enum RelativePhysicalStatDifference  //difference of attack and defence
{
    HITMONLEE = 1,
    HITMONTOP = 0,
    HITMONCHAN = -1,
    NULL = int.MinValue
}