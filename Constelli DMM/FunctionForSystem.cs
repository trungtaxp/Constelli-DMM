using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;

namespace Constelli_DMM
{
    public static class FunctionForSystem
    {
        //public static MySqlConnection conn;
        //public static MySqlConnection sec_conn;
        public static string _BTC_Connection = "USB0::0x05E6::0x6500::04637936::INSTR";
        // public static string _BTC_Connection = "TCPIP::192.168.0.101::INSTR";
        public static string _SFU_Connection = "TCPIP::192.168.148.89::INSTR";
        public static string _SFC_Connection = "TCPIP::192.168.148.90::INSTR";
        public static string TestItemName = "Test_Item_Name";
        
        public static string sTiengviet = " 0123456789AaĂăÂâBbCcDdĐđEeÊêFfGgHhIiJjKkLlMmNnOoÔôƠơPpQqRrSsTtUuƯưVvWwXxYyZzÀàẰằẦầÈèỀềÌìÒòỒồỜờÙùỪừỲỳÁáẮắẤấÉéẾếÍíÓóỐốỚớÚúỨứÝýẢảẲẳẨẩẺẻỂểỈỉỎỏỔổỞởỦủỬửỶỷÃãẴẵẪẫẼẽỄễĨĩÕõỖỗỠỡŨũỮữỸỹẠạẶặẬậẸẹỆệỊịỌọỘộỢợỤụỰựỴỵ,.-_%";
        public static string chuvaso = " 0123456789abcdefghijklmnopqtrsyvwxuzQWERTYUIOPASDFGHJKLZXCVBNM";
        public static string sEmail = " 0123456789abcdefghijklmnopqtrsyvwxuzQWERTYUIOPASDFGHJKLZXCVBNM_.";
        public static string sdienthoai = " 0123456789_.()+";
        
        public static bool Seaching;//True khi bắt đầu tìm kiếm. Tìm kiếm xong trả về False.
        
                
        public static bool chkstringtv(string tiengviet)
        {
            for (int i = 0; i < tiengviet.Length + 1; i++)
            {
                if (i == tiengviet.Length)
                {
                    return true;
                }
                else if (sTiengviet.IndexOf(tiengviet[i]) == -1)
                {
                    return false;
                }
            }
            return false;
        }
        public static bool chkstringemail(string email)
        {
            for (int i = 0; i < email.Length + 1; i++)
            {
                if (i == email.Length)
                {
                    return true;
                }
                else if (sEmail.IndexOf(email[i]) == -1)
                {
                    return false;
                }
            }
            return false;
        }
        public static bool chkstringchuvaso(string stringchuvaso)
        {
            for (int i = 0; i < stringchuvaso.Length + 1; i++)
            {
                if (i == stringchuvaso.Length)
                {
                    return true;
                }
                else if (chuvaso.IndexOf(stringchuvaso[i]) == -1)
                {
                    return false;
                }
            }
            return false;
        }
        public static bool isNaN(string checkString)//Hàm check chuỗi có chứa các chữ cái hay không
        {
            int j = 0;
            for (int i = 0; i < checkString.Length; i++)
            {
                if ((checkString[i].ToString() != "0") && (checkString[i].ToString() != "1") && (checkString[i].ToString() != "2") && (checkString[i].ToString() != "3") && (checkString[i].ToString() != "4") && (checkString[i].ToString() != "5") && (checkString[i].ToString() != "6") && (checkString[i].ToString() != "7") && (checkString[i].ToString() != "8") && (checkString[i].ToString() != "9"))
                {
                    return false;
                }
                else if (i != checkString.Length)
                {
                    j++;
                }
                else if (j == checkString.Length)
                {
                    return true;
                }
            }
            return true;
        }
        
        public static string ConvetDateTime(string datetime)
        {
            try
            {
                string ngay, thang;
                DateTime Thoidiemlap = Convert.ToDateTime(datetime);
                if (Thoidiemlap.Day < 10)
                {
                    ngay = "0" + Thoidiemlap.Day;
                }
                else
                {
                    ngay = Thoidiemlap.Day.ToString();
                }
                if (Thoidiemlap.Month < 10)
                {
                    thang = "0" + Thoidiemlap.Month;
                }
                else
                {
                    thang = Thoidiemlap.Month.ToString();
                }
                return ngay + "/" + thang + "/" + Thoidiemlap.Year + " " + Thoidiemlap.TimeOfDay;
            }
            catch (Exception)
            {
                return datetime;
            }

        }

        public static bool chkdienthoai(string stringdienthoai)
        {
            for (int i = 0; i < stringdienthoai.Length + 1; i++)
            {
                if (i == stringdienthoai.Length)
                {
                    return true;
                }
                else if (sdienthoai.IndexOf(stringdienthoai[i]) == -1)
                {
                    return false;
                }
            }
            return false;
        }

        

    }
}
