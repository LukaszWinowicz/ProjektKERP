namespace Task.SendReprocessLog
{
    public class AnalyzeReprocessLog
    {
        public List<ProductionOrder> GetProductionOrdersWithError(List<string> filesPath)
        {
            List<ProductionOrder> orders = new List<ProductionOrder>();
            List<ReprocessDate> dates = new List<ReprocessDate>();

            foreach (var file in filesPath)
            {
                string[] lines = File.ReadAllLines(file);
                string reprocessName = Path.GetFileName(file);

                // 1. Pobranie daty reprocesu danych zleceń produkcyjnych 
                string lastReprocessDate = LastReprocessDate.GetLastReprocessDate(lines);
                Console.WriteLine(lastReprocessDate);

                // 2. Jeżeli data sięga maksymalnie tydzień wstecz to należy taki plik analizować, jeśli nie to go pomijamy
                // lub trzecia opcja, gdy pojawił się błąd podczas parsowania i wtedy zapisze się komunikat w logach
                var result = CheckGenerationReprocessDate.CheckDate(lastReprocessDate);
                switch (result)
                {
                    case ReprocessDateResult.Success:
                        Console.WriteLine("Data mieści się w ostatnim tygodniu.");
                        break;

                    case ReprocessDateResult.DateTooOld:
                        Console.WriteLine("Data jest starsza niż tydzień.");
                        break;

                    case ReprocessDateResult.ParseError:
                        Console.WriteLine("Błąd parsowania daty.");
                        break;
                }

                // 3. Analiza poszczególnych itemów
                // wybieram linię item + linię poniżej
                // następnie sprawdzam poniżej czy jest no jak jest to zapisuję do tablicy tymczasowej i taką tablicę zwracam
                // tutaj pętla trwa aż wejdziemy do kolejnego pola "Item"

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("Item"))
                    {
                        
                    }
                }

                // 4. Jak wszystko się skończy i mam przygotowaną listę to zapisuję do pliku txt


            }

            // Wyświetlanie wyników
            foreach (var date in dates)
            {
                Console.WriteLine($"Reprocess Date: {date.Date}, Line: {date.Line}, Reprocess Name: {date.ReprocessName}");
            }

            Console.ReadKey();

            return orders;
        }

    }

    public class ReprocessDate
    {
        public string Date { get; set; }
        public int Line { get; set; }
        public string ReprocessName { get; set; }
    }

    public class ProductionOrder
    {
        public string ReprocessDate { get; set; }
        public string ReprocessName { get; set; }
        public string Item { get; set; }
        public string OrderNumber { get; set; }
        public string Reprocessed { get; set; }
        public string ErrorMessage { get; set; }
    }
}