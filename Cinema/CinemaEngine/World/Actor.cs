using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaEngine
{
    /// <summary>
    /// Актёр
    /// </summary>
    public class Actor
    {
        public enum Gender
        {
            Male,
            Female,
            Shemale,
            Butch
        }

        public static string GetGenderString(Gender eGender)
        {
            switch (eGender)
            {
                case Gender.Male:
                    return "Male";
                case Gender.Female:
                    return "Female";
                case Gender.Shemale:
                    return "Shemale";
                case Gender.Butch:
                    return "Butch";
                default:
                    return "Gender unknown!";
            }
        }
    }
}
