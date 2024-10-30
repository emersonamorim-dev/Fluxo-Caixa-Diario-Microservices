### Fluxo Caixa Diário Microservice -  C# com Entity Framework, MongoDB, RabbitMQ, ELK Stack 🚀 🔄 🌐


Codificação em C# com Entity Framework com uso DotNet Core 8.0 para projeto **Fluxo Caixa Diário Microservice** para um **Teste Técnico Carrefour** essa aplicação é uma solução moderna e robusta para o gerenciamento de Fluxos de Caixa Diários, projetada para ser escalável e altamente disponível. Esta aplicação foi desenvolvida utilizando uma arquitetura baseada em microserviços, que facilita a manutenção e a evolução da solução ao longo do tempo. Utilizamos diversas tecnologias para garantir a eficiência, confiabilidade e auditabilidade do sistema, tais como **Entity Framework**, **MongoDB**, **RabbitMQ**, **ELK Stack (Elasticsearch, Logstash, Kibana)**, **Redis**, e **Xunit** para implementação de testes.


#### Tecnologias Utilizadas
- **C# (.NET Core)**: Linguagem de programação usada para implementar a lógica de negócios, respeitando os princípios da Orientação a Objetos e SOLID.
- **Entity Framework**: ORM utilizado para simplificar o acesso e manipulação de dados do banco de dados relacional, promovendo abstração da camada de persistência.
- **MongoDB**: Banco de dados NoSQL para armazenamento de documentos, utilizado para dados que não precisam de esquema rígido, garantindo flexibilidade.
- **RabbitMQ**: Message broker utilizado para comunicação assíncrona entre os microserviços, proporcionando escalabilidade e desacoplamento.
- **Redis**: Utilizado como cache distribuído para aumentar a performance da aplicação, especialmente nas operações de leitura frequente.
- **Elasticsearch**, Logstash e Kibana (ELK Stack): Ferramentas utilizadas para a coleta, armazenamento, análise e visualização dos logs da aplicação. Proporcionam uma visão abrangente da saúde e desempenho da aplicação.
- **Logstash**: Coletor de logs responsável por coletar, transformar e enviar dados para o Elasticsearch.
- **Kibana**: Ferramenta para visualização e análise dos logs armazenados no Elasticsearch.
- **xUnit**: Framework de testes utilizado para validar a aplicação, garantindo a qualidade do código e o funcionamento correto dos serviços.
- **Docker**: Ferramenta de containerização usada para garantir que a aplicação seja executada de forma consistente em qualquer ambiente.
- **Ubuntu 24.04 e WSL2**: Aplicação configurada para rodar dentro do WSL2, utilizando Ubuntu 24.04 no ambiente de desenvolvimento com Docker Desktop

[![C#](https://img.shields.io/badge/C%23-.NET%20Core-blue)](https://dotnet.microsoft.com/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-ORM-blue)](https://docs.microsoft.com/en-us/ef/)
[![MongoDB](https://img.shields.io/badge/MongoDB-NoSQL-green)](https://www.mongodb.com/)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-Message%20Broker-orange)](https://www.rabbitmq.com/)
[![Redis](https://img.shields.io/badge/Redis-Cache-red)](https://redis.io/)
[![Elasticsearch](https://img.shields.io/badge/Elasticsearch-Log%20Storage-yellow)](https://www.elastic.co/elasticsearch/)
[![Logstash](https://img.shields.io/badge/Logstash-Log%20Collector-yellow)](https://www.elastic.co/logstash/)
[![Kibana](https://img.shields.io/badge/Kibana-Log%20Visualization-yellow)](https://www.elastic.co/kibana/)
[![xUnit](https://img.shields.io/badge/xUnit-Testing%20Framework-purple)](https://xunit.net/)
[![Docker](https://img.shields.io/badge/Docker-Containerization-blue)](https://www.docker.com/)
[![Ubuntu 24.04](https://img.shields.io/badge/Ubuntu-24.04-orange)](https://ubuntu.com/)
[![WSL2](https://img.shields.io/badge/WSL2-Windows%20Subsystem%20for%20Linux-blue)](https://docs.microsoft.com/en-us/windows/wsl/)


#### Passos para Instalação


1. **Clonar o Repositório**:
   ```
   git clone https://github.com/emersonamorim-dev/Fluxo-Caixa-Diario-Microservice.git
   ```
   
   ```
   cd Fluxo-Caixa-Diario-Microservice
   ```

   #### Comandos necessário para instalar o .NET Core 8.0 no Ubuntu 24.04 via WSL2

```
sudo apt update && sudo apt upgrade -y
```

```
sudo apt install -y wget apt-transport-https
```


- Adicionar o Repositório do Microsoft Package
```
# Baixar a chave GPG
wget https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb

# Instalar o pacote de repositório da Microsoft
sudo dpkg -i packages-microsoft-prod.deb
```

- Instalar o SDK do .NET Core 8.0

```
sudo apt update
```

```
sudo apt install -y dotnet-sdk-8.0
```

```
dotnet --version

```

2. **Rode aplicação com Docker Desktop**

   - Configurado para rodar dentro WSL2 com Ubuntu 24.04
   
   ``` 
   docker build -t fluxo-caixa-diario-microservice:latest .
   ``` 


   **Subir Aplicação via Docker**

   ``` 
   docker-compose up --build
   ```

   - Comando para permissão MongoDB em ambiente Linux
   ```
    sudo chmod -R 777 databases/mongo/data
   ```

3. **Acessar Endpoints**:
   ##### Segue o endpoint para requisição Post no Postman
   - http://localhost:5000/api/lancamentos
  
  ```
  {
  "id": "e1a94f5a-657b-4c6b-8f18-9c17cb3c43b8",
  "valor": 1800.00,
  "tipo": "Crédito",
  "data": "2024-10-28T18:25:43.511Z"
}
  
```

  - Retorno da Requisição:
  
  ```
  {
    "Id": "e1a94f5a-618b-4c6b-8f18-9c17cb3c43b8",
    "Valor": 1800.00,
    "Tipo": "Crédito",
    "Data": "2024-10-28T18:25:43.511Z"
}

```

##### Segue o endpoint para requisição Get para listar todos os Lançamentos no Postman

- http://localhost:5000/api/lancamentos

- Retorno da Requisição

```
[
    {
        "Id": "e1a94f5a-618b-4c6b-8f18-9c81cb3c43b8",
        "Valor": 3800.00,
        "Tipo": "Débito",
        "Data": "2024-10-28T18:25:43.511Z"
    },
    {
        "Id": "e1a94f5a-618b-4c6b-8f18-9c81cb3c53b8",
        "Valor": 5800.00,
        "Tipo": "Crédito",
        "Data": "2024-10-28T18:25:43.511Z"
    },
    {
        "Id": "e1a94f5a-618b-4c6b-8f18-9c81cb3c51b8",
        "Valor": 5800.00,
        "Tipo": "Crédito",
        "Data": "2024-10-28T18:25:43.511Z"
    },
    {
        "Id": "e1a94f5a-718b-4c6b-8f18-9c81cb3c51b8",
        "Valor": 5800.00,
        "Tipo": "Crédito",
        "Data": "2024-10-28T18:25:43.511Z"
    },
    {
        "Id": "e1a93f5a-718b-4c6b-8f18-9c81cb3c51b8",
        "Valor": 1800.00,
        "Tipo": "Débito",
        "Data": "2024-10-28T18:25:43.511Z"
    },
    {
        "Id": "e1a93f5a-818b-4c6b-8f18-9c81cb3c51b8",
        "Valor": 7800.00,
        "Tipo": "Crédito",
        "Data": "2024-10-30T18:25:43.511Z"
    },
    {
        "Id": "e1a93f5a-918b-4c6b-8f18-9c81cb3c51b8",
        "Valor": 6800.00,
        "Tipo": "Crédito",
        "Data": "2024-10-29T18:25:43.511Z"
    },
    {
        "Id": "e1a93f5a-918b-4c6b-8f19-9c81cb3c51b8",
        "Valor": 1800.00,
        "Tipo": "Débito",
        "Data": "2024-10-29T18:25:43.511Z"
    },
    {
        "Id": "e1a93f5a-518b-4c6b-8f19-9c81cb3c51b8",
        "Valor": 1800.00,
        "Tipo": "Débito",
        "Data": "2024-10-30T18:25:43.511Z"
    },
    {
        "Id": "e1a91f5a-518b-4c6b-8f19-9c81cb3c51b8",
        "Valor": 1800.00,
        "Tipo": "Débito",
        "Data": "2024-10-30T18:25:43.511Z"
    },
    {
        "Id": "e1a91f5a-518b-4c6b-8f29-9c81cb3c51b8",
        "Valor": 3800.00,
        "Tipo": "Crédito",
        "Data": "2024-10-30T18:25:43.511Z"
    }
]
```



##### Segue o endpoint para requisição Post no Postman

- http://localhost:5000/api/consolidados/consolidar

```
{
  "data": "2024-10-29"
}

```

- Retorno da Requisição 
```
Saldo diário consolidado com sucesso para a data: 2024-10-29
```

##### Segue o endpoint para requisição Post com parâmetro Data no Postman

- http://localhost:5000/api/consolidados/consolidar?data=2024-10-29

- Retorno da Requisição
  
```
Saldo diário consolidado com sucesso para a data: 2024-10-29
```


##### Segue o endpoint para requisição Post para Calcular Consolidados por Datas no Postman

- http://localhost:5000/api/consolidados/consolidado

```
{
  "data": "2024-10-30"
}
```

- Retorno da Requisição
  
```
{
    "data": "2024-10-30",
    "total_creditos": 7800.00,
    "total_debitos": 1800.00,
    "saldo_consolidado": 6000.00
}
```


#### Diagrama da Aplicação

![](https://raw.githubusercontent.com/emersonamorim-dev/fluxo-caixa-diario-microservice/refs/heads/main/Diagrama/Diagrama-Fluxo-Caixa-Diario-Microservice.png)


#### Arquitetura da Aplicação
A arquitetura do Fluxo Caixa Diário Microservice foi planejada para ser modular e desacoplada, seguindo princípios de **SOLID**, **orientação a objetos** e utilizando padrões de projeto (**design patterns**) como **Repository**, **Service**, **Observer**, e **Factory** para promover a manutenção e extensibilidade do sistema. A aplicação é organizada em camadas, cada uma com responsabilidades bem definidas:

- **Domain**: Contém as entidades do negócio, interfaces e regras que são fundamentais para o funcionamento da aplicação. Aqui estão implementadas entidades como `Lancamento`, `ConsolidadoDiario` e seus respectivos repositórios e interfaces.

- **Application**: Camada que define os **serviços** e **casos de uso**. Aqui são orquestradas as regras de negócio mais complexas e os fluxos de operações que requerem integrações, como o processamento de um novo lançamento e a consolidação diária do saldo.

- **Infrastructure**: Camada que fornece a integração com recursos externos, como banco de dados (MongoDB e SQL via **Entity Framework**), mensageria (RabbitMQ), armazenamento em cache (Redis), e a solução de log distribuído e monitoramento (**ELK Stack**).

- **Presentation**: Contém os controladores **API** que expõem os endpoints para consumo externo. Esta camada utiliza **ASP.NET Core** e implementa todos os endpoints necessários para o gerenciamento de lançamentos e visualização de consolidação de saldo.

- **Tests**: Testes automatizados implementados em **Xunit** para garantir que todas as funcionalidades essenciais estejam bem cobertas, proporcionando um sistema mais seguro e com menos falhas. Foram realizados testes unitários para os casos de uso, controladores e integração com RabbitMQ e Redis.


#### Resultado de Testes com xUnit

![](https://raw.githubusercontent.com/emersonamorim-dev/fluxo-caixa-diario-microservice/refs/heads/main/Diagrama/Teste-XUnit.png)



#### Funcionamento da Aplicação

##### 1. Criação de Lançamentos
Ao criar um lançamento, o usuário faz uma requisição via API para o controlador que, por sua vez, aciona o serviço correspondente. O serviço utiliza o caso de uso **AdicionarLancamentoUseCase** que, além de salvar o lançamento no banco de dados (utilizando **Entity Framework** ou **MongoDB**), armazena o lançamento em cache no **Redis** para otimizar as consultas futuras.

##### 2. Consolidação Diária
A funcionalidade de consolidação diária é responsável por calcular o saldo acumulado de todos os lançamentos de um dia. Este processo é acionado via API e utiliza **Redis** para obter lançamentos armazenados temporariamente, garantindo assim maior desempenho na leitura dos dados.

##### 3. Mensageria e Integração Assíncrona
Para integrações assíncronas e notificação de eventos, utilizamos o **RabbitMQ**. Cada vez que um lançamento é criado, um evento é publicado no RabbitMQ para que outros microserviços possam ser informados e realizar operações relacionadas, como auditoria e contabilidade.

##### 4. Monitoramento e Logs
Toda a aplicação é monitorada com **ELK Stack** (Elasticsearch, Logstash e Kibana). O **Logstash** é responsável por coletar os logs das aplicações e enviá-los para o **Elasticsearch**, onde podem ser consultados no **Kibana**. Esse monitoramento permite que a equipe de operação visualize problemas em tempo real e atue rapidamente para resolvê-los.

#### Princípios de SOLID e Padrões de Projeto
Durante o desenvolvimento do **FluxoCaixaDiarioMicroservice**, seguimos os princípios **SOLID** para garantir um código modular, flexível e facilmente manutenível:
- **Single Responsibility Principle (SRP)**: Cada classe tem uma única responsabilidade. Por exemplo, a classe `LancamentoService` é responsável apenas pela lógica de negócio relacionada aos lançamentos.
- **Open/Closed Principle (OCP)**: Classes estão abertas para extensão, mas fechadas para modificação. Usamos interfaces como **ILancamentoRepository** para permitir novas implementações sem modificar o contrato original.
- **Liskov Substitution Principle (LSP)**: As implementações concretas de interfaces como `IRedisCacheService` seguem este princípio ao manterem o contrato esperado sem alterar o comportamento original.
- **Interface Segregation Principle (ISP)**: Foram criadas interfaces pequenas e coesas, como **ILancamentoService** e **IConsolidadoDiarioService**, evitando interfaces muito grandes e complexas.
- **Dependency Inversion Principle (DIP)**: Utilizamos **injeção de dependência** para desacoplar classes de suas implementações específicas.

Além disso, aplicamos padrões de projeto como **Repository** para abstrair a lógica de acesso a dados, **Factory** para criação de objetos de maneira centralizada, e **Observer** para garantir que eventos sejam notificados e tratados por todas as partes interessadas.



### Conclusão
O **Fluxo Caixa Diário Microservice** é uma aplicação robusta e escalável, desenvolvida com tecnologias modernas e utilizando boas práticas de engenharia de software. A arquitetura baseada em microserviços, combinada com o uso de mensageria e armazenamento distribuído, permite a alta disponibilidade e manutenção da solução. Graças aos princípios de **SOLID** e **design patterns**, garantimos uma base de código limpa e pronta para expansões futuras. Isso possibilita uma evolução contínua do sistema, atendendo às necessidades do negócio de forma ágil e segura.

### Desenvolvido por:
Emerson Amorim [@emerson-amorim-dev](https://www.linkedin.com/in/emerson-amorim-dev/)


