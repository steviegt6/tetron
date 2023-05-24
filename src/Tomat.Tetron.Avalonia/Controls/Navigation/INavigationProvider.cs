﻿using System;
using System.Threading.Tasks;

namespace Tomat.Tetron.Avalonia.Controls.Navigation; 

public interface INavigationProvider {
    object? CurrentContext { get; }

    ValueTask NavigateAsync<TContext>() where TContext : class {
        return NavigateAsync<TContext>(_ => true, () => throw new InvalidOperationException());
    }

    ValueTask NavigateAsync<TContext>(Predicate<TContext> predicate, Func<TContext> factory) where TContext : class;

    ValueTask RemoveAllAsync<TContext>(Predicate<TContext> predicate, bool goBack = false) where TContext : class;

    ValueTask<TContext?> FindAsync<TContext>(Predicate<TContext> predicate) where TContext : class;

    ValueTask GoBackAsync();
}
