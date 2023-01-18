using System;
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace pkNX.IO.Archives;

internal sealed class PositionPreservingWriteOnlyStreamWrapper : Stream
{
    private readonly Stream _stream;
    private long _position;

    /// <summary>
    /// Creates a wrapper for write-only (non-readable, non-seekable) streams that keeps track of <see cref="Position"/> and allows it to be read.
    /// </summary>
    /// <param name="stream">The underlying stream, which handles all actual writes.</param>
    public PositionPreservingWriteOnlyStreamWrapper(Stream stream)
    {
        _stream = stream;
    }

    public override bool CanRead => false;
    public override bool CanSeek => false;
    public override bool CanWrite => true;

    public override long Position
    {
        get => _position;
        set => throw new NotSupportedException();
    }
    public override void Write(byte[] buffer, int offset, int count)
    {
        _position += count;
        _stream.Write(buffer, offset, count);
    }

    public override void Write(ReadOnlySpan<byte> buffer)
    {
        _position += buffer.Length;
        _stream.Write(buffer);
    }

    public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
    {
        _position += count;
        return _stream.BeginWrite(buffer, offset, count, callback, state);
    }

    public override void EndWrite(IAsyncResult asyncResult) => _stream.EndWrite(asyncResult);

    public override void WriteByte(byte value)
    {
        _position += 1;
        _stream.WriteByte(value);
    }

    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        _position += count;
        return _stream.WriteAsync(buffer, offset, count, cancellationToken);
    }

    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        _position += buffer.Length;
        return _stream.WriteAsync(buffer, cancellationToken);
    }

    public override bool CanTimeout => _stream.CanTimeout;
    public override int ReadTimeout
    {
        get => _stream.ReadTimeout;
        set => _stream.ReadTimeout = value;
    }
    public override int WriteTimeout
    {
        get => _stream.WriteTimeout;
        set => _stream.WriteTimeout = value;
    }

    public override void Flush() => _stream.Flush();
    public override Task FlushAsync(CancellationToken cancellationToken) => _stream.FlushAsync(cancellationToken);

    public override void Close()
    {
        _stream.Close();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _stream.Dispose();
    }

    public override long Length => throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }
}
