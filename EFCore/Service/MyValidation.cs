using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Service.valid
{
    public class MyValidation
    {

        public static bool IsMobileNumberValid(string mobileNumber)
        {
            string pattern = @"^09\d{9}$";

            return Regex.IsMatch(mobileNumber, pattern);
        }

        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com|example\.com)$";

            if (Regex.IsMatch(email, pattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsValidNameAndFamily(string name, string family)
        {
            if (name.Length < 3 || name.Length > 20 || family.Length < 3 || family.Length > 20)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsValidUserNameAndPassword(string userName, string password)
        {
            Regex regex = new Regex("^[a-zA-Z0-9!@#$%^&*()-_+=]+$");

            bool isValidUserName = userName.Length >= 4 && userName.Length <= 20 && regex.IsMatch(userName);
            bool isValidPassword = password.Length >= 4 && password.Length <= 20 && regex.IsMatch(password);

            return isValidUserName && isValidPassword;
        }



    }







}
