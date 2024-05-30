﻿using Store.Data.Entities;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Interfaces
{
    public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey? id);
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity> GetWithSpecificationByIdAsync(ISpecification<TEntity> specification);
        Task<IReadOnlyList<TEntity>> GetAllWithSpecificationAsync(ISpecification<TEntity> specification);
        Task<int> CountSpecificationAsync(ISpecification<TEntity> specification);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
