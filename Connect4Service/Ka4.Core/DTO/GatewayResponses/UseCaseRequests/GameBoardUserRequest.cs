using Ka4.Core.Domain.Entities;
using Ka4.Core.DTO.GatewayResponses.UseCaseResponses;
using Ka4.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ka4.Core.DTO.GatewayResponses.UseCaseRequests
{
    public class GameBoardUserRequest : IUseCaseRequest<GameBoardUserResponse>
    {
        public int selectedColumn { get; set; }
        public Player player { get; set; }
        public int?[,] gameboard { get; set; }
        public int columns { get; }
        public int rows { get; }

        public GameBoardUserRequest(int _selectedColumn,int?[,] _board,Player _player,int _rows, int _columns)
        {
            selectedColumn = _selectedColumn;
            gameboard = _board;
            player = _player;
            rows = _rows;
            columns = _columns;
        }
    }
}
