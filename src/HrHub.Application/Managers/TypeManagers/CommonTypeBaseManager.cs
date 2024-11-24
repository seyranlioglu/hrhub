using AutoMapper;
using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.Domain;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace HrHub.Application.Managers.TypeManagers
{
    [LifeCycle(Abstraction.Enums.LifeCycleTypes.NotRegister)]
    public class CommonTypeBaseManager<TTypeEntity> : ManagerBase, ICommonTypeBaseManager<TTypeEntity> where TTypeEntity : class, IBaseTypeEntity, new()
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<TTypeEntity> repository;
        private readonly IMapper mapper;

        public CommonTypeBaseManager(IHttpContextAccessor httpContextAccessor,
                                     IHrUnitOfWork unitOfWork) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            repository = unitOfWork.CreateRepository<TTypeEntity>();
        }

        public async Task<TResult> GetByIdAsync<TResult>(long id) where TResult : class
        {
            var result = await repository
                .GetAsync
                (
                predicate: p => p.Id == id
                );

            return DynamicMapper<TResult, TTypeEntity>(result);
        }

        public async Task<TResult> GetByTitleAsync<TResult>(string title) where TResult : class
        {
            var result = await repository
                .GetAsync
                (
                predicate: p => p.Title == title,
                selector: s => DynamicMapper<TResult, TTypeEntity>(s)
                );
            return result;
        }

        public async Task<TResult> GetByCodeAsync<TResult>(string code) where TResult : class
        {
            var result = await repository
                .GetAsync
                (
                predicate: p => p.Code == code,
                selector: s => DynamicMapper<TResult, TTypeEntity>(s)
                );
            return result;
        }

        public async Task<long> GetIdByCodeAsync(string code)
        {
            var result = await repository
            .GetAsync
            (
            predicate: p => p.Code == code,
            selector: s => s.Id
            );
            return result;
        }

        public async Task<TResponse> AddAsync<TSource, TResponse>(TSource data) where TSource : class
        {
            var entity = DynamicMapper<TTypeEntity, TSource>(data);
            var result = await repository.AddAndReturnAsync(entity);
            return DynamicMapper<TResponse, TTypeEntity>(result);
        }

        public async Task<IEnumerable<TResponse>> GetList<TResponse>(Expression<Func<TTypeEntity, bool>> predicate = null) where TResponse : class
        {
            var result = await repository
                .GetListAsync
                (
                predicate: predicate,
                selector: s => DynamicMapper<TResponse, TTypeEntity>(s)
                );
            return result;
        }

        public async Task UpdateAsync<TData>(long id, TData data)
        {
            var oldEntity = await repository.GetAsync(w => w.Id == id);
            var newEntity = DynamicMapper<TTypeEntity, TData>(oldEntity, data);
            await repository.UpdateAsync(newEntity);
        }

        public async Task DeleteAsync(long id)
        {
            var oldEntity = await repository.GetAsync(w => w.Id == id);
            await repository.DeleteAsync(oldEntity);
        }

        private TTarget DynamicMapper<TTarget, TSource>(TSource data)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(typeof(TSource), typeof(TTarget));
            });
            IMapper dynamicMapper = new AutoMapper.Mapper(configuration);
            return dynamicMapper.Map<TTarget>(data);
        }

        private TTarget DynamicMapper<TTarget, TSource>(TTarget target, TSource data)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(typeof(TSource), typeof(TTarget));
            });
            IMapper dynamicMapper = new AutoMapper.Mapper(configuration);
            return dynamicMapper.Map(data, target);
        }

    }
}
