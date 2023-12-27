﻿using Logic.Entities;
using Logic.Utils;

namespace Logic.Repositories;

public class CustomerRepository : Repository<Customer>
{
    public CustomerRepository(UnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public IReadOnlyList<Customer> GetList()
    {
        return _unitOfWork
            .Query<Customer>()
            .ToList();
    }

    public Customer GetByEmail(string email)
    {
        return _unitOfWork
            .Query<Customer>()
            .SingleOrDefault(x => x.Email == email);
    }
}
