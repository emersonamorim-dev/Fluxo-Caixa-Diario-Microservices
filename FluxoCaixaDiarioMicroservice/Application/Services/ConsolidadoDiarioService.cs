using FluxoCaixaDiarioMicroservice.Application.UseCases;
using FluxoCaixaDiarioMicroservice.Domain.Entities;
using FluxoCaixaDiarioMicroservice.Domain.Interfaces;
using FluxoCaixaDiarioMicroservice.Infrastructure.Messaging.Events;
using FluxoCaixaDiarioMicroservice.Infrastructure.Redis;
using MediatR;
using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FluxoCaixaDiarioMicroservice.Application.Services
{
    public class ConsolidadoDiarioService : IConsolidadoDiarioService
    {
        private readonly IConsolidarSaldoDiarioUseCase _consolidarSaldoDiarioUseCase;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IMediator _mediator;
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly IConnection _rabbitMqConnection;

        public ConsolidadoDiarioService(
            IConsolidarSaldoDiarioUseCase consolidarSaldoDiarioUseCase,
            IRedisCacheService redisCacheService,
            IMediator mediator,
            ILancamentoRepository lancamentoRepository,
            IConnection rabbitMqConnection)
        {
            _consolidarSaldoDiarioUseCase = consolidarSaldoDiarioUseCase;
            _redisCacheService = redisCacheService;
            _mediator = mediator;
            _lancamentoRepository = lancamentoRepository;
            _rabbitMqConnection = rabbitMqConnection;
        }

        public async Task ConsolidarSaldoDiarioAsync(DateTime data)
        {
            if (data == DateTime.MinValue)
            {
                throw new ArgumentException("A data fornecida é inválida. Certifique-se de enviar uma data válida.");
            }

            var cacheKey = $"ConsolidadoDiario:{data:yyyy-MM-dd}";

            var saldoConsolidado = await _redisCacheService.GetCacheValueAsync<SaldoConsolidado>(cacheKey);
            if (saldoConsolidado != null)
            {
                return;
            }

            saldoConsolidado = await _consolidarSaldoDiarioUseCase.ExecuteAsync(data);

            // Armazena o saldo consolidado no cache Redis por 24 horas
            await _redisCacheService.SetCacheValueAsync(cacheKey, saldoConsolidado, TimeSpan.FromHours(24));

            // Publica o evento de saldo consolidado criado
            var evento = new ConsolidadoDiarioCriadoEvent(data, saldoConsolidado.SaldoFinal);
            await _mediator.Publish(evento);

            PublicarMensagemRabbitMQ(saldoConsolidado);
        }

        public async Task<SaldoConsolidado> ObterSaldoConsolidadoAsync(DateTime data)
        {
            if (data == DateTime.MinValue)
            {
                throw new ArgumentException("A data fornecida é inválida. Certifique-se de enviar uma data válida.");
            }

            var cacheKey = $"ConsolidadoDiario:{data:yyyy-MM-dd}";
            var saldoConsolidado = await _redisCacheService.GetCacheValueAsync<SaldoConsolidado>(cacheKey);

            if (saldoConsolidado == null)
            {
                saldoConsolidado = await _consolidarSaldoDiarioUseCase.ExecuteAsync(data);
                await _redisCacheService.SetCacheValueAsync(cacheKey, saldoConsolidado, TimeSpan.FromHours(24));
            }

            return saldoConsolidado;
        }

        private void PublicarMensagemRabbitMQ(SaldoConsolidado saldoConsolidado)
        {
            using var channel = _rabbitMqConnection.CreateModel();

            // Declara a exchange do tipo direct
            string exchangeName = "ConsolidadoDiario";
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

            // Declara uma fila e a vincula à exchange com uma routingKey
            string queueName = "consolidado_diario_queue";
            string routingKey = "consolidado_diario";
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

            var mensagem = JsonSerializer.Serialize(saldoConsolidado);
            var body = Encoding.UTF8.GetBytes(mensagem);

            // Publica a mensagem na exchange RabbitMQ
            channel.BasicPublish(exchange: exchangeName,
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
