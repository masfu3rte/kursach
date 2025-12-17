# Курсовой проект: информационная система проектной организации

Пример настольного приложения на C# (Windows Forms), демонстрирующий основные сущности проектной организации: сотрудники, отделы, договоры, проекты, оборудование и субподряды. Реализация вдохновлена структурой [Cinema Rental System](https://github.com/Dikost1/cinema-rental-system/tree/main/CinemaRentalSystem), но адаптирована под требования курсовой работы.

## Основные возможности
- Моделирование категорий сотрудников (конструкторы, инженеры, техники, лаборанты, обслуживающий персонал) с уникальными атрибутами.
- Связанные отделы с руководителями и привязкой оборудования.
- Договоры и проекты, включая руководителей, команды, оборудование и субподряды.
- Простые отчеты: руководители отделов, активные проекты и договоры, использование и эффективность оборудования, эффективность договоров.
- Сохранение данных в JSON (`organization-data.json`) между запусками; при первом старте создаются демонстрационные данные.

## Структура проекта
```
src/ProjectOrganizationApp/ProjectOrganizationApp.csproj  // WinForms проект .NET 6
src/ProjectOrganizationApp/Program.cs                     // входная точка приложения
src/ProjectOrganizationApp/Models/                        // доменные модели
src/ProjectOrganizationApp/Services/                      // сервисы хранения и отчетов
src/ProjectOrganizationApp/Views/                         // форма и разметка интерфейса
```

## Запуск
1. Откройте решение в **Microsoft Visual Studio** (Windows) или соберите через CLI:
   ```bash
   dotnet restore
   dotnet build src/ProjectOrganizationApp/ProjectOrganizationApp.csproj
   dotnet run --project src/ProjectOrganizationApp/ProjectOrganizationApp.csproj
   ```
2. При первом запуске будут созданы демонстрационные данные; далее состояние хранится в `organization-data.json` рядом с исполняемым файлом.

## Использованные идеи
- Объектно-ориентированное моделирование по книгам Роберта Мартина и *Head First Design Patterns*.
- Четкое разделение модели, сервисов хранения/отчетности и UI в духе *Clean Architecture*.
