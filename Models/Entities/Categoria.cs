using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; //NOTA: Biblioteca que contém as regras de validação

// NOTA: O namespace serve para organizar os arquivos. É como se fosse o endereço dessa classe no projeto.
namespace DeskFlow.API.Models.Entities
{
    // NOTA: public class significa que essa classe pode ser acessada e instanciada por outras partes do sistema.
    public class Categoria
    {
        [Key] //NOTA: Indica explicitamente que é uma chave primária (Primary Key)
        // NOTA: Toda tabela no banco precisa de uma chave primária. O Entity Framework entende que 'Id' é a PK.
        public int Id { get; set; }
        
        // CORREÇÃO (RNF04): Protegendo a entidade. O nome não pode ser nulo e tem tamanho máximo definido.
        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; } // NOTA: O get pega o valor da variável e o set define o valor. 
        
        // NOTA: Uma categoria pode ter vários chamados. Por isso usamos uma List. 
        // Instanciei com 'new List' para evitar o erro de NullReferenceException depois.
        public List<Chamado> Chamados { get; set; } = new List<Chamado>();
    }
}