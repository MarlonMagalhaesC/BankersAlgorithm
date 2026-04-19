# BankersAlgorithm
## Descrição

Este projeto simula o Algoritmo do Banqueiro, utilizado para evitar deadlocks no gerenciamento de recursos. O sistema possui múltiplos clientes executados por threads, que solicitam e liberam recursos concorrentemente. Cada solicitação é analisada e só é aprovada se mantiver o sistema em estado seguro.

## Tecnologias utilizadas

- C#
- .NET
- System.Threading

## Estrutura do projeto

- `Program.cs`: ponto de entrada da aplicação
- `Models/Bank.cs`: lógica principal do algoritmo do banqueiro
- `Workers/CustomerWorker.cs`: simulação dos clientes executados concorrentemente

## Requisitos

É necessário ter o .NET SDK instalado na máquina.

## Compilação

No terminal, dentro da pasta do projeto, execute:

```bash
dotnet build
