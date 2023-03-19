using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbabilityWarGame
{
    public class Army
    {
        public IList<Unit>  Units           { get; }                = new List<Unit>();
        public int          SoldierCount    { get; private set; }
        public int          TankCount       { get; private set; }
        public int          PlaneCount      { get; private set; }
        public int          TotalSpeed      { get; private set; }
        

        public Army()
        {
        }



        public Army(Army existing)
        {
            foreach (Unit existingUnit in existing.Units)
            {
                AddUnit(new Unit(existingUnit));
            }
        }






        internal Unit GetShooter(int initiative)
        {
            int i = 0;
            while(initiative > Units[i].Speed)
            {
                initiative -= Units[i].Speed;
                i++;
            }
            return Units[i];
        }



        public void AddUnit(Unit unit)
        {
            Units.Add(unit);
            TotalSpeed += unit.Speed;
            switch (unit.UnitType)
            {
                case UnitType.Soldier:  SoldierCount++; break;
                case UnitType.Tank:     TankCount++;    break;
                case UnitType.Plane:    PlaneCount++;   break;
            }
        }





        public void RemoveUnit(Unit unit)
        {
            Units.Remove(unit);
            TotalSpeed -= unit.Speed;
            switch (unit.UnitType)
            {
                case UnitType.Soldier:  SoldierCount--; break;
                case UnitType.Tank:     TankCount--;    break;
                case UnitType.Plane:    PlaneCount--;   break;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            bool isFirstLine = true;

            if (SoldierCount > 0)
            {
                sb.AppendLine($"{SoldierCount} Soldier");
                isFirstLine = false;
            }

            foreach (var unitGroup in Units.GroupBy(u => u.UnitType).OrderBy(g => g.Key))
            {
                int count = unitGroup.Count();

                if (count > 0)
                {
                    int speed = unitGroup.Max(unit => unit.Speed);
                    int hp = unitGroup.Max(unit => unit.Hp);
                    int damage = unitGroup.Max(unit => unit.Damage);
                    bool hasAntiTank = unitGroup.Any(unit => unit.HasAntiTank);
                    bool hasAntiAir = unitGroup.Any(unit => unit.HasAntiAir);

                    if (!isFirstLine)
                    {
                        sb.AppendLine();
                    }
                    else
                    {
                        isFirstLine = false;
                    }

                    sb.Append($"{count} {unitGroup.Key.ToString()} {speed}S/{hp}HP/{damage}A{(hasAntiTank ? "/AT" : "")}{(hasAntiAir ? "/AA" : "")}");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
