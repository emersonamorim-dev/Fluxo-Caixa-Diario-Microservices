using FluxoCaixaDiarioMicroservice.Application.UseCases;
using FluxoCaixaDiarioMicroservice.Domain.Entities;
using FluxoCaixaDiarioMicroservice.Domain.Interfaces;
using FluxoCaixaDiarioMicroservice.Infrastructure.Messaging.Events;
using FluxoCaixaDiarioMicroservice.Infrastructure.Redis;
using MediatR;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FluxoCaixaDiarioMicroservice.Application.Services
{
    public class LancamentoService : ILancamentoService
    {
        private readonly IAdicionarLancamentoUseCase _adicionarLancamentoUseCase;
        private readonly IRedisCacheService _redisCacheService;  
        private readonly IMediator _mediator;
        private readonly IConnection _rabbitMqConnection;
        private readonly ILancamentoRepository _lancamentoRepository;

        public LancamentoService(
            IAdicionarLancamentoUseCase adicionarLancamentoUseCase,
            IRedisCacheService redisCacheService, 
            IMediator mediator,
            IConnection rabbitMqConnection,
            ILancamentoRepository lancamentoRepository)
        {
            _adicionarLancamentoUseCase = adicionarLancamentoUseCase;
            _redisCacheService = redisCacheService;
            _mediator = mediator;
            _rabbitMqConnection = rabbitMqConnection;
            _lancamentoRepository = lancamentoRepository;
        }

        public async Task AdicionarLancamentoAsync(Lancamento lancamento)
        {
            await _adicionarLancamentoUseCase.ExecuteAsync(lancamento);

            // Armazena cache Redis expiração de 10 minutos
            var cacheKey = $"Lancamento:{lancamento.Id}";
            await _redisCacheService.SetCacheValueAsync(cacheKey, lancamento, TimeSpan.FromMinutes(10));

            // Publica o evento de criação de lançamento via MediatR
            var evento = new LancamentoCriadoEvent(lancamento.Id, lancamento.Valor, lancamento.Tipo, lancamento.Data);
            await _mediator.Publish(evento);

            // Publica a mensagem no RabbitMQ
            PublicarMensagemRabbitMQ(lancamento);
        }

        public async Task<IEnumerable<Lancamento>> ObterTodosLancamentosAsync()
        {
            return await _lancamentoRepository.ObterTodosLancamentosAsync();
        }

        public async Task<IEnumerable<Lancamento>> ObterLancamentosPorDataAsync(DateTime data)
        {
            return await _lancamentoRepository.ObterLancamentosPorDataAsync(data);
        }

        public async Task<Lancamento> ObterLancamentoPorIdAsync(Guid lancamentoId)
        {
            return await _lancamentoRepository.ObterLancamentoPorIdAsync(lancamentoId);
        }

        public async Task<Lancamento> ObterLancamentoDoCacheAsync(Guid lancamentoId)
        {
            var cacheKey = $"Lancamento:{lancamentoId}";
            return await _redisCacheService.GetCacheValueAsync<Lancamento>(cacheKey);
        }

        private void PublicarMensagemRabbitMQ(Lancamento lancamento)
        {
            using var channel = _rabbitMqConnection.CreateModel();

            // Declara uma exchange do tipo direct
            string exchangeName = "Lancamento";
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

            // Declara uma fila e vincula à exchange com a routingKey "lancamento"
            string queueName = "lancamento_queue";
            string routingKey = "lancamento";
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

            // Serializa o objeto Lancamento para JSON
            var mensagem = JsonSerializer.Serialize(lancamento);
            var body = Encoding.UTF8.GetBytes(mensagem);

            // Publica a mensagem no RabbitMQ
            channel.BasicPublish(exchange: exchangeName,
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
