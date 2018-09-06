using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Waterful.Core;
using Waterful.Core.Models;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Table;
using Microsoft.AspNetCore.Hosting;
using Waterful.Back.ViewModels;

namespace Waterful.Back.Controllers
{
    /// <summary>
    /// excel导出https://github.com/VahidN/EPPlus.Core/blob/master/src/EPPlus.Core.SampleWebApp
    /// </summary>
    public class ExcelController : LoginControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hostingEnvironment;
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ExcelController(UnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string begin, string end, string mobile, string name, int type = 0, int status = -10, int p = 1, int pageSize = 99999)
        {
            var strDateTime = DateTime.Now.ToString("yyMMddhhmmssfff"); //取得时间字符串
            var strRan = Convert.ToString(new Random().Next(100, 999)); //生成三位随机数
            var fileDownloadName = strDateTime + strRan + ".xlsx";
            var reportsFolder = "Excel";

            int count = 0;
            IQueryable<Order> result = _unitOfWork.OrderRepository.SearchList(p, pageSize, out count, begin, end, mobile, name, status, type);
            List<OrderExcelVM> list = new List<OrderExcelVM>();
            var statusText = Waterful.Core.Enums.OrderEnum.OrderDic();
            foreach (var o in result)
            {
                list.Add(new OrderExcelVM
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    OrderNo = o.OrderNo,
                    ProductName = o.OrderItems.Count > 0 ? o.OrderItems.FirstOrDefault().Name : "",
                    Total = o.Total,
                    Amount = o.Amount,
                    DiscountAmount = o.DiscountAmount,
                    DepositAmount = o.DepositAmount,
                    InstallAmount = o.InstallAmount,
                    Name = o.Name,
                    Mobile = o.Mobile,
                    Street = o.Street,
                    CouponNo = o.CouponNo,
                    Status = statusText[o.Status],
                    ParentId = o.ParentId,
                    FilterPrice = o.FilterPrice,
                    ServiceNumber = o.ServiceNumber,
                    Remark = o.Remark,
                    CreateTime = o.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    UpdateTime = o.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Close = o.Close ? "已关闭" : "",
                    CloseTime = o.CloseTime != null ? ((DateTime)o.CloseTime).ToString("yyyy-MM-dd HH:mm:ss") : ""
                });
            }

            using (var package = createOrderExcelPackage())
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("订单列表");
                //填充数据
                sheet.Cells[2, 1].LoadFromCollection(list);
                //第一列
                sheet.Cells["A1"].Value = "订单Id";
                sheet.Cells["B1"].Value = "用户Id";
                sheet.Cells["C1"].Value = "订单号";
                sheet.Cells["D1"].Value = "产品名称";
                sheet.Cells["E1"].Value = "总金额";
                sheet.Cells["F1"].Value = "付款金额";
                sheet.Cells["G1"].Value = "抵扣金额";
                sheet.Cells["H1"].Value = "押金";
                sheet.Cells["I1"].Value = "安装费";
                sheet.Cells["J1"].Value = "收货姓名";
                sheet.Cells["K1"].Value = "收货电话";
                sheet.Cells["L1"].Value = "收货地址";
                sheet.Cells["M1"].Value = "优惠券编号";
                sheet.Cells["N1"].Value = "状态";
                sheet.Cells["O1"].Value = "续费订单Id";
                sheet.Cells["P1"].Value = "滤芯价格";
                sheet.Cells["Q1"].Value = "服务次数";
                sheet.Cells["R1"].Value = "备注";
                sheet.Cells["S1"].Value = "创建时间";
                sheet.Cells["T1"].Value = "更新时间";
                sheet.Cells["U1"].Value = "是否关闭";
                sheet.Cells["V1"].Value = "关闭时间";
                sheet.Row(1).Style.Font.Bold = true;
                //宽度自适应
                sheet.Cells.AutoFitColumns();

                var path = Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder);
                //目录效验
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //package.Save();
                package.SaveAs(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder, fileDownloadName)));
            }
            return File($"~/{reportsFolder}/{fileDownloadName}", XlsxContentType, fileDownloadName);
        }
        private ExcelPackage createOrderExcelPackage()
        {
            var package = new ExcelPackage();
            //var worksheet = package.Workbook.Worksheets.Add("订单列表");
            return package;
        }
        ///// <summary>
        ///// An in-memory report
        ///// </summary>
        //public IActionResult Index()
        //{
        //    byte[] reportBytes;
        //    using (var package = createExcelPackage())
        //    {
        //        reportBytes = package.GetAsByteArray();
        //    }

        //    return File(reportBytes, XlsxContentType, "report.xlsx");
        //}

        ///// <summary>
        ///// /Home/ReadFile
        ///// </summary>
        //public IActionResult ReadFile()
        //{
        //    var fileDownloadName = "report.xlsx";
        //    var reportsFolder = "reports";
        //    var fileInfo = new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder, fileDownloadName));
        //    if (!fileInfo.Exists)
        //    {
        //        using (var package = createExcelPackage())
        //        {
        //            package.SaveAs(fileInfo);
        //        }
        //    }

        //    return Content(readExcelPackage(fileInfo, worksheetName: "Employee"));
        //}

        //private string readExcelPackage(FileInfo fileInfo, string worksheetName)
        //{
        //    using (var package = new ExcelPackage(fileInfo))
        //    {
        //        var worksheet = package.Workbook.Worksheets[worksheetName];
        //        int rowCount = worksheet.Dimension.Rows;
        //        int ColCount = worksheet.Dimension.Columns;

        //        var sb = new StringBuilder();
        //        for (int row = 1; row <= rowCount; row++)
        //        {
        //            for (int col = 1; col <= ColCount; col++)
        //            {
        //                sb.AppendFormat("{0}\t", worksheet.Cells[row, col].Value);
        //            }
        //            sb.Append(Environment.NewLine);
        //        }
        //        return sb.ToString();
        //    }
        //}

        //private ExcelPackage createExcelPackage()
        //{
        //    var package = new ExcelPackage();
        //    package.Workbook.Properties.Title = "Salary Report";
        //    package.Workbook.Properties.Author = "Vahid N.";
        //    package.Workbook.Properties.Subject = "Salary Report";
        //    package.Workbook.Properties.Keywords = "Salary";


        //    var worksheet = package.Workbook.Worksheets.Add("Employee");

        //    //First add the headers
        //    worksheet.Cells[1, 1].Value = "ID";
        //    worksheet.Cells[1, 2].Value = "Name";
        //    worksheet.Cells[1, 3].Value = "Gender";
        //    worksheet.Cells[1, 4].Value = "Salary (in $)";

        //    //Add values

        //    var numberformat = "#,##0";
        //    var dataCellStyleName = "TableNumber";
        //    var numStyle = package.Workbook.Styles.CreateNamedStyle(dataCellStyleName);
        //    numStyle.Style.Numberformat.Format = numberformat;

        //    worksheet.Cells[2, 1].Value = 1000;
        //    worksheet.Cells[2, 2].Value = "Jon";
        //    worksheet.Cells[2, 3].Value = "M";
        //    worksheet.Cells[2, 4].Value = 5000;
        //    worksheet.Cells[2, 4].Style.Numberformat.Format = numberformat;

        //    worksheet.Cells[3, 1].Value = 1001;
        //    worksheet.Cells[3, 2].Value = "Graham";
        //    worksheet.Cells[3, 3].Value = "M";
        //    worksheet.Cells[3, 4].Value = 10000;
        //    worksheet.Cells[3, 4].Style.Numberformat.Format = numberformat;

        //    worksheet.Cells[4, 1].Value = 1002;
        //    worksheet.Cells[4, 2].Value = "Jenny";
        //    worksheet.Cells[4, 3].Value = "F";
        //    worksheet.Cells[4, 4].Value = 5000;
        //    worksheet.Cells[4, 4].Style.Numberformat.Format = numberformat;

        //    // Add to table / Add summary row
        //    var tbl = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: 4, toColumn: 4), "Data");
        //    tbl.ShowHeader = true;
        //    tbl.TableStyle = TableStyles.Dark9;
        //    tbl.ShowTotal = true;
        //    tbl.Columns[3].DataCellStyleName = dataCellStyleName;
        //    tbl.Columns[3].TotalsRowFunction = RowFunctions.Sum;
        //    worksheet.Cells[5, 4].Style.Numberformat.Format = numberformat;

        //    // AutoFitColumns
        //    worksheet.Cells[1, 1, 4, 4].AutoFitColumns();

        //    return package;
        //}
    }
}