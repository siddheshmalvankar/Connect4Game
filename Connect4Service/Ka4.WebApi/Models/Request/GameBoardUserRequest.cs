using Ka4.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ka4.WebApi.Models.Request
{
    public class GameBoardUserRequest
    {
        public int selectedColumn { get; set; }
        public bool isPlayer { get; set; }
        public int?[,] gameboard { get; set; }
        public int columns { get; set; }
        public int rows { get; set; }
    }
}
