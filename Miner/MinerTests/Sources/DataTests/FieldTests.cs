﻿using System;
using NSubstitute;
using NUnit.Framework;

using Miner.Data;
using Miner.Math;

namespace MinerTests.DataTests
{
    [TestFixture]
    public class FieldTests
    {
        [TestCase(1, 1, 1)]
        public void TestConstructor_ValidArguments_ObjectCreated(int width,
            int height, int numMines)
        {
            var field = new Field(width, height, numMines);
            Assert.AreEqual(width, field.Width);
            Assert.AreEqual(height, field.Height);
            Assert.AreEqual(numMines, field.NumMines);
            Assert.AreEqual(FieldState.NotInitialized, field.State);
        }

        [TestCase(0, 1, 1)]
        [TestCase(1, 0, 1)]
        [TestCase(1, 1, 0)]
        [TestCase(1, 1, 2)]
        public void TestConstructor_InvalidArgument_ThrowsException(int width,
            int height, int numMines)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var field = new Field(width, height, numMines);
            });
        }

        private class FakeField : Field
        {
            public FakeField(int width, int height, int numMines) : base(width, height, numMines)
            {
                // NOP
            }

            public void SetState(FieldState state)
            {
                State = state;
            }
        }

        [TestCase(FieldState.SomeCellsMarkedOrRevealed)]
        public void TestInitialize_InvalidState_ThrowsException(FieldState state)
        {
            var field = new FakeField(1,1,1);
            field.SetState(state);
            Assert.Throws<InvalidOperationException>(() => field.Initialize());
        }

        [Test]
        public void TestInitialize_AllCellsHidden()
        {
            var field = new Field(2, 2, 1);
            field.Initialize();

            for (int row = 0; row < field.Height; row++)
            {
                for (int col = 0; col < field.Width; col++)
                {
                    Assert.AreEqual(CellState.Hidden, field[row, col].State);
                }
            }
        }

        [TestCase(2, 2, -1, 1)]
        [TestCase(2, 2, 1, -1)]
        [TestCase(2, 2, 2, 1)]
        [TestCase(2, 2, 1, 2)]
        public void TestInitialize_RandomValueOutOfRange_Throws_Exception(int width, int height, int fakeRow, int fakeCol)
        {
            var fakeRandomizer = Substitute.For<IRandomizer>();
            bool isRow = false;
            fakeRandomizer.GetValue(Arg.Any<int>(), Arg.Any<int>()).Returns(info => 
            {
                isRow = !isRow;
                return isRow ? fakeRow : fakeCol;
            });

            var field = new Field(width, height, 1, fakeRandomizer);
            Assert.Throws<ArgumentOutOfRangeException>(() => field.Initialize());
        }

      
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        public void TestGetter_InvalidIndex_ThrowsException(int row, int col)
        {
            var field = new Field(1, 1, 1);
            field.Initialize();

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var cell = field[row, col];
            });
        }

        [TestCase(2,2,2)]
        public void TestInitialize_PlaceIncorrectNumberOfMines(int width, int height, int numMines)
        {
            var field = new Field(width, height, numMines);
            field.Initialize();
            int countOfMines = 0;

            for(int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (field.CellAt(i,j).Object is Mine)
                    {
                        countOfMines++;
                    }
                }
            }

            Assert.AreEqual(numMines, countOfMines);
        }

        
        [Test]
        public void TestInitialize_RandomizerChoosesTheSameCellTwice()
        {
            var fakeRandomizer = Substitute.For<IRandomizer>();

            int index = 0;
            int[] positions = { 0, 0, 0, 0, 1, 1 };
            fakeRandomizer.GetValue(Arg.Any<int>(), Arg.Any<int>()).Returns(callInfo =>
            {
                return positions[index++];
            });

            int numMines = 2;
            var field = new Field(2, 2, numMines, fakeRandomizer);
            field.Initialize();
        }
        
        
        [TestCase(2, 2, 2)]
        public void TestInitialize_AllMinesPlaced(int width, int height, int numMines)
        {
            var field = new Field(width, height, numMines);
            field.Initialize();

            int minesCounter = 0;
            for (int row = 0; row < field.Height; row++)
            {
                for (int col = 0; col < field.Width; col++)
                {
                    if (field[row, col].Object is Mine)
                    {
                        minesCounter++;
                    }
                }
            }

            Assert.AreEqual(numMines, minesCounter);
        }

        [TestCase(1, 1, 1, new int[] { 0, 0 })]
        [TestCase(1, 1, 2, new int[] { 0, 0, 0, 1 })]
        [TestCase(1, 1, 3, new int[] { 0, 0, 0, 1, 0, 2 })]
        [TestCase(1, 1, 4, new int[] { 0, 0, 0, 1, 0, 2, 1, 0 })]
        [TestCase(1, 1, 5, new int[] { 0, 0, 0, 1, 0, 2, 1, 0, 1, 2 })]
        [TestCase(1, 1, 6, new int[] { 0, 0, 0, 1, 0, 2, 1, 0, 1, 2, 2, 0 })]
        [TestCase(1, 1, 7, new int[] { 0, 0, 0, 1, 0, 2, 1, 0, 1, 2, 2, 0, 2, 1 })]
        [TestCase(1, 1, 8, new int[] { 0, 0, 0, 1, 0, 2, 1, 0, 1, 2, 2, 0, 2, 1, 2, 2 })]
        [TestCase(2, 2, 0, new int[] { 0, 0 })]
        public void TestInitialize_Field3x3_CorrectValueInStatedCell(int row, int col,
            int expectedValue, int[] minesPositions)
        {
            var fakeRandomizer = Substitute.For<IRandomizer>();

            int index = 0;
            fakeRandomizer.GetValue(Arg.Any<int>(), Arg.Any<int>()).Returns(callInfo =>
            {
                return minesPositions[index++];
            });

            int numMines = minesPositions.Length / 2;
            var field = new Field(3, 3, numMines, fakeRandomizer);
            field.Initialize();

            var numberOfMines = (NumberOfMines)field[row, col].Object;
            Assert.AreEqual(expectedValue, numberOfMines.Value);
        }

        [Test]
        public void TestInitialize_Field_FieldModTypeInitialized()
        {
            bool eventOccured = false;
            var field = new Field(1, 1, 1);

            field.Modified += delegate
            {
                eventOccured = true;
            };
            field.Initialize();
            Assert.IsTrue(eventOccured);
        } 

       
        [Test]
        public void TestRevealAllCells_AllCellsRevealed()
        {
            var field = new Field(2, 2, 2);
            field.Initialize();
            field.RevealAllCells();

            for (int row = 0; row < field.Height; row++)
            {
                for (int col = 0; col < field.Width; col++)
                {
                    Assert.AreEqual(CellState.Revealed, field[row, col].State);
                }
            }
        }

        [Test]
        public void TestRevealAllCells_ModifiedEventRaised()
        {
            var field = new Field(2, 2, 2);
            field.Initialize();
            bool eventOccured = false;
            field.Modified += delegate
            {
                eventOccured = true;
            };
            field.RevealAllCells();
            Assert.IsTrue(eventOccured);
        }

        [TestCase(0, 0)]
        public void TestMarkCell_ValidArgumentsForField1x1_NoException(int row, int col)
        {
            Field field = new Field(2, 2, 1);
            field.Initialize();
            field.MarkCell(row, col);
        }

        [Test]
        public void TestMarkCell_ModifiedEventRaised()
        {
            var field = new Field(2, 2, 2);
            field.Initialize();
            bool eventOccured = false;
            field.Modified += delegate
            {
                eventOccured = true;
            };
            field.MarkCell(0,0);
            Assert.IsTrue(eventOccured);
        }
        
        
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        public void TestMarkCell_InvalidArgumentsForField1x1_ThrowsException(int row, int col)
        {
            Field field = new Field(1, 1, 1);
            field.Initialize();
            Assert.Throws<ArgumentOutOfRangeException>(() => field.MarkCell(row, col));
        }

        
        [Test]
        public void TestMarkCell_CellsOfField2x2Hidden_CellMarked()
        {
            Field field = new Field(2, 2, 1);
            field.Initialize();

            field.MarkCell(0, 0);
            Assert.AreEqual(CellState.Marked, field[0, 0].State);
        }

        
        [Test]
        public void TestMarkCell_CellsOfField2x2Revealed_CellStateNotChanged()
        {
            var field = new FakeField(2, 2, 1);
            field.Initialize();
            field.RevealCell(0, 0);
            field.SetState(FieldState.SomeCellsMarkedOrRevealed);

            var previousState = field[0, 0].State;
            field.MarkCell(0, 0);

            Assert.AreEqual(previousState, field[0, 0].State);
        }

        
        [Test]
        public void TestMarkCell_CellsOfField2x2HiddedMethodCalled2Times_CellStillHidden()
        {
            Field field = new Field(2, 2, 1);
            field.Initialize();

            field.MarkCell(0, 0);
            field.MarkCell(0, 0);

            Assert.AreEqual(CellState.Hidden, field[0, 0].State);
        }

        
        [TestCase(0, 0)]
        public void TestRevealCell_ValidArgumentsForField1x1_NoException(int row, int col)
        {
            Field field = new Field(2, 2, 1);
            field.Initialize();
            field.RevealCell(row, col);
        }

        
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        public void TestRevealCell_InvalidArgumentsForField1x1_ThrowsException(int row, int col)
        {
            Field field = new Field(1, 1, 1);
            field.Initialize();
            Assert.Throws<ArgumentOutOfRangeException>(() => field.RevealCell(row, col));
        }

        
        [Test]
        public void TestRevealCell_OneCellRevealed()
        {
            var fakeRandomizer = Substitute.For<IRandomizer>();

            int index = 0;
            int[] positions = { 0, 0 };
            fakeRandomizer.GetValue(Arg.Any<int>(), Arg.Any<int>()).Returns(callInfo =>
            {
                return positions[index++];
            });

            var field = new Field(2, 2, 1, fakeRandomizer);
            field.Initialize();

            field.RevealCell(1, 1);
            Assert.AreEqual(CellState.Hidden, field[0, 0].State);
            Assert.AreEqual(CellState.Hidden, field[0, 1].State);
            Assert.AreEqual(CellState.Hidden, field[1, 0].State);
            Assert.AreEqual(CellState.Revealed, field[1, 1].State);
        }

        /*
        [Test]
        public void TestRevealCell_TriedToRevealMine_AllCellsRevealed()
        {
            var fakeRandomizer = Substitute.For<IRandomizer>();

            int index = 0;
            int[] positions = { 0, 0 };
            fakeRandomizer.GetValue(Arg.Any<int>(), Arg.Any<int>()).Returns(callInfo =>
            {
                return positions[index++];
            });

            var field = new Field(2, 2, 1, fakeRandomizer);
            field.Initialize();

            field.RevealCell(0, 0);
            for (int row = 0; row < field.Height; row++)
            {
                for (int col = 0; col < field.Width; col++)
                {
                    Assert.AreEqual(CellState.Revealed, field[row, col].State);
                }
            }
        }

        [Test]
        public void TestRevealCell_EmptyCellsRevealedRecursively()
        {
            var fakeRandomizer = Substitute.For<IRandomizer>();

            int index = 0;
            int[] positions = { 0, 0 };
            fakeRandomizer.GetValue(Arg.Any<int>(), Arg.Any<int>()).Returns(callInfo =>
            {
                return positions[index++];
            });

            var field = new Field(7, 7, 1, fakeRandomizer);
            field.Initialize();

            field.RevealCell(3, 3);

            Assert.AreEqual(CellState.Hidden, field[0, 0].State);
            for (int row = 0; row < field.Height; row++)
            {
                for (int col = 0; col < field.Width; col++)
                {
                    if (row != 0 && col != 0)
                    {
                        Assert.AreEqual(CellState.Revealed, field[row, col].State);
                    }
                }
            }
        }

        [Test]
        public void TestRevealCell_CellMarked_NotRevealed()
        {
            var field = new Field(2, 2, 1);
            field.Initialize();

            field.MarkCell(0, 0);
            field.RevealCell(0, 0);

            Assert.AreEqual(CellState.Marked, field[0, 0].State);
        }

        [Test]
        public void TestAllMinesMarked_AllMinesMarked()
        {
            var fakeRandomizer = Substitute.For<IRandomizer>();

            int index = 0;
            int[] positions = { 0, 0, 1, 1 };
            fakeRandomizer.GetValue(Arg.Any<int>(), Arg.Any<int>()).Returns(callInfo =>
            {
                return positions[index++];
            });

            var field = new Field(2, 2, 2, fakeRandomizer);
            field.Initialize();

            Assert.AreEqual(FieldState.AllMinesMarked, field.State);

            field.MarkCell(0, 0);
            field.MarkCell(1, 1);
            Assert.IsFalse(field.AllMinesMarked);

            field.RevealCell(0, 1);
            field.RevealCell(1, 0);
            Assert.IsTrue(field.AllMinesMarked);            
        }*/
    }
}
