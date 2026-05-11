# Investments - Calculadora de CDB

Aplicação Angular para cálculo de investimentos em CDB (Certificado de Depósito Bancário).

## ?? Caracter�sticas

- Interface moderna e responsiva
- Cálculo de investimentos em CDB
- Exibição de resultados bruto e l�quido
- Tabela de alíquotas progressivas de imposto
- Integração com API B3.CDB.WebApi
- Validação de entrada
- Formatação de moeda brasileira

## Pré-requisitos

- Node.js 18+ e npm
- Angular CLI 17+
- Backend B3.CDB.WebApi rodando em `https://localhost:7257`

## Instalação

1. Navegue até a pasta do projeto:
```bash
cd frontend/Investments
```

2. Instale as dependências:
```bash
npm install
```

## Execução

### Modo de Desenvolvimento

```bash
npm start
```

A aplicação estar disponível em `http://localhost:4200`

### Build para Produção

```bash
npm run build
```

## Estrutura do Projeto

```
Investments/
src/
 app/
 cdb-calculator/
 cdb-calculator.component.ts
 cdb-calculator.component.html
 cdb-calculator.component.css
 services/
 cdb.service.ts
 app.component.ts
 index.html
 main.ts
 styles.css
 angular.json
 package.json
 tsconfig.json
 README.md
```

## Configuração da API

A aplicação espera que a API B3.CDB.WebApi esteja rodando em:
```
https://localhost:7257/api/cdb
```

Se a API estiver em um endereço diferente, atualize a URL em:
```typescript
// src/app/services/cdb.service.ts
private apiUrl = 'https://seu-host:sua-porta/api/cdb';
```

## Uso

1. Preencha os campos:
   - **Valor Inicial**: Valor monetário positivo (ex: 10000.00)
   - **Prazo**: Número de meses (maior que 1)

2. Clique em "Calcular"

3. Visualize os resultados:
   - **Resultado Bruto**: Sem dedução de imposto
   - **Resultado L�quido**: Após dedução de imposto
   - **Alíquota de Imposto**: Baseada no prazo do investimento

## Tabela de Alíquotas

| Prazo | Alíquota |
|-------|----------|
| Até 6 meses | 22,5% |
| Até 12 meses | 20% |
| Até 24 meses | 17,5% |
| Acima de 24 meses | 15% |

## Tema

- Cores: Roxo (#667eea), Azul (#3498db), Verde (#27ae60)
- Layout responsivo para desktop, tablet e celular
- Interface intuitiva e amigável

## Responsividade

- Desktop: Layout com formulário fixo e resultados ao lado
- Tablet: Layout em coluna única com rolagem
- Celular: Interface otimizada para telas pequenas

## Troubleshooting

### Erro de conexão com API
- Verifique se o backend está rodando
- Confirme a URL da API em `cdb.service.ts`
- Verifique CORS no backend

### Validação de formulário não funciona
- Limpe o cache do navegador
- Reinicie o servidor Angular

### Problema de estilos
- Execute `npm run build` e serve os arquivos

## Exemplos de Uso

### Exemplo 1: Investimento de R$ 10.000 por 12 meses
- Valor Inicial: 10000.00
- Prazo: 12
- Aléquota: 20%

### Exemplo 2: Investimento de R$ 50.000 por 36 meses
- Valor Inicial: 50000.00
- Prazo: 36
- Alíquota: 15%

## Segurança

- Validação no cliente
- HTTPS recomendado em produção
- Proteção CORS configurada no backend

## Suporte

Para problemas ou dúvidas, verifique a documentação da API em:
```
https://localhost:7257/swagger
```

## Licença

Projeto B3.CDB - Investments 2026
