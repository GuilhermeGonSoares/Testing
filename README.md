# Testing - Teoria

## Unit Testing

- **Definição**: É um tipo de teste focado em vertificar pequenas unidades de código, como funções ou métodos, de forma isolada.
- **Objetivo**: Garantir que cada unidade de código funcione conforme o esperado, independentemente de outras partes do sistema.
- **Ambiente**: Geralmente é executado de forma automatizada, isolada de dependências externas usando mocks ou stubs.
- **Exemplo**: Testar uma função que calcula o imposto sobre uma transação financeira.

## Integration Testing

- **Definição**: Envolve testar a interação entre diferentes componentes ou módulos de um sistema.
- **Objetivo**: Verificar se as unidades de código interagem corretamente entre si, garantindo que integradas, funcionem conforme o esperado.
- **Ambiente**: Pode envolver mais de uma unidade de código e depende da configuração e ambiente onde o sistema é implantado.
- **Exemplo**: Testar a interação entre um banco de dados e a camada de aplicação de um sistema.

## Functional Testing

- **Definição**: Testa o sistema como um todo, verificando se ele atende aos requisitos funcionais especificados.
- **Objetivo**: Avaliar se o sistema funciona conforme as expectativas do usuário, simulando o uso real do software
- **Ambiente**: Pode envolver múltiplos componentes e dependências externas, replicando o ambiente de produção tanto quanto possível.
- **Exemplo**: Testar a funcionalidade completa de um aplicativo web, incluindo navegação, formulários, e validações de entrada.

## Resumo

- **Unit Testing**: Testa, de forma isolada, as unidades individuais do código. Não depende de coisas externas. O que precisamos verificar é se a lógica do código(daquela função, por exemplo) está funcionando corretamente.
- **Integration Testing**: Testa a interação entre as diferentes unidades de código. Neste caso, irá testar, por exemplo, se a função consegue se comunicar corretamente com o banco de dados.
- **Functional Testing**: Testa o sistema completo para garantir que ele funcione conforme o esperado pelo usuário.

## Como pensar na hora de implementar UNIT TESTING

Quando criamos nossos testes de unidade, é comum tentarmos encaixar nossa função ou código diretamente dentro do teste. Isso pode levar à passagem dos testes de forma artificial, gerando falsos positivos. O ideal é pensar nos testes como uma forma alternativa de interagir com nosso código. Estamos criando scripts que simulam a interação com nosso aplicativo, semelhante ao que ocorre quando um frontend faz uma requisição e interage com nossa API. Dessa forma, os testes validam essa interação de maneira rápida e automatizada.

## Por que usar MOCKS nos unit testing?

Usamos mocks nos testes unitários para isolar a unidade de código que estamos testando. Muitas vezes, essa unidade depende de outras partes do sistema, como acesso ao banco de dados ou chamadas a APIs externas. O objetivo dos testes unitários é verificar a lógica interna dessa unidade de código, não testar detalhes de implementação de suas dependências externas. Utilizando mocks, podemos simular o comportamento dessas dependências, definindo o que esperamos que elas retornem em resposta a certas chamadas. Por exemplo, ao simular uma chamada ao banco de dados para recuperar um objeto, configuramos o mock para retornar dados específicos. Isso nos permite focar exclusivamente na lógica da unidade de código sendo testada, garantindo testes mais rápidos, previsíveis e independentes.

# Implementação

## Por que utilizar WebApplicationFactory?

Oferece uma forma robusta e flexível de configurar o ambiente de teste que disfarça a execução da aplicação em um ambiente de produção ou desenvolvimento.

- Cria uma instância do aplicativo web em memoria, incluindo todas as configurações, middlewares e rotas, em um ambiente isolado. Isso significa que cada teste pode ser executado em um contexto limpo sem interferência de outros testes. Estamos simulado o comportamento real do aplicativo.
- Permite personalizar a configuração do ambiente de teste de maneira que se assemelhe ao ambiente de produção. Pode-se modificar a configuração de serviços, adicionar ou remover middleware, configurar diferentes ambientes e ajustar a base de dados que iremos utilizar para os testes.
- Fornece uma maneira de criar clientes HTTP que podem enviar requisições para o aplicativo de forma programática. Isso é útil para testar endpoints, verificar respostas HTTP e garantir que as rotas estejam comportando-se conforme o esperado.

## Passo a passo para configuração

1. Instalar o pacote Microsoft.AspNetCore.Mvc.Testing para que possamos utilizar o WebApplicationFactory
2. Configurar a WebApplicationFactory: Criar uma classe que herda de `WebApplicationFactory<Program>`, onde Program é a classe startup do projeto ASP.NET Core. Isso permite configurar o ambiente do teste conforme necessário.
    1. Para que a gente consiga definir o Program como noss Entry Point precisamos definir no .csproj da API a seguinte config para que o nosso projeto de testes consiga visualizar a classe Program que é definida como internal.

        ```csharp
        <ItemGroup>
                <InternalsVisibleTo Include="TODO.Tests"/>
        </ItemGroup>
        ```

    2. Além disso, precisamos definir no final do arquivo Program.cs o código abaixo porque Program é internal e a nossa classe que herda WebApplicationFactory é pública e com isso não poderia usar Program como tipo porque ele é menos acessível.

    ```csharp
    public partial class Program
    {
    }
    ```

3. **Configurar o Ambiente de Teste** (opcional): Podemos sobrescrever o método ConfigureWebHost para adicionar configurações adicionais, como usar um banco de dados em memória ou mock de serviços.
    1. Para conseguir definir um novo banco dados apenas para os testes de integração precisamos remover a configuração que já existe no Program para o banco que estamos usando para desenvolvimento/produção. E adicionar a nova configuração que será usada para os testes, no nosso caso iremos utilizar uma instância do postgresql apenas para teste, diferente do banco que esta sendo usado no desenvolvimento.

    ```csharp
    // Encontre o serviço de DbContext e remova-o
      var descriptor = services.SingleOrDefault(
          d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
      if (descriptor != null)
      {
          services.Remove(descriptor);
      }
      // Adicione o DbContext com Posgresql para teste
      services.AddDbContext<ApplicationDbContext>(options =>
      {
          options.UseNpgsql("Host=localhost;Port=5432;Database=todo-test;Username=test-postgres;Password=test-postgres");
      });
      
    ```
    Caso queira utilizar o "mesmo container":(Powershell)
   ```sh
       docker run -d `
      --name integration-testing `
      -e POSTGRES_DB="todo-test" `
      -e POSTGRES_USER="test-postgres" `
      -e POSTGRES_PASSWORD="test-postgres" `
      -p 5432:5432 `
      postgres:latest
   ```

5. Implementar a inteface `IAsyncLifetime`na classe que herda WebApplicationFactory: Com essa interface podemos sobrescrever os métodos `InitializeAsync` e `DisposeAsync` . Para realizar operações assíncronas de inicialização e limpeza, muito útil para a configuração do nosso banco de teste porque queremos criar o banco, aplicar as migrations e depois queremos deletar o banco. Com essa interface conseguimos uma redução no compartilhamento de estado entre os testes. Usando `InitializeAsync` para configurar um novo ambiente de teste e `DisposeAsync` para limpar após cada teste, você pode reduzir o risco de estado compartilhado entre testes, o que aumenta a confiabilidade dos resultados dos testes.

6. Utilização da interface IClassFixture<T> nos testes:

    a. O `IClassFixture<T>` é uma interface no framework de testes xUnit para C#, e ela é usada para implementar o padrão de design conhecido como "fixture setup" ou "test fixture". A principal função da `IClassFixture<T>` é fornecer uma maneira de criar uma instância de uma classe que contém dados ou serviços necessários para a execução dos testes, e compartilhá-la entre todos os testes na classe de teste, sem reinicializá-la a cada método de teste. Isso é especialmente útil quando a inicialização é custosa em termos de tempo ou recursos.

    b. Com o uso do IClassFixture teremos uma instância da nossa webapplication para cada classe de teste que implementa o IClassFixture.
    
    c. OBSERVAÇÃO: Como estamos compartilhando os dados e serviços entre todos os testes da classe quando adicionamos um usuário ele continuará no banco de teste para os demais testes isso é importante de ser lembrado!
