using System;

namespace ConnectFourEngine
{
    public class Engine
    {
        private bool?[,] _board;

        public Engine()
        {
            _board = new bool?[6, 7];
        }

        public void Play(int column)
        {
            if (Winner() != null)
            {
                throw new Exception("The game is over.");
            }

            var count = 0;

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    count += _board[i, j] != null ? 1 : 0;
                }
            }

            for (int i = 5; i >= 0; i--)
            {
                if (_board[i, column] == null)
                {
                    _board[i, column] = count % 2 == 0 ? true : false;
                    return;
                }
            }

            throw new Exception("It cannot be played there.");
        }
        public bool? Winner()
        {
            #region Vertical

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (_board[i, j] == null)
                    {
                        continue;
                    }

                    var flag = false;

                    for (int k = 0; k < 3; k++)
                    {
                        if (_board[i + k, j] != _board[i + k + 1, j])
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        return _board[i, j];
                    }
                }
            }

            #endregion

            #region Horizontal

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_board[i, j] == null)
                    {
                        continue;
                    }

                    var flag = false;

                    for (int k = 0; k < 3; k++)
                    {
                        if (_board[i, j + k] != _board[i, j + k + 1])
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        return _board[i, j];
                    }
                }
            }

            #endregion

            #region Diagonal Right

            for (int i = 3; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_board[i, j] == null)
                    {
                        continue;
                    }

                    var flag = false;

                    for (int k = 0; k < 3; k++)
                    {
                        if (_board[i - k, j + k] != _board[i - k - 1, j + k + 1])
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        return _board[i, j];
                    }
                }
            }

            #endregion

            #region Diagonal Left

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_board[i, j] == null)
                    {
                        continue;
                    }

                    var flag = false;

                    for (int k = 0; k < 3; k++)
                    {
                        if (_board[i + k, j + k] != _board[i + k + 1, j + k + 1])
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        return _board[i, j];
                    }
                }
            }

            #endregion

            return null;
        }
        public string[] Print()
        {
            var result = new string[6];

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    switch (_board[i, j])
                    {
                        case null:
                            result[i] += 0;
                            break;
                        case true:
                            result[i] += 1;
                            break;
                        case false:
                            result[i] += 2;
                            break;
                    }
                }
            }

            return result;
        }
        public Engine Copy()
        {
            var copy = new Engine();

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    copy._board[i, j] = _board[i, j];
                }
            }

            return copy;
        }
        public double Minimax(Engine node, int depth, bool maximizingPlayer)
        {
            if (depth == 0 || node.Winner() != null)
            {
                if (node.Winner() == null)
                {
                    var value = 0;

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            if (node._board[j, 3 - i] == false)
                            {
                                value += 3 - i;
                            }
                            else if (node._board[j, 3 - i] == true)
                            {
                                value -= 3 - i;
                            }

                            if (node._board[j, 3 + i] == false)
                            {
                                value += 3 - i;
                            }
                            else if (node._board[j, 3 + i] == true)
                            {
                                value -= 3 - i;
                            }
                        }
                    }

                    return value;
                }
                else
                {
                    return double.PositiveInfinity * (maximizingPlayer ? -1 : 1);
                }
            }

            if (maximizingPlayer)
            {
                var bestValue = double.NegativeInfinity;

                for (var i = 0; i < 7; i++)
                {
                    try
                    {
                        var child = node.Copy();
                        child.Play(i);
                        var val = Minimax(child, depth - 1, false);
                        bestValue = bestValue > val ? bestValue : val;
                    }
                    catch
                    {
                    }
                }

                return bestValue;
            }
            else
            {
                var bestValue = double.PositiveInfinity;

                for (var i = 0; i < 7; i++)
                {
                    try
                    {
                        var child = node.Copy();
                        child.Play(i);
                        var val = Minimax(child, depth - 1, true);
                        bestValue = bestValue < val ? bestValue : val;
                    }
                    catch
                    {
                    }
                }

                return bestValue;
            }
        }
        public void AIMove()
        {
            if (Winner() != null)
            {
                throw new Exception("The game is over.");
            }

            var bestIndex = default(int?);
            var bestValue = default(double?);

            for (int i = 0; i < 7; i++)
            {
                try
                {
                    var bestTentativeIndex = i;
                    var copy = Copy();
                    copy.Play(i);
                    var bestTentativeValue = Minimax(copy, 6, false);

                    if (bestValue == null || bestTentativeValue > bestValue)
                    {
                        bestIndex = bestTentativeIndex;
                        bestValue = bestTentativeValue;
                    }
                }
                catch
                {
                }
            }

            Play(bestIndex.Value);
        }
    }
}