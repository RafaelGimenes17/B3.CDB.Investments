# Guia de Instalação e Uso - Investments

## ?? Pré-requisitos

- **Node.js**: Versão 18.x ou superior
  - Download: https://nodejs.org/
  - Verifique a instalação: `node --version`

- **npm**: Geralmente incluído com Node.js
  - Verifique: `npm --version`

- **Angular CLI**: Será instalado globalmente
  - Install: `npm install -g @angular/cli@17`

- **Backend B3.CDB.WebApi**: Rodando e acessível

## ?? Passo 1: Instalação

### 1.1 Abra o terminal na pasta do projeto Angular

```bash
cd frontend/Investments
```

### 1.2 Instale as dependências

```bash
npm install
```

Isso instalará:
- Angular 17
- RxJS
- TypeScript
- Todas as ferramentas de build

## ?? Passo 2: Execução

### 2.1 Inicie o servidor de desenvolvimento

```bash
npm start
```

Ou alternativamente:

```bash
ng serve
```

### 2.2 Abra no navegador

Acesse: `http://localhost:4200`

A aplicação carregará automaticamente e recarregará quando você fizer alterações.

## ?? Configuração da API

### Opção 1: Usando Proxy (Recomendado para Desenvolvimento)

O projeto já está configurado com proxy. Verifique:

**proxy.conf.json:**
```json
{
  "/api": {
    "target": "http://localhost:5000",
    "secure": false,
    "changeOrigin": true
  }
}
```

Se a API estiver em outro endereço:

```bash
npm start -- --proxy-config=proxy.conf.json
```

### Opção 2: URL Absoluta (Produção)

Edite `src/app/services/cdb.service.ts`:

```typescript
private apiUrl = 'https://seu-dominio.com/api/cdb';
```

## ?? Testando a Aplicação

### Teste 1: Investimento Básico
- Valor Inicial: `10000`
- Prazo: `12`
- Clique em "Calcular"
- Deve mostrar resultados bruto e líquido

### Teste 2: Validação de Entrada
- Deixe os campos vazios
- Clique em "Calcular"
- Deve mostrar mensagem de erro

### Teste 3: Valores Inválidos
- Valor Inicial: `-1000`
- Prazo: `0`
- Deve indicar erro de validação

## ?? Interpretação dos Resultados

Quando você clica em "Calcular", a tela mostra:

1. **Resultado Bruto**
   - Valor Final: Quanto você terá sem imposto
   - Rendimento: Quanto ganhou (valor final - valor inicial)

2. **Imposto de Renda**
   - Valor: Quanto será descontado em impostos
   - Baseado na tabela de alíquotas progressivas

3. **Resultado Líquido**
   - Valor Final Líquido: O que você realmente vai receber
   - Rendimento Líquido: Ganho após descontos

4. **Comparação**
   - Diferença entre bruto e líquido
   - Percentual de rendimento

## ?? Solução de Problemas

### Erro: "Cannot GET /"
```
Solução: Execute `npm start` no diretório correto
```

### Erro: "Module not found"
```bash
# Limpe node_modules e reinstale
rm -rf node_modules
npm install
```

### Erro de CORS
```
Verifique se o backend tem CORS habilitado
Ou use o proxy.conf.json
```

### Porta 4200 já está em uso
```bash
ng serve --port 4300
```

### Conexão recusada com backend
```
Verifique se o backend está rodando em http://localhost:5000
Teste no navegador: http://localhost:5000/api/cdb
```

## ?? Atualizar Dependências

```bash
npm update
```

## ?? Build para Produção

```bash
npm run build
```

Isso criará a pasta `dist/investments/` com os arquivos otimizados.

Para servir os arquivos de produção:

```bash
npm install -g http-server
cd dist/investments
http-server
```

## ?? Personalização

### Mudar Cores

Edite `src/app/cdb-calculator/cdb-calculator.component.css`:

```css
/* Cores atuais */
background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);

/* Personalize conforme necessário */
background: linear-gradient(135deg, #YourColor1 0%, #YourColor2 100%);
```

### Mudar URL da API

Edite `src/app/services/cdb.service.ts`:

```typescript
private apiUrl = 'sua-url-da-api';
```

## ?? Estrutura de Pastas

```
Investments/
??? src/
?   ??? app/
?   ?   ??? cdb-calculator/       # Componente principal
?   ?   ?   ??? *.component.ts    # Lógica
?   ?   ?   ??? *.component.html  # Template
?   ?   ?   ??? *.component.css   # Estilos
?   ?   ??? services/
?   ?   ?   ??? cdb.service.ts    # Serviço HTTP
?   ?   ??? app.component.ts      # Componente raiz
?   ??? index.html                 # HTML principal
?   ??? main.ts                    # Arquivo de entrada
?   ??? styles.css                 # Estilos globais
??? angular.json                   # Configuração Angular
??? package.json                   # Dependências
??? tsconfig.json                  # Configuração TypeScript
??? proxy.conf.json                # Proxy para desenvolvimento
??? README.md                       # Documentação

```

## ?? Próximas Etapas

1. **Adicionar Autenticação**: Implementar login
2. **Histórico de Cálculos**: Salvar resultados anteriores
3. **Gráficos**: Visualizar evolução do investimento
4. **Temas**: Modo claro/escuro
5. **i18n**: Suporte para múltiplos idiomas

## ? Checklist de Verificação

- [ ] Node.js instalado e versão verificada
- [ ] npm instalado e versão verificada
- [ ] Dependências instaladas (npm install)
- [ ] Backend B3.CDB.WebApi rodando
- [ ] Servidor Angular iniciado (npm start)
- [ ] Aplicação acessível em http://localhost:4200
- [ ] Formulário carrega corretamente
- [ ] Cálculos retornam resultados esperados
- [ ] Interface responsiva em diferentes tamanhos

## ?? Pronto!

Você está pronto para usar a Calculadora de CDB! 

Para iniciar em futuras sessões, simplesmente execute:

```bash
cd frontend/Investments
npm start
```
