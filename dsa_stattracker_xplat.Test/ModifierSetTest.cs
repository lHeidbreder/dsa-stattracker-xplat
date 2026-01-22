namespace dsa_battle_tracker.Test;

using dsa_battle_tracker.Models;

[TestClass]
public class Tests
{
    [TestMethod]
    [DataRow(MeleeModifierSet.Distance.H, 0, 0)]
    [DataRow(MeleeModifierSet.Distance.H | MeleeModifierSet.Distance.N, 1, 0)]
    [DataRow(MeleeModifierSet.Distance.H | MeleeModifierSet.Distance.N | MeleeModifierSet.Distance.S | MeleeModifierSet.Distance.P, 3, 0)]
    [DataRow(MeleeModifierSet.Distance.S | MeleeModifierSet.Distance.P, 3, 2)]
    [DataRow(MeleeModifierSet.Distance.S, 2, 2)]
    public void Util_MaxBit_Tests(MeleeModifierSet.Distance distance, int max, int min)
    {
        Assert.AreEqual(max, Util.MaxBit(distance));
        Assert.AreEqual(min, Util.MinBit(distance));
    }

    [TestMethod]
    [DataRow(MeleeModifierSet.Distance.N, MeleeModifierSet.Distance.N, 0, DisplayName = "Same")]
    [DataRow(MeleeModifierSet.Distance.S, MeleeModifierSet.Distance.N, 1, DisplayName = "Basic Distance Advantage")]
    [DataRow(MeleeModifierSet.Distance.N, MeleeModifierSet.Distance.S, -1, DisplayName = "Basic Distance Disadvantage")]
    [DataRow(MeleeModifierSet.Distance.H, MeleeModifierSet.Distance.P, -3, DisplayName = "Massive Distance Disadvantage")]
    [DataRow(MeleeModifierSet.Distance.S | MeleeModifierSet.Distance.P, MeleeModifierSet.Distance.S, 0, DisplayName = "Own combined, included")]
    [DataRow(MeleeModifierSet.Distance.S, MeleeModifierSet.Distance.S | MeleeModifierSet.Distance.P, 0, DisplayName = "Target combined, included")]
    [DataRow(MeleeModifierSet.Distance.N | MeleeModifierSet.Distance.S, MeleeModifierSet.Distance.P, -1, DisplayName = "Combined Distance, target is more")]
    [DataRow(MeleeModifierSet.Distance.N | MeleeModifierSet.Distance.S, MeleeModifierSet.Distance.H, 1, DisplayName = "Combined Distance, target is less")]
    public void DK_Distance_Test(MeleeModifierSet.Distance own, MeleeModifierSet.Distance other, int expectedDistance)
    {
        Assert.AreEqual(expectedDistance, MeleeModifierSet.DK_Difference(own, other));
    }
}
