﻿using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BallroomDanceAPI.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;
        int SaveChanges();
        int ExecuteSqlCommand(string sql, params object[] parameters);

    }
}
