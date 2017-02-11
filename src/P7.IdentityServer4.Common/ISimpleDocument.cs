namespace P7.IdentityServer4.Common
{
    public interface ISimpleDocument
    {
        object Document { get; }
        string DocumentJson { get; }
    }
}