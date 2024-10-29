namespace Task.SendReprocessLog
{
    public enum ReprocessDateResult
    {
        Success,         // Data jest poprawna i mieści się w ostatnim tygodniu
        DateTooOld,      // Data poprawna, ale nie mieści się w ostatnim tygodniu
        ParseError       // Błąd parsowania daty
    }
}