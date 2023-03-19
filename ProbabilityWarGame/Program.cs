using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace ProbabilityWarGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // www.reddit.com/r/gameideas/comments/119cvmg/probability_guessing_war_game/
            //
            // Each unit has HP, attack, and speed. Speed is the most important 
            // stat. Every turns, a roll of dice determines which soldier gets to 
            // attack, based on all units's speed. For example, say i have 3 units 
            // with 20 speed, and you have 2 units with 30 speed, then each my 
            // units has 20% chance to be picked, and yours have 30% each.
            // 
            // Once a unit is choosen to attack, it picks a target randomly, and 
            // shoots that target. This is repeated until one side is eliminated. 
            // In other words, if you have the faster units, you might get to shoot 
            // several times in a row.
            // 
            // Also, here is the special rule for the unit types: Soldiers can only 
            // damage a tank if the other side has no soldiers left. The exception 
            // is if your soldier has an anti-tank weapon. Soldiers can only damage 
            // a plane if the other side has no soldiers and tanks left. The 
            // exception is if your soldier has an anti-air weapon. Tanks can only 
            // damage a plane if the other side has no soldiers and tanks left.
            // 
            // When determining the win probabilities, the game can simply simulate 
            // the fight a million times and find out the win % this way.
            // 
            // Here is a theorical example of what a round could look like:
            // 
            // You have 10 soldiers with 5 speed, 1 HP, 1 damage.
            // 
            // Computer has 10 soldiers with 5 speed, 1 HP, 1 damage, but also 5 
            // tanks with 5 speed, 3 HP, 1 damage.
            // 
            // You then have 4 choices to defeat his army.
            // 
            // A) 3 anti-tank soldiers with 7 speed, 2 HP, 3 damage
            // 
            // B) A single plane with 15 speed, 5 hp, 3 damage
            // 
            // C) 15 soldiers with 3 speed, 1 hp, 1 damage
            // 
            // D) One heroic soldier with 75 speed, 1 hp, 1 damage.
            // 
            // I honnestly have no idea which of these choices would be the correct 
            // one, but i think it illustrate what the game would be about.



            Army computerArmy = new Army();
            Army playerArmy;

            for (int i = 0; i < 10; i++)
            {
                computerArmy.AddUnit(new Unit(UnitType.Soldier, 5, 1, 1));
            }

            for (int i = 0; i < 5; i++)
            {
                computerArmy.AddUnit(new Unit(UnitType.Tank, 5, 3, 1));
            }

            // player variant A
            playerArmy = new Army();
            for (int i = 0; i < 10; i++)
            {
                playerArmy.AddUnit(new Unit(UnitType.Soldier, 5, 1, 1));
            }
            for (int i = 0; i < 3; i++)
            {
                playerArmy.AddUnit(new Unit(UnitType.Soldier, 7, 2, 3, hasAntiTank:true));
            }
            DoBattles(computerArmy, playerArmy);


            // player variant B
            playerArmy = new Army();
            for (int i = 0; i < 10; i++)
            {
                playerArmy.AddUnit(new Unit(UnitType.Soldier, 5, 1, 1));
            }
            playerArmy.AddUnit(new Unit(UnitType.Plane, 15, 5, 3));
            DoBattles(computerArmy, playerArmy);


            // player variant C
            playerArmy = new Army();
            for (int i = 0; i < 10; i++)
            {
                playerArmy.AddUnit(new Unit(UnitType.Soldier, 5, 1, 1));
            }
            for (int i = 0; i < 15; i++)
            {
                playerArmy.AddUnit(new Unit(UnitType.Soldier, 3, 2, 3));
            }
            DoBattles(computerArmy, playerArmy);


            // player variant D
            playerArmy = new Army();
            for (int i = 0; i < 10; i++)
            {
                playerArmy.AddUnit(new Unit(UnitType.Soldier, 5, 1, 1));
            }
            playerArmy.AddUnit(new Unit(UnitType.Soldier, 75, 1, 1));
            DoBattles(computerArmy, playerArmy);

        }



        private static void DoBattles(Army computerArmy, Army playerArmy)
        {
            int computerWins = 0;
            int playerWins = 0;

            //for (int i = 0; i < 100000; i++)
            //{
            //    Battle b = new Battle(new Army(computerArmy), new Army(playerArmy));
            //    if (b.Fight() == 1)
            //    {
            //        computerWins++;
            //    }
            //    else
            //    {
            //        playerWins++;
            //    }
            //}
            const int MAX_ITERATIONS = 100_000; // 1_000_000;

            Parallel.For(0
                        ,MAX_ITERATIONS
                        ,(int i) =>
                        {
                            Battle b = new Battle(new Army(computerArmy), new Army(playerArmy));
                            if (b.Fight() == 1)
                            {
                                Interlocked.Increment(ref computerWins);
                            }
                            else
                            {
                                Interlocked.Increment(ref playerWins);
                            }
                        });

            Console.WriteLine($"computerWins: {computerWins} ({computerWins / (double)MAX_ITERATIONS:n3}), playerWins: {playerWins} ({playerWins / (double)MAX_ITERATIONS:n3})");
        }
    }
}
