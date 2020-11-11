using Talent.Services.Profile.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talent.Services.Profile.Models;
using Talent.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Talent.Services.Profile.Domain.Contracts
{
    public interface IProfileService
    {
        bool AddNewLanguage(AddLanguageViewModel language);


        //Here

        //To be continued....................................................
        Task<bool> DeleteLanguage(string userId, string languageId);
        Task<bool> EditLanguage(string userId, AddLanguageViewModel language);
        Task<bool> AddSkill(string talentId, UserSkill newSkill);
        Task<bool> EditSkill(string userId, UserSkill EditSkill);
        Task<bool> DeleteSkill(string userId, string skillId);
        Task<List<UserExperience>> GetUserExperience(string userId);
        Task<bool> AddUserExperience(string userId, UserExperience userExperience);
        Task<bool> DeleteExperience(string userId, string experienceId);
        Task<bool> UpdateExperience(string userId, UserExperience newExperience);
        Task<bool> UpdateTalentPhoto(string talentId, IFormFile file);


        //Module 2

        //Original
        Task<IEnumerable<TalentSnapshotViewModel>> GetTalentSnapshotList(string employerOrJobId, bool forJob, int position, int increment);
        //Task<List<TalentSnapshotViewModel>> GetTalentSnapshotList(string employerOrJobId, bool forJob, int position, int increment);

        //Edited
        //Task<List<User>> GetTalentTesting(string employerId, bool forJob, int position, int increment);


        Task<TalentProfileViewModel> GetTalentProfile(String id);
        Task<IEnumerable<string>> GetTalentSuggestionIds(string employerOrJobId, bool forJob, int position, int increment);
        Task<IEnumerable<TalentSnapshotViewModel>> GetTalentSnapshotList(IEnumerable<string> ids);

        Task<bool> UpdateTalentProfile(TalentProfileViewModel profile, String updaterId);
       

        Task<bool> AddTalentVideo(string talentId, IFormFile file);
        Task<bool> RemoveTalentVideo(string talentId, string videoName);

        Task<EmployerProfileViewModel> GetEmployerProfile(String id, string role);

        Task<bool> UpdateEmployerProfile(EmployerProfileViewModel profile, String updaterId, string role);
        Task<bool> UpdateEmployerPhoto(string employerId, IFormFile file);

        Task<bool> AddEmployerVideo(string employerId, IFormFile file);
        Task<bool> AddTalentSuggestions(AddTalentSuggestionList selectedTalents);

        Task<bool> UpdateTalentCV(string talentId, IFormFile file);

        Task<IEnumerable<TalentSuggestionViewModel>> GetFullTalentList();
        IEnumerable<TalentMatchingEmployerViewModel> GetEmployerList();
        Task<IEnumerable<TalentMatchingEmployerViewModel>> GetEmployerListByFilterAsync(SearchCompanyModel model);
        Task<IEnumerable<TalentSuggestion>> GetSuggestionList(string employerOrJobId, bool forJob, string recruiterId);
        Task<IEnumerable<TalentSuggestionViewModel>> GetTalentListByFilterAsync(SearchTalentModel model);

        Task<IEnumerable<ClientViewModel>> GetClientListAsync(string recruiterId);
        Task<Employer> GetEmployer(string employerId);
    }
}
