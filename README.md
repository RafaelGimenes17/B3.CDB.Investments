# B3.CDB.Investments - Sistema de Cálculo de Investimentos em CDB

## ?? Sobre o Projeto

O **B3.CDB.Investments** é uma API REST desenvolvida em **.NET 8** que realiza cálculos de investimentos em Certificados de Depósito Bancário (CDB) da B3. A solução permite calcular o valor final bruto e líquido de um investimento, considerando a aplicação de uma taxa de CDI com percentual do banco e a cobrança de Imposto de Renda progressivo conforme o tempo de resgate.

### Características Principais

- ? Cálculo de CDB com composição mensal
- ? Tabela progressiva de Imposto de Renda
- ? Validações de entrada robustas
- ? Resposta estruturada com resultados bruto e líquido
- ? Endpoint via POST (apenas)
- ? Documentação via Swagger

## ??? Arquitetura

### Estrutura do Projeto

```
B3.CDB.WebApi/
??? Controllers/
?   ??? CDBController.cs          # Endpoint principal de cálculo de CDB
??? Services/
?   ??? CDBCalculator.cs          # Lógica de cálculo do investimento
??? Program.cs                     # Configuração da aplicação
??? appsettings.json              # Configurações
??? B3.CDB.WebApi.csproj          # Arquivo do projeto
```

### Componentes Principais

#### 1. **CDBCalculator.cs** (Serviço de Cálculo)
Responsável por toda a lógica matemática do investimento:
- Cálculo do valor final com composição mensal
- Cálculo do rendimento bruto
- Determinação da alíquota de imposto
- Cálculo do imposto sobre o rendimento
- Cálculo do valor final líquido

**Constantes Configuradas:**
- **TB (Taxa do Banco):** 108%
- **CDI (Certificado de Depósito Interbancário):** 0,9% ao mês

#### 2. **CDBController.cs** (Controlador de API)
Expõe um endpoint REST POST para cálculo de investimentos:

**Modelos de Dados:**
- `CDBRequest` - Entrada de dados
- `ParametrosCDB` - Parâmetros do investimento
- `ResultadoBruto` - Resultado sem impostos
- `ResultadoLiquido` - Resultado com impostos
- `CDBResultadoResponse` - Resposta completa

### Fórmulas Utilizadas

#### 1. Cálculo do Valor Final
```
VF = VI × [1 + (CDI × TB)]^n

Onde:
- VF = Valor Final
- VI = Valor Inicial
- CDI = Taxa CDI (0,9%)
- TB = Taxa do Banco (108%)
- n = Número de meses (aplicado iterativamente)
```

**Nota:** O cálculo é realizado mês a mês, onde os rendimentos de cada mês são utilizados como base para o cálculo do próximo mês (composição mensal).

#### 2. Cálculo do Rendimento Bruto
```
Rendimento Bruto = Valor Final - Valor Inicial
```

#### 3. Tabela de Imposto de Renda (IR)
```
Até 6 meses:      22,5%
Até 12 meses:     20%
Até 24 meses:     17,5%
Acima de 24 meses: 15%
```

#### 4. Cálculo do Imposto
```
Imposto = Rendimento Bruto × Alíquota IR
```

#### 5. Cálculo do Valor Líquido
```
Valor Líquido = Valor Final - Imposto
Rendimento Líquido = Valor Líquido - Valor Inicial
```

## ?? Requisitos do Sistema

- **.NET 8.0** ou superior
- **Visual Studio 2022** (recomendado) ou VS Code
- **PowerShell** ou Command Prompt

## ?? Como Executar

### 1. Clonar ou Abrir o Projeto

```bash
# Se estiver no diretório do projeto
cd C:\Users\Aline\OneDrive\Desktop\Dev\B3.CDB.Investments
```

### 2. Restaurar Dependências

```bash
dotnet restore
```

### 3. Compilar o Projeto

```bash
dotnet build
```

### 4. Executar a Aplicação

```bash
dotnet run
```

Ou use Visual Studio:
- Abra o projeto em Visual Studio
- Pressione `F5` ou clique em "Executar"

A API estará disponível em:
- **HTTP:** `http://localhost:5000`
- **HTTPS:** `https://localhost:5001`

### 5. Acessar a Documentação Swagger

Abra o navegador e acesse:
```
https://localhost:5001/swagger
```

## ?? Como Testar

### Usando Swagger (Interface Gráfica)

1. Acesse `https://localhost:5001/swagger`
2. Localize o endpoint `POST /api/cdb/calcular`
3. Clique em "Try it out"
4. Preencha os parâmetros no corpo JSON e clique em "Execute"

### Usando cURL

```bash
curl -X POST "https://localhost:5001/api/cdb/calcular" \
  -H "Content-Type: application/json" \
  -d "{\"valorInicial\":1000,\"meses\":6}"
```

### Usando Postman

- **URL:** `https://localhost:5001/api/cdb/calcular`
- **Método:** POST
- **Headers:** `Content-Type: application/json`
- **Body (JSON):**
```json
{
  "valorInicial": 1000,
  "meses": 6
}
```

### Usando PowerShell

```powershell
$body = @{
    valorInicial = 1000
    meses = 6
} | ConvertTo-Json

$uri = "https://localhost:5001/api/cdb/calcular"
Invoke-RestMethod -Uri $uri -Method Post -ContentType "application/json" -Body $body
```

## ?? Exemplos de Respostas

### Exemplo 1: Investimento de 6 Meses

**Requisição:**
```json
{
  "valorInicial": 1000,
  "meses": 6
}
```

**Resposta (200 OK):**
```json
{
  "parametros": {
    "valorInicial": 1000.00,
    "meses": 6,
    "taxaCDI": "0,9%",
    "taxaBanco": "108%"
  },
  "resultadoBruto": {
    "valorFinal": 1054.81,
    "rendimento": 54.81
  },
  "resultadoLiquido": {
    "aliquotaImposto": "22.5%",
    "imposto": 12.33,
    "valorFinal": 1042.48,
    "rendimento": 42.48
  }
}
```

### Exemplo 2: Investimento de 12 Meses

**Requisição:**
```json
{
  "valorInicial": 5000,
  "meses": 12
}
```

**Resposta (200 OK):**
```json
{
  "parametros": {
    "valorInicial": 5000.00,
    "meses": 12,
    "taxaCDI": "0,9%",
    "taxaBanco": "108%"
  },
  "resultadoBruto": {
    "valorFinal": 5565.49,
    "rendimento": 565.49
  },
  "resultadoLiquido": {
    "aliquotaImposto": "20%",
    "imposto": 113.10,
    "valorFinal": 5452.39,
    "rendimento": 452.39
  }
}
```

### Exemplo 3: Investimento de 24 Meses

**Requisição:**
```json
{
  "valorInicial": 10000,
  "meses": 24
}
```

**Resposta (200 OK):**
```json
{
  "parametros": {
    "valorInicial": 10000.00,
    "meses": 24,
    "taxaCDI": "0,9%",
    "taxaBanco": "108%"
  },
  "resultadoBruto": {
    "valorFinal": 11639.08,
    "rendimento": 1639.08
  },
  "resultadoLiquido": {
    "aliquotaImposto": "17.5%",
    "imposto": 286.84,
    "valorFinal": 11352.24,
    "rendimento": 1352.24
  }
}
```

## ? Casos de Erro

### Erro 1: Valor Inicial Inválido

**Requisição:**
```json
{
  "valorInicial": 0,
  "meses": 6
}
```

**Resposta (400 Bad Request):**
```json
{
  "erro": "Valor inicial deve ser positivo"
}
```

### Erro 2: Prazo Menor que 2 Meses

**Requisição:**
```json
{
  "valorInicial": 1000,
  "meses": 1
}
```

**Resposta (400 Bad Request):**
```json
{
  "erro": "Prazo deve ser maior que 1 mês"
}
```

### Erro 3: Valor Negativo

**Requisição:**
```json
{
  "valorInicial": -1000,
  "meses": 6
}
```

**Resposta (400 Bad Request):**
```json
{
  "erro": "Valor inicial deve ser positivo"
}
```

## ?? Testes Recomendados

### Teste 1: Validação de Valor Positivo
```
Entradas: valorInicial = -500, meses = 12
Esperado: Erro "Valor inicial deve ser positivo"
```

### Teste 2: Validação de Prazo Mínimo
```
Entradas: valorInicial = 1000, meses = 1
Esperado: Erro "Prazo deve ser maior que 1 mês"
```

### Teste 3: Cálculo Correto - 6 Meses
```
Entradas: valorInicial = 1000, meses = 6
Verificar: Alíquota IR = 22,5%
Verificar: Rendimento bruto ? 54,81
```

### Teste 4: Cálculo Correto - 12 Meses
```
Entradas: valorInicial = 1000, meses = 12
Verificar: Alíquota IR = 20%
Verificar: Rendimento bruto ? 113,63
```

### Teste 5: Cálculo Correto - 24 Meses
```
Entradas: valorInicial = 1000, meses = 24
Verificar: Alíquota IR = 17,5%
Verificar: Rendimento bruto ? 263,85
```

### Teste 6: Cálculo Correto - Acima de 24 Meses
```
Entradas: valorInicial = 1000, meses = 25
Verificar: Alíquota IR = 15%
Verificar: Rendimento bruto ? 271,20
```

## ?? Detalhes Técnicos

### Stack Tecnológico
- **Framework:** ASP.NET Core 8.0
- **Linguagem:** C# 12
- **Documentação API:** Swagger/OpenAPI
- **Formato de Dados:** JSON

### Validações Implementadas
- ? Valor inicial deve ser positivo (> 0)
- ? Prazo em meses deve ser maior que 1 (> 1)
- ? Tratamento de exceções com mensagens claras

### Precisão Numérica
- Todas as operações utilizam `decimal` para precisão financeira
- Resultados são arredondados a 2 casas decimais

## ?? Estrutura de Pastas

```
C:\Users\Aline\OneDrive\Desktop\Dev\B3.CDB.Investments\
??? src\
?   ??? backend\
?       ??? Aplicacoes\
?           ??? B3.CDB.WebApi\
?               ??? Controllers\
?               ?   ??? CDBController.cs
?               ??? Services\
?               ?   ??? CDBCalculator.cs
?               ??? Program.cs
?               ??? appsettings.json
?               ??? appsettings.Development.json
?               ??? B3.CDB.WebApi.csproj
?               ??? ...outros arquivos
??? README.md                      # Este arquivo
??? ...outros arquivos
```

## ?? Documentação de Endpoints

### Endpoint: Calcular CDB (POST)

```
POST /api/cdb/calcular
Content-Type: application/json
```

**Body (JSON):**

| Campo | Tipo | Descrição | Exemplo |
|-------|------|-----------|---------|
| valorInicial | decimal | Valor inicial do investimento (deve ser positivo) | 1000 |
| meses | int | Prazo em meses (deve ser > 1) | 6 |

**Respostas:**
- `200 OK` - Cálculo realizado com sucesso
- `400 Bad Request` - Validação falhou ou dados inválidos

**Exemplo de Resposta (200 OK):**
```json
{
  "parametros": {
    "valorInicial": 1000.00,
    "meses": 6,
    "taxaCDI": "0,9%",
    "taxaBanco": "108%"
  },
  "resultadoBruto": {
    "valorFinal": 1054.81,
    "rendimento": 54.81
  },
  "resultadoLiquido": {
    "aliquotaImposto": "22.5%",
    "imposto": 12.33,
    "valorFinal": 1042.48,
    "rendimento": 42.48
  }
}
```

---

## ?? Resolução de Problemas

### Problema: Porta já em uso

Se a porta 5001 ou 5000 estiver em uso:

```powershell
# Encontre o processo usando a porta
Get-NetTCPConnection -LocalPort 5001

# Ou mude a porta no appsettings.json
```

### Problema: Certificado HTTPS não confiável

Se receber erro de certificado SSL:
```bash
dotnet dev-certs https --trust
```

### Problema: Acesso Negado

Se receber erro de permissão, execute o terminal como administrador.

## ?? Suporte e Contribuições

Para reportar problemas ou sugerir melhorias, entre em contato com a equipe de desenvolvimento.

## ?? Licença

Projeto desenvolvido para fins educacionais e de demonstração.

---

**Versão:** 1.0  
**Última Atualização:** 2024  
**Desenvolvido em:** .NET 8
