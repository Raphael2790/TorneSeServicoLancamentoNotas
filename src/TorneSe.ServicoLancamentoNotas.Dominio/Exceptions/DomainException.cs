﻿namespace TorneSe.ServicoLancamentoNotas.Dominio.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException() {}
    protected DomainException(string message) : base(message) {}
    protected DomainException(string message, Exception innerException) : base(message, innerException) {}
}
