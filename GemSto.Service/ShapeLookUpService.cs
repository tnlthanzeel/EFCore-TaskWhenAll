using AutoMapper;
using GemSto.Common.Constants;
using GemSto.Data;
using GemSto.Domain.LookUp;
using GemSto.Service.Contracts;
using GemSto.Service.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service
{
    public class ShapeLookUpService : IShapeLookUpService
    {
        private readonly GemStoContext gemStoContext;
        private readonly IMapper mapper;
        private readonly IHostingEnvironment hostingEnvironment;

        public ShapeLookUpService(GemStoContext gemStoContext, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            this.gemStoContext = gemStoContext;
            this.mapper = mapper;
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<bool> AddNewShapeAsync(ShapeCreateModel shapeCreateModel)
        {
            using (gemStoContext)
            {
                var isAny = await gemStoContext.Shapes.AnyAsync(x => x.Value == shapeCreateModel.Value && !x.IsDeleted);
                if (isAny)
                {
                    return false;
                }

                else
                {
                    if (shapeCreateModel.ImagePath != null)
                        shapeCreateModel.ImagePath = FolderPath.ImagePath + shapeCreateModel.ImagePath;

                    var entity = mapper.Map<Shape>(shapeCreateModel);
                    await gemStoContext.Shapes.AddAsync(entity);
                    await gemStoContext.SaveChangesAsync();

                    return true;
                }
            }
        }

        public async Task DeleteShapeAsync(int id)
        {
            await gemStoContext.Database.ExecuteSqlCommandAsync($"UPDATE [dbo].[Shapes] SET IsDeleted = 1 WHERE Id={id}");
        }

        public async Task<ShapeModel> GetShapeByIdAsync(int id)
        {
            var entity = await gemStoContext.Shapes.FirstOrDefaultAsync(f => f.Id == id);
            var model = mapper.Map<ShapeModel>(entity);
            return model;
        }

        public async Task<IEnumerable<ShapeModel>> GetShapeKeyValuePairAsync()
        {
            var query = gemStoContext.Shapes.Where(w => !w.IsDeleted).OrderBy(o => o.Value);
            var entity = await query.AsNoTracking().ToListAsync();
            var model = mapper.Map<List<ShapeModel>>(entity);
            return model;
        }

        public async Task<bool> UpdateShapeAsync(ShapeCreateModel shapeUpdateModel)
        {
            using (gemStoContext)
            {
                var isAny = await gemStoContext.Shapes.AnyAsync(x => x.Value == shapeUpdateModel.Value && !x.IsDeleted && x.Id != shapeUpdateModel.Id);
                if (isAny)
                {
                    return false;
                }

                else
                {
                    if (shapeUpdateModel.ImagePath != null && !shapeUpdateModel.ImagePath.StartsWith(FolderPath.ImagePath))
                        shapeUpdateModel.ImagePath = FolderPath.ImagePath + shapeUpdateModel.ImagePath;

                    var entity = mapper.Map<Shape>(shapeUpdateModel);
                    gemStoContext.Shapes.Update(entity);
                    await gemStoContext.SaveChangesAsync();

                    return true;
                }
            }
        }
    }
}
