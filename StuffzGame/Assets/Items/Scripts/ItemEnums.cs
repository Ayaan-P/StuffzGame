public enum ItemAttribute
{
    COUNTABLE = 1,
    CONSUMABLE = 2,
    USABLE_IN_OVERWORLD = 3,
    USABLE_IN_BATTLE = 4,
    HOLDABLE = 5,
    HOLDABLE_PASSIVE = 6,
    HOLDABLE_ACTIVE = 7,
    UNDERGROUND = 8,
    NULL = -1
}

public enum ItemFlingEffect
{
    BADLY_POISON = 1,
    BURN = 2,
    BERRY_EFFECT = 3,
    HERB_EFFECT = 4,
    PARALYZE = 5,
    POISON = 6,
    FLINCH = 7,
    NULL = -1
}

// NOTE: 31 is missing from items-category json. Is not an error.
public enum ItemCategory
{
    STAT_BOOSTS = 1,
    EFFORT_DROP = 2,
    MEDICINE = 3,
    OTHER = 4,
    IN_A_PINCH = 5,
    PICKY_HEALING = 6,
    TYPE_PROTECTION = 7,
    BAKING_ONLY = 8,
    COLLECTIBLES = 9,
    EVOLUTION = 10,
    SPELUNKING = 11,
    HELD_ITEMS = 12,
    CHOICE = 13,
    EFFORT_TRAINING = 14,
    BAD_HELD_ITEMS = 15,
    TRAINING = 16,
    PLATES = 17,
    SPECIES_SPECIFIC = 18,
    TYPE_ENHANCEMENT = 19,
    EVENT_ITEMS = 20,
    GAMEPLAY = 21,
    PLOT_ADVANCEMENT = 22,
    UNUSED = 23,
    LOOT = 24,
    ALL_MAIL = 25,
    VITAMINS = 26,
    HEALING = 27,
    PP_RECOVERY = 28,
    REVIVAL = 29,
    STATUS_CURES = 30,
    MULCH = 32,
    SPECIAL_BALLS = 33,
    STANDARD_BALLS = 34,
    DEX_COMPLETION = 35,
    SCARVES = 36,
    ALL_MACHINES = 37,
    FLUTES = 38,
    APRICORN_BALLS = 39,
    APRICORN_BOX = 40,
    DATA_CARDS = 41,
    JEWELS = 42,
    MIRACLE_SHOOTER = 43,
    MEGA_STONES = 44,
    MEMORIES = 45,
    Z_CRYSTALS = 46,
    NULL = -1
}

public enum ItemPocket
{
    MISC,
    MEDICINE,
    POKEBALLS,
    MACHINES,
    BERRIES,
    MAIL,
    BATTLE,
    KEY
}

public enum BerryFlavor
{
    SPICY = 1,
    DRY = 2,
    SWEET = 3,
    BITTER = 4,
    SOUR = 5,
    NULL = -1
}

public enum BerryFirmness
{
    VERY_SOFT = 1,
    SOFT = 2,
    HARD = 3,
    VERY_HARD = 4,
    SUPER_HARD = 5,
    NULL = -1
}