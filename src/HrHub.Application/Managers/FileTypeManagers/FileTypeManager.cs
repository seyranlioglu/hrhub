using AutoMapper;
using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.FileTypeDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;

namespace HrHub.Application.Managers.FileTypeManagers
{

    public class FileTypeManager : ManagerBase, IFileTypeManager
    {
        private readonly IHrUnitOfWork unitOfWork;
        private readonly Repository<FileType> fileTypeRepository;
        private readonly IMapper mapper;

        public FileTypeManager(IHttpContextAccessor httpContextAccessor,
                           IHrUnitOfWork unitOfWork,
                           IMapper mapper) : base(httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.fileTypeRepository = unitOfWork.CreateRepository<FileType>();
            this.mapper = mapper;

        }

        public async Task<Response<GetFileTypeDto>> GetByIdFileTypeAsync(string fileExtension)
        {
            string code = fileExtension switch
            {
                ".mp4" => FileTypeConst.Video,
                ".pdf" => FileTypeConst.Document,
                ".jpg" or ".png" => FileTypeConst.Image,          
            };

            var fileType = await fileTypeRepository.GetAsync(
                predicate: p => p.Code == code,
                 selector: s => mapper.Map<GetFileTypeDto>(s)
            );

            return ProduceSuccessResponse(fileType);
        }
    }
}
