using Common.Shared;
using Common.Shared.Abstractions.Messaging;
using MultiplicationService.DAL.Repository;

namespace MultiplicationService.BLL.MultiplicationOperation.Commands;

public class RemoveMultiplicationOperationResultCommandHandler(IMultiplicationOperationRepository multiplicationOperationRepository) : ICommandHandler<RemoveMultiplicationOperationResultCommand>
{
    public async Task<Result> Handle(RemoveMultiplicationOperationResultCommand request, CancellationToken cancellationToken)
    {
        var result = new Result();
        try
        {
            await multiplicationOperationRepository.RemoveMultiplicationResult(request.MultiplicationOperationResultId);
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