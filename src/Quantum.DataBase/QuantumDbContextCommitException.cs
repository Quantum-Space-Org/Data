using System;
using System.Runtime.Serialization;

namespace Quantum.DataBase.EntityFramework;

[Serializable]
internal class QuantumDbContextCommitException : Exception
{
    public int Result;

    public QuantumDbContextCommitException()
    {
    }

    public QuantumDbContextCommitException(int result)
    {
        Result = result;
    }

    public QuantumDbContextCommitException(string message) : base(message)
    {
    }

    public QuantumDbContextCommitException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected QuantumDbContextCommitException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}