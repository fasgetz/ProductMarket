# Продуктовый магазин market-products.online

Описание проекта
-----------------------------------

[Проект](https://market-products.online)  написан с использованием современных инструментальных средств, который реализует основной функционал магазина продуктов (абстрактно - любых товаров) с использованием микросервисной архитектуры.

+ Инструментальные средства:
            
  + `AspNet Core 3.1`
  + `AspNet Identity`  
  + `MS SQL`
  + `ORM Entity Framework Core`
  + `Bootstrap 4`
  + `Vue JS`
  + `axios`
  + `Docker`
  + `Elastic Search`
  + `Kibana` 
  + `RabbitMQ`
  + `MassTransit` 

Сервисы проекта
-----------------------------------

+ Веб клиент ProductMarket

Клиент содержит всю необходимую разметку для функционирования. Логика работы реализована путем обращения к сервису API

+ API ProductMarket

API предоставляет из себя сервис, который предоставляет методы, необходимые для работы с функционалом веб клиента.

Методы, которые доступны только администратору должны в обязательном порядке пройти аутентификацию по правам администратора через JWT токен путем отправки в загаловок запроса Header.
