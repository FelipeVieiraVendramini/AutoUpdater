namespace Core.Interfaces
{
    /// <summary>
    /// This interface is used to define a cipher for packet processing. The socket systems use this interface to
    /// decrypt the header and body of packets. All packet ciphers should implement this method.
    /// </summary>
    public interface ICipher
    {
        byte[] Decrypt(byte[] buffer, int length);
        void Decrypt(byte[] packet, byte[] buffer, int length, int position);
        byte[] Encrypt(byte[] packet, int length);
        void GenerateKeys(int account, int authentication);
        void KeySchedule(byte[] key);
    }
}
