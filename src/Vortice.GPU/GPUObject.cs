// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Reflection;
using System.Runtime.CompilerServices;

namespace Vortice.GPU;

public abstract class GPUObject : IDisposable
{
#if NET6_0_OR_GREATER
    private volatile uint _isDisposed;
#else
    private volatile int _isDisposed;
#endif
    private string _label;

    /// <summary>
    /// Initializes a new instance of the <see cref="GPUObject" /> class.
    /// </summary>
    /// <param name="label">The label of the object or <c>null</c> to use <see cref="MemberInfo.Name" />.</param>
    protected GPUObject(string? label = default)
    {
        _isDisposed = 0;
        _label = label ?? GetType().Name;
    }

    /// <summary>
    /// Gets <c>true</c> if the object has been disposed; otherwise, <c>false</c>.
    /// </summary>
    public bool IsDisposed => _isDisposed != 0;

    /// <summary>
    /// Gets or set the label that identifies the resource.
    /// </summary>
    public string Label
    {
        get => _label;
        set
        {
            _label = value ?? GetType().Name;
            OnLabelChanged(_label);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (Interlocked.Exchange(ref _isDisposed, 1) == 0)
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
    }

    protected virtual void OnLabelChanged(string newLabel)
    {
    }

    /// <inheritdoc cref="Dispose()" />
    /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
    protected abstract void Dispose(bool isDisposing);

    /// <inheritdoc />
    public override string ToString() => _label;

    /// <summary>Throws an exception if the object has been disposed.</summary>
    /// <exception cref="ObjectDisposedException">The object has been disposed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void ThrowIfDisposed()
    {
        if (_isDisposed != 0)
        {
            throw new ObjectDisposedException(_label);
        }
    }

    /// <summary>Marks the object as being disposed.</summary>
    protected void MarkDisposed()
    {
        _ = Interlocked.Exchange(ref _isDisposed, 1);
    }
}
