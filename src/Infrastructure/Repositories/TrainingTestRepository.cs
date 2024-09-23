using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Context;
using Infrastructure.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{

    public sealed class TrainingTestRepository : Repository<TrainingTest>, ITrainingTestRepository
    {
        private readonly AppDbContext _context;

        public TrainingTestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<TrainingTest>> GetAllTrainingTest()
        {
            var list = await _context.TrainingTests.Include(x=>x.TrainingTestOptions).Where(x=>x.Publish == true).OrderBy(x=>x.SortOrder).ToListAsync();
            return list;
        }
    }
}
