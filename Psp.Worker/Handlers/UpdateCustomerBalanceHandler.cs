using Psp.Shared.Domain.Entities;
using Psp.Shared.Services.Db;
using Psp.Shared.Services.Db.Repositories;
using Psp.Worker.Business;

namespace Psp.Worker.UseCases;

public class UpdateCustomerBalanceHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly UnitOfWork _unitOfWork;

    public UpdateCustomerBalanceHandler(
        ICustomerRepository customerRepository, UnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Update(Payable payable)
    {
        const int MAX_TRIES = 3;
        var count = 0;
        var wasUpdated = false;
        do
        {
            var customer = await _customerRepository.GetById(payable.CustomerId);
            if (customer is null)
            {
                throw new ArgumentException();
            }

            new UpdateCustomer(customer).UpdateBalance(payable);
            wasUpdated = await _customerRepository.UpdateBalanceIfUpToDate(customer);
        }
        while(++count < MAX_TRIES && !wasUpdated);

        if (!wasUpdated)
        {
            throw new Exception("race condition error: update customer");
        }
    }
}
