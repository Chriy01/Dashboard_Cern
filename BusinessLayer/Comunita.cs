namespace Dashboard.BusinessLayer
{
    public class Comunita
    {
        public int Comunita_Id { get; set; }
        public string Nome { get; set; }
        public int Anno_di_riferimento { get; set; }
        public bool IsPersonalizzato { get; set; }
        public string Zona_di_mercato { get; set; }
        public bool IsComunita { get; set; }
        public double Tasso_inflazione_mercato { get; set; }
        public double Tasso_interesse_mercato { get; set; }
        public string Zona_geografica { get; set; }
    }
}
