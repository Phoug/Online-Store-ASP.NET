# 🛒 OnlineStore

**OnlineStore** — это полнофункциональное веб-приложение интернет-магазина, построенное на **ASP.NET Core** (REST API) и **Blazor WebAssembly** с общей библиотекой моделей (**Shared**).  

Проект демонстрирует архитектуру full‑stack приложения на .NET с разделением клиентской и серверной логики.

---

## 📂 Структура решения

- **Server** — ASP.NET Core Web API, Entity Framework Core, работа с БД, авторизация.
- **Client** — Blazor WebAssembly, UI-компоненты, взаимодействие с API.
- **Shared** — общие классы сущностей (`Product`, `Category`, `Order`, `User`, `Review`, `Delivery`, `PickupPoint`).

---

## 🚀 Основные возможности (MVP)

- Просмотр каталога товаров
- Регистрация и авторизация пользователей
- Добавление товаров в корзину
- Оформление заказов
- Управление товарами и заказами (админ-панель)
- Отзывы о товарах
- Доставка и пункты выдачи

---

## 🛠️ Технологии

- **.NET 8**
- **ASP.NET Core Web API**
- **Blazor WebAssembly**
- **Entity Framework Core**
- **PostgreSQL** или **SQL Server**
- **Swagger / OpenAPI** для документации API

---

## 🕜 На текущий момент:

Был добавлен файл ```Requirements Document RUS``` в котором размещена основная информация о проекте.

Добавлены модели в каталог ```Online-Store-ASP.NET.Shared```, для дальнейшей разработки. 

---

## ❔ Мокапы проекта:

[Главный экран](mockups\main_page.png)

[Информация о товаре](mockups\product_info.png)

[Список категорий](mockups\category_widget.png)

[Окно редактирования](mockups\edit_window.png)

---

## ⚙️ Запуск проекта

1. **Клонировать репозиторий**
   ```bash
   git clone https://github.com/Phoug/Online-Store-ASP.NET.git