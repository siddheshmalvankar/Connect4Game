using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ka4.Core.Domain.Entities;
using Ka4.Core.DTO.GatewayResponses.UseCaseRequests;
using Ka4.Core.Interfaces.UseCases;
using Ka4.WebApi.Presenters;
using Microsoft.AspNetCore.Mvc;

namespace Ka4.WebApi.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Ka4Controller : ControllerBase
    {
        private readonly IGameBoardUseCase _ka4UseCase;
        private readonly GameBoardUserPresenter _ka4Presenter;
        private GameBoardUserRequest _board;
        public Ka4Controller(IGameBoardUseCase ka4usercase, GameBoardUserPresenter userPresenter)
        {
            //_board = new GameBoardUserRequest(0,new int?[7, 6], Player.First,6, 7);
            _ka4UseCase = ka4usercase;
            _ka4Presenter = userPresenter;
        }
        
        // GET api/ka4
        [HttpGet]
        public string GetState()
        {           
            return "Web Api Service running.....";
        }

        [HttpGet]
        public async Task<ActionResult> ResetBoard(int rows,int columns)
        {
          
            //var player = Player.Computer;
            await _ka4UseCase.Handle(new GameBoardUserRequest(0, new int?[columns, rows], Player.First, rows, columns)
                , _ka4Presenter);
            return _ka4Presenter.ContentResult;
        }
        [HttpPost]  
        public async Task<ActionResult> PlayMove([FromBody] Models.Request.GameBoardUserRequest request)
        {
            var player = request.isPlayer ? Player.User : Player.Computer;
            await _ka4UseCase.Handle(new GameBoardUserRequest(request.selectedColumn, request.gameboard, player, request.rows,request.columns)
                , _ka4Presenter);
            return _ka4Presenter.ContentResult;
        }


    }
}
