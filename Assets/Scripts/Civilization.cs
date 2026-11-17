using System;
using System.Collections.Generic;

namespace GameStudio.MiniCivilization
{
    // Pure C# domain model: deterministic, offline and independent of ad/IAP services.
    public sealed class Civilization
    {
        public int Turn { get; private set; }
        public int Population { get; private set; }
        public int Food { get; private set; }
        public int Wood { get; private set; }
        public int Farms { get; private set; }
        public int Lumberyards { get; private set; }
        public bool IsGameOver { get; private set; }

        public Civilization(int population = 3, int food = 12, int wood = 8)
        {
            if (population < 1 || food < 0 || wood < 0)
                throw new ArgumentOutOfRangeException(nameof(population));
            Population = population;
            Food = food;
            Wood = wood;
        }

        public bool BuildFarm()
        {
            if (IsGameOver || Wood < 5) return false;
            Wood -= 5;
            Farms++;
            return true;
        }

        public bool BuildLumberyard()
        {
            if (IsGameOver || Wood < 4) return false;
            Wood -= 4;
            Lumberyards++;
            return true;
        }

        public bool Recruit()
        {
            if (IsGameOver || Food < 6) return false;
            Food -= 6;
            Population++;
            return true;
        }

        public void AdvanceTurn()
        {
            if (IsGameOver) return;
            // Production is resolved before consumption; starvation removes one citizen.
            Food += Farms * 4;
            Wood += Lumberyards * 3;
            Food -= Population * 2;
            if (Food < 0)
            {
                Food = 0;
                Population--;
            }
            Turn++;
            IsGameOver = Population <= 0;
        }

        public CivilizationSnapshot Snapshot()
        {
            return new CivilizationSnapshot(Turn, Population, Food, Wood, Farms, Lumberyards, IsGameOver);
        }

        public static Civilization Restore(CivilizationSnapshot s)
        {
            if (s == null || s.Turn < 0 || s.Population < 0 || s.Food < 0 ||
                s.Wood < 0 || s.Farms < 0 || s.Lumberyards < 0 ||
                (s.Population == 0) != s.IsGameOver)
                throw new ArgumentException("Invalid save data", nameof(s));
            var game = new Civilization(Math.Max(1, s.Population), s.Food, s.Wood);
            game.Turn = s.Turn;
            game.Population = s.Population;
            game.Farms = s.Farms;
            game.Lumberyards = s.Lumberyards;
            game.IsGameOver = s.IsGameOver;
            return game;
        }
    }

    [Serializable]
    public sealed class CivilizationSnapshot
    {
        public int Turn;
        public int Population;
        public int Food;
        public int Wood;
        public int Farms;
        public int Lumberyards;
        public bool IsGameOver;

        public CivilizationSnapshot(int turn, int population, int food, int wood,
            int farms, int lumberyards, bool isGameOver)
        {
            Turn = turn;
            Population = population;
            Food = food;
            Wood = wood;
            Farms = farms;
            Lumberyards = lumberyards;
            IsGameOver = isGameOver;
        }
    }
}
