using System;
using System.Globalization; 
using System.Text;


namespace Evaluation2.Models
{
    public class Utile{   
        public static string[] CustomSplit(string input, char delimiter)
        {
            List<string> parts = new List<string>();
            StringBuilder currentPart = new StringBuilder();
            bool insideQuotes = false;

            foreach (char c in input)
            {
                if (c == '"')
                {
                    insideQuotes = !insideQuotes;
                }
                else if (c == delimiter && !insideQuotes)
                {
                    parts.Add(currentPart.ToString());
                    currentPart.Clear();
                }
                else
                {
                    currentPart.Append(c);
                }
            }

            parts.Add(currentPart.ToString());

            for (int i = 0; i < parts.Count; i++)
            {
                if (parts[i].StartsWith('"') && parts[i].EndsWith('"'))
                {
                    parts[i] = parts[i].Trim('"');
                }
            }

            return parts.ToArray();
        }
    }
}