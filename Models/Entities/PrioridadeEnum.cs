namespace DeskFlow.API.Models.Enums
{
    // NOTA: Enums limitam as opções possíveis. O banco gravará os números (1, 2, 3), economizando espaço de armazenamento e acelerando os índices das tabelas.
    public enum PrioridadeEnum
    {
        Baixa = 1,
        Media = 2,
        Alta = 3
    }
}