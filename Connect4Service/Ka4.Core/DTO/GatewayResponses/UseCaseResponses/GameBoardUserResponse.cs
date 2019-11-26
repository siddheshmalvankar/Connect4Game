using Ka4.Core.Domain.Entities;
using Ka4.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ka4.Core.DTO.GatewayResponses.UseCaseResponses
{
    public class LastMovePosition
    {
        public int column { get; set; }
        public int cell { get; set; }
    }
    public class GameBoardUserResponse : UseCaseResponseMessage
    {           
        public Player player { get; set; }
        public int?[,] gameboard { get; set; }
        public int? winner { get; set; }
        public int columns { get; }
        public int rows { get; }
        public LastMovePosition lastmoveposition { get; set; }
        public bool isBoardFull { get; set; }
        public IEnumerable<string> Errors { get; }

        public GameBoardUserResponse(int?[,] _board, LastMovePosition lastMove, Player _player, int? _winner, int _rows, int _columns, bool _full, IEnumerable<string> errors, bool success = false, string message = null) : base(success, message)
        {
        
            gameboard = _board;
            lastmoveposition = lastMove;
            isBoardFull = _full;
            player = _player;
            rows = _rows;
            columns = _columns;
            winner = _winner;
            Errors = errors;
        }
      
    }
}
