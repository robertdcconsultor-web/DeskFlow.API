using System;
using System.ComponentModel.DataAnnotations;

namespace DeskFlow.API.Models.Entities
{
    // NOTA: Essa tabela vai guardar o histórico de conversa dentro do chamado.
    public class Interacao
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "O autor é obrigatório.")]
        [MaxLength(100)]
        public string Autor { get; set; }
        
        [Required(ErrorMessage = "A mensagem é obrigatória.")]
        [MaxLength(1000, ErrorMessage = "A mensagem não pode exceder 1000 caracteres.")]
        public string Mensagem { get; set; }
        public DateTime DataRegistro { get; set; }

        // NOTA: Referência ao Chamado (Chave Estrangeira).
        public int ChamadoId { get; set; }
        public Chamado? Chamado { get; set; }
    }
}