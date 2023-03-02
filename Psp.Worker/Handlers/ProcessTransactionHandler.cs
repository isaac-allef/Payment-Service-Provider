using Psp.Shared.Domain.Entities;
using Psp.Shared.Services.Db;
using Psp.Shared.Services.Db.Repositories;
using Psp.Worker.Business;

namespace Psp.Worker.UseCases;

public sealed class ProcessTransactionHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly UnitOfWork _unitOfWork;

    public ProcessTransactionHandler(
        ICustomerRepository customerRepository, ITransactionRepository transactionRepository, UnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Payable> Process(Transaction transaction)
    {
        var customer = await _customerRepository.GetById(transaction.CustomerId);
        if (customer is null)
        {
            throw new ArgumentNullException(nameof(Customer));
        }

        if (transaction.Payment.CardExpirationDate is not null
            && transaction.Payment.CardExpirationDate.Value.IsExpired)
        {
            throw new ArgumentException("Card is expired");
        }

        var payable = new CreatePayable().Create(transaction);

        try
        {
            _unitOfWork.BeginTransaction();
            await _transactionRepository.Insert(transaction);
            await _transactionRepository.Attach(payable);
            _unitOfWork.Commit();
        }
        catch(Exception ex)
        {
            _unitOfWork.Rollback();
            throw ex;
        }
        finally
        {
            _unitOfWork.Dispose();
        }

        return payable;
    }
}
