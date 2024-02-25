using Common.Shared;
using Common.Shared.Abstractions.Messaging;
using DivisionService.DAL.Repository;

namespace DivisionService.BLL.MultiplicationOperation.Commands;

public class RemoveDivisionOperationResultCommandHandler(IDivisionOperationRepository multiplicationOperationRepository) : ICommandHandler<RemoveDivisionOperationResultCommand>
{
    public async Task<Result> Handle(RemoveDivisionOperationResultCommand request, CancellationToken cancellationToken)
    {
        var result = new Result();
        try
        {
            await multiplicationOperationRepository.RemoveDivisionResult(request.DivisionOperationResultId);
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