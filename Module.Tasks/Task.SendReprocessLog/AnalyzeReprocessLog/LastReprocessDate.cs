namespace Task.SendReprocessLog
{
    public class LastReprocessDate
    {
        public static string GetLastReprocessDate(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Date"))
                {
                    // Znajdujemy pozycję początku daty
                    int startIndex = lines[i].IndexOf(":") + 2;

                    // Wyciągamy 21 znaków (format yy-mm-dd [hh:mm, Eur])
                    string date = lines[i].Substring(startIndex, 21);
                    return date;
                }
            }

            return "yy-mm-dd [hh:mm, Eur]"; 
        }
    }
}