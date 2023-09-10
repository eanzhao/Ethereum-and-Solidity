namespace EAS.Section1;

public class Transaction
{
    public decimal Value { get; set; }
    public string From { get; set; }
    public string To { get; set; }

    public override string ToString()
    {
        return $"{From} transfers {Value} tokens to {To}";
    }
}

public class TransactionList
{
    public List<Transaction> Transactions { get; set; }

    public override string ToString()
    {
        return Transactions.Aggregate(string.Empty, (current, tx) => current + $"{tx}\n");
    }
}