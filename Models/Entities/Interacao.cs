namespace DeskFlow.API.Models.Entities
{
    // NOTA: Essa tabela vai guardar o histórico de conversa dentro do chamado.
    public class Interacao
    {
        public int Id { get; set; }
        public string Autor { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataRegistro { get; set; }

        // NOTA: Referência ao Chamado (Chave Estrangeira).
        public int ChamadoId { get; set; }
        public Chamado Chamado { get; set; }
    }
}