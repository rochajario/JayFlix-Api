namespace plataforma_videos_api.Constants
{
    public static class ErrorMessages
    {
        public const string ARGUMENTO_INVALIDO = "Parâmetro Inválido, {0} não pode ter valor nulo ou vazio.";
        public const string ARGUMENTO_INCOMPLETO = "Parâmetro Inválido, {0} precisa de ajustes, {1}.";
        public const string VIDEO_PRE_CADASTRADO = "Parâmetro Inválido, Já existe um vídeo cadastrado com o título {0}.";
        public const string ERRO_INESPERADO = "Ocorreu um erro Inesperado.";
        public const string SEM_RESULTADOS = "A busca realizada não retornou resultados.";
        public const string FALHA_AO_CADASTRAR = "Ocorreu uma Falha ao Cadastrar o Novo Item";
        public const string FALHA_AO_ATUALIZAR = "Ocorreu uma Falha ao Atualizar o Item";
        public const string CATEGORIA_PRE_CADASTRADA = "Parâmetro Inválido, Já existe uma categoria cadastrada com o título {0}.";
        public const string CATEGORIA_NECESSARIA = "Necessário Pré Cadastro da Categoria Informada";
    }
}
