using System;
using System.Collections.Generic;
using System.Text;

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

    public enum EntidadeEnum
    {
        Nenhum,
        Protagonista,
    }
}
