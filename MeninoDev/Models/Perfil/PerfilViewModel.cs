namespace MeninoDev.Models.Perfil
{
    public class PerfilViewModel
    {
        public PerfilViewModel()
        {
            Cadastro = new CadastroViewModel();
            TrocaSenha = new TrocarSenhaViewModel();
        }
        public CadastroViewModel Cadastro { get; set; }
        public TrocarSenhaViewModel TrocaSenha { get; set; }
    }
}
