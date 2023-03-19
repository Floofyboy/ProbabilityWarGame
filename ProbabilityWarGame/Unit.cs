using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbabilityWarGame
{
    public enum UnitType
    {
         Soldier
        ,Tank
        ,Plane
    }



    public class Unit
    {
        public UnitType UnitType    { get; }
        public int      Speed       { get; }
        public int      Hp          { get; set; }
        public int      Damage      { get; }
        public bool     HasAntiTank { get; }
        public bool     HasAntiAir  { get; }



        public Unit(UnitType unitType, int speed, int hp, int damage, bool hasAntiTank = false, bool hasAntiAir = false)
        {
            UnitType = unitType;
            Speed = speed;
            Hp = hp;
            Damage = damage;
            HasAntiTank = hasAntiTank;
            HasAntiAir = hasAntiAir;
        }



        public Unit(Unit existing)
        {
            UnitType    = existing.UnitType   ;
            Speed       = existing.Speed      ;
            Hp          = existing.Hp         ;
            Damage      = existing.Damage     ;
            HasAntiTank = existing.HasAntiTank;
            HasAntiAir  = existing.HasAntiAir ;
        }
        public override string ToString()
        {
            string unitTypeToString = "";
            switch (UnitType)
            {
                case UnitType.Soldier:
                    unitTypeToString = "S";
                    break;
                case UnitType.Tank:
                    unitTypeToString = "T";
                    break;
                case UnitType.Plane:
                    unitTypeToString = "P";
                    break;
            }

            string unitStats = $"SP{Speed}/HP{Hp}/DMG{Damage}";
            if (HasAntiTank)
            {
                unitStats += "/AT";
            }
            if (HasAntiAir)
            {
                unitStats += "/AA";
            }

            return $"{unitTypeToString}{unitStats}";
        }
    }
}
