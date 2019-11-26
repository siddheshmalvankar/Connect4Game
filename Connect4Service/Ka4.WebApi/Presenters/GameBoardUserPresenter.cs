using Ka4.Core.DTO.GatewayResponses.UseCaseResponses;
using Ka4.Core.Interfaces.UseCases;
using Ka4.WebApi.Serialization;
using System.Net;


namespace Ka4.WebApi.Presenters
{
    public class GameBoardUserPresenter : IOutputPort<GameBoardUserResponse>
    {
        public JsonContentResult ContentResult { get; }
        public GameBoardUserPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(GameBoardUserResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            ContentResult.Content = JsonSerializer.SerializeObject(response);
        }
    }
}
