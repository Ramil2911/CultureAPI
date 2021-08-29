using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MovieWebsite.Shared
{
    public class AuthOptions
    {
        public const string ISSUER = "cultureAuthServer"; // издатель токена
        public const string AUDIENCE = "cultureAuthClient"; // потребитель токена
        const string KEY = "*(&G(*&Gugb80puai8wuIAOii398h4j-U-0(J^*%KAC-0ikN(_%(&OKJ4&%*kjBL579gvuhjLJK";   // ключ для шифрации
        public const int LIFETIME = 60*12; // время жизни токена - 12 часов
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }

    public class RefreshOptions
    {
        public const string ISSUER = "cultureRefreshServer"; // издатель токена
        public const string AUDIENCE = "cultureRefreshClient"; // потребитель токена
        const string KEY = "bi;l@poub0*(B&E)(AUbfPAIFUGQ#*)FB#@_(F*N{@IO+!)@(_+ffhebfo8YBFpiobH(*F&EBP(E*FUBP(e(!B@!#(N=0hf0qfn9o";   // ключ для шифрации
        public const int LIFETIME = 60*24*31; // время жизни токена - 31 день
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}