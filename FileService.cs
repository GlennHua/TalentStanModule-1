using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Talent.Common.Aws;
using Talent.Common.Contracts;

namespace Talent.Common.Services
{
    public class FileService : IFileService
    {
        private readonly IHostingEnvironment _environment;
        private readonly string _tempFolder;
        private IAwsService _awsService;

        public FileService(IHostingEnvironment environment, 
            IAwsService awsService)
        {
            _environment = environment;
            _tempFolder = "images\\";
            _awsService = awsService;
        }

        public async Task<string> GetFileURL(string fileName, FileType type)
        {
            //string pathWeb = _environment.WebRootPath;
            //string pathValue = pathWeb + _tempFolder;
            string fileUrl = "";
            string bucketName = "talent-standard-glenn";

            //if (fileName != null && type == FileType.ProfilePhoto && pathWeb != "")
            //{
            //    string path = pathValue + fileName;
            //    fileUrl = path;
            //}

            //return fileUrl;


            if (fileName != null && type == FileType.ProfilePhoto)
            {

                fileUrl = await _awsService.GetPresignedUrlObject(fileName, bucketName);

            }
            return fileUrl;

           // throw new NotImplementedException();


        }
         

        public async Task<string> SaveFile(IFormFile file, FileType type)
        {
            var uniqueFileName = "";
            string pathWeb = _environment.WebRootPath;  
            string bucketName = "talent-standard-glenn";

            if (file != null && type == FileType.ProfilePhoto && pathWeb != "")
            {
                //string pathValue = pathWeb + _tempFolder;
                string pathValue = pathWeb;

                uniqueFileName = $@"{DateTime.Now.Ticks}_" + file.FileName;

                var path = pathValue + uniqueFileName;

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);

                    if (!await _awsService.PutFileToS3(uniqueFileName, fileStream, bucketName))
                    {
                        uniqueFileName = "";
                    }
                }
                
            }

            return uniqueFileName;

        }

        public async Task<bool> DeleteFile(string fileName, FileType type)
        {
            string bucketName = "talent-standard-glenn";

            if (fileName != null && type == FileType.ProfilePhoto)
            {
                await _awsService.RemoveFileFromS3(fileName, bucketName);
            }
            return true;
        }


        #region Document Save Methods

        private async Task<string> SaveFileGeneral(IFormFile file, string bucket, string folder, bool isPublic)
        {
            //Your code here;
            throw new NotImplementedException();
        }
        
        private async Task<bool> DeleteFileGeneral(string id, string bucket)
        {
            //Your code here;
            throw new NotImplementedException();
        }
        #endregion
    }
}
