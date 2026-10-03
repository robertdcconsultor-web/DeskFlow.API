using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DeskFlow.API.Models.Enums; // Adicionado para enxergar os Enums

namespace DeskFlow.API.Models.Entities
{
    // NOTA: Essa classe vai virar a tabela central do sistema.
    public class Chamado
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "O título do chamado é obrigatório.")]
        [MaxLength(150, ErrorMessage = "O título não pode exceder 150 caracteres.")]
        public string Titulo { get; set; }
        
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; }
        
        
        [Required]
        [MaxLength(20)]
        public PrioridadeEnum Prioridade { get; set; } // CORREÇÃO (Erros 8 e 9): Usando Enums para garantir a integridade.
        
        [Required]
        [MaxLength(20)]
        public StatusEnum Status { get; set; } // CORREÇÃO (Erros 10 e 11): Usando Enums para garantir a integridade.
        
        [Required(ErrorMessage = "O nome do solicitante é obrigatório.")]
        [MaxLength(100)]
        public string SolicitanteNome { get; set; }

        public DateTime DataAbertura { get; set; }
        
        // NOTA: DataFechamento e Solucao podem ser nulos (?) porque o chamado nasce aberto.
        public DateTime? DataFechamento { get; set; }
        public string? Solucao { get; set; }

        // NOTA: Foreign Key (Chave Estrangeira). O Entity Framework entende que esse campo aponta para a tabela Categoria.
        public int CategoriaId { get; set; }
        
        // NOTA: O "?" evita alertas de nulo do compilador para propriedades de navegação
        public Categoria? Categoria { get; set; }

        // NOTA: Relacionamento 1:N com Interacoes (um chamado tem vários comentários).
        public List<Interacao> Interacoes { get; set; } = new List<Interacao>();
    }
}