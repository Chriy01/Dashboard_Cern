namespace Dashboard.BusinessLayer
{
    public class Dati_Impianto
    {
        public Int64 Dati_Impianto_Id { get; set; }
        public string? Longitudine { get; set; }
        public string? Latitudine { get; set; }
        public double? Potenza_Impianto { get; set; }
        public bool? Is_Abilitato_Rinnovabile { get; set; }
        public double? Quota_Potenza_Rinnovabile { get; set; }
        public double? Potenza_Inverter { get; set; }
        public double? Costo_Impianto { get; set; }
        public double? Capacita_Batteria { get; set; }
        public bool? Is_Costo_KW { get; set; }
        public double? Costo_Totale { get; set; }
        public double? Costo_KW { get; set; }
        public double? Is_Escluso_Premio { get; set; }
        public double? Area_Impianto { get; set; }
        public int? Tipologia_Impianto { get; set; }
        public string? Data_Esercizio { get; set; }
        public bool? Is_Seconda_Falda { get; set; }
        public double? Potenza_Sezione { get; set; }
        public double? Angolo_di_tilt { get; set; }
        public double? Angolo_di_Azimut { get; set; }
        public double? Potenza_Sezione_S { get; set; }
        public double? Angolo_di_tilt_S { get; set; }
        public double? Angolo_di_Azimut_S { get; set; }
        public double? Efficienza { get; set; }
        public double? Coefficiente_T { get; set; }
        public double? NOCT { get; set; }
        public double? Fattore_Riduzione { get; set; }
        public double? Efficienza_Inverter { get; set; }
        public double? Costo_Ricambio_Batt { get; set; }
        public double? Altre_Perdite { get; set; }
        public int? Is_Prosumer { get; set; }
        public Int64? Utenza_Id { get; set; }
    }

}
