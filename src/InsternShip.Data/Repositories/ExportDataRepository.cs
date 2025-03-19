using AutoMapper;
using InsternShip.Common;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Data;
using System.Reflection;

namespace InsternShip.Data.Repositories
{
    public class ExportDataRepository: Repository<Candidate>, IExportDataRepository
    {
        private readonly RecruitmentDB _context;

        public ExportDataRepository(RecruitmentDB context) : base(context)
        {
            _context = context;
        }

        public async Task<byte[]> Export(List<CandidateReportModel>? canrep, List<RecruitmentReportModel>? recrep, List<EventReportModel>? evrep, List<InterviewReportModel>? invrep)
        {
            DataTable dataTable = new();
            if (canrep != null)
                dataTable = ListToDatatable(canrep);
            else if (recrep != null)
                dataTable = ListToDatatable(recrep);
            else if (evrep != null)
                dataTable = ListToDatatable(evrep);
            else if (invrep != null)
                dataTable = ListToDatatable(invrep);
            var ExcelData = DatatableToExcel(dataTable);
            return await Task.FromResult(ExcelData);
        }

        public DataTable ListToDatatable<T>(List<T> list)
        {
            DataTable dataTable = new(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in list)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public byte[] DatatableToExcel(DataTable dataTable)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");

                // Write column headers
                for (int col = 0; col < dataTable.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1].Value = dataTable.Columns[col].ColumnName;
                }

                // Write data rows
                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
                    }
                }

                // Save the Excel package to a memory stream
                using (var stream = new MemoryStream())
                {
                    package.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
