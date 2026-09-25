# PriceGuard — Project Blueprint

## Product Goal

PriceGuard, kullanıcıların desteklenen mağazalardaki ürünleri takip
etmesini ve ürün hedef fiyatın altına düştüğünde bildirim almasını sağlar.

## First Public Beta

- Tek gerçek mağaza
- Kullanıcı başına en fazla 5 takip
- 6–12 saatlik fiyat kontrolü
- E-posta bildirimi
- 5–10 beta kullanıcısı
- Yalnızca izin verilen mağaza domain'leri

## Technology Stack

### Backend

- .NET 10 LTS
- ASP.NET Core 10
- Entity Framework Core 10
- PostgreSQL 18
- xUnit

### Frontend

- React 19
- TypeScript strict
- Vite 8
- ESLint
- Tailwind CSS 4 (daha sonra eklenecek)

### Infrastructure

- Docker Compose
- GitHub Actions
- Managed PostgreSQL
- Container-based deployment

## Repository Structure

```text
PriceGuard/
├── .github/
│   └── workflows/
│       └── ci.yml
├── src/
│   └── PriceGuard.Api/
│       ├── Features/
│       ├── Data/
│       ├── Scraping/
│       ├── Jobs/
│       └── Common/
├── tests/
│   └── PriceGuard.Tests/
├── web/
│   └── priceguard-web/
├── docs/
│   └── PROJECT_BLUEPRINT.md
├── docker-compose.yml
├── .gitignore
├── README.md
└── PriceGuard.slnx
```

Initial Entities

- User
- Store
- Product
- PriceWatch
- PriceObservation
- Notification

Development Phases
Phase 1 — Foundation

- Repository structure
- Backend and frontend projects
- Tests
- CI pipeline
- PostgreSQL development environment
  Phase 2 — FakeStore Vertical Slice
- FakeStore price source
- Product tracking endpoint
- Manual price check
- PostgreSQL persistence
- Minimal React interface
  Phase 3 — Real Price Source
- One approved store/source
- Domain allowlist
- SSRF protection
- Timeouts and rate limiting
- HTML fixture tests
  Phase 4 — Background Checks
- BackgroundService
- Periodic price checks
- Price history
- Target-price detection
- E-mail notifications
  Phase 5 — Public Beta
- Authentication
- Deployment
- Health checks
- Structured logging
- 5–10 beta users
  Phase 6 — Incident Detection
- PriceCheckAttempt records
- Repeated-failure detection
- Sanitized HTML snapshots
- Incident management screen
  Phase 7 — AI Incident Assistant
- MCP tools for incidents, snapshots and repository access
- Root-cause analysis
- Reproduction test
- Patch generation
- Human-approved draft pull request
- No automatic merge

Explicitly Deferred
The first MVP will not include:

- Separate Clean Architecture projects
- CQRS or MediatR
- RabbitMQ
- Redis
- Hangfire
- Multiple stores
- Telegram notifications
- MCP or LangChain
- Automatic pull requests
- Microservices

Current Milestone
A user can submit a FakeStore product URL and target price.
The product is persisted in PostgreSQL, manually checked,
and displayed in the React interface.
