using SuperStore.Shared;

namespace SuperStore.carts.Messages;

public record FundsMessage(long CustomerId, decimal CurrentFunds) : IMessage;