﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace ProbabilityWarGame
{
    class Program
    {
        static Random rand = new Random();
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

            for (int i = 0; i < 10; i++)
            {
                computerArmy.AddUnit(new Unit(UnitType.Soldier, 5, 1, 1));
            }

            for (int i = 0; i < 5; i++)
            {
                computerArmy.AddUnit(new Unit(UnitType.Tank, 5, 3, 1));
            }

            // Generate random player choices
            Army[] playerChoices = new Army[4];

            for (int i = 0; i < 4; i++)
            {
                playerChoices[i] = GenerateRandomChoices();
            }
            // Display choices to player and ask them to choose
            Console.WriteLine("Choose your army:");

            for (int i = 0; i < playerChoices.Length; i++)
            {
                Console.WriteLine($"Option {i + 1}:");
                Console.WriteLine(playerChoices[i].ToString());
                Console.WriteLine();
            }

            int playerChoiceIndex = 0;

            while (playerChoiceIndex < 1 || playerChoiceIndex > playerChoices.Length)
            {
                Console.Write("Enter your choice (1-4): ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out playerChoiceIndex))
                {
                    if (playerChoiceIndex < 1 || playerChoiceIndex > playerChoices.Length)
                    {
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 4.");
                }
            }

            DoBattles(computerArmy, playerChoices[0]);
            DoBattles(computerArmy, playerChoices[1]);
            DoBattles(computerArmy, playerChoices[2]);
            DoBattles(computerArmy, playerChoices[3]);

        }
        private static Army GenerateRandomChoices()
        {
            Army choices = new Army();

            int numUnitTypes = GetRandomInt(1, 3); // Choose 1 or 2 unit types

            // Generate units for each unit type
            for (int i = 0; i < numUnitTypes; i++)
            {
                UnitType unitType = (UnitType)GetRandomInt(0, 3);

                int numUnits = GetRandomInt(1, 70);

                int speed = GetRandomInt(1, 6);
                int hp = GetRandomInt(1, 3);
                int damage = GetRandomInt(1, 3);
                bool antiTank = false;
                bool antiAir = false;

                for (int j = 0; j < numUnits; j++)
                {


                    switch (unitType)
                    {
                        case UnitType.Soldier:
                            //speed = Math.Min(speed, 10);
                            //damage = Math.Min(damage, 5);
                            break;
                        case UnitType.Tank:
                            //speed = Math.Min(speed,15);
                            //damage = Math.Max(damage, 5);
                            antiTank = true;
                            break;
                        case UnitType.Plane:
                            //speed = Math.Max(speed, 50);
                            //hp = Math.Max(hp, 10);
                            //damage = Math.Max(damage, 5);
                            antiTank = true;
                            antiAir = true;
                            break;
                    }

                    choices.AddUnit(new Unit(unitType, speed, hp, damage, antiTank, antiAir));
                }
            }

            // Calculate score for the army
            double score = CalculateArmyScore(choices);

            // If the score is too high or too low, regenerate the army
            while (score > 200 || score < 180)
            {
                choices = GenerateRandomChoices();
                score = CalculateArmyScore(choices);
            }

            return choices;
        }

        private static double CalculateArmyScore(Army army)
        {
            const double soldierMultiplier = 1;
            const double tankMultiplier = 1.2;
            const double planeMultiplier = 1.3;

            double score = 0;
            foreach (Unit unit in army.Units)
            {
                double unitScore = unit.Speed*2 + unit.Hp + unit.Damage;

                switch (unit.UnitType)
                {
                    case UnitType.Soldier:
                        unitScore *= soldierMultiplier;
                        break;
                    case UnitType.Tank:
                        unitScore *= tankMultiplier;
                        break;
                    case UnitType.Plane:
                        unitScore *= planeMultiplier;
                        break;
                }

                score += unitScore;
            }

            return score;
        }

        private static int GetRandomSeed()
        {
            return Guid.NewGuid().GetHashCode();
        }

        private static int GetRandomInt(int min, int max)
        {
            // Create a new Random object with a random seed
            Random rand = new Random(GetRandomSeed());

            return rand.Next(min, max);
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
