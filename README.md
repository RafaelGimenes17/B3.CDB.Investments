# B3.CDB.Investments - Sistema de Cálculo de Investimentos em CDB

## 📋 Sobre o Projeto

O **B3.CDB.Investments** é uma solução full-stack composta por uma **API REST em .NET 8** e uma **SPA em Angular 17** que realiza cálculos de investimentos em Certificados de Depósito Bancário (CDB) da B3. A solução permite calcular o valor final bruto e líquido de um investimento, considerando a aplicação de uma taxa de CDI com percentual do banco e a cobrança de Imposto de Renda progressivo conforme o tempo de resgate.

### **Autor**
- ** Rafael Fernando Gimenes **
- ** rafael.gimenes17@gmail.com **

### Características Principais

- ✅ Cálculo de CDB com composição mensal
- ✅ Tabela progressiva de Imposto de Renda
- ✅ Validações de entrada robustas
- ✅ Resposta estruturada com resultados bruto e líquido
- ✅ Endpoint via POST (apenas)
- ✅ Documentação via Swagger
- ✅ Interface web em Angular com formulário reativo
- ✅ Testes unitários no frontend (Karma/Jasmine)

---

## 🏗️ Arquitetura

### Estrutura do Projeto

```
B3.CDB.Investments/
├── src/
│   ├── backend/
│   │   └── Aplicacoes/
│   │       └── B3.CDB.WebApi/
│   │           ├── Controllers/
│   │           │   └── CDBController.cs       # Endpoint REST (apenas roteamento)
│   │           ├── Services/
│   │           │   ├── ICDBService.cs         # Interface de serviço (DIP)
│   │           │   ├── CDBService.cs          # Orquestração e montagem do resultado
│   │           │   └── CDBCalculator.cs       # Cálculos matemáticos do CDB
│   │           ├── DTOs/
│   │           │   ├── CalculoCDBRequest.cs
│   │           │   ├── CDBResultadoResponse.cs
│   │           │   └── ResultadoDetalhadoResponse.cs
│   │           ├── Configurations/
│   │           │   ├── ApiConfig.cs
│   │           │   └── SwaggerConfig.cs
│   │           └── Program.cs
│   └── frontend/
│       └── Investments/
│           ├── src/app/
│           │   ├── cdb-calculator/
│           │   │   ├── cdb-calculator.component.ts
│           │   │   ├── cdb-calculator.component.html
│           │   │   ├── cdb-calculator.component.css
│           │   │   └── cdb-calculator.component.spec.ts  # Testes unitários
│           │   └── services/
│           │       └── cdb.service.ts
│           ├── karma.conf.js
│           └── tsconfig.spec.json
└── README.md
```

---

## 🔧 Princípios SOLID Aplicados (Refatoração Recente)

### SRP — Single Responsibility Principle

O `CDBController` foi refatorado para ter **uma única responsabilidade**: receber a requisição HTTP, validar o `ModelState` e retornar a resposta. Toda a lógica de negócio (orquestração de cálculos, arredondamentos, montagem do DTO de resposta) foi extraída para o `CDBService`.

**Antes (anti-pattern):**
```csharp
// CDBController fazia tudo: roteamento + cálculos + montagem de resposta
decimal valorFinal = _cdbCalculator.CalcularValorFinal(...);
decimal rendimento = _cdbCalculator.CalcularRendimento(...);
decimal imposto    = _cdbCalculator.CalcularImposto(...);
// ... mais 3 chamadas e Math.Round espalhados no controller
```

**Depois (SRP aplicado):**
```csharp
// CDBController: apenas roteia e delega
var resultado = _cdbService.Calcular(request.ValorInicial, request.Meses);
return Ok(resultado);
```

### DIP — Dependency Inversion Principle

O controller agora depende da **abstração** `ICDBService`, não da implementação concreta. Isso permite trocar a implementação sem alterar o controller (ex: para testes, mocks, ou futuras implementações alternativas).

```csharp
// Interface (abstração)
public interface ICDBService
{
    CDBResultadoResponse Calcular(decimal valorInicial, int meses);
}

// Controller depende da interface, não da classe concreta
public CDBController(ICDBService cdbService) { ... }
```

### Injeção de Dependência (DI Container)

O binding entre interface e implementação é registrado no `Program.cs`:

```csharp
builder.Services.AddScoped<CDBCalculator>();
builder.Services.AddScoped<ICDBService, CDBService>();
```

---

## 🧮 Componentes do Backend

### 1. **ICDBService** (Interface)
Define o contrato do serviço de cálculo. Permite inversão de dependência e facilita testes com mocks.

### 2. **CDBService** (Implementação)
Responsável por:
- Orquestrar as chamadas ao `CDBCalculator`
- Aplicar arredondamentos financeiros (2 casas decimais)
- Converter alíquota para percentual
- Calcular o rendimento líquido (`rendimento - imposto`)
- Montar e retornar o `CDBResultadoResponse`

### 3. **CDBCalculator** (Cálculos Matemáticos)
Responsável pela matemática pura do investimento:
- Cálculo do valor final com composição mensal
- Cálculo do rendimento bruto
- Determinação da alíquota de imposto (tabela progressiva)
- Cálculo do imposto sobre o rendimento
- Cálculo do valor final líquido

**Constantes Configuradas:**
- **TB (Taxa do Banco):** 108%
- **CDI (Certificado de Depósito Interbancário):** 0,9% ao mês

### 4. **CDBController** (Controlador de API)
Responsabilidade única: receber a requisição HTTP, validar o `ModelState` e retornar a resposta HTTP adequada. Não contém lógica de negócio.

---

## 🧮 Fórmulas Utilizadas

### 1. Cálculo do Valor Final
```
VF = VI × [1 + (CDI × TB)]^n

Onde:
- VF = Valor Final
- VI = Valor Inicial
- CDI = Taxa CDI (0,9%)
- TB = Taxa do Banco (108%)
- n = Número de meses (aplicado iterativamente)
```

> O cálculo é realizado mês a mês, onde os rendimentos de cada mês são utilizados como base para o cálculo do próximo mês (composição mensal).

### 2. Cálculo do Rendimento Bruto
```
Rendimento Bruto = Valor Final - Valor Inicial
```

### 3. Tabela de Imposto de Renda (IR)
```
Até 6 meses:       22,5%
Até 12 meses:      20%
Até 24 meses:      17,5%
Acima de 24 meses: 15%
```

### 4. Cálculo do Imposto
```
Imposto = Rendimento Bruto × Alíquota IR
```

### 5. Cálculo do Valor Líquido
```
Valor Líquido    = Valor Final - Imposto
Rendimento Líquido = Rendimento Bruto - Imposto
```

---

## 🖥️ Requisitos do Sistema

### Backend
- **.NET 8.0** ou superior
- **Visual Studio 2022** (recomendado) ou VS Code

### Frontend
- **Node.js 18+** e **npm**
- **Angular CLI 17**
- **Google Chrome** (para execução dos testes com Karma)

---

## 🚀 Como Executar

### Backend (.NET)

```bash
# 1. Navegar até o projeto da API
cd src/backend/Aplicacoes/B3.CDB.WebApi

# 2. Restaurar dependências
dotnet restore

# 3. Compilar
dotnet build

# 4. Executar
dotnet run
```

A API estará disponível em:
- **HTTP:** `http://localhost:5000`
- **HTTPS:** `https://localhost:7257`
- **Swagger:** `https://localhost:7257/swagger`

### Frontend (Angular)

```bash
# 1. Navegar até o projeto frontend
cd src/frontend/Investments

# 2. Instalar dependências
npm install

# 3. Iniciar o servidor de desenvolvimento (com proxy para a API)
npm start
```

A aplicação estará disponível em `http://localhost:4200`.

> O proxy está configurado em `proxy.conf.json` para redirecionar `/api/*` para `http://localhost:7257`.

---

## 🧪 Testes

### Testes do Frontend (Angular — Karma/Jasmine)

O projeto possui **20 testes unitários** para o `CDBCalculatorComponent`, cobrindo:

| Grupo | O que é testado |
|---|---|
| Criação | Componente criado, estado inicial correto |
| Validação do formulário | Campos vazios, `valorInicial=0`, `meses=1`, dados válidos |
| Chamada ao serviço | Payload correto enviado ao `CDBService`, roteamento para `calcularInvestimento2` |
| Exibição no DOM | Valor bruto, valor líquido, imposto e alíquota renderizados |
| Tratamento de erro | Mensagem de erro no estado e no DOM quando a API falha |
| Limpar | Formulário, resultado e erro resetados |
| Formatadores | `formatarMoeda` e `formatarPercentual` com todas as alíquotas |

**Como rodar os testes do frontend:**

```bash
cd src/frontend/Investments

# Rodar uma vez (modo CI)
npm test -- --watch=false --browsers=ChromeHeadless

# Rodar em modo watch (desenvolvimento)
npm test
```

**Resultado esperado:**
```
TOTAL: 20 SUCCESS
```

> Os testes utilizam **mocks do `CDBService`** via `jasmine.createSpy`, garantindo que nenhuma chamada HTTP real é feita durante os testes.

---

### Testes do Backend (.NET)

> Se houver projetos de teste .NET na solução, execute:

```bash
# Na raiz da solução
dotnet test

# Com relatório detalhado
dotnet test --verbosity normal

# Com cobertura de código
dotnet test --collect:"XPlat Code Coverage"
```

---

## 📡 Documentação de Endpoints

### Endpoint: Calcular CDB (POST)

```
POST /api/cdb/calcular
Content-Type: application/json
```

**Body (JSON):**

| Campo | Tipo | Descrição | Validação |
|-------|------|-----------|-----------|
| `valorInicial` | decimal | Valor inicial do investimento | Obrigatório, > 0 |
| `meses` | int | Prazo em meses | Obrigatório, >= 2 |

**Resposta (200 OK):**
```json
{
  "valorInicial": 1000.00,
  "meses": 12,
  "aliquota": 20.00,
  "resultadoBruto": {
    "valor": 1113.63,
    "rendimento": 113.63
  },
  "resultadoLiquido": {
    "valor": 1090.90,
    "rendimento": 90.90
  },
  "imposto": 22.73
}
```

**Respostas de erro:**
- `400 Bad Request` — Validação falhou ou dados inválidos

---

## 📊 Exemplos de Respostas

### Exemplo 1: Investimento de 6 Meses (Alíquota 22,5%)

**Requisição:**
```json
{ "valorInicial": 1000, "meses": 6 }
```

**Resposta (200 OK):**
```json
{
  "valorInicial": 1000.00,
  "meses": 6,
  "aliquota": 22.50,
  "resultadoBruto": { "valor": 1054.81, "rendimento": 54.81 },
  "resultadoLiquido": { "valor": 1042.48, "rendimento": 42.48 },
  "imposto": 12.33
}
```

### Exemplo 2: Investimento de 12 Meses (Alíquota 20%)

**Requisição:**
```json
{ "valorInicial": 5000, "meses": 12 }
```

**Resposta (200 OK):**
```json
{
  "valorInicial": 5000.00,
  "meses": 12,
  "aliquota": 20.00,
  "resultadoBruto": { "valor": 5568.17, "rendimento": 568.17 },
  "resultadoLiquido": { "valor": 5454.51, "rendimento": 454.51 },
  "imposto": 113.63
}
```

### Exemplo 3: Investimento de 24 Meses (Alíquota 17,5%)

**Requisição:**
```json
{ "valorInicial": 10000, "meses": 24 }
```

**Resposta (200 OK):**
```json
{
  "valorInicial": 10000.00,
  "meses": 24,
  "aliquota": 17.50,
  "resultadoBruto": { "valor": 11639.08, "rendimento": 1639.08 },
  "resultadoLiquido": { "valor": 11352.24, "rendimento": 1352.24 },
  "imposto": 286.84
}
```

---

## ❌ Casos de Erro

### Valor Inicial Inválido
```json
// Requisição
{ "valorInicial": 0, "meses": 6 }

// Resposta 400
{ "mensagem": "O valor inicial deve ser maior que zero." }
```

### Prazo Menor que 2 Meses
```json
// Requisição
{ "valorInicial": 1000, "meses": 1 }

// Resposta 400
{ "mensagem": "O prazo deve ser maior que 1 mês." }
```

---

## 🔍 Resolução de Problemas

### Porta já em uso (backend)

```powershell
# Encontre o processo usando a porta
Get-NetTCPConnection -LocalPort 7257

# Ou mude a porta no appsettings.json / launchSettings.json
```

### Certificado HTTPS não confiável

```bash
dotnet dev-certs https --trust
```

### Chrome não encontrado para os testes do frontend

Certifique-se de que o Google Chrome está instalado. Os testes usam `ChromeHeadless` configurado no `karma.conf.js`.

---

## 🛠️ Detalhes Técnicos

### Stack Tecnológico

| Camada | Tecnologia |
|--------|-----------|
| Backend | ASP.NET Core 8.0 / C# 12 |
| Frontend | Angular 17 (Standalone Components) |
| Testes Frontend | Karma 6 + Jasmine |
| Documentação API | Swagger / OpenAPI |
| Formato de Dados | JSON |

### Validações Implementadas
- ✅ Valor inicial deve ser positivo (> 0)
- ✅ Prazo em meses deve ser maior que 1 (>= 2)
- ✅ Tratamento de exceções com mensagens claras
- ✅ Validação via `DataAnnotations` no DTO de request

### Precisão Numérica
- Todas as operações utilizam `decimal` para precisão financeira
- Resultados são arredondados a 2 casas decimais no `CDBService`

---

## 📄 Licença

Projeto desenvolvido para fins educacionais e de demonstração.

---

**Versão:** 2.0
**Última Atualização:** Maio/2026
**Desenvolvido em:** .NET 8 + Angular 17
