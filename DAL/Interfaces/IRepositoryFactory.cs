﻿namespace BallroomDanceAPI.DAL.Interfaces
{
    public interface IRepositoryFactory
    {
        IBaseRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;
    }
}
