using Ka4.Core.DTO.GatewayResponses.UseCaseRequests;
using Ka4.Core.DTO.GatewayResponses.UseCaseResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ka4.Core.Interfaces.UseCases
{
    public interface IGameBoardUseCase : IUseCaseRequestHandler<GameBoardUserRequest, GameBoardUserResponse>
    {
    }
}
