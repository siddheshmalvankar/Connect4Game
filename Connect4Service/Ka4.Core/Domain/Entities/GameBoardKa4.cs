using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ka4.Core.Domain.Entities
{
    public enum Player
    {
        First =0,
        User = 1,
        Computer = 2
    }
    public class GameBoardKa4
    {
        public int Columns { get; }
        public int Rows { get; }

        private readonly int?[,] _gameBoard;
        private int? _gamewinner;
        private bool _changed;
        public GameBoardKa4(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            _gameBoard = new int?[columns, rows];
        }
    }
}
