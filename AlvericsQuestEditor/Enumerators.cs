namespace AlvericsQuestEditor
{
    /* Enumeração de todas as ações do menu */
    public enum Acao
    {
        Nenhum,
        IndicarEntidade,
        NovoMundo,
        Salvar,
        Carregar,
        AdicionarObjeto,
        ExcluirObjeto,
        GerenciarConexao,
        GerenciarPropriedades,
        GerenciarDialogos,
        GerenciarEventos,
        GerenciarMusicas,
        AumentarTempoInicio,
        ReduzirTempoInicio,
        AumentarTempoAtivo,
        ReduzirTempoAtivo,
        AumentarTempoEntreAtivacoes,
        ReduzirTempoEntreAtivacoes,
    }

    /* Enumeração dos tipos de entidade no que tange sua relação com outras entidades. 
     * Entidades tangíveis como armadilhas, inimigos e caixas podem se sobrepor à entidades intangíveis 
     * como o chão */
    public enum TipoEntidade
    {
        Tangivel,
        Intangivel,
    }

    /* Enumerador para o tipo de armadilha. Útil para definir se a armadilha contém valores para o tempo ativado */
    public enum TipoArmadilha
    {
        Espinhos,
        Atirador,
    }
}
