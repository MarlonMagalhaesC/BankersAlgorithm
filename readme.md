# BankersAlgorithm
## Tecnologias utilizadas

- C#
- .NET

## Estrutura do projeto

- `Program.cs`: ponto de entrada da aplicação
- `Models/Bank.cs`: lógica principal do algoritmo do banqueiro
- `Workers/CustomerWorker.cs`: simulação dos clientes executados concorrentemente

## Requisitos

É necessário ter o .NET SDK 7.0 instalado na máquina.

## Compilação

No terminal, dentro da pasta do projeto (onde fica BankersAlgorithm.csproj), execute:

```bash
dotnet build
```

## Execução 

Ainda no mesmo caminho que na compilação, para executar o programa, informe na linha de comando a quantidade inicial de cada tipo de recurso disponível no sistema.

Exemplo:

```bash
dotnet run -- 10 5 7
```
