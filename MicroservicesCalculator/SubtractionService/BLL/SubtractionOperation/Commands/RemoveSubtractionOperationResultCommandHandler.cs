using Common.Shared;
using Common.Shared.Abstractions.Messaging;
using SubtractionService.DAL.Repository;

namespace SubtractionService.BLL.SubtractionOperation.Commands;

public class RemoveSubtractionOperationResultCommandHandler(ISubtractionOperationRepository subtractionOperationRepository) : ICommandHandler<RemoveSubtractionOperationResultCommand>
{
    public async Task<Result> Handle(RemoveSubtractionOperationResultCommand request, CancellationToken cancellationToken)
    {
        var result = new Result();
        try
        {
            await subtractionOperationRepository.RemoveSubtractionResult(request.SubtractionOperationResultId);
            result.IsSuccess = true;
            return result;
        }
        catch (Exception e)
        {
            result.IsSuccess = false;
            result.Errors.Add(e.Message);
            return result;
        }
    }
}