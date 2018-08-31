using System;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Models.Games.TicTacToe.AI
{

    public abstract class BaseMinimaxPlayer : GenericAIPlayer, IAIPlayer
    {
        public BaseMinimaxPlayer(string id)
        : base(id)
        { }

        public abstract byte NextMove(GameManager gameManager);

        /*
         * Depth 2 is easy to win
         * Depth 3 is somewhat harder to win
         * Depth 4 is hard to win
         * Depth 5 is unbeatable
         * Depth 6 is smarter and can win in more situations
         */
        protected byte NextMove(GameManager gameManager, byte distanceFromEnd)
        {
            byte move;
            var choiceTree = new ChoiceTree();
            var bestChoice = this.Minimax(gameManager, distanceFromEnd);

            // Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(choiceTree));

            if (bestChoice == null)
            {
                move = (byte)gameManager.GetRemainingMoves().First();
            }
            else
            {
                move = bestChoice.Move;
            }

            return move;
        }

        private Choice Minimax(GameManager gameManager, byte maxDistanceFromEnd, byte? distanceFromEnd = null, sbyte? alpha = null, sbyte? beta = null, bool maximizingPlayer = true, ChoiceTree choiceTree = null)
        {
            if (distanceFromEnd == null)
            {
                distanceFromEnd = maxDistanceFromEnd;
            }

            sbyte? bestValue = null;
            byte? bestMove = null;

            var moves = gameManager.GetRemainingMoves();
            var bestChoices = new List<Choice>();

            if (choiceTree != null)
            {
                choiceTree.Choices = new List<ChoiceTreeEl>();
                choiceTree.EndChoices = new List<Choice>();
            }

            foreach (byte currMove in moves)
            {
                gameManager.NewMove(currMove, maximizingPlayer ? this : gameManager.GetOpponent(this));
                sbyte? currValue = null;

                if (gameManager.IsGameOver)
                {
                    currValue = (gameManager.Winner == this)
                           ? (sbyte)(10 + distanceFromEnd)
                           : (gameManager.Winner == null) ? (sbyte)(-distanceFromEnd) : (sbyte)(-10 - distanceFromEnd);

                    if (choiceTree != null)
                    {
                        choiceTree.EndChoices.Add(new Choice() { Value = (sbyte)currValue, Move = (byte)currMove });
                    }
                }
                else if (distanceFromEnd > 0)
                {
                    ChoiceTree currChoiceTree = null;
                    Choice currChoice = null;
                    if (choiceTree != null)
                    {
                        currChoice = new Choice() { Move = currMove };
                        currChoiceTree = new ChoiceTree();
                        choiceTree.Choices.Add(new ChoiceTreeEl() { Choice = currChoice, ChoiceTree = currChoiceTree });
                    }
                    var choice = this.Minimax(gameManager, maxDistanceFromEnd, (byte)(distanceFromEnd - 1), alpha, beta, !maximizingPlayer, currChoiceTree);
                    if (choice != null)
                    {
                        currValue = choice.Value;
                        currValue = (currValue == 9) ? -9 : ((currValue == -9) ? 9 : currValue);

                        if (choiceTree != null)
                        {
                            currChoice.Value = (sbyte)currValue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("impossible");
                    }
                }
                else
                {
                    currValue = (sbyte)(maximizingPlayer ? -9 : 9);

                    if (choiceTree != null)
                    {
                        choiceTree.EndChoices.Add(new Choice() { Value = (sbyte)currValue, Move = (byte)currMove });
                    }
                }

                if (currValue != null)
                {
                    if (bestValue == null
                        || (maximizingPlayer && currValue > bestValue)
                        || (!maximizingPlayer && currValue < bestValue))
                    {
                        bestChoices.Clear();
                        bestChoices.Add(new Choice() { Value = (sbyte)currValue, Move = (byte)currMove });
                        bestValue = currValue;
                        bestMove = currMove;
                    }
                    else if (currValue == bestValue)
                    {
                        bestChoices.Add(new Choice() { Value = (sbyte)currValue, Move = (byte)currMove });
                        bestValue = currValue;
                        bestMove = currMove;
                    }

                    if (maximizingPlayer)
                    {
                        if (alpha == null || currValue > alpha)
                        {
                            alpha = currValue;
                        }
                    }
                    else
                    {
                        if (beta == null || currValue < beta)
                        {
                            beta = currValue;
                        }
                    }

                    if (alpha != null
                        && beta != null
                        && beta < alpha)
                    {
                        gameManager.NewMove(currMove, null);
                        break;
                    }
                }

                gameManager.NewMove(currMove, null);
            }

            Choice bestChoice;

            // Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(bestChoices));

            // Prever blocking moves
            if (bestChoices.Count > 1)
            {
                if (distanceFromEnd == maxDistanceFromEnd)
                {
                    var counts = new Dictionary<Choice, byte>();
                    var bestCount = 0;

                    bestChoices.ForEach(choice =>
                    {
                        byte count = 0;
                        var move = choice.Move;

                        gameManager.GetContainingPaths(move).ForEach(path =>
                        {
                            var otherCells = path.Where(cell => cell != move)
                                                .GroupBy(cell => gameManager.GetCellOwner(cell))
                                                .Select(group => new { Owner = group.Key, Cells = group.ToList() })
                                                .ToList();

                            if (otherCells.Count == 1
                                && otherCells[0].Owner != null)
                            {
                                count++;
                            }
                        });

                        if (count > bestCount)
                        {
                            bestCount = count;
                        }

                        counts.Add(choice, count);
                    });

                    bestChoices = counts
                        .Keys
                        .Where(choice => counts[choice] == bestCount)
                        .ToList();
                }

                if (bestChoices.Count == 1)
                {
                    bestChoice = bestChoices[0];
                }
                else
                {
                    var rand = new Random();
                    var randNum = (byte)rand.Next(bestChoices.Count);
                    bestChoice = bestChoices[randNum];
                }
            }
            else
            {
                bestChoice = new Choice() { Value = (sbyte)bestValue, Move = (byte)bestMove };
            }

            if (choiceTree != null)
            {
                choiceTree.BestChoice = bestChoice;
            }

            return (bestValue == null)
                ? null
                : bestChoice;
        }
    }

    class Choice
    {
        public sbyte Value { get; set; }
        public byte Move { get; set; }
    }

    class ChoiceTree
    {
        public Choice BestChoice { get; set; }
        public List<ChoiceTreeEl> Choices { get; set; }
        public List<Choice> EndChoices { get; set; }
    }

    class ChoiceTreeEl
    {
        public Choice Choice { get; set; }
        public ChoiceTree ChoiceTree { get; set; }
    }
}
