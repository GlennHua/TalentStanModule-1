using Talent.Common.Contracts;
using Talent.Common.Models;
using Talent.Common.Security;
using Talent.Services.Profile.Models.Profile;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Driver;
using Talent.Services.Profile.Domain.Contracts;
using Talent.Common.Aws;
using Talent.Services.Profile.Models;
using Newtonsoft.Json.Converters;

namespace Talent.Services.Profile.Controllers
{
    [Route("profile/[controller]")]
    public class ProfileController : Controller
    {
        private readonly IBusClient _busClient;
        private readonly IAuthenticationService _authenticationService;
        private readonly IProfileService _profileService;
        private readonly IFileService _documentService;
        private readonly IUserAppContext _userAppContext;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserLanguage> _userLanguageRepository;
        private readonly IRepository<UserDescription> _personDescriptionRespository;
        private readonly IRepository<UserAvailability> _userAvailabilityRepository;
        private readonly IRepository<UserSkill> _userSkillRepository;
        private readonly IRepository<UserEducation> _userEducationRepository;
        private readonly IRepository<UserCertification> _userCertificationRepository;
        private readonly IRepository<UserLocation> _userLocationRepository;
        private readonly IRepository<Employer> _employerRepository;
        private readonly IRepository<UserDocument> _userDocumentRepository;
        private readonly IHostingEnvironment _environment;
        private readonly IRepository<Recruiter> _recruiterRepository;
        private readonly IAwsService _awsService;
        private readonly string _profileImageFolder;

        public ProfileController(IBusClient busClient,
            IProfileService profileService,
            IFileService documentService,
            IRepository<User> userRepository,
            IRepository<UserLanguage> userLanguageRepository,
            IRepository<UserDescription> personDescriptionRepository,
            IRepository<UserAvailability> userAvailabilityRepository,
            IRepository<UserSkill> userSkillRepository,
            IRepository<UserEducation> userEducationRepository,
            IRepository<UserCertification> userCertificationRepository,
            IRepository<UserLocation> userLocationRepository,
            IRepository<Employer> employerRepository,
            IRepository<UserDocument> userDocumentRepository,
            IRepository<Recruiter> recruiterRepository,
            IHostingEnvironment environment,
            IAwsService awsService,
            IUserAppContext userAppContext)
        {
            _busClient = busClient;
            _profileService = profileService;
            _documentService = documentService;
            _userAppContext = userAppContext;
            _userRepository = userRepository;
            _personDescriptionRespository = personDescriptionRepository;
            _userLanguageRepository = userLanguageRepository;
            _userAvailabilityRepository = userAvailabilityRepository;
            _userSkillRepository = userSkillRepository;
            _userEducationRepository = userEducationRepository;
            _userCertificationRepository = userCertificationRepository;
            _userLocationRepository = userLocationRepository;
            _employerRepository = employerRepository;
            _userDocumentRepository = userDocumentRepository;
            _recruiterRepository = recruiterRepository;
            _environment = environment;
            _profileImageFolder = "images\\";
            _awsService = awsService;
        }

        #region Talent

        [HttpGet("getProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = _userAppContext.CurrentUserId;
            var user = await _userRepository.GetByIdAsync(userId);
            return Json(new { Username = user.FirstName});
            //return Json(new { Username = user });
        }

        [HttpGet("getProfileById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> GetProfileById(string uid)
        {
            var userId = uid;
            var user = await _userRepository.GetByIdAsync(userId);
            return Json(new { userName = user.FirstName, createdOn = user.CreatedOn });
        }

        [HttpGet("isUserAuthenticated")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> IsUserAuthenticated()
        {
            if (_userAppContext.CurrentUserId == null)
            {
                return Json(new { IsAuthenticated = false });
            }
            else
            {
                var person = await _userRepository.GetByIdAsync(_userAppContext.CurrentUserId);
                if (person != null)
                {
                    return Json(new { IsAuthenticated = true, Username = person.FirstName, Type = "talent" });
                }
                var employer = await _employerRepository.GetByIdAsync(_userAppContext.CurrentUserId);
                if (employer != null)
                {
                    return Json(new { IsAuthenticated = true, Username = employer.CompanyContact.Name, Type = "employer" });
                }
                var recruiter = await _recruiterRepository.GetByIdAsync(_userAppContext.CurrentUserId);
                if (recruiter != null)
                {
                    return Json(new { IsAuthenticated = true, Username = recruiter.CompanyContact.Name, Type = "recruiter" });
                }
                return Json(new { IsAuthenticated = false, Type = "" });
            }
        }





        //Here for languages
        [HttpGet("GetLanguages")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> GetLanguages(String id = "")
        {
            try
            {
                string talentId = String.IsNullOrWhiteSpace(id) ? _userAppContext.CurrentUserId : id;

                var userLanguage = await _profileService.GetTalentProfile(talentId);

                return Json(new { Success = true, data = userLanguage.Languages });

            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e });
            }


            //throw new NotImplementedException();//delete this line
        }



        [HttpPost("addLanguage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public ActionResult AddLanguage([FromBody] AddLanguageViewModel language)
        {

            //try
            //{

            //    if (ModelState.IsValid)
            //    {

            //        if (_profileService.UpdateTalentProfile())
            //        { 
                        
            //        }
                    
            //    }
            //    return Json(new { Success = false });
            //}
            //catch (Exception e)
            //{
            //    return Json(new { Success = false, message = e });
            //}

    


            throw new NotImplementedException();
        }



        //To be continued....................................................
        [HttpPost("updateLanguage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<ActionResult> UpdateLanguage([FromBody] AddLanguageViewModel language)
        {
            string goodNews = "Update Successed";
            string userId = _userAppContext.CurrentUserId;

            try
            {

                if (await _profileService.EditLanguage(userId, language))
                {
                    return Json(new { Success = true, data = goodNews });
                }
                return Json(new { Success = false });

            }
            catch (Exception e)
            {
                return Json(new { Success = false, message = e.Message });
            }

        }

        [HttpPost("deleteLanguage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<ActionResult> DeleteLanguage([FromBody]string languageId)
        {
            string goodNews = "Delete Successed";

            string userId = _userAppContext.CurrentUserId;
            //languageId = "5f901a1cb565dd5754acad65";

            try
            {
                if (await _profileService.DeleteLanguage(userId, languageId))
                {
                    return Json(new { Success = true, data = goodNews });
                }
                return Json(new { Success = false });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, message = e.Message });
            }

        }






        [HttpGet("GetSkills")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> GetSkills(String id = "")
        {

            try
            {

                string talentId = String.IsNullOrWhiteSpace(id) ? _userAppContext.CurrentUserId : id;

                var profile = await _profileService.GetTalentProfile(talentId);

                return Json(new { Success = true, data = profile.Skills });

            }
            catch (Exception e)
            {
                return Json(new { Success = false, message = e.Message });
            }

        }



        //To be continued...........................................
        [HttpPost("AddSkill")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<ActionResult> AddSkill([FromBody]UserSkill skill)
        {

            try
            {
                string goodNews = "Skill Added successfully";

                string talentId = _userAppContext.CurrentUserId;

                skill.Id = ObjectId.GenerateNewId().ToString();
                skill.UserId = null;
                skill.IsDeleted = false;

                if (await _profileService.AddSkill(talentId, skill))
                {
                    return Json(new { Success = true, data = goodNews });
                }
                return Json(new { Success = false });

            }
            catch (Exception e)
            {
                return Json(new { Success = false, message = e.Message });
            }

        }


        [HttpPost("UpdateSkill")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> UpdateSkill([FromBody]UserSkill updatedSkill)
        {

            try
            {

                string goodNews = "Skill Updated Successfully";

                string userId = _userAppContext.CurrentUserId;

                updatedSkill.IsDeleted = false;
                updatedSkill.UserId = null;

                if (await _profileService.EditSkill(userId, updatedSkill))
                {

                    return Json(new { Success = true, data = goodNews });
                }

                return Json(new { Success = false });

            }
            catch (Exception e)
            {
                return Json(new { Success = false, message = e.Message });
            }

        }




        [HttpPost("deleteSkill")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> DeleteSkill([FromBody]string skillId)
        {
            string goodNews = "Delete Successed";
            string userId = _userAppContext.CurrentUserId;

            try
            {
                if (await _profileService.DeleteSkill(userId, skillId))
                {
                    return Json(new { Success = true, data = goodNews });
                }
                return Json(new { Success = false});

            }
            catch (Exception e)
            {
                return Json(new { Success = false, message = e.Message });
            }
            
        }



        [HttpGet("GetExperience")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> GetExperience(String id = "")
        {

            try
            {

                string userId = String.IsNullOrWhiteSpace(id) ? _userAppContext.CurrentUserId : id;

                var expList = await _profileService.GetUserExperience(userId);

                return Json(new { Success = true, data = expList });

            }
            catch (Exception e)
            {
                return Json(new { Success = false, message = e.Message });
            }

        }

         
        [HttpPost("AddExperience")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> AddExperience([FromBody]UserExperience experience)
        {
            
            try
            {
                string goodNews = "Experience Added successfully";
                string userId = _userAppContext.CurrentUserId;

                experience.Id = ObjectId.GenerateNewId().ToString();
                //experience.Position = "Dev";
                //experience.Responsibilities = "Coding";
                //experience.Company = "MVP";
                //experience.Start = DateTime.Parse("2016-03-22T15:00:00Z", null, System.Globalization.DateTimeStyles.RoundtripKind); ;
                //experience.End = DateTime.Parse("2018-08-20T15:00:00Z", null, System.Globalization.DateTimeStyles.RoundtripKind); ;

                experience.Start = Convert.ToDateTime(experience.Start);
                experience.End = Convert.ToDateTime(experience.End);


                if (await _profileService.AddUserExperience(userId, experience))
                {
                    return Json(new { Success = true, data = goodNews });
                }
                return Json(new { Success = false});

            }
            catch (Exception e)
            {
                return Json(new { Success = false, message = "Failed: "+e.Message });
            }

        }


        [HttpPost("DeleteExperience")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> DeleteExperience([FromBody] string experienceId, String userId = "")
        {

            try
            {
                string id = String.IsNullOrWhiteSpace(userId) ? _userAppContext.CurrentUserId : userId;

                if (await _profileService.DeleteExperience(id, experienceId))
                {

                    return Json(new { Success = true, message = "Experience Deleted" });
                
                }
                return Json(new { Success = false });


            }
            catch (Exception e)
            {
                return Json(new { Success = false, message = e.Message });
            }
        
        }



        [HttpPost("UpdateExperience")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> UpdateExperience([FromBody] UserExperience updatedExp, String userId = "")
        {
            string goodNews = "Updated Successfully";

            try
            {

                string id = String.IsNullOrWhiteSpace(userId) ? _userAppContext.CurrentUserId : userId;

                if (await _profileService.UpdateExperience(id, updatedExp))
                {

                    return Json(new { Success = true, message = goodNews });

                }
                return Json( new { Success = false});

            }
            catch (Exception e)
            {

                return Json(new { Success = false, message = e.Message });

            }
        
        }










        [HttpGet("getCertification")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> getCertification()
        {
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpPost("addCertification")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public ActionResult addCertification([FromBody] AddCertificationViewModel certificate)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpPost("updateCertification")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> UpdateCertification([FromBody] AddCertificationViewModel certificate)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpPost("deleteCertification")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> DeleteCertification([FromBody] AddCertificationViewModel certificate)
        {
            //Your code here;
            throw new NotImplementedException();
        }



        [HttpGet("getProfileImage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult getProfileImage(string Id)
        {
            var profileUrl = _documentService.GetFileURL(Id, FileType.ProfilePhoto);
            //Please do logic for no image available - maybe placeholder would be fine
            return Json(new { profilePath = profileUrl });
        }



        [HttpGet("getImage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult getImage()
        {
            var profileUrl = _awsService.GetPresignedUrlObject("637406461214323854_flag1.png", "talent-standard-glenn");
            //Please do logic for no image available - maybe placeholder would be fine
            return Json(new { profilePath = profileUrl });
        }


        [HttpPost("updateProfilePhoto")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<ActionResult> UpdateProfilePhoto([FromForm] IFormFile file)
        {
            string goodNews = "Updated Successfully";

            try
            {
                string userId = _userAppContext.CurrentUserId;

                if (await _profileService.UpdateTalentPhoto(userId, file))
                {
                    return Json(new { Success = true, data = goodNews });
                }
                return Json( new { Success = false});

            }
            catch (Exception e)
            {

                return Json(new { Success = false, message = e.Message});

            }
        }





        [HttpPost("updateTalentCV")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<ActionResult> UpdateTalentCV()
        {
            IFormFile file = Request.Form.Files[0];
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpPost("updateTalentVideo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> UpdateTalentVideo()
        {
            IFormFile file = Request.Form.Files[0];
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpGet("getInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> GetInfo()
        {
            //Your code here;
            throw new NotImplementedException();
        }


        [HttpPost("addInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> AddInfo([FromBody] DescriptionViewModel pValue)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpGet("getEducation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> GetEducation()
        {
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpPost("addEducation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public IActionResult AddEducation([FromBody]AddEducationViewModel model)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpPost("updateEducation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> UpdateEducation([FromBody]AddEducationViewModel model)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpPost("deleteEducation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> DeleteEducation([FromBody] AddEducationViewModel model)
        {
            //Your code here;
            throw new NotImplementedException();
        }

     
        #endregion

        #region EmployerOrRecruiter

        [HttpGet("getEmployerProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public async Task<IActionResult> GetEmployerProfile(String id = "", String role = "")
        {
            try
            {
                string userId = String.IsNullOrWhiteSpace(id) ? _userAppContext.CurrentUserId : id;
                string userRole = String.IsNullOrWhiteSpace(role) ? _userAppContext.CurrentRole : role;

                var employerResult = await _profileService.GetEmployerProfile(userId, userRole);

                return Json(new { Success = true, employer = employerResult });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, message = e });
            }
        }

        [HttpPost("saveEmployerProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public async Task<IActionResult> SaveEmployerProfile([FromBody] EmployerProfileViewModel employer)
        {
            if (ModelState.IsValid)
            {
                if (await _profileService.UpdateEmployerProfile(employer, _userAppContext.CurrentUserId, _userAppContext.CurrentRole))
                {
                    return Json(new { Success = true });
                }
            }
            return Json(new { Success = false });
        }

        [HttpPost("saveClientProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public async Task<IActionResult> SaveClientProfile([FromBody] EmployerProfileViewModel employer)
        {
            if (ModelState.IsValid)
            {
                //check if employer is client 5be40d789b9e1231cc0dc51b
                var recruiterClients =(await _recruiterRepository.GetByIdAsync(_userAppContext.CurrentUserId)).Clients;

                if (recruiterClients.Select(x => x.EmployerId == employer.Id).FirstOrDefault())
                {
                    if (await _profileService.UpdateEmployerProfile(employer, _userAppContext.CurrentUserId, "employer"))
                    {
                        return Json(new { Success = true });
                    }
                }
            }
            return Json(new { Success = false });
        }

        [HttpPost("updateEmployerPhoto")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public async Task<ActionResult> UpdateEmployerPhoto()
        {
            IFormFile file = Request.Form.Files[0];
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpPost("updateEmployerVideo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public async Task<IActionResult> UpdateEmployerVideo()
        {
            IFormFile file = Request.Form.Files[0];
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpGet("getEmployerProfileImage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer, recruiter")]
        public async Task<ActionResult> GetWorkSample(string Id)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        [HttpGet("getEmployerProfileImages")]
        public ActionResult GetWorkSampleImage(string Id)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        #endregion

        #region TalentFeed





        //[HttpGet("getProfile")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        //public async Task<IActionResult> GetProfile()
        //{
        //    var userId = _userAppContext.CurrentUserId;
        //    var user = await _userRepository.GetByIdAsync(userId);
        //    return Json(new { Username = user.FirstName });
        //}




        //[HttpGet("getTalentProfile")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent, employer, recruiter")]
        //public async Task<IActionResult> GetTalentProfile(String id = "")
        //{
        //    String talentId = String.IsNullOrWhiteSpace(id) ? _userAppContext.CurrentUserId : id;
        //    var userProfile = await _profileService.GetTalentProfile(talentId);

        //    return Json(new { Success = true, data = userProfile });
        //}



        [HttpGet("getTalentProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent, employer, recruiter")]
        public async Task<IActionResult> GetTalentProfile(String id = "")
        {
            try
            {
                string talentId = String.IsNullOrWhiteSpace(id) ? _userAppContext.CurrentUserId : id;

                var userProfile = await _profileService.GetTalentProfile(talentId);

                return Json(new { Success = true, data = userProfile});
            }
            catch (Exception e)
            {
                return Json(new { Success = false, message = e});
            }
        }



        [HttpPost("updateTalentProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        public async Task<IActionResult> UpdateTalentProfile([FromBody] TalentProfileViewModel profile)
        {
            string goodNews = "Update successed";
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _profileService.UpdateTalentProfile(profile, _userAppContext.CurrentUserId))
                    {
                        return Json(new { Success = true, data = goodNews});
                        //return Json(new { Success = true, data = profile });
                    }
                    return Json(new { Success = false });
                }
                return Json(new { Success = false });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, message = e.Message });
            }
        }

        //[HttpPost("updateTalentProfile")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "talent")]
        //public async Task<IActionResult> UpdateTalentProfile([FromBody] TalentProfileViewModel profile)
        //{
        //    try
        //    {



        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Success = false, message = e.Message });
        //    }

        //}







        [HttpGet("GetTalentSnapshots")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter, employer")]
        public async Task<IActionResult> GetTalentSnapshots(FeedIncrementModel feed)
        {
            try
            {
                //Original
                var result = (await _profileService.GetTalentSnapshotList(_userAppContext.CurrentUserId, false, feed.Position, feed.Number));

                //var result = (await _profileService.GetTalentTesting(_userAppContext.CurrentUserId, false, feed.Position, feed.Number));


                //After edited
                //var result = (await _profileService.GetTalentSnapshotList(_userAppContext.CurrentUserId, false, feed.Position, feed.Number));

                // Dummy talent to fill out the list once we run out of data
                //if (result.Count == 0)
                //{
                //    result.Add(
                //            new Models.TalentSnapshotViewModel
                //            {
                //                CurrentEmployment = "Software Developer at XYZ",
                //                Level = "Junior",
                //                Name = "Dummy User...",
                //                PhotoId = "",
                //                Skills = new List<string> { "C#", ".Net Core", "Javascript", "ReactJS", "PreactJS" },
                //                Summary = "Veronika Ossi is a set designer living in New York who enjoys kittens, music, and partying.",
                //                Visa = "Citizen"
                //            }
                //        );
                //}
                int length = result.Count();

                if (result != null)
                {
                    return Json(new { Success = true, data = result, TheLength = length });
                }
                return Json(new { Success = false, data = "Nothing returned" });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, e.Message });
            }
        }
        #endregion

        #region TalentMatching

        [HttpGet("getTalentList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public async Task<IActionResult> GetTalentListAsync()
        {
            try
            {
                var result = await _profileService.GetFullTalentList();
                return Json(new { Success = true, Data = result });
            }
            catch (MongoException e)
            {
                return Json(new { Success = false, e.Message });
            }
        }

        [HttpGet("getEmployerList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public IActionResult GetEmployerList()
        {
            try
            {
                var result = _profileService.GetEmployerList();
                return Json(new { Success = true, Data = result });
            }
            catch (MongoException e)
            {
                return Json(new { Success = false, e.Message });
            }
        }

        [HttpPost("getEmployerListFilter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public IActionResult GetEmployerListFilter([FromBody]SearchCompanyModel model)
        {
            try
            {
                var result = _profileService.GetEmployerListByFilterAsync(model);//change to filters
                if (result.IsCompletedSuccessfully)
                    return Json(new { Success = true, Data = result.Result });
                else
                    return Json(new { Success = false, Message = "No Results found" });
            }
            catch (MongoException e)
            {
                return Json(new { Success = false, e.Message });
            }
        }

        [HttpPost("getTalentListFilter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetTalentListFilter([FromBody] SearchTalentModel model)
        {
            try
            {
                var result = _profileService.GetTalentListByFilterAsync(model);//change to filters
                return Json(new { Success = true, Data = result.Result });
            }
            catch (MongoException e)
            {
                return Json(new { Success = false, e.Message });
            }
        }

        [HttpGet("getSuggestionList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public IActionResult GetSuggestionList(string employerOrJobId, bool forJob)
        {
            try
            {
                var result = _profileService.GetSuggestionList(employerOrJobId, forJob, _userAppContext.CurrentUserId);
                return Json(new { Success = true, Data = result });
            }
            catch (MongoException e)
            {
                return Json(new { Success = false, e.Message });
            }
        }

        [HttpPost("addTalentSuggestions")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public async Task<IActionResult> AddTalentSuggestions([FromBody] AddTalentSuggestionList talentSuggestions)
        {
            try
            {
                if (await _profileService.AddTalentSuggestions(talentSuggestions))
                {
                    return Json(new { Success = true });
                }

            }
            catch (Exception e)
            {
                return Json(new { Success = false, e.Message });
            }
            return Json(new { Success = false });
        }

        #endregion


        #region ManageClients

        [HttpGet("getClientList")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        public async Task<IActionResult> GetClientList()
        {
            try
            {
                var result=await _profileService.GetClientListAsync(_userAppContext.CurrentUserId);

                return Json(new { Success = true, result });
            }
            catch(Exception e)
            {
                return Json(new { Success = false, e.Message });
            }
        }

        //[HttpGet("getClientDetailsToSendMail")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "recruiter")]
        //public async Task<IActionResult> GetClientDetailsToSendMail(string clientId)
        //{
        //    try
        //    {
        //            var client = await _profileService.GetEmployer(clientId);

        //            string emailId = client.Login.Username;
        //            string companyName = client.CompanyContact.Name;

        //            return Json(new { Success = true, emailId, companyName });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Success = false, Message = e.Message });
        //    }
        //}

        #endregion

        public IActionResult Get() => Content("Test");

    }
}
