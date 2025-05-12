namespace LoginAPI.Models
{
    public enum Role
    {
        Admin,
        Kurir,
        User
    }

    public enum LoginState
    {
        Awal,
        Validasi,
        Berhasil,
        Gagal
    }

    public enum LoginTrigger
    {
        Submit,
        Valid,
        Invalid
    }
}
