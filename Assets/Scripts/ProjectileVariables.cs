using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileVariables : MonoBehaviour
{
    [Tooltip("Sets the damage of the projectile")]
    public float damage = 0;

    public AOE AOEVariables;

    public DOT DOTVariables;


    [Tooltip("Set to the desired Damage Type")]
    public DamageType damageType;
}

[System.Serializable]
public class AOE
{
    [Tooltip("Enable if you want the damage of the Projectile to be dealt in an Area")]
    public bool usesAOE = false;

    [Tooltip("Set to the desired AOE radius")]
    public float aoeRadius = 0;


    [Tooltip("Enable if you want the damage of the AOE to Separate from the base damage. When this is enabled, the base damage will be applied to the hit entity, but the AOE will not be applied to only surrounding entities, not the hit one")]
    public bool AOEHasSeparateDamage = false;

    [Tooltip("Enable if you want the damage of the AOE to be a different type")]
    public bool AOEHasSeparateDamageType = false;

    [Tooltip("Set to the desired AOE Damage Type")]
    public DamageType AOEDamageType;

    [Tooltip("Enable if you want the damage of the AOE to be dealt over time. This will take the damage and apply it every DOT Length/Period")]
    public bool AOEUsesDOT = false;
}
[System.Serializable]
public class DOT
{
    [Tooltip("Enable if you want the damage of the Projectile to be dealt over time. This will take the damage and apply it every DOT Length/Period")]
    public bool usesDOT = false;

    [Tooltip("Enable if you want the damage of damage over time to Separate from the base damage. When this is enabled, the base damage will be applied, then the DOT Damage")]
    public bool DOTHasSeparateDamage = false;

    [Tooltip("Enable if you want the damage of the DOT to be a different type")]
    public bool DOTHasSeparateDamageType = false;

    [Tooltip("Set to the desired DOT Damage Type")]
    public DamageType DOTDamageType;


    [Tooltip("Set to the desired Duration of damage over time (in milliseconds)")]
    public float DOTLength = 0;

    [Tooltip("Set to the desired times to apply")]
    public float DOTPeriod = 0;
}

public enum DamageType { Physical, Fire, Water, Air, Earth, Light, Dark };

