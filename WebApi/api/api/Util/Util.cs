using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace api.Util
{
    public static class Util
    {
        public static int GetDisplayItemPerPage()
        {
            // การแบ่งหน้าแสดงผล : จำนวนบรรทัดต่อหน้า
            return 15;
        }
        public static void CopyProperties(this object destinationObject, object sourceObject, bool overwriteAll = true)
        {
            try
            {
                if (sourceObject != null)
                {
                    PropertyInfo[] sourceProps = sourceObject.GetType().GetProperties();
                    List<string> sourcePropNames = sourceProps.Select(p => p.Name).ToList();
                    foreach (PropertyInfo pi in destinationObject.GetType().GetProperties())
                    {
                        if (sourcePropNames.Contains(pi.Name))
                        {
                            PropertyInfo sourceProp = sourceProps.First(srcProp => srcProp.Name == pi.Name);
                            if (sourceProp.PropertyType == pi.PropertyType && sourceProp.CanWrite)
                                if (overwriteAll || pi.GetValue(destinationObject, null) == null)
                                {
                                    pi.SetValue(destinationObject, sourceProp.GetValue(sourceObject, null), null);
                                }
                        }
                    }
                }
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        public static string ThaiBaht(string txt)
        {
            string bahtTxt, bahtTH = "";
            double amount;
            try { amount = Convert.ToDouble(txt); }
            catch { amount = 0; }
            bahtTxt = amount.ToString("####.00");
            string[] temp = bahtTxt.Split('.');
            string intVal = temp[0];
            string decVal = temp[1];
            if (Convert.ToDouble(bahtTxt) == 0)
                bahtTH = "ศูนย์บาทถ้วน";
            else
            {
                bahtTH += NumberToWordsTH(Convert.ToInt64(intVal));
                bahtTH += "บาท";
                if (decVal == "00")
                    bahtTH += "ถ้วน";
                else
                {
                    bahtTH += NumberToWordsTH(Convert.ToInt64(decVal));
                    bahtTH += "สตางค์";
                }
            }
            return bahtTH;
        }

        public static string NumberToWordsTH(long numbermoney)
        {
            string Textreturn = "";
            if (numbermoney == 0)
            {
                return "ศูนย์";
            }

            if (numbermoney < 0)
            {
                Textreturn = "ลบ";
                numbermoney = -(numbermoney);
            }

            if ((numbermoney / 1000000) > 0)
            {
                Textreturn += NumberToWordsTH(numbermoney / 1000000) + "ล้าน";
                numbermoney %= 1000000;
            }
            if ((numbermoney / 100000) > 0)
            {
                Textreturn += NumberToWordsTH(numbermoney / 100000) + "แสน";
                numbermoney %= 100000;
            }
            if ((numbermoney / 10000) > 0)
            {
                Textreturn += NumberToWordsTH(numbermoney / 10000) + "หมื่น";
                numbermoney %= 10000;
            }
            if ((numbermoney / 1000) > 0)
            {
                Textreturn += NumberToWordsTH(numbermoney / 1000) + "พัน";
                numbermoney %= 1000;
            }

            if ((numbermoney / 100) > 0)
            {
                Textreturn += NumberToWordsTH(numbermoney / 100) + "ร้อย";
                numbermoney %= 100;
            }

            if (numbermoney > 0)
            {
                if (Textreturn != "")
                {
                    Textreturn += "";
                }

                var unitsMap = new[] { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "เเปด", "เก้า", "สิบ", "สิบเอ็ด", "สิบสอง", "สิบสาม", "สิบสี่", "สิบห้า", "สิบหก", "สิบเจ็ด", "สิบเเปด", "สิบเก้า" };
                var tensMap = new[] { "ศูนย์", "สิบ", "ยี่สิบ", "สามสิบ", "สี่สิบ", "ห้าสิบ", "หกสิบ", "เจ็ดสิบ", "แปดสิบ", "เก้าสิบ" };

                if (numbermoney < 20)
                {
                    Textreturn += unitsMap[numbermoney];
                }
                else
                {
                    Textreturn += tensMap[numbermoney / 10];
                    if ((numbermoney % 10) > 0)
                    {
                        Textreturn += "" + unitsMap[numbermoney % 10];
                    }
                }
            }

            return Textreturn;
        }

        public static string NumberToWords(long number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static int GetNumOfMonth(int month, int year)
        {
            int n = 0;
            bool isLeapYear = false;

            if (year % 4 == 0 && ((year % 100 != 0) || (year % 400 == 0)))
            {
                isLeapYear = true;
            }
            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                n = 31;
            }
            else if (month == 2 && isLeapYear)
            {
                n = 29;
            }
            else if (month == 2 && !isLeapYear)
            {
                n = 28;
            }
            else if (month == 4 || month == 6 || month == 9 || month == 11)
            {
                n = 30;
            }

            return n;
        }

        public static string GetMonthNameThai(int month)
        {
            string monthName = "";
            switch (month)
            {
                case 1:
                    monthName = "มกราคม";
                    break;
                case 2:
                    monthName = "กุมภาพันธ์";
                    break;
                case 3:
                    monthName = "มีนาคม";
                    break;
                case 4:
                    monthName = "เมษายน";
                    break;
                case 5:
                    monthName = "พฤษภาคม";
                    break;
                case 6:
                    monthName = "มิถุนายน";
                    break;
                case 7:
                    monthName = "กรกฎาคม";
                    break;
                case 8:
                    monthName = "สิงหาคม";
                    break;
                case 9:
                    monthName = "กันยายน";
                    break;
                case 10:
                    monthName = "ตุลาคม";
                    break;
                case 11:
                    monthName = "พฤศจิกายน";
                    break;
                case 12:
                    monthName = "ธันวาคม";
                    break;
            }

            return monthName;
        }

        public static string GetMonthShortNameThai(int month)
        {
            string monthName = "";
            switch (month)
            {
                case 1:
                    monthName = "ม.ค.";
                    break;
                case 2:
                    monthName = "ก.พ.";
                    break;
                case 3:
                    monthName = "มี.ค.";
                    break;
                case 4:
                    monthName = "เม.ย.";
                    break;
                case 5:
                    monthName = "พ.ค.";
                    break;
                case 6:
                    monthName = "มิ.ย.";
                    break;
                case 7:
                    monthName = "ก.ค.";
                    break;
                case 8:
                    monthName = "ส.ค.";
                    break;
                case 9:
                    monthName = "ก.ย.";
                    break;
                case 10:
                    monthName = "ต.ค.";
                    break;
                case 11:
                    monthName = "พ.ย.";
                    break;
                case 12:
                    monthName = "ธ.ค.";
                    break;
            }

            return monthName;
        }

        public static string GetGuid()
        {
            Guid id = Guid.NewGuid();
            return id.ToString();
        }
    }
}