using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Structs
{
    [Serializable]
    public struct PlayerStat
    {
        public string name;
        public int level;
        public int health;
        public int depense;
        public float attack;
        public float attackSpeed;
        public int criticalChance;
        public float criticalDamageMultiplier;
        public float skillability;
    }

    [Serializable]
    public struct GrowStatTable
    {
        public int health;
        public int depense;
        public float attack;
        public float attackSpeed;
        public float skillability;
    }
}
