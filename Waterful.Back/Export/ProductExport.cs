using Npoi.Core.SS.UserModel;
using Npoi.Core.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waterful.Core.Enums;
using Waterful.Core.Models;

namespace Waterful.Back.Export
{
    public class ProductExport
    {
        public static XSSFWorkbook GetProductExport(List<Product> list, PaymentEnum pagyenType)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = null;
            int headRowIndex = 0;
            string sheetName = "Sheet1";
            sheet = workbook.CreateSheet(sheetName);

            XSSFRow headerRow = (XSSFRow)sheet.CreateRow(headRowIndex);

            ICellStyle headStyle = workbook.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.Center;
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 10;
            font.Boldweight = 700;
            headStyle.SetFont(font);
            headerRow.CreateCell(0).SetCellValue("编号");
            headerRow.CreateCell(1).SetCellValue("商品名称");
            headerRow.CreateCell(2).SetCellValue(pagyenType == PaymentEnum.Buy ? "价格" : "租金");
            headerRow.CreateCell(3).SetCellValue(pagyenType == PaymentEnum.Buy ? "原价" : "押金");
            headerRow.CreateCell(4).SetCellValue("后续滤芯");
            headerRow.CreateCell(5).SetCellValue("安装费");
            headerRow.CreateCell(6).SetCellValue("库存");
            headerRow.CreateCell(7).SetCellValue("产品状态");
            //将数据逐步写入sheet1各个行 yyyy-MM-dd HH:mm:ss
            for (int i = 0; i < list.Count; i++)
            {
                XSSFRow rowtemp = (XSSFRow)sheet.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].Id);
                rowtemp.CreateCell(1).SetCellValue(list[i].Name);
                rowtemp.CreateCell(2).SetCellValue((double)list[i].Price);
                if (pagyenType == PaymentEnum.Buy)
                    rowtemp.CreateCell(3).SetCellValue((double)list[i].OriginalPrice);
                else
                    rowtemp.CreateCell(3).SetCellValue((double)list[i].DepositAmount);

                rowtemp.CreateCell(4).SetCellValue((double)list[i].FilterPrice);
                rowtemp.CreateCell(5).SetCellValue((double)list[i].InstallFee);
                rowtemp.CreateCell(6).SetCellValue(list[i].Storage);
                rowtemp.CreateCell(7).SetCellValue(list[i].Status == 1 ? "启用" : "停用");

            }

            return workbook;
        }
    }
}
