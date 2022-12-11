namespace Wealth.Domain.Securities;

public sealed record SecurityDto(string Id, string Name, DateTime Modified, int Term, bool Deleted);