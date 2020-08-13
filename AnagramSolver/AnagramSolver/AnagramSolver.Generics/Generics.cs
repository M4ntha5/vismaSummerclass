using AnagramSolver.Contracts.Enums;
using System;

namespace AnagramSolver.Generics
{
    public class Generics
    {
        public static Gender MapIntToGender(int value)
        {
            if (!Enum.TryParse(value.ToString(), out Gender result))
                throw new Exception();

            return result;
        }
        public static Gender MapStringToGender(string value)
        {
            if (!Enum.TryParse(value, out Gender result))
                throw new Exception();

            return result;
        }
        public static Weekday MapStringToWeekday(string value)
        {
            if (!Enum.TryParse(value.ToString(), out Weekday result))
                throw new Exception();

            return result;
        }

        public static E MapValueToEnum<E, T>(T value) where E : struct
        {
            if (!Enum.TryParse(value.ToString(), out E result))
                throw new Exception();

            return result;
        }


    }
}
