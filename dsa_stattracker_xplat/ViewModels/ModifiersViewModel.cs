namespace dsa_battle_tracker.ViewModels;

using System;
using System.Collections.Generic;
using dsa_battle_tracker.Models;

public partial class ModifiersViewModel : ViewModelBase
{
    #region Melee
    public string Header_Melee { get; } = "Nahkampf";
    public MeleeModifierSet MeleeModifiers { get; private set; } = new();

    public static string Label_HeaderReach => "DK";
    public static string Label_HeaderTarget => "Ziel";
    public static string Label_Environment => "Umgebung";
    public static string Label_OwnReach => "Eigene DK";

    public string PrintedWeaponDK {
        get
        {
            string rtn = "";
            if ((MeleeModifiers.Weapon_DK & MeleeModifierSet.Distance.H) > 0) rtn += "H";
            if ((MeleeModifiers.Weapon_DK & MeleeModifierSet.Distance.N) > 0) rtn += "N";
            if ((MeleeModifiers.Weapon_DK & MeleeModifierSet.Distance.S) > 0) rtn += "S";
            if ((MeleeModifiers.Weapon_DK & MeleeModifierSet.Distance.P) > 0) rtn += "P";
            return rtn;
        }
    }
    private int _lowerOwnDK = 0;
    public int LowerOwnDK
    {
        get => Util.MinBit(MeleeModifiers.Weapon_DK);
        set
        {
            _lowerOwnDK = value;
            RecalcDK();
            UpdateMelee();
        }
    }
    private int _upperOwnDK = 3;
    public int UpperOwnDK
    {
        get => Util.MaxBit(MeleeModifiers.Weapon_DK);
        set
        {
            _upperOwnDK = value;
            RecalcDK();
            UpdateMelee();
        }
    }
    private void RecalcDK()
    {
        int rtn = 0;
        for (int i = _lowerOwnDK; i <= _upperOwnDK; i++)
        {
            rtn |= (1 << i);
        }
        MeleeModifiers.Weapon_DK = (MeleeModifierSet.Distance)rtn;
        this.OnPropertyChanged(nameof(PrintedWeaponDK));
    }

    public static string Label_ActualReach => "Tatsächliche DK";
    public static IEnumerable<MeleeModifierSet.Distance> DK_Options => Enum.GetValues<MeleeModifierSet.Distance>();
    public MeleeModifierSet.Distance ActualDK
    {
        get => MeleeModifiers.Actual_DK;
        set
        {
            MeleeModifiers.Actual_DK = value;
            UpdateMelee();
        }
    }
    
    public static string Label_TargetLocation => "Ziel";
    public static IEnumerable<MeleeModifierSet.Hitlocation?> HitLocations => [null, .. Enum.GetValues<MeleeModifierSet.Hitlocation>()];
    public MeleeModifierSet.Hitlocation? TargetedLocation
    {
        get => MeleeModifiers.TargetLocation;
        set
        {
            MeleeModifiers.TargetLocation = value;
            UpdateMelee();
        }
    }

    public static string Label_DarknessModifier => "Licht";
    public static IEnumerable<MeleeModifierSet.DarknessModifiers?> DarknessModifiers => [null, .. Enum.GetValues<MeleeModifierSet.DarknessModifiers>()];
    public MeleeModifierSet.DarknessModifiers? LightLevel
    {
        get => MeleeModifiers.DarknessModifier;
        set
        {
            MeleeModifiers.DarknessModifier = value;
            UpdateMelee();
        }
    }

    public static string Label_CloseQuartersModifier => "Beengte Umgebung";
    public static IEnumerable<MeleeModifierSet.CloseQuartersModifiers?> CQC_Modifiers => [null, .. Enum.GetValues<MeleeModifierSet.CloseQuartersModifiers>()];
    public MeleeModifierSet.CloseQuartersModifiers? CQC
    {
        get => MeleeModifiers.CloseQuartersModifier;
        set
        {
            MeleeModifiers.CloseQuartersModifier = value;
            UpdateMelee();
        }
    }

    public static IEnumerable<MeleeModifierSet.PositionModifier?> Position_Modifiers => [null, .. Enum.GetValues<MeleeModifierSet.PositionModifier>()];
    public static string Label_TargetPosition => "Position des Gegners";
    public MeleeModifierSet.PositionModifier? TargetPosition
    {
        get => MeleeModifiers.TargetPosition;
        set
        {
            MeleeModifiers.TargetPosition = value;
            UpdateMelee();
        }
    }
    public static string Label_OwnPosition => "Eigene Position";
    public MeleeModifierSet.PositionModifier? OwnPosition
    {
        get => MeleeModifiers.OwnPosition;
        set
        {
            MeleeModifiers.OwnPosition = value;
            UpdateMelee(); 
        }
    }

    public static string Label_OffhandAbility => "Linkhand SF";
    public static IEnumerable<MeleeModifierSet.OffhandAbilities?> OffhandAbilities => [null, .. Enum.GetValues<MeleeModifierSet.OffhandAbilities>()];
    public MeleeModifierSet.OffhandAbilities? OffhandAbility
    {
        get => MeleeModifiers.OffhandAbility;
        set
        {
            MeleeModifiers.OffhandAbility = value;
            UpdateMelee();
        }
    }
    public static string Label_IsOffhand => "Falsche Hand?";
    public bool IsOffhand
    {
        get => MeleeModifiers.IsOffhand;
        set
        {
            MeleeModifiers.IsOffhand = value;
            UpdateMelee();
        }
    }

    public static string Label_TargetSurpriseLevel => "Ziel überrascht?";
    public static IEnumerable<MeleeModifierSet.TargetSurprise?> SurpriseLevels => [null, .. Enum.GetValues<MeleeModifierSet.TargetSurprise>()];
    public MeleeModifierSet.TargetSurprise? TargetSurprise
    {
        get => MeleeModifiers.TargetSurpriseLevel;
        set
        {
            MeleeModifiers.TargetSurpriseLevel = value;
            UpdateMelee();
        }
    }

    public static string Label_OutnumberedLevel => "Überzahl";
    public static IEnumerable<MeleeModifierSet.Outnumbered?> OutnumberedLevels => [null, .. Enum.GetValues<MeleeModifierSet.Outnumbered>()];
    public MeleeModifierSet.Outnumbered? Outnumbered
    {
        get => MeleeModifiers.OutnumberedLevel;
        set
        {
            MeleeModifiers.OutnumberedLevel = value;
            UpdateMelee();
        }
    }

    public void UpdateMelee()
    {
        this.OnPropertyChanged(nameof(Sum_AT_Mod));
        this.OnPropertyChanged(nameof(Sum_PA_Mod));
    }

    public void Reset_Melee()
    {
        this.MeleeModifiers = new();
        //FIXME: only results are updated
        OnPropertyChanged(string.Empty);
    }
    public string Sum_AT_Mod
    {
        get
        {
            var result = MeleeModifiers.AttackMod();
            if (!result.Item1)
                return "AT: Unmöglich";
            return "AT: " + (result.Item2 > 0 ? "+" : "") + result.Item2.ToString();
        }
    }
    public string Sum_PA_Mod
    {
        get
        {
            var result = MeleeModifiers.DefenseMod();
            if (!result.Item1)
                return "PA: Unmöglich";
            return "PA: " + (result.Item2 > 0 ? "+" : "") + result.Item2.ToString();
        }
    }
    #endregion

    #region Ranged
    public static string Header_Ranged => "Fernkampf";
    public RangedModifierSet RangedModifiers { get; } = new();

    //Shooter
    public static string Label_Shooter => "Schütze";
    public static string Label_HasDistanceSense => "Entfernungssinn";
    public bool HasDistanceSense
    {
        get => RangedModifiers.HasDistanceSense;
        set
        {
            RangedModifiers.HasDistanceSense = value;
            UpdateFK();
        }
    }
    public static string Label_WeaponType => "Waffentyp";
    public static IEnumerable<RangedModifierSet.RangedWeaponTypes> AllWeaponTypes => Enum.GetValues<RangedModifierSet.RangedWeaponTypes>();
    public RangedModifierSet.RangedWeaponTypes WeaponType
    {
        get => RangedModifiers.WeaponType;
        set
        {
            RangedModifiers.WeaponType = value;
            UpdateFK();
        }
    }
    public static string Label_RangedAbility => "Sonderfertigkeit";
    public static IEnumerable<RangedModifierSet.SharpshooterAbilities?> AllRangedAbilities => [null, .. Enum.GetValues<RangedModifierSet.SharpshooterAbilities>()];
    public RangedModifierSet.SharpshooterAbilities? RangedAbility
    {
        get => RangedModifiers.SharpshooterAbility;
        set
        {
            RangedModifiers.SharpshooterAbility = value;
            UpdateFK();
        }
    }

    //Target Size
    public static string Label_Target => "Das Ziel";
    public static string Label_TargetSize => "Zielgröße";
    public static IEnumerable<RangedModifierSet.SizeModifiers> AllRangedTargetSizes => Enum.GetValues<RangedModifierSet.SizeModifiers>();
    public RangedModifierSet.SizeModifiers RangedTargetSize
    {
        get => RangedModifiers.TargetSize;
        set
        {
            RangedModifiers.TargetSize = value;
            UpdateFK();
        }
    }
    public static string Label_TargetCover => "Deckung";
    public static IEnumerable<RangedModifierSet.CoverModifiers?> AllCoverModifiers => [null, .. Enum.GetValues<RangedModifierSet.CoverModifiers>()];
    public RangedModifierSet.CoverModifiers? RangedTargetCover
    {
        get => RangedModifiers.CoverModifier;
        set
        {
            RangedModifiers.CoverModifier = value;
            UpdateFK();
        }
    }

    //Distance
    public static string Label_Distance => "Distanz";
    public static IEnumerable<RangedModifierSet.DistanceModifiers> AllDistanceModifiers => Enum.GetValues<RangedModifierSet.DistanceModifiers>();
    public RangedModifierSet.DistanceModifiers Distance
    {
        get => RangedModifiers.Distance;
        set
        {
            RangedModifiers.Distance = value;
            UpdateFK();
        }
    }

    //Movement
    public static string Label_Movement => "Bewegung";
    public static IEnumerable<RangedModifierSet.MovementModifiers> AllMovementModifiers => Enum.GetValues<RangedModifierSet.MovementModifiers>();
    public RangedModifierSet.MovementModifiers RangedTargetMovement
    {
        get => RangedModifiers.TargetMovement;
        set
        {
            RangedModifiers.TargetMovement = value;
            UpdateFK();
        }
    }
    public static string Label_CombatantsInH => "Kämpfer in H";
    public int CombatantsInH
    {
        get => RangedModifiers.CombatantsInH;
        set
        {
            RangedModifiers.CombatantsInH = value;
            UpdateFK();
        }
    }
    public static string Label_CombatantsInNS => "Kämpfer in NS";
    public int CombatantsInNS
    {
        get => RangedModifiers.CombatantsInNS;
        set
        {
            RangedModifiers.CombatantsInNS = value;
            UpdateFK();
        }
    }

    //Sight
    public static string Label_Environment => "Umgebung";
    public static IEnumerable<RangedModifierSet.DarknessModifiers?> AllRangedDarknessModifiers => [null, .. Enum.GetValues<RangedModifierSet.DarknessModifiers>()];
    public RangedModifierSet.DarknessModifiers? RangedDarkness
    {
        get => RangedModifiers.Darkness;
        set
        {
            RangedModifiers.Darkness = value;
            UpdateFK();
        }
    }
    public static IEnumerable<RangedModifierSet.NightVisionBoons?> AllNightVisionBoons => [null, .. Enum.GetValues<RangedModifierSet.NightVisionBoons>()];
    public RangedModifierSet.NightVisionBoons? NightVisionBoon
    {
        get => RangedModifiers.NightVision;
        set
        {
            RangedModifiers.NightVision = value;
            UpdateFK();
        }
    }
    public static string Label_Fog => "Dunst";
    public static IEnumerable<RangedModifierSet.FogModifiers?> AllFogModifiers => [null, .. Enum.GetValues<RangedModifierSet.FogModifiers>()];
    public RangedModifierSet.FogModifiers? FogModifier
    {
        get => RangedModifiers.Fog;
        set
        {
            RangedModifiers.Fog = value;
            UpdateFK();
        }
    }
    public static string Label_IsInvisible => "Ziel unsichtbar";
    public bool IsRangedTargetInvisible
    {
        get => RangedModifiers.IsTargetInvisible;
        set
        {
            RangedModifiers.IsTargetInvisible = value;
            UpdateFK();
        }
    }

    public static string Label_Steepness => "Steilschuss";
    public static IEnumerable<RangedModifierSet.SteepnessModifiers?> AllSteepnessModifiers => [null, .. Enum.GetValues<RangedModifierSet.SteepnessModifiers>()];
    public RangedModifierSet.SteepnessModifiers? Steepness
    {
        get => RangedModifiers.Steepness;
        set
        {
            RangedModifiers.Steepness = value;
            UpdateFK();
        }
    }

    public static string Label_Wind => "Wind";
    public static IEnumerable<RangedModifierSet.WindModifiers?> AllWindModifiers => [null, .. Enum.GetValues<RangedModifierSet.WindModifiers>()];
    public RangedModifierSet.WindModifiers? Wind
    {
        get => RangedModifiers.Wind;
        set
        {
            RangedModifiers.Wind = value;
            UpdateFK();
        }
    }

    public static string Label_Aiming => "Zielen (Aktionen)";
    public int AimingTime
    {
        get => RangedModifiers.ActionsAiming;
        set
        {
            RangedModifiers.ActionsAiming = value;
            this.OnPropertyChanged(nameof(IsQuickshot));
            UpdateFK();
        }
    }
    public static string Label_Quickshot => "Schnellschuss";
    public bool IsQuickshot
    {
        get => RangedModifiers.IsQuickshot;
        set
        {
            RangedModifiers.IsQuickshot = value;
            this.OnPropertyChanged(nameof(AimingTime));
            UpdateFK();
        }
    }

    public static string Label_Other => "Sonstige";
    public static string Label_IsMounted => "Beritten";
    public bool IsMounted
    {
        get => RangedModifiers.IsMounted;
        set
        {
            RangedModifiers.IsMounted = value;
            this.OnPropertyChanged(nameof(AimingTime));
            UpdateFK();
        }
    }
    public static IEnumerable<RangedModifierSet.MountMovements> AllMountMovements => Enum.GetValues<RangedModifierSet.MountMovements>();
    public RangedModifierSet.MountMovements MountMovement
    {
        get => RangedModifiers.MountMovement;
        set
        {
            RangedModifiers.MountMovement = value;
            UpdateFK();
        }
    }

    public static string Label_IsSecondShot => "Zweiter Schuss/Wurf?";
    public bool IsSecondShot
    {
        get => RangedModifiers.IsSecondShot;
        set
        {
            RangedModifiers.IsSecondShot = value;
            UpdateFK();
        }
    }
    public static string Label_IsInWater => "Im Wasser?";
    public bool IsInWater
    {
        get => RangedModifiers.IsInWater;
        set
        {
            RangedModifiers.IsInWater = value;
            UpdateFK();
        }
    }

    public void UpdateFK()
    {
        this.OnPropertyChanged(nameof(Sum_FK_Mod));
    }
    public string Sum_FK_Mod
    {
        get
        {
            var result = RangedModifiers.AttackMod();
            if (!result.Item1)
                return "FK: Unmöglich";
            return "FK: " + (result.Item2 > 0 ? "+" : "") + result.Item2.ToString();
        }
    }
    #endregion

    #region Spells
    public string Header_Magic { get; } = "Zauber";
    #endregion

    #region Miracles
    public string Header_Carmic { get; } = "Liturgien";
    public CarmicModifierSet CarmicModifiers { get; } = new();

    public static string Label_Liturgy => "Die Liturgie";
    public static string Label_Grade => "Grad";
    public int CarmicGrade
    {
        get => CarmicModifiers.Grade;
        set
        {
            CarmicModifiers.Grade = value;
            UpdateCarmic();
        }
    }
    public static string Label_Upgrades => "Aufstufungen";
    public int CarmicUpgrades
    {
        get => CarmicModifiers.Upgrades;
        set
        {
            CarmicModifiers.Upgrades = value;
            UpdateCarmic();
        }
    }

    public static string Label_CarmicCaster => "Der Geweihte";
    public static string Label_Desperation => "Aus Notlage";
    public bool InDesperation
    {
        get => CarmicModifiers.Desperation;
        set
        {
            CarmicModifiers.Desperation = value;
            UpdateCarmic();
        }
    }
    public static string Label_DivineOrders => "Kirchlicher/Göttlicher Auftrag";
    public static IEnumerable<CarmicModifierSet.DivineOrders?> AllDivineOrders => [null, .. Enum.GetValues<CarmicModifierSet.DivineOrders>()];
    public CarmicModifierSet.DivineOrders? DivineOrders
    {
        get => CarmicModifiers.OnChurchOrders;
        set
        {
            CarmicModifiers.OnChurchOrders = value;
            UpdateCarmic();
        }
    }
    public static string Label_FromSelfishness => "Aus Eigensucht";
    public int FromSelfishness
    {
        get => CarmicModifiers.FromSelfishness;
        set
        {
            CarmicModifiers.FromSelfishness = value;
            UpdateCarmic();
        }
    }
    public static string Label_MagicCompulsion => "Unter magischem Einfluss";
    public bool UnderMagicCompulsion
    {
        get => CarmicModifiers.UnderMagicCompulsion;
        set
        {
            CarmicModifiers.UnderMagicCompulsion = value;
            UpdateCarmic();
        }
    }
    public static string Label_IsCasterOathbreaker => "Eidbrecher";
    public static IEnumerable<CarmicModifierSet.OathbreakerModifiers?> AllOathbreakerModifiers => [null, .. Enum.GetValues<CarmicModifierSet.OathbreakerModifiers>()];
    public CarmicModifierSet.OathbreakerModifiers? Oathbreaker
    {
        get => CarmicModifiers.IsOathbreaker;
        set
        {
            CarmicModifiers.IsOathbreaker = value;
            UpdateCarmic();
        }
    }

    public static string Label_Area => "Das Gebiet";
    public static string Label_ConsecratedGround => "Geweihter Boden";
    public static IEnumerable<CarmicModifierSet.ConsecratedGroundModifiers?> AllConsecratedGroundModifiers => [null, .. Enum.GetValues<CarmicModifierSet.ConsecratedGroundModifiers>()];
    public CarmicModifierSet.ConsecratedGroundModifiers? ConsecratedGround
    {
        get => CarmicModifiers.ConsecratedGround;
        set
        {
            CarmicModifiers.ConsecratedGround = value;
            UpdateCarmic();
        }
    }
    public static string Label_FullOfInfidels => "Gebiet voller Ungläubiger";
    public bool FullOfInfidels
    {
        get => CarmicModifiers.FullOfInfidels;
        set
        {
            CarmicModifiers.FullOfInfidels = value;
            UpdateCarmic();
        }
    }
    public static string Label_IsOtherworldly => "Jenseitiger Ort";
    public bool IsOtherworldly
    {
        get => CarmicModifiers.Otherworldly;
        set
        {
            CarmicModifiers.Otherworldly = value;
            UpdateCarmic();
        }
    }

    public static string Label_Time => "Die Zeit";
    public static string Label_GodsMonth => "Monat des Gottes";
    public bool GodsMonth
    {
        get => CarmicModifiers.GodsMonth;
        set
        {
            CarmicModifiers.GodsMonth = value;
            UpdateCarmic();
        }
    }
    public static string Label_GodsHoliday => "Göttlicher Feiertag";
    public bool GodsHoliday
    {
        get => CarmicModifiers.GodsHoliday;
        set
        {
            CarmicModifiers.GodsHoliday = value;
            UpdateCarmic();
        }
    }
    public static string Label_NamelessDays => "Namenlose Tage";
    public bool NamelessDays
    {
        get => CarmicModifiers.NamelessDays;
        set
        {
            CarmicModifiers.NamelessDays = value;
            UpdateCarmic();
        }
    }

    public static string Label_IsOrdering => "Greift ordnend/chaotisch ein";
    public int OrderScale
    {
        get => CarmicModifiers.OrderScale;
        set
        {
            CarmicModifiers.OrderScale = value;
            UpdateCarmic();
        }
    }

    public static string Label_IsTargetOathbreaker => "Ziel ist Eidbrecher";
    public bool TargetIsOathbreaker
    {
        get => CarmicModifiers.TargetIsOathbreaker;
        set
        {
            CarmicModifiers.TargetIsOathbreaker = value;
            UpdateCarmic();
        }
    }
    public static string Label_TargetIsHeretic => "Ziel ist Frevler";
    public bool TargetIsHeretic
    {
        get => CarmicModifiers.TargetIsHeretic;
        set
        {
            CarmicModifiers.TargetIsHeretic = value;
            UpdateCarmic();
        }
    }
    public static string Label_TargetHasPact => "Ziel ist Paktierer";
    public static IEnumerable<CarmicModifierSet.Pact?> AllPacts => [null, .. Enum.GetValues<CarmicModifierSet.Pact>()];
    public CarmicModifierSet.Pact? TargetPact
    {
        get => CarmicModifiers.TargetPact;
        set
        {
            CarmicModifiers.TargetPact = value;
            UpdateCarmic();
        }
    }
    public static string Label_TargetAnathema => "Ziel ist Anathema";
    public bool TargetHasAnathema
    {
        get => CarmicModifiers.TargetHasAnathema;
        set
        {
            CarmicModifiers.TargetHasAnathema = value;
            UpdateCarmic();
        }
    }

    public static string Label_Misc => "Sonstige";
    public static string Label_Assistants => "Vorbeter";
    public int Assistants
    {
        get => CarmicModifiers.Assistants;
        set
        {
            CarmicModifiers.Assistants = value;
            UpdateCarmic();
        }
    }

    public void UpdateCarmic()
    {
        this.OnPropertyChanged(nameof(Sum_CarmicMod));
        this.OnPropertyChanged(nameof(Sum_CarmicCost));
        this.OnPropertyChanged(nameof(Sum_EffectiveGrade));
    }
    public string Sum_CarmicMod
    {
        get
        {
            var result = CarmicModifiers.CastMod();
            if (!result.Item1)
                return "Wirkung unmöglich";
            return "Probe " + (result.Item2 > 0 ? "+" : "") + result.Item2.ToString();
        }
    }
    public string Sum_CarmicCost => "Kosten: " + CarmicModifiers.EffectiveCost;
    public string Sum_EffectiveGrade => "Grad: " + CarmicModifiers.EffectiveGrade; //TODO: as roman numeral
    #endregion
}
