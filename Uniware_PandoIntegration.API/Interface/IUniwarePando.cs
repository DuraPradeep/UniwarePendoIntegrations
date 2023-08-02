using Uniware_PandoIntegration.Entities;

namespace Uniware_PandoIntegration.API.Folder
{
    public interface IUniwarePando
    {
        string GenerateJWTTokens(TokenEntity tokenEntity,out TokenEntity tokenEntity1); 
    }
}
