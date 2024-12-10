# Unistream Test Task

Задание находится в папке `./docs`

## Пара слов
Для ухода от блокировок БД и esrialized транзакций написал свой легкий "блокировщик" с использованием:
- SemaphoreSlim
- ConcurrentDictionary

Благодаря этому блокируется работа только с конкретным клиентом или транзакцией, а не "вся таблица" или "вся критическая секция".
Подробнее см. интерфейсы в `./src/core/Domain`

## Запуск

Если захочется запустить и "пощупать" (swagger в наличии), можно собрать образ docker или вытянуть из docker-hub.
Подробные инструкции в папке `./scripts`.
Ну или просто запустить проект - ведь для простоты под капотом `SQL-Lite`.
Других зависимостей нет.

## Used packages
- [FastEndpoints](https://fast-endpoints.com/) (vpn required)
- Automapper
- [Automapper extension](https://www.nuget.org/packages/KutCode.AutoMapper.Extensions) (свой пакет)
- [Optionality](https://www.nuget.org/packages/KutCode.Optionality) (свой пакет)
- Serilog
- EFCore + NamingConventions
- MediatR

## TODOs
- add trace-id in context with accepted header or generate 
- add JWT bearer authorization
- логика общения с бд внутри транзакций - не оптимальна, идеально - переделать с использованием persistence кэшей и хранимых процедур
- идемпотентность revert нарушена - т.к. не понятно какой именно баланс отдавать в ответе, по условию задачи, подробнее об жтом в комментариях в коде

## P.s.
Почитайте перевод статьи про CUPID:
https://github.com/noncommunicado/cupid