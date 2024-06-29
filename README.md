# Testing

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