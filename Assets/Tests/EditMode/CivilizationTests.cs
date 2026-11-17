using System;
using GameStudio.MiniCivilization;
using NUnit.Framework;

namespace GameStudio.MiniCivilization.Tests
{
    public sealed class CivilizationTests
    {
        [Test]
        public void FarmCostsWoodAndProducesFoodBeforeConsumption()
        {
            var game = new Civilization();
            Assert.IsTrue(game.BuildFarm());
            game.AdvanceTurn();
            Assert.AreEqual(3, game.Wood);
            Assert.AreEqual(10, game.Food);
            Assert.AreEqual(1, game.Turn);
        }

        [Test]
        public void CannotBuildWithoutResources()
        {
            var game = new Civilization(3, 0, 0);
            Assert.IsFalse(game.BuildFarm());
            Assert.IsFalse(game.BuildLumberyard());
            Assert.IsFalse(game.Recruit());
        }

        [Test]
        public void StarvationEndsGameWithoutNegativePopulation()
        {
            var game = new Civilization(1, 0, 0);
            game.AdvanceTurn();
            Assert.IsTrue(game.IsGameOver);
            Assert.AreEqual(0, game.Population);
            game.AdvanceTurn();
            Assert.AreEqual(1, game.Turn);
        }

        [Test]
        public void SaveRestoreKeepsEconomyAndTurn()
        {
            var game = new Civilization();
            game.BuildLumberyard();
            game.AdvanceTurn();
            var copy = Civilization.Restore(game.Snapshot());
            Assert.AreEqual(game.Turn, copy.Turn);
            Assert.AreEqual(game.Wood, copy.Wood);
            Assert.AreEqual(game.Food, copy.Food);
            Assert.AreEqual(game.Lumberyards, copy.Lumberyards);
        }

        [Test]
        public void InvalidSaveIsRejected()
        {
            Assert.Throws<ArgumentException>(() =>
                Civilization.Restore(new CivilizationSnapshot(0, -1, 0, 0, 0, 0, false)));
        }
    }
}
