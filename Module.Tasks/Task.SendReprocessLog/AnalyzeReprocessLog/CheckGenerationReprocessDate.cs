namespace Task.SendReprocessLog
{
	public class CheckGenerationReprocessDate
	{
		public static ReprocessDateResult CheckDate(string lastReprocessDate)
		{
            // Wyciągnięcie pierwszych 8 znaków formatu yy-MM-dd
            string datePart = lastReprocessDate.Substring(0, 8);

            // Konwersja stringa w formie yy-MM-dd na DateTime
            DateTime parsedDate;
            bool isParsed = DateTime.TryParseExact(
                datePart,
                "yy-MM-dd",
                null,
                System.Globalization.DateTimeStyles.None,
                out parsedDate
            );

            // Jeśli konwersja się nie powiodła, zwraca false
            if (isParsed == false)
            {
				return ReprocessDateResult.ParseError;
            }

			// Obliczenie daty sprzed tygodnia
			DateTime oneWeekAgo = DateTime.Now.AddDays(-7);

			// Sprawdzenie czy data jest w ciagu ostatniego tygodnia
			bool isGeneratedLastWeek = parsedDate >= oneWeekAgo && parsedDate <= DateTime.Now;

            if (isGeneratedLastWeek == true)
            {
                return ReprocessDateResult.Success;
            }
            else 
            {
                return ReprocessDateResult.DateTooOld;
            }
		}
	}
}