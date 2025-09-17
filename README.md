# Online Store

Веб-приложение интернет-магазина, разработанное на **ASP.NET Core** (REST API) и **Blazor WebAssembly** с общей библиотекой моделей (**Shared**).  
Проект создан для демонстрации архитектуры full-stack приложения на .NET с разделением клиентской и серверной логики.

## Структура решения

- **OnlineStore.Server** — серверная часть:
  - REST API на ASP.NET Core
  - Подключение к базе данных через Entity Framework Core
  - Авторизация и аутентификация
  - Логирование и обработка ошибок

- **OnlineStore.Client** — клиентская часть:
  - Blazor WebAssembly SPA
  - Взаимодействие с API через HttpClient
  - Компоненты и страницы для отображения каталога, корзины, заказов

- **OnlineStore.Shared** — общие модели:
  - Классы сущностей (`Product`, `Order`, `OrderItem`, `UserDto`)
  - DTO для обмена данными между клиентом и сервером

## Возможности (MVP)

- Просмотр каталога товаров
- Регистрация и авторизация пользователей
- Добавление товаров в корзину
- Оформление заказов
- Панель администратора для управления товарами и заказами

## Технологии

- **.NET 8**
- **ASP.NET Core Web API**
- **Blazor WebAssembly**
- **Entity Framework Core**
- **PostgreSQL** или **SQL Server**
- **Swagger/OpenAPI** для документации API

## Запуск проекта

1. Клонировать репозиторий:
   ```bash
   git clone https://github.com/username/OnlineStore.git
