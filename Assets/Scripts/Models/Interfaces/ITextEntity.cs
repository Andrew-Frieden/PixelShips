using UnityEngine.XR.WSA.Persistence;

namespace Models
{
    public interface ITextEntity
    {
        string Id { get; }
        string GetLinkText();
    }
}