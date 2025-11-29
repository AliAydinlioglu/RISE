namespace Rise.Domain.Common;

public class Price : ValueObject
{
    public decimal? Student { get; }
    public decimal? Extern { get; }

    private Price() {} 
    
    private Price(decimal? forStudent, decimal? forExtern)
    {
        Validate(forStudent, nameof(Student));
        Validate(forExtern, nameof(Extern));
        
        if (forStudent > forExtern)
            throw new ArgumentException("Student price cannot exceed extern price.");

        Student = forStudent;
        Extern = forExtern;
    }

    private static void Validate(decimal? amount, string field)
    {
        if (!amount.HasValue)
            return;
        
        Guard.Against.NegativeOrZero(amount.Value, $"{field} cannot be negative.");

        if (decimal.Round(amount.Value, 2) != amount)
            throw new ArgumentException($"{field} must have max 2 decimals.");
    }

    public static Price ForMenuItem(decimal? forStudent, decimal? forExtern)
        => new (forStudent, forExtern);

    public static Price ForPriceListItem(decimal forStudent, decimal? forExtern)
        => new (forStudent, forExtern);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Student;
        yield return Extern;
    }
}