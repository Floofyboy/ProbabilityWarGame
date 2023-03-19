using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbabilityWarGame
{
    public class Battle
    {
        private Army _computerArmy;
        private Army _playerArmy;
        private Random _rnd = new Random();



        public Battle(Army computerArmy, Army playerArmy)
        {
            _computerArmy   = computerArmy;
            _playerArmy     = playerArmy;
        }



        public int Fight()
        {
            Unit shooter;
            Unit victim;

            while (_computerArmy.Units.Count > 0 && _playerArmy.Units.Count > 0)
            {
                // pick shooter
                int computerTotalSpeed = _computerArmy.TotalSpeed;
                int playerTotalSpeed = _playerArmy.TotalSpeed;
                int initiative = _rnd.Next(computerTotalSpeed + playerTotalSpeed);
                bool computerShooting;


                if (initiative < computerTotalSpeed)
                {
                    computerShooting = true;
                    shooter = _computerArmy.GetShooter(initiative);
                    victim  = _playerArmy.Units[_rnd.Next(_playerArmy.Units.Count)];
                }
                else
                {
                    computerShooting = false;
                    shooter = _playerArmy.GetShooter(initiative - computerTotalSpeed);
                    victim  = _computerArmy.Units[_rnd.Next(_computerArmy.Units.Count)];
                }

                //Console.WriteLine($"{computerTotalSpeed}, {playerTotalSpeed}, {initiative}, {computerShooting}");


                switch (shooter.UnitType)
                {
                    case UnitType.Soldier:
                        switch (victim.UnitType)
                        {
                            case UnitType.Soldier:
                                victim.Hp -= shooter.Damage;
                                break;
                            case UnitType.Tank:
                                if (computerShooting)
                                {
                                    if (_playerArmy.SoldierCount == 0 || shooter.HasAntiTank == true)
                                    {
                                        victim.Hp -= shooter.Damage;
                                    }
                                }
                                else
                                {
                                    if (_computerArmy.SoldierCount == 0 || shooter.HasAntiTank == true)
                                    {
                                        victim.Hp -= shooter.Damage;
                                    }
                                }
                                break;
                            case UnitType.Plane:
                                if (computerShooting)
                                {
                                    if ((_playerArmy.SoldierCount == 0 && _playerArmy.TankCount == 0) || shooter.HasAntiAir == true)
                                    {
                                        victim.Hp -= shooter.Damage;
                                    }
                                }
                                else
                                {
                                    if ((_computerArmy.SoldierCount == 0 && _computerArmy.TankCount == 0) || shooter.HasAntiAir == true)
                                    {
                                        victim.Hp -= shooter.Damage;
                                    }
                                }
                                break;
                        }
                        break;
                    case UnitType.Tank:
                        switch (victim.UnitType)
                        {
                            case UnitType.Soldier:
                                victim.Hp -= shooter.Damage;
                                break;
                            case UnitType.Tank:
                                victim.Hp -= shooter.Damage;
                                break;
                            case UnitType.Plane:
                                if (computerShooting)
                                {
                                    if (_playerArmy.SoldierCount == 0 && _playerArmy.TankCount == 0)
                                    {
                                        victim.Hp -= shooter.Damage;
                                    }
                                }
                                else
                                {
                                    if (_computerArmy.SoldierCount == 0 && _computerArmy.TankCount == 0)
                                    {
                                        victim.Hp -= shooter.Damage;
                                    }
                                }
                                break;
                        }
                        break;
                    case UnitType.Plane:
                        switch (victim.UnitType)
                        {
                            case UnitType.Soldier:
                                victim.Hp -= shooter.Damage;
                                break;
                            case UnitType.Tank:
                                victim.Hp -= shooter.Damage;
                                break;
                            case UnitType.Plane:
                                victim.Hp -= shooter.Damage;
                                break;
                        }
                        break;
                }

                if (victim.Hp <= 0)
                {
                    if (computerShooting)
                    {
                        _playerArmy.RemoveUnit(victim);
                    }
                    else
                    {
                        _computerArmy.RemoveUnit(victim);
                    }
                }

            }

            //------
            if (_computerArmy.Units.Count > 0)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
    }
}
