using AdditionService.DAL.Repository;
using Common.Shared;
using Common.Shared.Abstractions.Messaging;

namespace AdditionService.BLL.AdditionOperation.Commands;

public class RemoveAdditionOperationResultCommandHandler(IAdditionOperationRepository additionOperationRepository) : ICommandHandler<RemoveAdditionOperationResultCommand>
{
    public async Task<Result> Handle(RemoveAdditionOperationResultCommand request, CancellationToken cancellationToken)
    {
        var result = new Result();
        try
        {
            await additionOperationRepository.RemoveAdditionResult(request.AdditionOperationResultId);
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