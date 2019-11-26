using Ka4.Core.Domain.Entities;
using Ka4.Core.DTO.GatewayResponses.UseCaseRequests;
using Ka4.Core.DTO.GatewayResponses.UseCaseResponses;
using Ka4.Core.Interfaces.UseCases;
using System;
using System.Threading.Tasks;
using Ka4.Core.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Ka4.Core.UseCases
{
  
    public sealed class PlayConnect4 : IGameBoardUseCase
    {
        private bool _changed;
        private string responseMessage;
        public async Task<bool> Handle(GameBoardUserRequest message, IOutputPort<GameBoardUserResponse> outputPort)
        {
            responseMessage = string.Empty;
            bool responseStatus = false;
            List<string> errors = new List<string>();
            int?[,] board = message.gameboard;
            LastMovePosition lastMove = new LastMovePosition();
            int? boardWinner=-1;
            bool boardIsFull = false;
            bool gameOver = false;
            try
            {
                switch (message.player)
                {
                    case Player.User:                        
                        if (!PushBall(Player.User,message.selectedColumn-1, ref board,ref lastMove, message.rows,message.columns))
                            responseMessage = "Select different Column, Column full";
                        boardWinner = GetWinner(ref board, message.rows, message.columns)??-1;
                        if (boardWinner == 1)
                        {
                            responseMessage = $"{Player.User.ToString()} Won!";
                            WinningPositions(ref board, boardWinner);
                            gameOver = true;
                        }
                        break;
                    case Player.Computer:
                        var random = new Random();
                        var moves = new List<Tuple<int, int>>();
                        for (int position = 0; position < message.columns; position++)
                        {
                            if (!PushBall(Player.Computer, position, ref board, ref lastMove, message.rows, message.columns))
                                continue;
                            var item = await Task.Run(()=>MinMax(5, ref board, ref lastMove, false, message.rows, message.columns));
                            moves.Add(Tuple.Create(position, item));
                            PopTopBall(position, ref board, message.rows);
                        }
                        int maxMoveScore = moves.Max(t => t.Item2);
                        var bestMoves = moves.Where(t => t.Item2 == maxMoveScore).ToList();
                        PushBall(Player.Computer, bestMoves[random.Next(0, bestMoves.Count)].Item1, ref board, ref lastMove, message.rows, message.columns);
                        boardWinner = GetWinner(ref board, message.rows, message.columns) ?? -1;
                        if (boardWinner == 2)
                        {
                            responseMessage = $"{Player.Computer.ToString()} Won!";
                            WinningPositions(ref board, boardWinner);
                            gameOver = true;
                        }
                        break;
                    default:
                        responseMessage = "Welcome to Connect 4. Make your first move...";
                        break;
                }
                boardIsFull = IsGameBoardFull(message.columns, ref board);
                if (boardIsFull)
                {
                    responseMessage = "Game Board Full. Its a Tie!";
                    gameOver = true;
                }

                responseStatus = true;
            }
            catch(Exception ex)
            {
                responseStatus = false;
                responseMessage = "Unexpectd Error Occured";
                gameOver = true;
            }    
            finally
            {
                outputPort.Handle(new GameBoardUserResponse(board,lastMove, message.player, boardWinner, message.rows, message.columns, gameOver, errors, responseStatus, responseMessage));
            }
            return responseStatus;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="board"></param>
        /// <param name="maximizingPlayer"></param>
        /// <returns></returns>
        private int MinMax(int depth, ref int?[,] gameboard, ref LastMovePosition lastMove, bool maximizingPlayer,int rows,int columns)
        {
            if (depth <= 0)
                return 0;

            var winner = GetWinner(ref gameboard, rows, columns);
            if (winner == 2)
                return depth;
            if (winner == 1)
                return -depth;
            if (IsGameBoardFull(columns,ref gameboard))
                return 0;


            int bestValue = maximizingPlayer ? -1 : 1;
            for (int i = 0; i < columns; i++)
            {
                if(!PushBall(maximizingPlayer?Player.Computer:Player.User,i,ref gameboard,ref lastMove,rows, columns))             
                    continue;
                int v = MinMax(depth - 1, ref gameboard,ref lastMove, !maximizingPlayer,rows,columns);
                bestValue = maximizingPlayer ? Math.Max(bestValue, v) : Math.Min(bestValue, v);
                PopTopBall(i, ref gameboard, rows);
              
            }

            return bestValue;
        }

        /// <summary>
        /// Method to place to valid position in Grid
        /// </summary>
        /// <param name="player"></param>
        /// <param name="selectedColumn"></param>
        /// <param name="gameBoard"></param>
        /// <returns></returns>
        private bool PushBall(Player player,int selectedColumn, ref int?[,] board, ref LastMovePosition lastMove,int rows, int columns)
        {            
            try
            {
                int row = 0;
                while (row < rows && !board[selectedColumn, row].HasValue)
                {
                    row++;
                }
                if (row == 0)
                    return false;
                //var playerId = player.ToDictionary();
                var playedId = (int)Enum.Parse(typeof(Player), player.ToString());
                lastMove.column = selectedColumn;
                lastMove.cell = row - 1;
                board[ selectedColumn, row - 1] = playedId;
                _changed = true;
                return true;

            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Method to Remove last pushed ball 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="selectedColumn"></param>
        /// <param name="board"></param>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private bool PopTopBall(int selectedColumn, ref int?[,] board, int rows)
        {
            try
            {
                int row = 0;
                while (row < rows && !board[selectedColumn, row].HasValue)
                {
                    row++;
                }

                if (row == rows)
                    return false;
                board[selectedColumn, row] = null;
                _changed = true;
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        /// <summary>
        /// Method determined Winner
        /// </summary>
        /// <param name="player"></param>
        /// <param name="selectedColumn"></param>
        /// <param name="board"></param>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public int? GetWinner(ref int?[,] board, int rows, int columns)
        {
            int? winner = null;
            try
            {
                if (!_changed)
                    return winner;

                _changed = false;
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        if (!board[i, j].HasValue)
                            continue;

                        bool horizontal = i + 3 < columns;
                        bool vertical = j + 3 < rows;

                        if (!horizontal && !vertical)
                            continue;

                        bool forwardDiagonal = horizontal && vertical;
                        bool backwardDiagonal = vertical && i - 3 >= 0;

                        for (int k = 1; k < 4; k++)
                        {
                            horizontal = horizontal && board[i, j] == board[i + k, j];
                            vertical = vertical && board[i, j] == board[i, j + k];
                            forwardDiagonal = forwardDiagonal && board[i, j] == board[i + k, j + k];
                            backwardDiagonal = backwardDiagonal && board[i, j] == board[i - k, j + k];
                            if (!horizontal && !vertical && !forwardDiagonal && !backwardDiagonal)
                                break;
                        }

                        if (horizontal || vertical || forwardDiagonal || backwardDiagonal)
                        {
                            winner = board[i, j];                            
                            return winner;
                        }
                    }
                }
                            

                winner = null;
                return winner;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public bool WinningPositions(ref int?[,] board,int? winner)
        {
            try
            {               
                var rowstack = new Stack<Tuple<int, int>>();
                for (int columns = 0; columns < board.GetLength(0); columns++)
                {
                    for (int rows = 0; rows < board.GetLength(1); rows++)
                    {
                        rowstack.Push(Tuple.Create(columns, rows));
                        if (!(board[columns, rows] == winner))
                            rowstack.Pop();
                    }
                    if (rowstack.Count >= 4)
                        break;
                    else
                        rowstack.Clear();
                }
                if (rowstack.Count >= 4)
                    foreach (var obj in rowstack)
                    {
                        if (board[obj.Item1, obj.Item2].HasValue)
                        board[obj.Item1, obj.Item2] = 5;
                    }
                    
                
                var columnstack = new Stack<Tuple<int, int>>();
                for (int columns = 0; columns < board.GetLength(1); columns++)
                {
                    int row = 0;
                    while (row < board.GetLength(0))
                    {
                        columnstack.Push(Tuple.Create(columns, row));
                        if (!(board[row, columns] == winner))
                            columnstack.Pop();
                        row++;
                    }
                    if (columnstack.Count >= 4)
                        break;
                    else
                        columnstack.Clear();
                }
                if (columnstack.Count >= 4)
                    foreach (var obj in columnstack)
                    {
                        if(board[obj.Item2, obj.Item1].HasValue)
                        board[obj.Item2, obj.Item1] = 5;
                    }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// Method to check of Board if full of moves
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool IsGameBoardFull(int columns, ref int?[,] board)
        {
            try
            {
                for (int i = 0; i < columns; i++)
                {
                    if (!board[0,i].HasValue)
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
            
        }
    }
}
