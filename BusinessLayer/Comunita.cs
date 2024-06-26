namespace Dashboard.BusinessLayer
{
    public class Comunita
    {
        public int Comunita_Id { get; set; }
        public string Nome { get; set; }
        public int Anno_di_riferimento { get; set; }
        public bool IsPersonalizzato { get; set; }
        public string Zona_di_mercato { get; set; }
        public bool iscomunita { get; set; }
        public double tasso_inflazione_mercato { get; set; }
        public double tasso_interesse_mercato { get; set; }
        public string zona_geografica { get; set; }
    }
}
