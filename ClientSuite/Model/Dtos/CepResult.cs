namespace ClientSuite.Model.Dtos
{
    public sealed record CepResult(
       string Cep, string State, string City, string? Neighborhood, string? Street, string Service);

}
