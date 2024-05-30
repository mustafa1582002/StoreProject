﻿using Microsoft.EntityFrameworkCore;
using Store.Data.Entities;
using Store.Data.Entities.Context;
using Store.Repository.Interfaces;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Repository
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _context;
        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        => await _context.Set<TEntity>().AddAsync(entity);

        

        public void Delete(TEntity entity)
        => _context.Set<TEntity>().Remove(entity);

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        => await _context.Set<TEntity>().ToListAsync();

        public async Task<IReadOnlyList<TEntity>> GetAllWithSpecificationAsync(ISpecification<TEntity> specification)
        => await ApplySpecification(specification).ToListAsync();


        public async Task<TEntity> GetByIdAsync(TKey? id)
        => await _context.Set<TEntity>().FindAsync(id);

        public async Task<TEntity> GetWithSpecificationByIdAsync(ISpecification<TEntity> specification)
        => await ApplySpecification(specification).FirstOrDefaultAsync();

        public async Task<int> CountSpecificationAsync(ISpecification<TEntity> specification)
        => await ApplySpecification(specification).CountAsync();


        public void Update(TEntity entity)
        => _context.Set<TEntity>().Update(entity);
        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
            => SpecificationEvaluater<TEntity, TKey>.GetQuery(_context.Set<TEntity>(), specification);
    }
}
