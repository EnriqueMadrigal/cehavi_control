using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace cehavi_control
{

    public static class TextFilters
    {
        public static string
            intPattern = "(?<Number>[0-9])",
            decPattern = @"(?<Number>^[0-9]*\.?[0-9]*)",
            currPattern = @"^\$?(?<Number>[0-9]*\.?[0-9]{0,2})";

        public static string GetNumber(string RegexPattern, string SourceString, bool PosOnly = false)
        {
            string newNumber = string.Empty;
            if (!PosOnly)
                if (SourceString.StartsWith("-"))
                    newNumber += "-";

            SourceString = SourceString.Replace("-", string.Empty);

            Regex r = new Regex(RegexPattern);
            Match m = r.Match(SourceString);
            while (m.Success)
            {
                newNumber += m.Groups["Number"].Value;
                m = m.NextMatch();
            }

            return newNumber;
        }

        public static void SetControlText(System.Windows.Controls.TextBox TextBoxControl, string FilteredString)
        {
            int cursorPos = TextBoxControl.SelectionStart; // Get the cursor position
            string textBoxContent = TextBoxControl.Text;

            if (FilteredString.Length < textBoxContent.Length)// this might mean that an invalid character was entered part way through the string
            {
                cursorPos--; // Step back one character
                textBoxContent = textBoxContent.Remove(cursorPos, 1); // Remove the offending character
                TextBoxControl.Text = textBoxContent; // Set the string
            }
            else
                TextBoxControl.Text = FilteredString;

            if (cursorPos >= TextBoxControl.Text.Length) // If the cursor was at the end of the text
                TextBoxControl.SelectionStart = TextBoxControl.Text.Length;
            else
                TextBoxControl.SelectionStart = cursorPos;
        }

    }

}
