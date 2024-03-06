using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer
{

    // general attributes
    public string Name { get; set; } = "";

    public bool IsWatching { get; set; } = false;

    public bool IsFollowing { get; set; } = false;

    public bool IsSubscribed { get; set; } = false;

    public float StreamInterest { get; set; } = 0f;

    public float StreamSatisfaction { get; set; } = 0f;

    public float DonatedAmount { get; set; } = 0f;

    // gaming actions affinity
    public float AffinityForRPG { get; set; } = 0f;

    public float AffinityForArcade { get; set; } = 0f;

    public float AffinityForAction { get; set; } = 0f;

    public float AffinityForSimulation { get; set; } = 0f;

    public float GameAffinityAverage{
        get { return (AffinityForRPG + AffinityForArcade + AffinityForAction + AffinityForSimulation) / 4; }
    }


// social actions affinity
    public float AffinityForFlirt { get; set; } = 0f;

    public float AffinityForGiggle { get; set; } = 0f;

    public float AffinityForHype { get; set; } = 0f;

    public float AffinityForRage { get; set; } = 0f;

    public float SocialAffinityAverage{
        get { return (AffinityForFlirt + AffinityForGiggle + AffinityForHype + AffinityForRage) / 4; }
    }

}
