namespace Dashboard.BusinessLayer
{
    public class Dati_Economici
    {
        public Int64 Dati_Economici_Id { get; set; }
        public int? Tipo_Utenza_Id { get; set; }
        public int? Modalita_Vendita { get; set; }
        public double? Prezzo_F1 { get; set; }
        public double? Prezzo_F2 { get; set; }
        public double? Prezzo_F3 { get; set; }
        public int? Modalita_Acquisto { get; set; }
        public double? Costo_F1 { get; set; }
        public double? Costo_F2 { get; set; }
        public double? Costo_F3 { get; set; }
        public double? Altre_Spese { get; set; }
        public double? Altre_Spese_Investimento { get; set; }
        public double? Spese_Manutenzione { get; set; }
        public bool? Is_Detrazione { get; set; }
        public bool? Is_Finanziamento { get; set; }
        public double? Importo_Finanziamento { get; set; }
        public double? Interesse_Finanziamento { get; set; }
        public bool? Is_Conto { get; set; }
        public double? Conto_Capitale { get; set; }
        public int? Is_Prosumer { get; set; }
        public Int64? Utenza_Id { get; set; }
    }

}
