# ⚖️ Load Balancer em C# (Round Robin + Health Check)

Este é um projeto simples de Load Balancer HTTP. Ele distribui requisições entre múltiplos servidores backend usando o algoritmo Round Robin, e realiza verificações periódicas de saúde para garantir que apenas servidores online recebam tráfego.


## Funcionalidades

- **Algoritmo Round Robin** para distribuição de requisições
- **Health Check automático** a cada 5 segundos (`/health`)
- Ignora backends offline até que voltem a responder
- Construído com **Minimal API**
- Fácil de estender para log, HTTPS ou dashboard


## Estrutura do Projeto

```bash
load-balancer/
├── LoadBalancer/   # Projeto do balanceador
├── Backend1/       # Backend fictício 1
├── Backend2/       # Backend fictício 2
└── Backend3/       # Backend fictício 3
