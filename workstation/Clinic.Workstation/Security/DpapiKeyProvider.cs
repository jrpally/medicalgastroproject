namespace Clinic.Workstation.Security;

public sealed class DpapiKeyProvider
{
    public byte[] Protect(byte[] plaintext)
    {
        // TODO: Wrap Windows DPAPI ProtectedData.Protect implementation.
        return plaintext;
    }

    public byte[] Unprotect(byte[] ciphertext)
    {
        // TODO: Wrap Windows DPAPI ProtectedData.Unprotect implementation.
        return ciphertext;
    }
}
