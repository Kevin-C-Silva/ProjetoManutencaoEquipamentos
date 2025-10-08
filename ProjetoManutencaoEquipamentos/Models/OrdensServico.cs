namespace ProjetoManutencaoEquipamentos.Models
{
    public class OrdensServico
    {
        public int id_ordem { get; set; }
        public int id_equipamento { get; set; }
        public int id_tecnico { get; set; }
        public string? mensagem { get; set; }
        public string? situacao { get; set; }
        public DateTime criado_em { get; set; }

    }
}
