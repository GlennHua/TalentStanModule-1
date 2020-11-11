using Talent.Common.Contracts;
using Talent.Common.Models;
using Talent.Services.Profile.Domain.Contracts;
using Talent.Services.Profile.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Talent.Services.Profile.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Talent.Common.Security;
using Talent.Common.Aws;

namespace Talent.Services.Profile.Domain.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserAppContext _userAppContext;
        IRepository<UserLanguage> _userLanguageRepository;
        IRepository<User> _userRepository;
        IRepository<Employer> _employerRepository;
        IRepository<Job> _jobRepository;
        IRepository<Recruiter> _recruiterRepository;
        IFileService _fileService;
        IAwsService awsService;


        public ProfileService(IUserAppContext userAppContext,
                              IRepository<UserLanguage> userLanguageRepository,
                              IRepository<User> userRepository,
                              IRepository<Employer> employerRepository,
                              IRepository<Job> jobRepository,
                              IRepository<Recruiter> recruiterRepository,
                              IFileService fileService,
                              IAwsService awsService)
        {
            _userAppContext = userAppContext;
            _userLanguageRepository = userLanguageRepository;
            _userRepository = userRepository;
            _employerRepository = employerRepository;
            _jobRepository = jobRepository;
            _recruiterRepository = recruiterRepository;
            _fileService = fileService;
        }



        public bool AddNewLanguage(AddLanguageViewModel language)
        {
            //Your code here;
            throw new NotImplementedException();
        }

         



        //public async task<talentprofileviewmodel> gettalentprofile(string id)
        //{
        //    //your code here;
        //    throw new notimplementedexception();
        //}

        public async Task<TalentProfileViewModel> GetTalentProfile(string Id)
        {
            //User profile = null;

            User profile = (await _userRepository.GetByIdAsync(Id));

            //await awsService.GetAllObjectFromS3("talent-standard-glenn");

            if (profile != null)
            {

                var languages = profile.Languages.Select(x => ViewModelFromLanguage(x)).ToList();
                var skills = profile.Skills.Select(x => ViewModelFromSkill(x)).ToList();

                string currentUrl = "";

                //var a = profile.Skills.ToList();
                if (!string.IsNullOrWhiteSpace(profile.ProfilePhoto))
                {
                    currentUrl = await _fileService.GetFileURL(profile.ProfilePhoto, FileType.ProfilePhoto);
                }
                else
                {
                    currentUrl = "";
                }
                

                var result = new TalentProfileViewModel
                {

                    Id = profile.Id,

                    FirstName = profile.FirstName,
                    MiddleName = profile.MiddleName,
                    LastName = profile.LastName,

                    LinkedAccounts = profile.LinkedAccounts,
                    Phone = profile.Phone,
                    Email = profile.Email,
                    Description = profile.Description,
                    Summary = profile.Summary,
                    Address = profile.Address,
                    Nationality = profile.Nationality,
                    Languages = languages,
                    Skills = skills,
                    VisaStatus = profile.VisaStatus,
                    VisaExpiryDate = profile.VisaExpiryDate,
                    JobSeekingStatus = profile.JobSeekingStatus,
                    ProfilePhoto = profile.ProfilePhoto,

                    //ProfilePhotoUrl = profile.ProfilePhotoUrl,
                    ProfilePhotoUrl = currentUrl



                    
                };
                return result;
            }

            return null;
        }

        protected AddLanguageViewModel ViewModelFromLanguage(UserLanguage language)
        {
            return new AddLanguageViewModel
            {

                Id = language.Id,
                Level = language.LanguageLevel,
                Name = language.Language,
                CurrentUserId = language.UserId

            };
        }


        protected void UpdateLanguageFromView(AddLanguageViewModel model, UserLanguage original)
        {
            original.Language = model.Name;
            original.LanguageLevel = model.Level;
        }


        //protected AddSkillViewModel ViewModelFromSkill(UserSkill skill)
        //{
        //    return new AddSkillViewModel
        //    {

        //        Id = skill.Id,
        //        Level = skill.ExperienceLevel,
        //        Name = skill.Skill

        //    };
        //}



//        protected void UpdateSkillFromView(AddSkillViewModel model, UserSkill original)
//        {
//            original.ExperienceLevel = model.Level;
//            original.Skill = model.Name;
//        }



//        #region Build Views from Model

//        protected AddSkillViewModel ViewModelFromSkill(UserSkill skill)
//        {
//            return new AddSkillViewModel
//            {
//                Id = skill.Id,
//                Level = skill.ExperienceLevel,
//                Name = skill.Skill
//            };
//        }





        public async Task<bool> UpdateTalentProfile(TalentProfileViewModel talent, string updaterId)
        {
            try
            {

                if (talent.Id != null)
                {
                    User currentTalent = (await _userRepository.GetByIdAsync(talent.Id));

                    //To be continued
                    currentTalent.FirstName = talent.FirstName;
                    currentTalent.LastName = talent.LastName;

                    currentTalent.LinkedAccounts = talent.LinkedAccounts;
                    currentTalent.Phone = talent.Phone;
                    currentTalent.Email = talent.Email;
                    currentTalent.Description = talent.Description;
                    currentTalent.Summary = talent.Summary;
                    currentTalent.Address = talent.Address;
                    currentTalent.Nationality = talent.Nationality;
                    currentTalent.VisaStatus = talent.VisaStatus;
                    currentTalent.VisaExpiryDate = talent.VisaExpiryDate;
                    currentTalent.JobSeekingStatus = talent.JobSeekingStatus;

                    var newLanguage = new List<UserLanguage>();

                    foreach (var item in talent.Languages)
                    {

                        var Currentlanguage = currentTalent.Languages.SingleOrDefault(x => x.Id == item.Id);

                        if (Currentlanguage == null)
                        {
                            Currentlanguage = new UserLanguage
                            {
                                Id = ObjectId.GenerateNewId().ToString(),
                                IsDeleted = false
                            };
                        }

                        UpdateLanguageFromView(item, Currentlanguage);
                        newLanguage.Add(Currentlanguage);
                    }
                    currentTalent.Languages = newLanguage;


                    await _userRepository.Update(currentTalent);
                    return true;
                }
                return false;
            }

            catch (MongoException)
            {
                return false;
            }
        }


        public async Task<bool> DeleteLanguage(string userId, string languageId)
        {
            try
            {

                if (userId != null)
                {
                    User currentTalent = (await _userRepository.GetByIdAsync(userId));
                    
                    var whereRemove = currentTalent.Languages.First(x => x.Id == languageId);

                    currentTalent.Languages.Remove(whereRemove);

                    await _userRepository.Update(currentTalent);
                    return true;
                }
                return false;
            }

            catch (MongoException)
            {
                return false;
            }
        }





        
        //This piece of code is working fine, but set isDeleted to true will not delete target row
        public async Task<bool> EditLanguage(string userId, AddLanguageViewModel language)
        {
            try
            {

                if (userId != null)
                {
                    User currentTalent = (await _userRepository.GetByIdAsync(userId));

                    //var newlanguage = new list<userlanguage>();


                    currentTalent.Languages.SingleOrDefault(x => x.Id == language.Id).Language = language.Name;
                    currentTalent.Languages.SingleOrDefault(x => x.Id == language.Id).LanguageLevel = language.Level;

                    //updatelanguagefromview(item, targetlanguage);
                    //newlanguage.add(targetlanguage);

                    //currenttalent.languages = newlanguage;


                    await _userRepository.Update(currentTalent);
                    
                    return true;
                }
                return false;
            }

            catch (MongoException)
            {
                return false;
            }
        }


        //User Skill
        public async Task<bool> AddSkill(string talentId, UserSkill newSkill)
        {

            try
            {
                if (talentId != null)
                {
                    User currentTalent = (await _userRepository.GetByIdAsync(talentId));

                    currentTalent.Skills.Add(newSkill);

                    await _userRepository.Update(currentTalent);
                }
                return false;
            }
            catch (MongoException)
            {
                return false;
            }
            
        }


        public async Task<bool> EditSkill(string userId, UserSkill EditSkill)
        {

            try
            {

                if (userId != null && EditSkill.Id != null)
                {
                    User currentUser = (await _userRepository.GetByIdAsync(userId));

                    currentUser.Skills.SingleOrDefault(x => x.Id == EditSkill.Id).Skill = EditSkill.Skill;
                    currentUser.Skills.SingleOrDefault(x => x.Id == EditSkill.Id).ExperienceLevel = EditSkill.ExperienceLevel;

                    await _userRepository.Update(currentUser);

                    return true;

                }
                return false;
            }
            catch (MongoException)
            {
                return false;
            }

        }


        public async Task<bool> DeleteSkill(string userId, string skillId)
        {

            try
            {

                if (userId != null && skillId != null)
                {

                    User profile = await _userRepository.GetByIdAsync(userId);

                    UserSkill targetRow = profile.Skills.Single(x => x.Id == skillId);

                    profile.Skills.Remove(targetRow);

                    await _userRepository.Update(profile);

                    return true;

                }
                return false;

            }
            catch (MongoException)
            {
                return false;
            }

        }






        //User Experience
        public async Task<List<UserExperience>> GetUserExperience(string userId)
        {
            try
            {

                User profile = await _userRepository.GetByIdAsync(userId);

                if (profile != null)
                {

                    var currentExperience = profile.Experience;
                    return currentExperience;

                }
                return null;

            }
            catch (MongoException)
            {
                return null;
            }
        }


        
        public async Task<bool> AddUserExperience(string userId, UserExperience userExperience)
        {
            try
            {
                if (userId != null && userExperience != null)
                {
                    User profile = await _userRepository.GetByIdAsync(userId);
                    profile.Experience.Add(userExperience);
                    await _userRepository.Update(profile);
                    return true;

                }
                return false;
            }
            catch (MongoException)
            {
                return false;
            }
            
        }


        public async Task<bool> DeleteExperience(string userId, string experienceId)
        {

            try
            {
                if (userId != null)
                {
                    User profile = await _userRepository.GetByIdAsync(userId);

                    UserExperience whereToDelete = profile.Experience.SingleOrDefault(x => x.Id == experienceId);

                    profile.Experience.Remove(whereToDelete);

                    await _userRepository.Update(profile);

                    return true;

                }
                return false;
            }
            catch (MongoException)
            {
                return false;
            }
        
        }


        public async Task<bool> UpdateExperience(string userId, UserExperience newExperience)
        {

            try
            {

                if (userId != null && newExperience != null)
                {

                    User profile = await _userRepository.GetByIdAsync(userId);

                    //List<UserExperience> expList = profile.Experience;

                    //UserExperience targetExp = expList.SingleOrDefault(x => x.Id == newExperience.Id);

                    profile.Experience.SingleOrDefault(x => x.Id == newExperience.Id).Company = newExperience.Company;
                    profile.Experience.SingleOrDefault(x => x.Id == newExperience.Id).Position = newExperience.Position;
                    profile.Experience.SingleOrDefault(x => x.Id == newExperience.Id).Responsibilities = newExperience.Responsibilities;
                    profile.Experience.SingleOrDefault(x => x.Id == newExperience.Id).Start = newExperience.Start;
                    profile.Experience.SingleOrDefault(x => x.Id == newExperience.Id).End = newExperience.End;

                    await _userRepository.Update(profile);

                    return true;

                }
                return false;

            }
            catch (MongoException)
            {

                return false;
            }
        
        }





        public async Task<EmployerProfileViewModel> GetEmployerProfile(string Id, string role)
        {

            Employer profile = null;
            switch (role)
            {
                case "employer":
                    profile = (await _employerRepository.GetByIdAsync(Id));
                    break;
                case "recruiter":
                    profile = (await _recruiterRepository.GetByIdAsync(Id));
                    break;
            }

            var videoUrl = "";

            if (profile != null)
            {
                videoUrl = string.IsNullOrWhiteSpace(profile.VideoName)
                          ? ""
                          : await _fileService.GetFileURL(profile.VideoName, FileType.UserVideo);

                var skills = profile.Skills.Select(x => ViewModelFromSkill(x)).ToList();

                var result = new EmployerProfileViewModel
                {
                    Id = profile.Id,
                    CompanyContact = profile.CompanyContact,
                    PrimaryContact = profile.PrimaryContact,
                    Skills = skills,
                    ProfilePhoto = profile.ProfilePhoto,
                    ProfilePhotoUrl = profile.ProfilePhotoUrl,
                    VideoName = profile.VideoName,
                    VideoUrl = videoUrl,
                    DisplayProfile = profile.DisplayProfile,
                };
                return result;
            }

            return null;
        }

        public async Task<bool> UpdateEmployerProfile(EmployerProfileViewModel employer, string updaterId, string role)
        {
            try
            {
                if (employer.Id != null)
                {
                    switch (role)
                    {
                        case "employer":
                            Employer existingEmployer = (await _employerRepository.GetByIdAsync(employer.Id));
                            existingEmployer.CompanyContact = employer.CompanyContact;
                            existingEmployer.PrimaryContact = employer.PrimaryContact;
                            existingEmployer.ProfilePhoto = employer.ProfilePhoto;
                            existingEmployer.ProfilePhotoUrl = employer.ProfilePhotoUrl;
                            existingEmployer.DisplayProfile = employer.DisplayProfile;
                            existingEmployer.UpdatedBy = updaterId;
                            existingEmployer.UpdatedOn = DateTime.Now;

                            var newSkills = new List<UserSkill>();
                            foreach (var item in employer.Skills)
                            {
                                var skill = existingEmployer.Skills.SingleOrDefault(x => x.Id == item.Id);
                                if (skill == null)
                                {
                                    skill = new UserSkill
                                    {
                                        Id = ObjectId.GenerateNewId().ToString(),
                                        IsDeleted = false
                                    };
                                }
                                UpdateSkillFromView(item, skill);
                                newSkills.Add(skill);
                            }
                            existingEmployer.Skills = newSkills;

                            await _employerRepository.Update(existingEmployer);
                            break;

                        case "recruiter":
                            Recruiter existingRecruiter = (await _recruiterRepository.GetByIdAsync(employer.Id));
                            existingRecruiter.CompanyContact = employer.CompanyContact;
                            existingRecruiter.PrimaryContact = employer.PrimaryContact;
                            existingRecruiter.ProfilePhoto = employer.ProfilePhoto;
                            existingRecruiter.ProfilePhotoUrl = employer.ProfilePhotoUrl;
                            existingRecruiter.DisplayProfile = employer.DisplayProfile;
                            existingRecruiter.UpdatedBy = updaterId;
                            existingRecruiter.UpdatedOn = DateTime.Now;

                            var newRSkills = new List<UserSkill>();
                            foreach (var item in employer.Skills)
                            {
                                var skill = existingRecruiter.Skills.SingleOrDefault(x => x.Id == item.Id);
                                if (skill == null)
                                {
                                    skill = new UserSkill
                                    {
                                        Id = ObjectId.GenerateNewId().ToString(),
                                        IsDeleted = false
                                    };
                                }
                                UpdateSkillFromView(item, skill);
                                newRSkills.Add(skill);
                            }
                            existingRecruiter.Skills = newRSkills;
                            await _recruiterRepository.Update(existingRecruiter);

                            break;
                    }
                    return true;
                }
                return false;
            }
            catch (MongoException e)
            {
                return false;
            }
        } 

        public async Task<bool> UpdateEmployerPhoto(string employerId, IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            List<string> acceptedExtensions = new List<string> { ".jpg", ".png", ".gif", ".jpeg" };

            if (fileExtension != null && !acceptedExtensions.Contains(fileExtension.ToLower()))
            {
                return false;
            }

            var profile = (await _employerRepository.Get(x => x.Id == employerId)).SingleOrDefault();

            if (profile == null)
            {
                return false;
            }

            var newFileName = await _fileService.SaveFile(file, FileType.ProfilePhoto);

            if (!string.IsNullOrWhiteSpace(newFileName))
            {
                var oldFileName = profile.ProfilePhoto;

                if (!string.IsNullOrWhiteSpace(oldFileName))
                {
                    await _fileService.DeleteFile(oldFileName, FileType.ProfilePhoto);
                }

                profile.ProfilePhoto = newFileName;
                profile.ProfilePhotoUrl = await _fileService.GetFileURL(newFileName, FileType.ProfilePhoto);

                await _employerRepository.Update(profile);
                return true;
            }

            return false;

        }

        public async Task<bool> AddEmployerVideo(string employerId, IFormFile file)
        {
            //Your code here;
            throw new NotImplementedException();
        }


        //Last Part
        public async Task<bool> UpdateTalentPhoto(string talentId, IFormFile file)
        {
            try
            {
                var fileExtension = Path.GetExtension(file.FileName);
                List<string> acceptedExtensions = new List<string> { ".jpg", ".png", ".gif", ".jpeg" };

                if (fileExtension != null && !acceptedExtensions.Contains(fileExtension.ToLower()))
                {
                    return false;
                }



                var profile = await _userRepository.GetByIdAsync(talentId);

                if (profile == null)
                {
                    return false;
                }


              var newFileName = await _fileService.SaveFile(file, FileType.ProfilePhoto);
                

                if (!string.IsNullOrWhiteSpace(newFileName))
                {

                    var oldFileName = profile.ProfilePhoto;

                    if (!string.IsNullOrWhiteSpace(oldFileName))
                    {
                        await _fileService.DeleteFile(oldFileName, FileType.ProfilePhoto);
                    }

                    profile.ProfilePhoto = newFileName;

                    profile.ProfilePhotoUrl = await _fileService.GetFileURL(newFileName, FileType.ProfilePhoto);


                    await _userRepository.Update(profile);
                    return true;

                }
                return false;

            }
            catch (MongoException)
            {

                return false;
            }

        }



        //public async Task<List<User>> GetTalentList()
        //{

        //    try
        //    {
        //        return null;
        //    }
        //    catch (MongoException)
        //    {
        //        return null;
        //    }
        
        //}



        public async Task<bool> AddTalentVideo(string talentId, IFormFile file)
        {
            //Your code here;
            throw new NotImplementedException();

        }

        public async Task<bool> RemoveTalentVideo(string talentId, string videoName)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateTalentCV(string talentId, IFormFile file)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetTalentSuggestionIds(string employerOrJobId, bool forJob, int position, int increment)
        {
            //Your code here;
            throw new NotImplementedException();
        }



        //Module 2----------------------------------------------------------------------------------------------------------------------

        //Original
        //public async Task<List<User>> GetTalentSnapshotList(string employerOrJobId, bool forJob, int position, int increment)
        //{

        //    throw new NotImplementedException();
        //}


        //After Edited----------------------------------------------------------------------------------------------------------------------
        public async Task<IEnumerable<TalentSnapshotViewModel>> GetTalentSnapshotList(string employerId, bool forJob, int position, int increment)
        {

           
                //_userRepository.Collection;
                if (employerId != null)
                {
                  
                        var list = _userRepository.GetQueryable().Skip(increment*(position-1)).Take(increment);
          
                        var result = new List<TalentSnapshotViewModel>();

                        foreach (var user in list)
                        {

                            var newUser = new TalentSnapshotViewModel();
                            newUser.Id = user.Id;
                            newUser.Name = user.LastName;
                            newUser.Summary = user.Summary;

                            result.Add(newUser);

                        }

                        return result;

                }
                return null;
          
            

            //throw new NotImplementedException();
        }





        public async Task<IEnumerable<TalentSnapshotViewModel>> GetTalentSnapshotList(IEnumerable<string> ids)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        #region TalentMatching

        public async Task<IEnumerable<TalentSuggestionViewModel>> GetFullTalentList()
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public IEnumerable<TalentMatchingEmployerViewModel> GetEmployerList()
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TalentMatchingEmployerViewModel>> GetEmployerListByFilterAsync(SearchCompanyModel model)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TalentSuggestionViewModel>> GetTalentListByFilterAsync(SearchTalentModel model)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TalentSuggestion>> GetSuggestionList(string employerOrJobId, bool forJob, string recruiterId)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<bool> AddTalentSuggestions(AddTalentSuggestionList selectedTalents)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        #endregion

        #region Conversion Methods

        #region Update from View

        protected void UpdateSkillFromView(AddSkillViewModel model, UserSkill original)
        {
            original.ExperienceLevel = model.Level;
            original.Skill = model.Name;
        }

        #endregion

        #region Build Views from Model

        protected AddSkillViewModel ViewModelFromSkill(UserSkill skill)
        {
            return new AddSkillViewModel
            {
                Id = skill.Id,
                Level = skill.ExperienceLevel,
                Name = skill.Skill
            };
        }

        #endregion

        #endregion

        #region ManageClients

        public async Task<IEnumerable<ClientViewModel>> GetClientListAsync(string recruiterId)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<ClientViewModel> ConvertToClientsViewAsync(Client client, string recruiterId)
        {
            //Your code here;
            throw new NotImplementedException();
        }
         
        public async Task<int> GetTotalTalentsForClient(string clientId, string recruiterId)
        {
            //Your code here;
            throw new NotImplementedException();

        }

        public async Task<Employer> GetEmployer(string employerId)
        {
            return await _employerRepository.GetByIdAsync(employerId);
        }

        public Task<bool> DeleteLanguage(AddLanguageViewModel language, string languageId)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
