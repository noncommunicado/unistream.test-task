namespace Unistream.Transaction.Domain.SharedModels;

public sealed record InsertCommandResult(decimal ClientBalance, DateTime InsertDateTime);