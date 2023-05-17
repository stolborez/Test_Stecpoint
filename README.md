# Тестовое задание Stecpoint
#### Стэк:
- net Core 3.1
- MediatR
- MassTransit(реализует паттерн шины для RabbitMq)
- AutoMapper (приведение одних типов классов к другим)
- Serilog
- Entity Framework
- Docker
- Postgres(MS SQL)
- FluentValidation

##### Задание:

В Docker поднять:
- RabbitMq
- 2 сервиса которые будут между собой общаться по шине
- БД любую из перечисленных

1 сервис:
- Принимает запросы типа post (имя, фамилия, отчество, номер, email)
- Предусмотреть простую валидацию что все кроме отчества должно быть заполнено.
- В бизнес логике этого запроса отправлять по шине полученную информацию на 2 сервис.
- Так же при отправке в консоль выводить логирование (сообщение о отправке вышеперечисленных данных)

2 сервис:
- Принять сообщение с шины и положить его в БД (таблица Users) и залогировать это в консоль.
- Так же у сервиса есть post запрос который связывает пользователя с организацией (таблица Organizations)
- И post-запрос который возвращает пагинацию пользователей по организации. (когда возвращаем ответ используем AutoMapper)
- При старте сделать seed в БД если там ничего нет.

##### БД:
Таблица Users:
- Ключ пользователя + Ключ организации к которой привязан пользователь + Информация о пользователе
- Таблица Organizations:
- Ключ организации + Название организации

##### Написать юнит тесты
