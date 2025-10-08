namespace ProjetoManutencaoEquipamentos.Models
{
    public class Equipamentos
    {
        public int id_equipamento { get; set; }
        public int id_usuario { get; set; }
        public string? nome { get; set; }
        public string? modelo { get; set; }
        public string? foto { get; set; }
        public string? situacao { get; set; }
        public DateTime criado_em { get; set; }
    }
}
