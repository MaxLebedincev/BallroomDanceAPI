﻿using BallroomDanceAPI.DAL.Interfaces;
using BallroomDanceAPI.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BallroomDanceAPI.DAL
{
    public class UnitOfWork : IRepositoryFactory, IUnitOfWork
    {
        private readonly DbContext _context;
        private bool _disposed = false;
        private Dictionary<Type, object>? _repositories;

        public UnitOfWork(DbContext context) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IBaseRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            if (hasCustomRepository)
            {
                var customRepo = _context.GetService<IBaseRepository<TEntity>>();
                if (customRepo != null)
                {
                    return customRepo;
                }
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new BaseRepository<TEntity>(_context);
            }

            return (IBaseRepository<TEntity>)_repositories[type];
        }
        public int ExecuteSqlCommand(string sql, params object[] parameters) => _context.Database.ExecuteSqlRaw(sql, parameters);
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_repositories != null)
                    {
                        _repositories.Clear();
                    }

                    _context.Dispose();
                }
            }

            _disposed = true;
        }

    }
}
