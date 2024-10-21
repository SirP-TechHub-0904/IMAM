using Domain.DTOs;
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
    public sealed class TrainingCategoryRepository : Repository<TrainingCategory>, ITrainingCategoryRepository
    {
        private readonly AppDbContext _context;

        public TrainingCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IList<TrainingCategory>> GetAll()
        {
            return await _context.TrainingCategories
                .Include(x=>x.Training).ToListAsync();
        }

         
        public async Task<IList<ParticipantAndFacilitatorByCategoryDto>> GetParticipantFacilitatorByTrainingAll()
        {
            var result = await _context.TrainingCategories
                .Include(tc => tc.Training)
                .ThenInclude(t => t.TrainingParticipants)
                .Include(tc => tc.Training)
                .ThenInclude(t => t.TrainingFacilitators)
                .Select(tc => new ParticipantAndFacilitatorByCategoryDto
                {
                    Id = tc.Id,
                    Category  = tc.Title,
                    ParticipantList = tc.Training.SelectMany(t => t.TrainingParticipants).Count(),
                    FacilitatorList = tc.Training.SelectMany(t => t.TrainingFacilitators).Count(),
                    //Participants = tc.Training.SelectMany(t => t.TrainingParticipants)
                    //                .Select(p => new ParticipantDto
                    //                {
                    //                    UserId = p.UserId,
                    //                    UserName = p.User.UserName,  // Assuming User entity has a UserName
                    //                    Status = p.ParticipantTrainingStatus.ToString(),
                    //                    TrainingId = p.TrainingId,
                    //                    Reasons = p.Reasons
                    //                })
                    //                .ToList(),
                    //Facilitators = tc.Training.SelectMany(t => t.TrainingFacilitators)
                    //                .Select(f => new FacilitatorDto
                    //                {
                    //                    UserId = f.UserId,
                    //                    UserName = f.User.UserName,  // Assuming User entity has a UserName
                    //                    Status = f.FacilitatorTrainingStatus.ToString(),
                    //                    TrainingId = f.TrainingId,
                    //                    Position = f.Position,
                    //                    Reasons = f.Reasons
                    //                })
                    //                .ToList()
                }).ToListAsync();

            return result;
        }

    }
}
