using System;

namespace Calculator;

public class ParserException(string message) : Exception(message)
{
}
