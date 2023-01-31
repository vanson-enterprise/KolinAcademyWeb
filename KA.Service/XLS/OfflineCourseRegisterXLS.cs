using ClosedXML.Excel;
using KA.ViewModels.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Service.XLS
{
    public class OfflineCourseRegisterXLS
    {
        public byte[] Edition(List<OfflineCourseRegisterVm> data)
        {
            XLWorkbook wb = new XLWorkbook();
            wb.Properties.Title = "Kolin Offline Course Register Export";
            

            var ws = wb.Worksheets.Add("Kolin Offline Course Register");
            ws.Column("B").Width = 20;
            ws.Column("C").Width = 20;
            ws.Column("D").Width = 20;
            ws.Column("E").Width = 20;
            ws.Column("F").Width = 20;
            ws.Column("G").Width = 20;

            ws.Cell(1, 1).Value = "STT";
            ws.Cell(1, 2).Value = "Họ và tên";
            ws.Cell(1, 3).Value = "Email";
            ws.Cell(1, 4).Value = "Số điện thoại";
            ws.Cell(1, 5).Value = "Số lượng thành viên";
            ws.Cell(1, 6).Value = "Tên khóa học";
            ws.Cell(1, 7).Value = "Ngày đăng ký";

            for (int row = 0; row < data.Count; row++)
            {
                // The apostrophe is to force ClosedXML to treat the date as a string
                ws.Cell(row + 2, 1).Value = data[row].Index;
                ws.Cell(row + 2, 2).Value = data[row].FullName;
                ws.Cell(row + 2, 3).Value = data[row].Email;
                ws.Cell(row + 2, 4).Value = data[row].PhoneNumber;
                ws.Cell(row + 2, 5).Value = data[row].MemberAmount;
                ws.Cell(row + 2, 6).Value = data[row].CourseName;
                ws.Cell(row + 2, 7).Value = data[row].CreatedDate;
            }

            MemoryStream XLSStream = new();
            wb.SaveAs(XLSStream);

            return XLSStream.ToArray();
        }
    }
}
