using System;

namespace P7.Store
{
    public interface IDocumentBase
    {
        Guid Id_G { get; }
        string Id { get; }
    }
}