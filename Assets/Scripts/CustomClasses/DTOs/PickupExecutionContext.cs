using System;
using System.Collections.Generic;
using UnityEngine;

// Describes the transaction details for pick ups
public sealed class PickupExecutionContext : BaseInteractionContext
{
    readonly IReadOnlyDictionary<Type, IPickupHandler> _handlers;
    public PickupTransactionData PickupTransactionData { get; }

    public PickupExecutionContext(PlayerMain player, PickupTransactionData pickup = null) : base(player)
    {
        PickupTransactionData = pickup;
        _handlers = player.GetPickupHandlers();
    }


    public bool TryGet<T>(out T handler) where T : class, IPickupHandler
    {
        if (_handlers.TryGetValue(typeof(T), out var found))
        {
            handler = found as T;
            return handler != null;
        }

        handler = null;
        return false;
    }
}





