using InsternShip.Common;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static InsternShip.Data.Model.ImportDataModel;

namespace InsternShip.Data.Repositories
{
    public class ImportDataRepository : IImportDataRepository
    {
        public ImportDataRepository()
        {
        }

        public async Task<ImportUserModel> GetDataUser(IFormFile file)
        {
            if(!(file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx")))
            {
                throw new KeyNotFoundException(ExceptionMessages.ExcelFileRequired);
            }
            if (file != null || file.Length > 0)
            {
                try
                {
                    var listUser = new List<CreateUserModel>();
                    var listCan = new List<CreateCandidateModel>();
                    var listInter = new List<CreateInterviewerModel>();
                    var listRec = new List<CreateRecruiterModel>();
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                            var rowcount = worksheet.Dimension.Rows;
                            var columncount = worksheet.Dimension.Columns;
                            for (int row = 2; row <= rowcount; row++)
                            {
                                if (worksheet.Cells[row, 9].Value == null) { continue; }
                                var role = worksheet.Cells[row, 9].Value.ToString().Trim();
                                if (!(role.ToUpper().Equals("ADMIN") || role.ToUpper().Equals("CANDIDATE")
                                    || role.ToUpper().Equals("RECRUITER") || role.ToUpper().Equals("INTERVIEWER"))) { continue; }
                                DateTime dateOfBirth;
                                DateTime.TryParse(worksheet.Cells[row, 5].Value.ToString().Trim(), out dateOfBirth);
                                listUser.Add(new CreateUserModel
                                {
                                    Avatar = worksheet.Cells[row, 1].Value.ToString()?.Trim(),
                                    FirstName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                    LastName = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                    Gender = worksheet.Cells[row, 4].Value.ToString()?.Trim(),
                                    DateOfBirth = dateOfBirth,
                                    Location = worksheet.Cells[row, 6].Value.ToString()?.Trim(),
                                    Email = worksheet.Cells[row, 7].Value.ToString().Trim(),
                                    Password = worksheet.Cells[row, 8].Value.ToString().Trim(),
                                    Role = role,

                                });
                                if (role.ToUpper().Equals("CANDIDATE"))
                                {
                                    listCan.Add(new CreateCandidateModel
                                    {
                                        Description = worksheet.Cells[row, 10].Value.ToString()?.Trim(),
                                        Education = worksheet.Cells[row, 11].Value.ToString()?.Trim(),
                                        Experience = worksheet.Cells[row, 12].Value.ToString()?.Trim(),
                                        Language = worksheet.Cells[row, 13].Value.ToString()?.Trim(),
                                        Skillsets = worksheet.Cells[row, 14].Value.ToString()?.Trim(),
                                    });
                                }
                                else if (role.ToUpper().Equals("INTERVIEWER"))
                                {
                                    listInter.Add(new CreateInterviewerModel
                                    {
                                        Description = worksheet.Cells[row, 10].Value.ToString()?.Trim(),
                                        UrlContact = worksheet.Cells[row, 15].Value.ToString()?.Trim(),
                                    });
                                }
                                else if (role.ToUpper().Equals("RECRUITER"))
                                {
                                    listRec.Add(new CreateRecruiterModel
                                    {
                                        Description = worksheet.Cells[row, 10].Value.ToString()?.Trim(),
                                        UrlContact = worksheet.Cells[row, 15].Value.ToString()?.Trim(),
                                    });
                                }

                            }
                        }
                    }
                    return new ImportUserModel
                    {
                        ListUser = listUser,
                        ListCan = listCan,
                        ListInter = listInter,
                        listRec = listRec,
                    };
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            else { throw new MissingFieldException(MissingFieldMessage.MissingFile); }
        }

        public async Task<IEnumerable<ImportJobPostModel>> GetDataJobPost(IFormFile file)
        {
            if (!(file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx")))
            {
                throw new KeyNotFoundException(ExceptionMessages.ExcelFileRequired);
            }
            if (file != null || file.Length > 0)
            {
                try
                {
                    var listJobPost = new List<ImportJobPostModel>();
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                            var rowcount = worksheet.Dimension.Rows;
                            for (int row = 2; row <= rowcount; row++)
                            {
                                DateTime endDate;
                                DateTime.TryParse(worksheet.Cells[row, 11].Value.ToString().Trim(), out endDate);
                                var jobPost = new CreateJobModel
                                {
                                    Type = worksheet.Cells[row, 1].Value.ToString()?.Trim(),
                                    Level = worksheet.Cells[row, 2].Value.ToString()?.Trim(),
                                    Name = worksheet.Cells[row, 3].Value.ToString()?.Trim(),
                                    Location = worksheet.Cells[row, 4].Value.ToString()?.Trim(),
                                    Description = worksheet.Cells[row, 5].Value.ToString()?.Trim(),
                                    Requirement = worksheet.Cells[row, 6].Value.ToString()?.Trim(),
                                    Benefit = worksheet.Cells[row, 7].Value.ToString()?.Trim(),
                                    MinSalary = Int32.Parse(worksheet.Cells[row, 8].Value.ToString().Trim()),
                                    MaxSalary = Int32.Parse(worksheet.Cells[row, 9].Value.ToString().Trim()),
                                    Quantity = Int32.Parse(worksheet.Cells[row, 10].Value.ToString().Trim()),
                                    EndDate = endDate,
                                    JobStatus = (bool)worksheet.Cells[row, 12].Value
                                };
                                var recEmail = worksheet.Cells[row, 13].Value.ToString()?.Trim();
                                var data = new ImportJobPostModel { RecEmail = recEmail, JobData = jobPost };
                                listJobPost.Add(data);
                            }
                        }
                    }
                    return await Task.FromResult(listJobPost);
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            else { throw new MissingFieldException(MissingFieldMessage.MissingFile); }
        }

        public async Task<IEnumerable<ImportEventPostModel>> GetDataEventPost(IFormFile file)
        {
            if (!(file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx")))
            {
                throw new KeyNotFoundException(ExceptionMessages.ExcelFileRequired);
            }
            if (file != null || file.Length > 0)
            {
                try
                {
                    var listEventPost = new List<ImportEventPostModel>();
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                            var rowcount = worksheet.Dimension.Rows;
                            for (int row = 2; row <= rowcount; row++)
                            {
                                DateTime startDate;
                                DateTime endDate;
                                DateTime deadlineDate;
                                DateTime.TryParse(worksheet.Cells[row, 7].Value.ToString().Trim(), out startDate);
                                DateTime.TryParse(worksheet.Cells[row, 8].Value.ToString().Trim(), out endDate);
                                DateTime.TryParse(worksheet.Cells[row, 9].Value.ToString().Trim(), out deadlineDate);
                                var eventPost = new CreateEventModel
                                {
                                    Name = worksheet.Cells[row, 1].Value.ToString()?.Trim(),
                                    Location = worksheet.Cells[row, 2].Value.ToString()?.Trim(),
                                    MaxCandidate = Int32.Parse(worksheet.Cells[row, 3].Value.ToString()?.Trim()),
                                    Description = worksheet.Cells[row, 4].Value.ToString()?.Trim(),
                                    Poster = worksheet.Cells[row, 5].Value.ToString()?.Trim(),
                                    Status = (bool)worksheet.Cells[row, 6].Value,
                                    StartDate = startDate,
                                    EndDate = endDate,
                                    DeadlineDate = deadlineDate
                                };
                                var recEmail = worksheet.Cells[row, 10].Value.ToString()?.Trim();
                                var data = new ImportEventPostModel { RecEmail = recEmail, EventData = eventPost };
                                listEventPost.Add(data);
                            }
                        }
                    }
                    return await Task.FromResult(listEventPost);
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            else { throw new MissingFieldException(MissingFieldMessage.MissingFile); }
        }
    }
}
