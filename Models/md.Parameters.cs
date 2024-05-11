using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dashboard.Models
{
    public class ParametersMd
    {
        public long Count_Famiglie { get; set; }

        public string Posizione { get; set; } = "nord";

        public double Max_power { get; set; } = 3;

        public string Classe_energetica { get; set; } = "A+";

        public double Media_metriq { get; set; } = 100;

        public long Step { get; set; } = 60;

        public string Tipo { get; set; } = "parameter";

        public double Portata { get; set; } = 2;

        public double Portata_Min { get; set; } = 2;

        public double Portata_Max { get; set; } = 2;

        public double Count { get; set; } = 1;

        public string TipoFotovoltaico { get; set; } = "fixed";

        public long CountFotovoltaico { get; set; } = 2;

        public string JavascriptToRun { get; set; } = "";
    }
}
