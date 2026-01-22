namespace dsa_battle_tracker;

using dsa_battle_tracker.Models;

public class Util
{
    public static int MinBit(MeleeModifierSet.Distance value)
    {
        if (value == 0) return 0;
        int bit = 0;
        while (((int)value & (1 << bit)) == 0)
            bit++;
        return bit;
    }

    public static int MaxBit(MeleeModifierSet.Distance value)
    {
        if (value == 0) return 0;
        int bit = 31;
        while (((int)value & (1 << bit)) == 0)
            bit--;
        return bit;
    }
}
