using System;
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
        


        //public int TotalSpeed
        //{
        //    get
        //    {
        //        return Units.Sum((x) => x.Speed);
        //    }
        //}



        //public Army(IEnumerable<Unit> units)
        //{
        //    foreach (Unit unit in units)
        //    {
        //        AddUnit(unit);
        //    }
        //}



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
    }
}
