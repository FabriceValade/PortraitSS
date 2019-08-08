using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.Model
{
    public class Gender
    {
        public enum GenderValue { Male,Female}
        public static string Male = "Male";
        public static string Female = "Female";
        public static string MalePropertyName = "standard_male";
        public static string FemalePropertyName = "standard_female";

        public GenderValue Value { get; set; } = GenderValue.Male;

        public Gender() { }
        public Gender(Gender other) { Value = other.Value; }

        public override string ToString()
        {
            if (Value == GenderValue.Male)
                return Gender.Male;
            else
                return Gender.Female;
        }
        public string ToPropertyName()
        {
            if (Value == GenderValue.Male)
                return Gender.MalePropertyName;
            else
                return Gender.FemalePropertyName;
        }

    }
}
