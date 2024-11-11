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
using static Domain.Models.EnumStatus;

namespace Infrastructure.Repositories
{
    public sealed class UserTestRepository : Repository<UserTest>, IUserTestRepository
    {
        private readonly AppDbContext _context;

        public UserTestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CheckIfUserTookTest(string userid, long trainingId, int testType)
        {
            TrainingTestType status = (TrainingTestType)Enum.ToObject(typeof(TrainingTestType), testType);
            var data = await _context.UserTests.Where(x => x.TrainingId == trainingId && x.UserId == userid && x.Submitted == true 
            && x.TrainingTest.TrainingTestType == status).FirstOrDefaultAsync();
            if (data != null)
            {
                return true;
            }
            return false;
        }
        public async Task<List<UserTestListDto>> ListUserTestByState(string state)
        {
            var userTests = await _context.UserTests
                .Include(x => x.User)
                .Include(x => x.Training)
                .Include(x => x.TrainingTest).ThenInclude(x => x.TrainingTestOptions)
                .Where(x => x.Training.State == state && x.Submitted)
                .ToListAsync();

            var groupedUserTests = userTests.GroupBy(x => x.UserId);

            var userTestListDtos = new List<UserTestListDto>();

            foreach (var group in groupedUserTests)
            {
                var userId = group.Key;
                var user = group.First().User;
                bool PreTestTaken = true;
                bool PostTestTaken = true;

                // Get the training ID based on the first test in the group (assuming all tests in the group have the same TrainingId)
                var trainingId = group.First().TrainingId;

                // Calculate pre-test score
                var preTests = group.Where(x => x.TrainingTest.TrainingTestType == TrainingTestType.PreTest).ToList();
                var preTestScore = "";
                var preTestClass = "danger"; // Default to 'danger'
                var preTestStatus = "Fail"; // Default to 'Fail'
                if (preTests.Count > 0)
                {
                    var correctPreAnswers = preTests.Count(x => x.Answer != null
                       && x.TrainingTest?.TrainingTestOptions != null
                       && x.TrainingTest.TrainingTestOptions.Any(opt => opt.CorrectAnswer == true && opt.Id == x.Answer));

                    double _preTestScore = (double)correctPreAnswers / preTests.Count * 100;
                    preTestScore = $"{_preTestScore:F0}%";

                    // Set class and status based on score
                    if (_preTestScore >= 70)
                    {
                        preTestClass = "success";
                        preTestStatus = "Pass";
                    }
                }
                else
                {
                    PreTestTaken = false;
                }

                // Calculate post-test score
                var postTests = group.Where(x => x.TrainingTest.TrainingTestType == TrainingTestType.PostTest).ToList();
                var postTestScore = "";
                var postTestClass = "danger"; // Default to 'danger'
                var postTestStatus = "Fail"; // Default to 'Fail'
                double postTestScoreValue = 0.00;
                if (postTests.Count > 0)
                {
                    var correctPostAnswers = postTests.Count(x => x.Answer != null
                       && x.TrainingTest?.TrainingTestOptions != null
                       && x.TrainingTest.TrainingTestOptions.Any(opt => opt.CorrectAnswer == true && opt.Id == x.Answer));

                    double _postTestScore = (double)correctPostAnswers / postTests.Count * 100;
                    postTestScore = $"{_postTestScore:F0}%";
                    postTestScoreValue = _postTestScore;
                    // Set class and status based on score
                    if (_postTestScore >= 70)
                    {
                        postTestClass = "success";
                        postTestStatus = "Pass";
                    }
                }
                else
                {
                    PostTestTaken = false;
                }

                userTestListDtos.Add(new UserTestListDto
                {
                    UserId = userId,
                    User = user,
                    PreTestScore = preTestScore,
                    PostTestScore = postTestScore,
                    PostTestTaken = PostTestTaken,
                    PreTestTaken = PreTestTaken,
                    TrainingId = trainingId,
                    PostTestScoreValue = postTestScoreValue,
                    PreTestClass = preTestClass, // New column for pre-test class
                    PreTestStatus = preTestStatus, // New column for pre-test status
                    PostTestClass = postTestClass, // New column for post-test class
                    PostTestStatus = postTestStatus // New column for post-test status
                });
            }

            return userTestListDtos.OrderByDescending(x => x.PostTestScoreValue).ToList();
        }

        public async Task<List<UserTestListDto>> ListUserTestByTrainingId(long trainingId)
        {
            var userTests = await _context.UserTests
                .Include(x => x.User)
                .Include(x => x.TrainingTest).ThenInclude(x=>x.TrainingTestOptions)
                .Where(x => x.TrainingId == trainingId && x.Submitted)
                .ToListAsync();

            var groupedUserTests = userTests.GroupBy(x => x.UserId);

            var userTestListDtos = new List<UserTestListDto>();

            foreach (var group in groupedUserTests)
            {
                var userId = group.Key;
                var user = group.First().User;
                bool PreTestTaken = true;
                bool PostTestTaken = true;
                // Calculate pre-test score
                var preTests = group.Where(x => x.TrainingTest.TrainingTestType == TrainingTestType.PreTest).ToList();
                var preTestScore = "";
                var preTestClass = "danger"; // Default to 'danger'
                var preTestStatus = "Fail"; // Default to 'Fail'
                if (preTests.Count > 0)
                {
                    var correctPreAnswers = preTests.Count(x => x.Answer != null
                       && x.TrainingTest?.TrainingTestOptions != null
                       && x.TrainingTest.TrainingTestOptions.Any(opt => opt.CorrectAnswer == true && opt.Id == x.Answer));


                    //var correctPreAnswers = preTests.Count(x => x.Answer == x.TrainingTest.CorrectAnswer);
                    double _preTestScore = (double)correctPreAnswers / preTests.Count * 100;

                    preTestScore = $"{_preTestScore:F0}%";
                    // Set class and status based on score
                    if (_preTestScore >= 70)
                    {
                        preTestClass = "success";
                        preTestStatus = "Pass";
                    }
                }
                else
                {
                    PreTestTaken = false;
                }

                // Calculate post-test score
                var postTests = group.Where(x => x.TrainingTest.TrainingTestType == TrainingTestType.PostTest).ToList();
                var postTestScore = "";
                var postTestClass = "danger"; // Default to 'danger'
                var postTestStatus = "Fail"; // Default to 'Fail'
                double postTestScoreValue = 0.00;
                if (postTests.Count > 0)
                {
                    var correctPostAnswers = postTests.Count(x => x.Answer != null
                       && x.TrainingTest?.TrainingTestOptions != null
                       && x.TrainingTest.TrainingTestOptions.Any(opt => opt.CorrectAnswer == true && opt.Id == x.Answer));
                    //var  = postTests.Count(x => x.Answer == x.TrainingTest.CorrectAnswer);
                   double _postTestScore = (double)correctPostAnswers / postTests.Count * 100;

                     postTestScore = $"{_postTestScore:F0}%";
                    postTestScoreValue = _postTestScore;
                    // Set class and status based on score
                    if (_postTestScore >= 70)
                    {
                        postTestClass = "success";
                        postTestStatus = "Pass";
                    }
                }
                else
                {
                    PostTestTaken = false;
                }

                userTestListDtos.Add(new UserTestListDto
                {
                    UserId = userId,
                    User = user,
                    PreTestScore = preTestScore,
                    PostTestScore = postTestScore,
                    PostTestTaken = PostTestTaken,
                    PreTestTaken = PreTestTaken,
                    PostTestScoreValue = postTestScoreValue,
                    TrainingId = trainingId,
                    PreTestClass = preTestClass, // New column for pre-test class
                    PreTestStatus = preTestStatus, // New column for pre-test status
                    PostTestClass = postTestClass, // New column for post-test class
                    PostTestStatus = postTestStatus // New column for post-test status
                });
            }

            return userTestListDtos.OrderByDescending(x=>x.PostTestScoreValue).ToList();
        }


        public async Task<UserTestResultDto> UserTestResult(long trainingId, string userId, int testType)
        {

            TrainingTestType status = (TrainingTestType)Enum.ToObject(typeof(TrainingTestType), testType);
            UserTestResultDto result = new UserTestResultDto();
             var userresult = await _context.UserTests
                .Include(x=>x.TrainingTest).ThenInclude(x=>x.TrainingTestOptions)
                .Where(x=>x.TrainingId == trainingId&& x.UserId == userId && x.TrainingTest.TrainingTestType == status).ToListAsync();

            result.UserTest = userresult;

            if (userresult.Count > 0)
            {
                int totalQuestions = userresult.Count;
                int correctAnswers = userresult.Count(x => x.Answer != null
                       && x.TrainingTest?.TrainingTestOptions != null
                       && x.TrainingTest.TrainingTestOptions.Any(opt => opt.CorrectAnswer == true && opt.Id == x.Answer));

                double percentage = (double)correctAnswers / totalQuestions * 100;
                string scorePercent = $"{percentage:F0}%";

                result.TotalQuestions = totalQuestions;
                result.PercentageResult = scorePercent;
            }

            

            return result;
        }

        public async Task UserTestSubmit(List<(long questionId, int answer, string userId, long trainingId)> assessmentData)
        {
            foreach (var (questionId, answer, userId, trainingId) in assessmentData)
            {
                UserTest nxtest = new UserTest();
                nxtest.UserId = userId;
                nxtest.TrainingId = trainingId;
                nxtest.Answer = answer;
                nxtest.TrainingTestId = questionId;
                nxtest.Submitted = true;
                await _context.UserTests.AddAsync(nxtest);
            }


            await _context.SaveChangesAsync();

        }
    }
}
